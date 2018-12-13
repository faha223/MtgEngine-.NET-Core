using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;

namespace MtgEngine
{
    public class Game
    {
        private List<Player> _players { get; } = new List<Player>();

        private Player _activePlayer { get; set; }

        private Player _priorityPlayer { get; set; }

        private Stack<Card> Stack { get; } = new Stack<Card>();

        public void AddPlayer(Player player)
        {
            if (_players.Contains(player))
                _players.Add(player);
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                _players.ForEach(player => player.ShuffleLibrary());
                _players.ForEach(player => player.Draw(7));

                // TODO: Mulligans

                do
                {
                    BeginningPhase();

                    MainPhase(true);

                    CombatPhase();

                    MainPhase(false);

                    EndingPhase();
                } while (true);
            });
        }

        internal void ChangeZone(Card card, Zone newZone)
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

        internal void BeginningPhase()
        {
            UntapStep();

            UpkeepStep();

            DrawStep();
        }

        internal void UntapStep()
        {
            _activePlayer.Battlefield.ForEach(c => c.Untap());
        }

        internal void UpkeepStep()
        {
            // TODO: Add Upkeep Triggers to the stack in ApNap order according to their controllers
        }

        internal void DrawStep()
        {
            // TODO: Add Beginning of Draw Step Triggers to the Stack

            _activePlayer.Draw(1);
        }

        internal void MainPhase(bool beforeCombat)
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
        }

        internal void CombatPhase()
        {
            BeginningOfCombatStep();

            DeclareAttackersStep();

            DeclareBlockersStep();

            DamageStep();

            EndOfCombatStep();
        }

        internal void BeginningOfCombatStep()
        {
            // TODO: Add Beginning of Combat Triggers to the Stack

            // TODO: Cycle Priority, do NOT give the Active Player the ability to cast Sorcery Speed spells
        }

        internal void DeclareAttackersStep()
        {
            // TODO: Ask the Active Player to declare attackers

            // TODO: Cycle Priority in ApNap order
        }

        internal void DeclareBlockersStep()
        {
            // TODO: Ask the Defending Players, in ApNap order, to declare Blockers

            // TODO: Cycle Priority in ApNap order
        }

        internal void DamageStep()
        {
            // TODO: Deal First Strike Damage

            // TODO: Check State-Based Actions

            // TODO: Deal Regular Damage

            // TODO: Check State-Based Actions

            // TODO: Check State-Based Actions
        }

        internal void EndOfCombatStep()
        {
            // TODO: Cycle Priority in ApNap order
        }

        internal void EndingPhase()
        {
            EndStep();

            CleanupStep();
        }

        internal void EndStep()
        {
            // TODO: Add EndStep Triggers to the Stack in ApNap Order
        }

        internal void CleanupStep()
        {
            // TODO: Discard down to Maximum Hand Size

            // TODO: Remove Marked Damage from Permanents, and "Until End of Turn" and "This Turn" effects go away

            // TODO: If there are triggered abilities on the stack then players may receive priority, in ApNap order, to react to them before the stack resolves.
            // Example: Madness is a triggered ability that happens when a player discards a card with Madness. This can potentially be put on the stack from the First part of the Cleanup step
        }

        internal void ApNapLoop(Player startingPlayer, bool startingPlayerCanCastSorceries)
        {
            // Idea: This might need to be put in a separate class, a Stack Resolver

            // TODO: Loop through all players, starting with startingPlayer, and ask them if they would like to take an action.
            // If the player responds PassPriority, then go to the next player.
            // If the player responds with a legal action that doesn't go on the stack (playing a land during main phase) then perform the action and ask again.
            // If the player responds with a legal action that does go on the stack (activating an ability of a permanent, or casting a spell), then put it on 
            //   the stack and begin another ApNap loop starting with the next player in turn order
            // If the player responds with an illegal action then inform the user of why the action is illegal, and ask again

            // This function returns when the stack is empty and all players have Passed Priority since the last spell resolved
        }
    }
}
