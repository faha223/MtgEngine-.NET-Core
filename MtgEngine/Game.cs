using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private Player _activePlayer { get; set; }

        private Player _nextPlayer
        {
            get
            {
                int i = _players.IndexOf(_activePlayer) + 1;
                return _players[i % _players.Count];
            }
        }

        private Queue<Player> _extraTurns { get; } = new Queue<Player>();

        private Player _priorityPlayer { get; set; }

        private Stack<Card> Stack { get; } = new Stack<Card>();

        delegate void CardEvent(Card card);
        private event CardEvent CardHasEnteredStack;
        private event CardEvent CardHasEnteredBattlefield;

        delegate void GameStepEvent(string step);
        private event GameStepEvent CurrentStepHasChanged;

        public void AddPlayer(Player player)
        {
            if (!_players.Contains(player))
            {
                CurrentStepHasChanged += player.GameStepChanged;
                CardHasEnteredBattlefield += player.CardHasEnteredBattlefield;
                CardHasEnteredStack += player.CardHasEnteredStack;
                _players.Add(player);
            }
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                // Determine Turn Order
                ShufflePlayers();
                _activePlayer = _players.First();

                // Deal Opening Hands
                _players.ForEach(player => player.ShuffleLibrary());
                _players.ForEach(player => player.Draw(7));

                // Offer Players the option of mulliganing
                OfferMulligans();

                // Go to Player 1 Turn 1
                do
                {
                    _players.ForEach(player => ResetLandsPlayed(player));

                    BeginningPhase();

                    MainPhase(true);

                    CombatPhase();

                    MainPhase(false);

                    EndingPhase();

                    _activePlayer = _nextPlayer;
                } while (true);
            });
        }

        public void ChangeZone(Card card, Zone newZone)
        {
            card.Controller.Hand.RemoveAll(c => c == card);
            card.Controller.Battlefield.RemoveAll(c => c == card);
            card.Controller.Graveyard.RemoveAll(c => c == card);
            card.Controller.Exile.RemoveAll(c => c == card);

            switch (newZone)
            {
                case Zone.Stack:
                    Stack.Push(card);
                    break;
                case Zone.Battlefield:
                    card.Controller.Battlefield.Add(card);
                    break;
                case Zone.CommandZone:
                    card.Controller.CommandZone.Add(card);
                    break;
                case Zone.Exile:
                    card.Owner.Exile.Add(card);
                    break;
                case Zone.Graveyard:
                    card.Owner.Graveyard.Add(card);
                    break;
                case Zone.Hand:
                    card.Owner.Hand.Add(card);
                    break;
            }
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

            UpkeepStep();

            DrawStep();
        }

        private void UntapStep()
        {
            CurrentStepHasChanged("Untap Step");
            _activePlayer.Battlefield.ForEach(c => c.Untap());

            DrainManaPools();
        }

        private void UpkeepStep()
        {
            CurrentStepHasChanged("Upkeep Step");

            // TODO: Add Upkeep Triggers to the stack in ApNap order according to their controllers
            ApNapLoop(_activePlayer, false);

            // Drain each player's mana pool
            DrainManaPools();
        }

        private void DrawStep()
        {
            CurrentStepHasChanged("Draw Step");
            // TODO: Add Beginning of Draw Step Triggers to the Stack

            _activePlayer.Draw(1);

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
            ApNapLoop(_activePlayer, true);

            DrainManaPools();
        }

        private void CombatPhase()
        {
            BeginningOfCombatStep();

            // If the active player controls no creatures without summoning sickness, is this really necessary?
            DeclareAttackersStep();

            // If Attackers were declared
            if (_activePlayer.Battlefield.Any(card => card is CreatureCard && (card as CreatureCard).IsAttacking))
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

            ApNapLoop(_activePlayer, false);

            DrainManaPools();
        }

        private void DeclareAttackersStep()
        {
            CurrentStepHasChanged("Declare Attackers Step");
            // TODO: Ask the Active Player to declare attackers
            var attackers = _activePlayer.DeclareAttackers();
            if (attackers != null)
            {
                foreach (var declaration in attackers)
                {
                    declaration.AttackingCreature.DefendingPlayer = declaration.DefendingPlayer;
                }
            }

            // TODO: Any "When this creature attacks" triggers get put onto the stack

            ApNapLoop(_activePlayer, false);

            DrainManaPools();
        }

        private void DeclareBlockersStep()
        {
            CurrentStepHasChanged("Declare Blockers Step");

            // TODO: Ask the Defending Players, in ApNap order, to declare Blockers
            var defendingPlayers = _players.StartAt(_activePlayer)
                .Where(p => _activePlayer.Battlefield.Creatures.Any(c => c.DefendingPlayer == p));
            // Iterate over the defending players in turn order
            foreach(var player in defendingPlayers)
            {
                var blockers = player.DeclareBlockers(_activePlayer.Battlefield.Creatures.Where(c => c.DefendingPlayer == player));
                if(blockers != null)
                {
                    foreach(var blocker in blockers)
                    {
                        blocker.Blocker.Blocking = blocker.Attacker;
                    }
                }
            }

            ApNapLoop(_activePlayer, false);

            DrainManaPools();
        }

        private void DamageStep()
        {
            CurrentStepHasChanged("Damage Step");

            // If any attackers have firststrike or doublestrike
            if (false)
            {
                // TODO: Deal First Strike Damage

                CheckStateBasedActions();
            }

            // If any remaining attackers have doublestrike or don't have firststrike
            if (_activePlayer.Battlefield.Creatures.Any(c => c.IsAttacking))
            {
                // TODO: Deal Regular Damage

                CheckStateBasedActions();
            }

            DrainManaPools();
        }

        private void EndOfCombatStep()
        {
            CurrentStepHasChanged("End of Combat Step");

            // Give Priority to Players
            ApNapLoop(_activePlayer, false);

            // TODO: Add any "End of Combat" triggers to the stack

            // Remove all creatures from combat
            foreach (var creature in _activePlayer.Battlefield.Creatures)
                creature.DefendingPlayer = null;

            DrainManaPools();
        }

        private void EndingPhase()
        {
            EndStep();

            CleanupStep();
        }

        private void EndStep()
        {
            CurrentStepHasChanged("End Step");
            // TODO: Add EndStep Triggers to the Stack in ApNap Order
            ApNapLoop(_activePlayer, false);

            DrainManaPools();
        }

        private void CleanupStep()
        {
            CurrentStepHasChanged("Cleanup Step");
            // TODO: Discard down to Maximum Hand Size
            _activePlayer.DiscardToHandSize();

            // TODO: Remove Marked Damage from Permanents, and "Until End of Turn" and "This Turn" effects go away

            // TODO: If there are triggered abilities on the stack then players may receive priority, in ApNap order, to react to them before the stack resolves.
            // Example: Madness is a triggered ability that happens when a player discards a card with Madness. This can potentially be put on the stack from the First part of the Cleanup step

            DrainManaPools();
        }

        private void ApNapLoop(Player startingPlayer, bool startingPlayerCanCastSorceries)
        {
            // Idea: This might need to be put in a separate class, a Stack Resolver

            // TODO: Loop through all players, starting with startingPlayer, and ask them if they would like to take an action.
            // If the player responds PassPriority, then go to the next player.
            // If the player responds with a legal action that doesn't go on the stack (playing a land during main phase) then perform the action and ask again.
            // If the player responds with a legal action that does go on the stack (activating an ability of a permanent, or casting a spell), then put it on 
            //   the stack and begin another ApNap loop starting with the next player in turn order
            // If the player responds with an illegal action then inform the user of why the action is illegal, and ask again

            // This function returns when the stack is empty and all players have Passed Priority since the last spell resolved
            foreach(var player in _players.StartAt(startingPlayer))
            {
                // TODO: Give Priority
                _priorityPlayer = player;

                bool playerHasPassedPriority = false;
                do
                {
                    var chosenAction = player.GivePriority(_activePlayer, player == startingPlayer && Stack.Count == 0 && startingPlayerCanCastSorceries);

                    switch (chosenAction.ActionType)
                    {
                        case ActionType.PassPriority:
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
                                        CardHasEnteredBattlefield?.Invoke(action.Card);
                                    }
                                    else
                                    {
                                        // Player can't play a land right now
                                    }
                                }
                                else
                                {
                                    // TODO : Make player pay the cost of the Card
                                    // TODO : Put the Card onto the stack
                                    if (action.Card.Cost.Pay())
                                    {
                                        player.Hand.Remove(action.Card);
                                        Stack.Push(action.Card);
                                        CardHasEnteredStack?.Invoke(action.Card);
                                        ApNapLoop(_players[(_players.IndexOf(player) + 1) % _players.Count], false);

                                        // If we made it back with no responses, resolve the spell
                                        if (Stack.Count > 0)
                                        {
                                            var card = Stack.Pop();
                                            if(card is PermanentCard)
                                            {
                                                card.OnResolve(this);
                                                card.Controller.Battlefield.Add(card);
                                                CardHasEnteredBattlefield?.Invoke(card);
                                            }
                                            else if(card is SpellCard)
                                            {
                                                card.OnResolve(this);
                                            }
                                        }
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
                                        manaAbility.Cost.Pay();
                                        manaAbility.OnResolve(this);
                                    }
                                }
                                else
                                {
                                    // TODO : Put the Ability onto the stack
                                    ApNapLoop(_players[(_players.IndexOf(player) + 1) % _players.Count], false);
                                }
                            }
                            break;
                    }
                } while (!playerHasPassedPriority);
            }
        }

        private void CheckStateBasedActions()
        {
            // TODO: Kill any creatures that have 0 toughness, or have sustained enough damage to be destroyed and don't have indestructible

            // TODO: Any players with 0 life total lose the game

            // TODO: If an effect has caused a player to win the game, all other players lose

            // TODO: If a player has lost the game, remove them from the _players list. If they were the active player, go to the start of the next player's turn
        }

        /// <summary>
        /// Play the specified Land card
        /// </summary>
        /// <param name="card"></param>
        private void PlayLand(LandCard card)
        {
            // Remove the card from the player's hand and place it on the battlefield
            card.Controller.Hand.Remove(card);
            card.Controller.Battlefield.Add(card);

            // Add any ETB triggers to the stack
            ChangeZone(card, Zone.Battlefield);
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
            player.MaxLandsPlayedThisTurn = (player == _activePlayer ? 1 : 0);
        }

        /// <summary>
        /// Resets each player's mana pool to zero
        /// </summary>
        private void DrainManaPools()
        {
            _players.ForEach(c => c.ManaPool.Clear());
        }

        #endregion Utility Methods
    }
}
