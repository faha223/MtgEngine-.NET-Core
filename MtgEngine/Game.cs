using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Utilities;

namespace MtgEngine
{
    public class Game
    {
        private List<Player> _players { get; } = new List<Player>();

        public Player ActivePlayer { get; private set; }

        private Player _nextPlayer
        {
            get
            {
                int i = _players.IndexOf(ActivePlayer) + 1;
                return _players[i % _players.Count];
            }
        }

        private Queue<Player> _extraTurns { get; } = new Queue<Player>();

        private Player _priorityPlayer { get; set; }

        private Stack<IResolvable> Stack { get; } = new Stack<IResolvable>();

        public List<Card> CardsOnStack()
        {
            return Stack.Where(c => c is Card).Select(c => c as Card).ToList();
        }

        public List<Ability> AbilitiesOnStack()
        {
            return Stack.Where(c => c is Ability).Select(c => c as Ability).ToList();
        }

        private List<Ability> AbilitiesTriggered { get; } = new List<Ability>();

        public List<IResolvable> ObjectsOnStack()
        {
            return Stack.ToList();
        }

        public List<Player> Players()
        {
            return new List<Player>(_players);
        }

        /// <summary>
        /// This method removes a spell from the stack and puts it in its owner's graveyard. If the given spell is not on the stack, then this effect does nothing.
        /// </summary>
        /// <param name="card">This spell is the spell that is to be countered.</param>
        public void Counter(IResolvable obj)
        {
            if(Stack.Contains(obj))
            {
                Stack<IResolvable> temp = new Stack<IResolvable>(Stack.Count);
                IResolvable topmost = null;
                do
                {
                    topmost = Stack.Pop();
                    if (topmost == obj)
                        break;
                    temp.Push(topmost);
                    topmost = null;
                } while (Stack.Count > 0);

                while (temp.Count > 0)
                    Stack.Push(temp.Pop());

                if (topmost != null)
                {
                    if(topmost is Card)
                    (topmost as Card).Owner.Graveyard.Add(topmost as Card);
                }
            }
            CheckStateBasedActions();
        }

        /// <summary>
        /// This spell destroys all lands that meet the selector's requirement
        /// </summary>
        /// <param name="selector"></param>
        public void DestroyLands(Func<Card, bool> selector)
        {
            List<Card> landsDestroyed = new List<Card>();
            foreach(var player in _players)
            {
                foreach(var land in player.Battlefield.Lands.Where(c => selector(c)).ToList())
                {
                    player.Battlefield.Remove(land);
                    landsDestroyed.Add(land);
                }
            }

            // TODO: Add Land On LeaveBattlefield or OnDestroy triggers to the stack in order
        }

        delegate void CardEvent(Game game, Card card);
        private event CardEvent CardHasEnteredStack;
        private event CardEvent CardHasEnteredBattlefield;

        delegate void AbilityEvent(Game game, Ability ability);
        private event AbilityEvent AbilityHasEnteredStack;

        delegate void GameStepEvent(string step);
        private event GameStepEvent CurrentStepHasChanged;

        delegate void PlayerTookDamageEvent(Player player, int damageDealt);
        private event PlayerTookDamageEvent PlayerTookDamage;

        public void AddPlayer(Player player)
        {
            if (!_players.Contains(player))
            {
                CurrentStepHasChanged += player.GameStepChanged;
                CardHasEnteredBattlefield += player.CardHasEnteredBattlefield;
                CardHasEnteredStack += player.CardHasEnteredStack;
                PlayerTookDamage += player.PlayerTookDamage;
                _players.Add(player);
            }
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                // Determine Turn Order
                ShufflePlayers();
                ActivePlayer = _players.First();

                // Deal Opening Hands
                _players.ForEach(player => player.ShuffleLibrary());
                _players.ForEach(player => player.Draw(7));

                // Offer Players the option of mulliganing
                OfferMulligans();

                // Go to Player 1 Turn 1
                do
                {
                    _players.ForEach(player => ResetLandsPlayed(player));

                    CheckStateBasedActions();

                    BeginningPhase();

                    CheckStateBasedActions();

                    MainPhase(true);

                    CheckStateBasedActions();

                    CombatPhase();

                    CheckStateBasedActions();

                    MainPhase(false);

                    CheckStateBasedActions();

                    EndingPhase();

                    ActivePlayer = _nextPlayer;
                } while (true);
            });
        }

        /// <summary>
        /// This method shuffles the Players collection by having the players Roll for Initiative, similarly to how we play in Paper MTG, except they roll d100 instead of 2d6
        /// </summary>
        private void ShufflePlayers()
        {
            var temp = new Queue<Player>(_players.Count);
            foreach (var player in _players)
                temp.Enqueue(player);
            _players.Clear();

            Dictionary<int, Player> initiative = new Dictionary<int, Player>(temp.Count);
            while(temp.Count > 0)
            {
                var player = temp.Dequeue();
                int i = player.RollInitiative();

                if(initiative.ContainsKey(i))
                {
                    temp.Enqueue(initiative[i]);
                    temp.Enqueue(player);

                    initiative.Remove(i);
                }
                else
                {
                    initiative.Add(i, player);
                }
            }

            foreach (var pair in initiative.ToList().OrderBy(c => c.Key))
            {
                _players.Add(pair.Value);
            }
            initiative.Clear();
        }

        /// <summary>
        /// This method offers all players mulligans in Turn order, then gives them each a mulligan. The process repeats until no player has opted to take their hand
        /// </summary>
        private void OfferMulligans()
        {
            List<Player> playersTakingMulligan = new List<Player>(_players);
            do
            {
                // Offer each player a mulligan, note which ones chose to keep their current hand
                List<Player> playersKeeping = new List<Player>(playersTakingMulligan.Count);
                foreach (var player in playersTakingMulligan)
                {
                    if (!player.OfferMulligan())
                        playersKeeping.Add(player);
                }

                // Remove players who chose to keep their hand from the players taking mulligan
                playersTakingMulligan.RemoveAll(player => playersKeeping.Contains(player));

                // Apply the Vancouver Mulligan strategy to all players who elected to take a mulligan
                playersTakingMulligan.ForEach(player => VancouverMulligan(player));

            } while (playersTakingMulligan.Count > 0);

            // Players who are starting with a hand of less than 7 scry 1
            foreach(var player in _players)
            {
                if (player.Hand.Count < 7)
                {
                    var scryedCards = player.Library.Take(1).ToList();
                    player.Library.RemoveRange(0, 1);
                    player.ScryChoice(scryedCards, out var cardsOnTop, out var cardsOnBottom);
                    player.Library.AddRange(cardsOnBottom);
                    foreach (var card in cardsOnTop.Reverse())
                        player.Library.Insert(0, card);
                }
            }
        }

        private void BeginningPhase()
        {
            UntapStep();

            CheckStateBasedActions();

            UpkeepStep();

            CheckStateBasedActions();

            DrawStep();
        }

        private void UntapStep()
        {
            CurrentStepHasChanged("Untap Step");
            ActivePlayer.Battlefield.Creatures.ForEach(c => c.HasSummoningSickness = false);
            ActivePlayer.Battlefield.ForEach(c => c.Untap());

            DrainManaPools();
        }

        private void UpkeepStep()
        {
            CurrentStepHasChanged("Upkeep Step");

            // TODO: Add Upkeep Triggers to the stack in ApNap order according to their controllers
            ApNapLoop(ActivePlayer, false);

            // Drain each player's mana pool
            DrainManaPools();
        }

        private void DrawStep()
        {
            CurrentStepHasChanged("Draw Step");
            // TODO: Add Beginning of Draw Step Triggers to the Stack

            ActivePlayer.Draw(1);

            DrainManaPools();
        }

        private void MainPhase(bool beforeCombat)
        {
            CurrentStepHasChanged($"{(beforeCombat ? "Precombat" : "Postcombat")} Main Phase");

            // TODO: Add Beginning of Main Phase Triggers to the stack

            if (beforeCombat)
            {
                // TODO: Add Beginning of Precombat Main Phase Triggers to the stack
            }
            else
            {
                // TODO: Add Beginning of Postcombat Main Phase Triggers to the stack
            }

            // TODO: Cycle Priority starting with the Active Player, give only the Active Player the ability to play Sorcery-speed spells
            ApNapLoop(ActivePlayer, true);

            DrainManaPools();
        }

        private void CombatPhase()
        {
            BeginningOfCombatStep();

            // If the active player controls no creatures without summoning sickness, is this really necessary?
            DeclareAttackersStep();

            // If Attackers were declared
            if (ActivePlayer.Battlefield.Creatures.Any(card => card.IsAttacking))
            {
                DeclareBlockersStep();

                DamageStep();
            }

            // TODO : If there are no attackers, is this really necessary?
            EndOfCombatStep();
        }

        private void BeginningOfCombatStep()
        {
            CurrentStepHasChanged("Beginning of Combat");
            // TODO: Add Beginning of Combat Triggers to the Stack

            ApNapLoop(ActivePlayer, false);

            DrainManaPools();
        }

        private void DeclareAttackersStep()
        {
            CurrentStepHasChanged("Declare Attackers Step");

            // Ask the Active Player to declare attackers
            var attackers = ActivePlayer.DeclareAttackers(_players.Except(new[] { ActivePlayer }).ToList());
            if (attackers != null)
            {
                foreach (var declaration in attackers)
                {
                    declaration.AttackingCreature.DefendingPlayer = declaration.DefendingPlayer;

                    // Tap all attacking creatures that don't have vigilance
                    if (!declaration.AttackingCreature.StaticAbilities.Contains(StaticAbility.Vigilance))
                        declaration.AttackingCreature.Tap();
                }
            }

            // TODO: Any "When this creature attacks" triggers get put onto the stack

            ApNapLoop(ActivePlayer, false);

            DrainManaPools();
        }

        private void DeclareBlockersStep()
        {
            CurrentStepHasChanged("Declare Blockers Step");

            // TODO: Ask the Defending Players, in ApNap order, to declare Blockers
            var defendingPlayers = _players.StartAt(ActivePlayer)
                .Where(p => ActivePlayer.Battlefield.Creatures.Any(c => c.DefendingPlayer == p));
            // Iterate over the defending players in turn order
            foreach(var player in defendingPlayers)
            {
                var blockers = player.DeclareBlockers(ActivePlayer.Battlefield.Creatures.Where(c => c.DefendingPlayer == player).ToList());
                if(blockers != null)
                {
                    foreach(var blocker in blockers)
                    {
                        blocker.Blocker.Blocking = blocker.Attacker;
                    }
                }
            }

            ApNapLoop(ActivePlayer, false);

            DrainManaPools();
        }

        private void DamageStep()
        {
            CurrentStepHasChanged("Damage Step");

            // These creatures have been blocked
            var blockedCreatures = ActivePlayer.Battlefield.Creatures.Where(c => c.IsAttacking && c.DefendingPlayer.Battlefield.Creatures.Any(d => d.Blocking == c)).ToList();

            // Have the active player sort blockers for each of their attackers
            Dictionary<PermanentCard, List<PermanentCard>> blockerMap = new Dictionary<PermanentCard, List<PermanentCard>>(blockedCreatures.Count);
            foreach(var attacker in blockedCreatures)
            {
                var blockers = attacker.DefendingPlayer.Battlefield.Creatures.Where(c => c.Blocking == attacker).ToList();

                if (blockers.Count > 1)
                    blockerMap.Add(attacker, ActivePlayer.SortBlockers(attacker, blockers).ToList());
                else
                    blockerMap.Add(attacker, blockers);
            }

            // If any attackers have firststrike or doublestrike
            if (ActivePlayer.Battlefield.Creatures.Any(c => c.IsAttacking && (doesFirstStrikeDamage(c) || takesFirstStrikeDamage(c))))
            {
                // TODO: Deal First Strike Damage
                foreach(var attacker in ActivePlayer.Battlefield.Creatures.Where(c => c.IsAttacking && (doesFirstStrikeDamage(c) || takesFirstStrikeDamage(c))))
                {
                    // Deal combat damage to, and take combat damage from, blockers                    
                    CombatDamage(attacker, 
                        blockedCreatures.Contains(attacker), 
                        blockerMap.ContainsKey(attacker) ? blockerMap[attacker].Where(c => doesFirstStrikeDamage(attacker) || doesFirstStrikeDamage(c)) : null,
                        true);
                }

                CheckStateBasedActions();
            }

            // Remove all blockers from the blocker map that have left the defending player's battlefield after firststrike damage before applying normal combat damage
            foreach(var attacker in blockerMap.Keys)
            {
                var blockers = blockerMap[attacker];
                blockers.RemoveAll(c => !c.Controller.Battlefield.Contains(c));
                blockerMap[attacker] = blockers;
            }

            // If any remaining attackers have doublestrike or don't have firststrike
            if (ActivePlayer.Battlefield.Creatures.Any(c => c.IsAttacking && (doesNormalDamage(c) || takesNormalDamage(c))))
            {
                foreach(PermanentCard attacker in ActivePlayer.Battlefield.Creatures.Where(c => c.IsAttacking && doesNormalDamage(c)))
                {
                    // Deal combat damage to, and take combat damage from, blockers
                    CombatDamage(attacker, 
                        blockedCreatures.Contains(attacker),
                        blockerMap.ContainsKey(attacker) ? blockerMap[attacker].Where(c => doesNormalDamage(attacker) || doesNormalDamage(c)) : null,
                        false);
                }

                CheckStateBasedActions();
            }

            DrainManaPools();
        }

        private void CombatDamage(PermanentCard attacker, bool blocked, IEnumerable<PermanentCard> blockers, bool firstStrike)
        {
            // If the defending player didn't block (we might not have blockers right now)
            if (!blocked)
            {
                ApplyDamage(attacker.DefendingPlayer, attacker, attacker.Power);
            }
            else if(blockers != null)
            {
                // Deal damage to the defending player's creatures
                var damageOutput = attacker.Power;

                // Have the attacking creatures apply damage to and take damage from blocking creatures
                foreach (var blocker in blockers)
                {
                    // The attacker hits
                    if ((firstStrike && doesFirstStrikeDamage(attacker)) || (!firstStrike && doesNormalDamage(attacker)))
                    {
                        if (damageOutput > 0)
                        {
                            int damageDealt = Math.Min(blocker.Toughness, damageOutput);
                            blocker.TakeDamage(damageDealt, attacker);
                            damageOutput -= damageDealt;
                        }
                    }

                    // The blocker hits back
                    if((firstStrike && doesFirstStrikeDamage(blocker)) || (!firstStrike && doesNormalDamage(blocker)))
                        attacker.TakeDamage(blocker.Power, blocker);
                }

                // Trample Damage to defending player
                if (attacker.HasTrample && damageOutput > 0)
                {
                    ApplyDamage(attacker.DefendingPlayer, attacker, damageOutput);
                }
            }
        }

        // Do first strike damage only if the creature has first strike or double strike
        private bool doesFirstStrikeDamage(PermanentCard creature)
        {
            if (creature.StaticAbilities.Contains(StaticAbility.FirstStrike) || creature.StaticAbilities.Contains(StaticAbility.DoubleStrike))
                return true;
            return false;
        }

        // Take first strike damage if any blocking creatures do first strike damage
        private bool takesFirstStrikeDamage(PermanentCard attacker)
        {
            return attacker.DefendingPlayer.Battlefield.Creatures.Any(c => c.Blocking == attacker && doesFirstStrikeDamage(c));
        }

        // Do normal damage unless the creature has first strike and NOT double strike
        private bool doesNormalDamage(PermanentCard creature)
        {
            if (creature.StaticAbilities.Contains(StaticAbility.DoubleStrike))
                return true;
            if (creature.StaticAbilities.Contains(StaticAbility.FirstStrike))
                return false;
            return true;
        }

        // Take normal damage if any blocking creatures do normal damage
        private bool takesNormalDamage(PermanentCard attacker)
        {
            return attacker.DefendingPlayer.Battlefield.Creatures.Any(c => c.Blocking == attacker && doesNormalDamage(c));
        }

        private void EndOfCombatStep()
        {
            CurrentStepHasChanged("End of Combat Step");

            // Give Priority to Players
            ApNapLoop(ActivePlayer, false);

            // TODO: Add any "End of Combat" triggers to the stack

            // Remove all creatures from combat
            foreach (var creature in ActivePlayer.Battlefield.Creatures)
                creature.DefendingPlayer = null;
            foreach(var player in _players)
            {
                foreach (var creature in player.Battlefield.Creatures)
                    creature.Blocking = null;
            }

            DrainManaPools();
        }

        private void EndingPhase()
        {
            EndStep();

            CheckStateBasedActions();

            CleanupStep();
        }

        private void EndStep()
        {
            CurrentStepHasChanged("End Step");
            // TODO: Add EndStep Triggers to the Stack in ApNap Order
            ApNapLoop(ActivePlayer, false);

            DrainManaPools();
        }

        private void CleanupStep()
        {
            CurrentStepHasChanged("Cleanup Step");
            // TODO: Discard down to Maximum Hand Size
            ActivePlayer.DiscardToHandSize();

            // TODO: Remove Marked Damage from Permanents, and "Until End of Turn" and "This Turn" effects go away
            foreach (var player in _players)
                foreach (var creature in player.Battlefield.Creatures)
                    creature.ResetDamage();

            // TODO: If there are triggered abilities on the stack then players may receive priority, in ApNap order, to react to them before the stack resolves.
            // Example: Madness is a triggered ability that happens when a player discards a card with Madness. This can potentially be put on the stack from the First part of the Cleanup step

            DrainManaPools();
        }

        private void ApNapLoop(Player startingPlayer, bool startingPlayerCanCastSorceries)
        {
            // Idea: This might need to be put in a separate class, a Stack Resolver

            // If the player responds with an illegal action then inform the user of why the action is illegal, and ask again

            // This function returns when the stack is empty and all players have Passed Priority since the last spell resolved

            // Loop through all players, starting with startingPlayer, and ask them if they would like to take an action.
            foreach (var player in _players.StartAt(startingPlayer))
            {
                // TODO: Give Priority
                _priorityPlayer = player;

                bool playerHasPassedPriority = false;
                do
                {
                    var chosenAction = player.GivePriority(this, player == startingPlayer && Stack.Count == 0 && startingPlayerCanCastSorceries);

                    switch (chosenAction.ActionType)
                    {
                        case ActionType.PassPriority:
                            // If the player responds PassPriority, then go to the next player.
                            playerHasPassedPriority = true;
                            continue;
                        case ActionType.PlayCard:
                            {
                                var action = chosenAction as PlayCardAction;

                                // If it's a land card, it bypasses the stack
                                if (action.Card is LandCard)
                                {
                                    if (player == startingPlayer && startingPlayerCanCastSorceries && player.LandsPlayedThisTurn < player.MaxLandsPlayedThisTurn)
                                    {
                                        PlayLand(action.Card as LandCard);
                                        player.LandsPlayedThisTurn++;
                                        CardHasEnteredBattlefield?.Invoke(this, action.Card);
                                    }
                                    else
                                    {
                                        // Player can't play a land right now
                                    }
                                }
                                else
                                {
                                    // Make player pay the cost of the Card
                                    if (action.Card.Cost.Pay())
                                    {
                                        // Put the Card onto the stack
                                        action.Card.OnCast(this);
                                        player.Hand.Remove(action.Card);
                                        PushOntoStack(action.Card, _players[(_players.IndexOf(player) + 1) % _players.Count]);
                                    }
                                }
                            }
                            break;
                        case ActionType.ActivateAbility:
                            {
                                var action = chosenAction as ActivateAbilityAction;
                                var ability = action.Ability as ActivatedAbility;

                                // If it's a mana ability, it bypasses the stack
                                if (action.Ability is ManaAbility)
                                {
                                    var manaAbility = action.Ability as ManaAbility;
                                    if (manaAbility.Cost.CanPay())
                                    {
                                        if (manaAbility.Cost.Pay())
                                        {
                                            manaAbility.OnResolve(this);
                                        }
                                    }
                                }
                                else
                                {
                                    var activatedAbility = action.Ability as ActivatedAbility;
                                    if (activatedAbility.Cost.CanPay())
                                    {
                                        if(activatedAbility.Cost.Pay())
                                        {
                                            PushOntoStack(activatedAbility, _players[(_players.IndexOf(player) + 1) % _players.Count]);
                                        }
                                        
                                    }
                                }
                            }
                            break;
                    }
                } while (!playerHasPassedPriority);
            }
        }

        private void PushOntoStack(IResolvable resolvable, Player playerToGivePriority)
        {
            Stack.Push(resolvable);
            if (resolvable is Card)
                CardHasEnteredStack?.Invoke(this, resolvable as Card);
            else if (resolvable is Ability)
                AbilityHasEnteredStack?.Invoke(this, resolvable as Ability);
            ApNapLoop(playerToGivePriority, false);
            
            // If we made it back with no responses, resolve the spell
            while (Stack.Count > 0)
            {
                ResolveTopOfStack();
                if (Stack.Count > 0)
                {
                    ApNapLoop(ActivePlayer, false);
                }
            }
        }

        private void ResolveTopOfStack()
        {
            if (Stack.Count > 0)
            {
                var obj = Stack.Pop();
                if (obj is Card)
                {
                    var card = obj as Card;
                    if (card is PermanentCard)
                    {
                        var permanent = card as PermanentCard;
                        permanent.OnResolve(this);
                        PutPermanentOnBattlefield(permanent);
                    }
                    else if (card is SpellCard)
                    {
                        card.OnResolve(this);
                        card.Owner.Graveyard.Add(card);
                    }
                }
                else if (obj is Ability)
                {
                    var ability = obj as Ability;
                    ability.OnResolve(this);
                }
            }
            CheckStateBasedActions();
        }

        private void CheckStateBasedActions()
        {
            // TODO: Kill any creatures that have 0 toughness, or have sustained enough damage to be destroyed and don't have indestructible
            foreach(var player in _players)
            {
                var deadCreatures = player.Battlefield.Creatures.Where(c => c.IsDead).ToList();
                foreach(var creature in deadCreatures)
                {
                    creature.Controller.Battlefield.Remove(creature);
                    creature.ClearCounters();
                    creature.Owner.Graveyard.Add(creature);
                }

                var deadPlaneswalkers = player.Battlefield.Planeswalkers.Where(c => c.IsDead).ToList();
                foreach(var planeswalker in deadPlaneswalkers)
                {
                    planeswalker.Controller.Battlefield.Remove(planeswalker);
                    planeswalker.ClearCounters();
                    planeswalker.Owner.Graveyard.Add(planeswalker);
                }
            }

            // TODO: Any players with 0 life total lose the game

            // TODO: Any players with 10 poison counters lose the game
            
            // TODO: If an effect has caused a player to win the game, all other players lose

            // TODO: If a player has lost the game, remove them from the _players list. If they were the active player, go to the start of the next player's turn

            // Check State Triggered Abilities
            foreach(var player in _players)
            {
                foreach(PermanentCard permanent in player.Battlefield)
                {
                    foreach(StateTriggeredAbility ability in permanent.Abilities.Where(c => c is StateTriggeredAbility))
                    {
                        if(ability.CheckState(this))
                        {
                            AbilitiesTriggered.Add(ability);
                        }
                    }
                }
            }
            if(AbilitiesTriggered.Count > 0)
            {
                // If there was more than 1 trigger, they'll need to be sorted
                if (AbilitiesTriggered.Count > 1)
                {
                    foreach (var player in AbilitiesTriggered.Select(c => c.Source.Controller).Distinct())
                    {
                        // TODO: Have the Player sort the abilities
                    }
                }

                // Add the triggered abilities to the stack
                foreach(var ability in AbilitiesTriggered)
                    Stack.Push(ability);
                
                // Resolve the stack
                while (Stack.Count > 0)
                {
                    ApNapLoop(ActivePlayer, false);
                    ResolveTopOfStack();
                }
            }
        }

        /// <summary>
        /// Play the specified Land card
        /// </summary>
        /// <param name="card"></param>
        private void PlayLand(LandCard card)
        {
            // Remove the card from the player's hand and place it on the battlefield
            card.Controller.Hand.Remove(card);
            PutPermanentOnBattlefield(card);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        private void CastSpell(Card card)
        {
            // Can't do anything if we can't pay the cost
            if (card.Cost.CanPay())
            {
                card.Cost.Pay();
                card.Controller.Hand.Remove(card);
                Stack.Push(card);
            }
        }

        #region Utility Methods

        /// <summary>
        /// Vancouver mulligan works by having the player return their hand to their library and 
        /// drawing 1 fewer cards. In Multiplayer, the player gets a free first mulligan.
        /// </summary>
        /// <param name="player">The player that is taking a mulligan</param>
        private void VancouverMulligan(Player player)
        {
            // The player Shuffles their current Hand into their library
            player.Library.AddRange(player.Hand);
            player.Hand.Clear();
            player.ShuffleLibrary();

            // Then draws a new hand, the number of cards is determined by how many mulligans the player has taken already
            if (_players.Count > 2) // Free first mulligan if multiplayer
            {
                player.Draw(Math.Max(0, 7 - player.MulligansTaken));
                player.MulligansTaken++;
            }
            else
            {
                player.Draw(Math.Max(0, 6 - player.MulligansTaken));
                player.MulligansTaken++;
            }                
        }

        /// <summary>
        /// Resets the Player's lands played per turn and max lands played per turn
        /// </summary>
        /// <param name="player"></param>
        private void ResetLandsPlayed(Player player)
        {
            player.LandsPlayedThisTurn = 0;
            player.MaxLandsPlayedThisTurn = (player == ActivePlayer ? 1 : 0);
        }

        /// <summary>
        /// Resets each player's mana pool to zero
        /// </summary>
        private void DrainManaPools()
        {
            _players.ForEach(c => c.ManaPool.Clear());
        }

        public void AbilityTriggered(Ability ability)
        {
            AbilitiesTriggered.Add(ability);
        }

        public void ApplyDamage(IDamageable target, Card source, int amount)
        {
            target.TakeDamage(amount, source);
            if(target is Player)
                PlayerTookDamage?.Invoke(target as Player, amount);
        }

        public void PutPermanentOnBattlefield(PermanentCard permanent)
        {
            permanent.Controller.Battlefield.Add(permanent);
            if (permanent.IsACreature)
                permanent.HasSummoningSickness = true;
            CardHasEnteredBattlefield?.Invoke(this, permanent);
        }

        #endregion Utility Methods
    }
}
