using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Effects;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Exceptions;
using MtgEngine.Common.Players;

namespace MtgEngine
{
    public partial class Game
    {
        private List<Player> _players { get; } = new List<Player>();
        private List<Player> _losingPlayers { get; } = new List<Player>();

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

        delegate void BasicGameEvent(Game game);
        private event BasicGameEvent GameEndedInDraw;

        delegate void CardChangedZonesEvent(Game game, Card card, Common.Enums.Zone oldZone, Common.Enums.Zone newZone);
        private event CardChangedZonesEvent CardHasChangedZones;

        delegate void AbilityEvent(Game game, Ability ability);
        private event AbilityEvent AbilityHasEnteredStack;

        delegate void GameStepEvent(Game game, string step);
        private event GameStepEvent CurrentStepHasChanged;

        delegate void PlayerDrewCardEvent(Game game, Player player, int cardsDrawn);
        private event PlayerDrewCardEvent PlayerDrewCards;

        delegate void PlayerLostTheGameEvent(Game game, Player player, string reason);
        private event PlayerLostTheGameEvent PlayerLostTheGame;
        private event PlayerLostTheGameEvent PlayerWonTheGame;

        public void AddPlayer(Player player)
        {
            if (!_players.Contains(player))
            {
                player.Game = this;
                player.TookDamage += player_tookDamage;
                player.CountersCreated += player_CountersCreated;
                player.CountersRemoved += player_CountersRemoved;
                
                CurrentStepHasChanged += player.GameStepChanged;
                CardHasChangedZones += player.CardHasChangedZones;
                AbilityHasEnteredStack += player.AbilityHasEnteredStack;
                PlayerWonTheGame += player.PlayerWonTheGame;
                PlayerLostTheGame += player.PlayerLostTheGame;
                GameEndedInDraw += player.GameEndedInDraw;
                CountersAddedToPlayer += player.CountersAddedToPlayer;
                _players.Add(player);
            }
        }

        public delegate void PlayerCountersEvent(Game game, Player player, CounterType counterType, int amount);
        public event PlayerCountersEvent CountersAddedToPlayer;
        public event PlayerCountersEvent CountersRemovedFromPlayer;

        private void player_CountersCreated(Player player, CounterType counterType, int amount)
        {
            CountersAddedToPlayer?.Invoke(this, player, counterType, amount);
        }

        private void player_CountersRemoved(Player player, CounterType counterType, int amount)
        {
            CountersRemovedFromPlayer?.Invoke(this, player, counterType, amount);
        }

        private void player_tookDamage(IDamageable player, Card source, int amount)
        {
            PlayerTookDamage?.Invoke(this, (Player)player, source, amount);
        }

        private void creature_tookDamage(IDamageable creature, Card source, int amount)
        {
            CreatureTookDamage?.Invoke(this, (Card)creature, source, amount);
        }

        public delegate void PermanentCountersEvent(Game game, Card card, IResolvable source, CounterType counterType, int amount);
        public event PermanentCountersEvent CountersAddedToPermanent;
        public event PermanentCountersEvent CoutnersRemovedFromPermanent;

        private void permanent_CountersCreated(Card card, IResolvable source, CounterType counterType, int amount)
        {
            CountersAddedToPermanent?.Invoke(this, card, source, counterType, amount);
        }

        private void permanent_CountersRemoved(Card card, IResolvable source, CounterType counterType, int amount)
        {
            CoutnersRemovedFromPermanent?.Invoke(this, card, source, counterType, amount);
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
                try
                {
                    do
                    {
                        try
                        {
                            _players.ForEach(player => ResetLandsPlayed(player));

                            CheckStateBasedActionsAndResolveStack();

                            BeginningPhase();

                            CheckStateBasedActionsAndResolveStack();

                            MainPhase(true);

                            CheckStateBasedActionsAndResolveStack();

                            CombatPhase();

                            CheckStateBasedActionsAndResolveStack();

                            MainPhase(false);

                            CheckStateBasedActionsAndResolveStack();

                            EndingPhase();

                            ActivePlayer = _nextPlayer;
                        }
                        catch (ActivePlayerLostTheGameException)
                        {
                            var temp = ActivePlayer;
                            ActivePlayer = _nextPlayer;

                            _players.Remove(temp);
                            _losingPlayers.Add(temp);
                        }
                    } while (true);
                }
                catch(GameEndedInDrawException)
                {
                    GameEndedInDraw?.Invoke(this);
                }
                catch(PlayerWonTheGameException ex)
                {
                    PlayerWonTheGame?.Invoke(this, ex.Winner, ex.Message);
                }
            });
        }

        private void BeginningPhase()
        {
            UntapStep();

            CheckStateBasedActions();
            if (Stack.Count > 0)
                ApNapLoop(false);

            UpkeepStep();

            CheckStateBasedActions();
            if (Stack.Count > 0)
                ApNapLoop(false);

            DrawStep();
        }

        private void UntapStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Untap Step");
            ActivePlayer.Battlefield.ForEach(c =>
            {
                c.HasSummoningSickness = false;
                if(c.UntapsDuringUntapStep())
                    c.Untap();
            });

            DrainManaPools();
        }

        private void UpkeepStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Upkeep Step");

            // TODO: Add Upkeep Triggers to the stack in ApNap order according to their controllers
            CheckForTriggeredAbilities();
            ApNapLoop(false);

            // Drain each player's mana pool
            DrainManaPools();
        }

        private void DrawStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Draw Step");
            // TODO: Add Beginning of Draw Step Triggers to the Stack

            ActivePlayer.Draw(1);
            PlayerDrewCards?.Invoke(this, ActivePlayer, 1);

            CheckStateBasedActions();

            // Cycle Priority starting with the Active Player, give only the Active Player the ability to play Sorcery-speed spells
            ApNapLoop(false);

            DrainManaPools();
        }

        private void MainPhase(bool beforeCombat)
        {
            CurrentStepHasChanged?.Invoke(this, $"{(beforeCombat ? "Precombat" : "Postcombat")} Main Phase");

            // CheckStateBasedActiont to add Beginning of Main Phase Triggers to the stack
            CheckStateBasedActions();

            // Cycle Priority starting with the Active Player, give only the Active Player the ability to play Sorcery-speed spells
            ApNapLoop(true);

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

        private void EndingPhase()
        {
            EndStep();

            CheckStateBasedActions();
            if (Stack.Count > 0)
                ApNapLoop(false);

            CleanupStep();
        }

        private void EndStep()
        {
            CurrentStepHasChanged?.Invoke(this, "End Step");
            
            CheckForTriggeredAbilities();
            ApNapLoop(false);

            DrainManaPools();
        }

        private void CleanupStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Cleanup Step");
            
            ActivePlayer.DiscardToHandSize();

            // Remove Marked Damage from Permanents, and "Until End of Turn" and "This Turn" effects go away
            foreach (var player in _players)
                foreach (var creature in player.Battlefield.Creatures)
                    creature.ResetDamage();

            // TODO: If there are triggered abilities on the stack then players may receive priority, in ApNap order, to react to them before the stack resolves.
            // Example: Madness is a triggered ability that happens when a player discards a card with Madness. This can potentially be put on the stack from the First part of the Cleanup step
            CheckStateBasedActions();
            if (Stack.Count > 0)
                ApNapLoop(false);

            DrainManaPools();
        }

        /// <summary>
        /// Check for invalid game state, and take necessary actions
        /// </summary>
        private void CheckStateBasedActions()
        {
            // Remove any effects that come from permanents that have left the battlefield
            foreach(ContinuousEffect effect in new List<Effect>(ActiveEffects))
            {
                if(effect.Source is Card)
                {
                    var card = effect.Source as Card;
                    if(card.IsAPermanent)
                    {
                        if (!Battlefield.Contains(card))
                            ActiveEffects.Remove(effect);
                    }
                }
            }

            // Add any effects from permanents that are on the battlefield
            foreach (var permanent in Battlefield)
            {
                foreach (ContinuousEffect effect in permanent.Effects)
                {
                    if (!ActiveEffects.Contains(effect))
                        ActiveEffects.Add(effect);
                }
            }

            // Kill any creatures that have 0 toughness, or have sustained enough damage to be destroyed and don't have indestructible
            foreach (var player in _players)
            {
                var deadCreatures = player.Battlefield.Creatures.Where(c => c.IsDead).ToList();
                foreach(var creature in deadCreatures)
                {
                    MoveFromBattlefieldToGraveyard(creature);
                }

                var deadPlaneswalkers = player.Battlefield.Planeswalkers.Where(c => c.IsDead).ToList();
                foreach(var planeswalker in deadPlaneswalkers)
                {
                    MoveFromBattlefieldToGraveyard(planeswalker);
                }
            }

            // Any players with 0 life total lose the game
            Dictionary<Player, string> _deadPlayers = new Dictionary<Player, string>();
            foreach (var player in _players)
            {
                if (player.LifeTotal <= 0)
                {
                    // Player has Died
                    _deadPlayers.Add(player, "Life Total has Reached Zero");
                }
            }

            // Any players that have attempted to draw from an empty library lose the game
            foreach (var player in _players)
            {
                if (player.PlayerAttemptedToDrawIntoEmptyLibrary)
                    // Player has Died
                    _deadPlayers.Add(player, "Player Attempted to Draw into an Empty Library");
            }

            // Any players with 10 or more poison counters lose the game
            foreach (var player in _players)
            {
                if (player.Counters.Count(c => c == CounterType.Poison) >= 10)
                    _deadPlayers.Add(player, "Player has accumulated 10 poison counters");
            }

            // TODO: If an effect has caused a player to win the game, all other players lose

            // If All Players Lost Simultaneously, then it's a Draw
            if (_players.Except(_deadPlayers.Keys).Count() == 0)
                throw new GameEndedInDrawException();

            // If a player has lost the game, remove them from the _players list.
            foreach (var player in _deadPlayers.Keys)
            {
                PlayerLostTheGame?.Invoke(this, player, _deadPlayers[player]);

                // Active Player gets removed after it gets changed to the next player
                if (player != ActivePlayer)
                {
                    _players.Remove(player);
                    _losingPlayers.Add(player);
                }
            }

            // If only 1 players remains, that player won the game
            if (_players.Except(_deadPlayers.Keys).Count() == 1)
                throw new PlayerWonTheGameException(_players.Except(_deadPlayers.Keys).First(), "Last Man Standing");
            
            // If the active player died, go to the start of the next player's turn
            if (_deadPlayers.ContainsKey(ActivePlayer))
                throw new ActivePlayerLostTheGameException(_deadPlayers[ActivePlayer]);

            foreach(var player in _players)
            {
                foreach(var card in player.Graveyard)
                {
                    if (card.IsAToken)
                        player.Graveyard.Remove(card);
                    else if (card.IsCopying != null)
                        card.StopCopying(card.IsCopying, card.copyingSource);
                }
            }

            // Legend Rule (players cannot control more than 1 copy of a legendary permanent with the same name
            foreach (var player in _players)
            {
                // Create a list to hold all the legends that will be removed
                var sacrificedLegends = new List<Card>();

                // While the player has duplicate legends
                while (player.Battlefield.Except(sacrificedLegends).Any(card => card.IsLegendary && player.Battlefield.Except(sacrificedLegends).Count(c => c.Name == card.Name && c.IsLegendary) > 1))
                {
                    var playerControlledLegends = player.Battlefield.Except(sacrificedLegends).Where(card => card.IsLegendary && player.Battlefield.Except(sacrificedLegends).Count(c => c.Name == card.Name && c.IsLegendary) > 1).ToList();
                    var selected = player.MakeChoice("You control duplicate legends. Choose one to sacrifice", 1, playerControlledLegends).First();
                    sacrificedLegends.Add(selected);
                }

                // If the player had to sacrifice permanents, then sacrifice them
                if (sacrificedLegends.Count > 0)
                {
                    // If the player needed to sacrifice more than 1 permanent, then they need to sort them because they enter the graveyard one at a time
                    if(sacrificedLegends.Count > 1)
                        sacrificedLegends = player.Sort("Choose the order that you wish these permanents to enter the graveyard", sacrificedLegends);
                    foreach (var permanent in sacrificedLegends)
                    {
                        MoveFromBattlefieldToGraveyard(permanent);
                    }
                }
            }

            // Check State Triggered Abilities
            CheckForTriggeredAbilities();
        }

        private void CheckStateBasedActionsAndResolveStack()
        {
            CheckStateBasedActions();
            if (Stack.Count > 0)
                ApNapLoop(false);
        }

        #region Utility Methods

        /// <summary>
        /// Check for Abilities that have been triggered by events and changes in state, and put them on the stack in the desired or required order
        /// </summary>
        private void CheckForTriggeredAbilities()
        {
            foreach (var player in _players)
            {
                foreach (var permanent in player.Battlefield)
                {
                    foreach (StateTriggeredAbility ability in permanent.AbilitiesAfterModifiersApplied.Where(c => c is StateTriggeredAbility))
                    {
                        if (ability.CheckState(this))
                        {
                            AbilitiesTriggered.Add(ability);
                        }
                    }
                }
            }
            if (AbilitiesTriggered.Count > 0)
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
                foreach (var ability in AbilitiesTriggered)
                {
                    PushOntoStack(ability, Common.Enums.Zone.Exile);
                }

                // Remove them from the queue, we don't want to accidentally add them to the stack more than once when they only triggered once.
                AbilitiesTriggered.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ability"></param>
        public void AbilityTriggered(Ability ability)
        {
            AbilitiesTriggered.Add(ability);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permanent"></param>
        public void MoveFromBattlefieldToGraveyard(Card permanent)
        {
            RemovePermanentFromBattlefield(permanent);
            permanent.ClearCounters();
            permanent.Owner.Graveyard.Add(permanent);
            CardHasChangedZones?.Invoke(this, permanent, Common.Enums.Zone.Battlefield, Common.Enums.Zone.Graveyard);
        }

        #endregion Utility Methods
    }
}
