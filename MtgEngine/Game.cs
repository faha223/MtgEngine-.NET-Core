using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
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

        public void AddPlayer(Player player)
        {
            if (!_players.Contains(player))
                _players.Add(player);
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                // TODO: Determine Turn Order

                ShufflePlayers();
                _activePlayer = _players.First();

                _players.ForEach(player => player.ShuffleLibrary());
                _players.ForEach(player => player.Draw(7));

                // TODO: Mulligans

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

        private void BeginningPhase()
        {
            UntapStep();

            UpkeepStep();

            DrawStep();
        }

        private void UntapStep()
        {
            _activePlayer.Battlefield.ForEach(c => c.Untap());
        }

        private void UpkeepStep()
        {
            // TODO: Add Upkeep Triggers to the stack in ApNap order according to their controllers
        }

        private void DrawStep()
        {
            // TODO: Add Beginning of Draw Step Triggers to the Stack

            _activePlayer.Draw(1);
        }

        private void MainPhase(bool beforeCombat)
        {
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
        }

        private void CombatPhase()
        {
            BeginningOfCombatStep();

            // If the active player controls no creatures without summoning sickness, is this really necessary?
            DeclareAttackersStep();

            // If Attackers were declared
            if (true)
            {
                DeclareBlockersStep();

                DamageStep();
            }

            // TODO : If there are no attackers, is this really necessary?
            EndOfCombatStep();
        }

        private void BeginningOfCombatStep()
        {
            // TODO: Add Beginning of Combat Triggers to the Stack

            // TODO: Cycle Priority, do NOT give the Active Player the ability to cast Sorcery Speed spells
        }

        private void DeclareAttackersStep()
        {
            // TODO: Ask the Active Player to declare attackers

            // TODO: Cycle Priority in ApNap order
        }

        private void DeclareBlockersStep()
        {
            // TODO: Ask the Defending Players, in ApNap order, to declare Blockers

            // TODO: Cycle Priority in ApNap order
        }

        private void DamageStep()
        {
            // If any attackers have firststrike or doublestrike
            if(false)
            {
                // TODO: Deal First Strike Damage

                CheckStateBasedActions();
            }

            // If any remaining attackers have doublestrike or don't have firststrike
            if (false)
            {
                // TODO: Deal Regular Damage

                CheckStateBasedActions();
            }
        }

        private void EndOfCombatStep()
        {
            // TODO: Cycle Priority in ApNap order
        }

        private void EndingPhase()
        {
            EndStep();

            CleanupStep();
        }

        private void EndStep()
        {
            // TODO: Add EndStep Triggers to the Stack in ApNap Order
        }

        private void CleanupStep()
        {
            // TODO: Discard down to Maximum Hand Size

            // TODO: Remove Marked Damage from Permanents, and "Until End of Turn" and "This Turn" effects go away

            // TODO: If there are triggered abilities on the stack then players may receive priority, in ApNap order, to react to them before the stack resolves.
            // Example: Madness is a triggered ability that happens when a player discards a card with Madness. This can potentially be put on the stack from the First part of the Cleanup step
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
                var chosenAction = player.GivePriority(_activePlayer, player == startingPlayer && Stack.Count == 0 && startingPlayerCanCastSorceries);

                bool playerHasPassedPriority = false;
                while (!playerHasPassedPriority)
                {
                    switch (chosenAction.ActionType)
                    {
                        case ActionType.PassPriority:
                            playerHasPassedPriority = true;
                            continue;
                        case ActionType.PlayCard:
                            {
                                var action = chosenAction as PlayCardAction;
                                if (action.Card is LandCard)
                                {
                                    if (player == startingPlayer && startingPlayerCanCastSorceries && player.LandsPlayedThisTurn < player.MaxLandsPlayedThisTurn)
                                    {
                                        PlayLand(action.Card as LandCard);
                                        player.LandsPlayedThisTurn++;
                                    }
                                }
                                else
                                {
                                    // TODO : Make player pay the cost of the Card
                                    // TODO : Put the Card onto the stack
                                    ApNapLoop(_players[_players.IndexOf(player) + 1 % _players.Count], false);
                                }
                            }
                            break;
                        case ActionType.ActivateAbility:
                            {
                                var action = chosenAction as ActivateAbilityAction;
                                var ability = action.Ability as ActivatedAbility;
                                // TODO : Make player pay the cost of the Ability
                                if (action.Ability is ManaAbility)
                                {
                                    (action.Ability as ManaAbility).OnResolve(this);
                                }
                                else
                                {
                                    // TODO : Put the Ability onto the stack
                                }


                                ApNapLoop(_players[_players.IndexOf(player) + 1 % _players.Count], false);
                            }
                            break;
                    }
                }
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
            player.Library.AddRange(player.Hand);
            player.Hand.Clear();
            player.ShuffleLibrary();

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

        #endregion Utility Methods
    }
}
