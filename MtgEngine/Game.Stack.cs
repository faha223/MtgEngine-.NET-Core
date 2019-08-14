using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Effects;
using MtgEngine.Common.Players;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine
{
    public partial class Game
    {
        public delegate void PlayerGainedPriorityEvent(Game game, Player player);
        public event PlayerGainedPriorityEvent PlayerGainedPriority;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startingPlayer"></param>
        /// <param name="startingPlayerCanCastSorceries"></param>
        private void ApNapLoop(bool mainPhase)
        {
            bool repeatLoop;
            do
            {
                var playersThatHavePassedPriority = new List<Player>(_players.Count);

                // Make a queue of players starting at startingPlayer. This will allow us to enqueue all the players that have passed priority in case someone after them put something on the stack
                var priorityQueue = new Queue<Player>(_players.StartAt(ActivePlayer));
                repeatLoop = false;

                while(priorityQueue.Count > 0)
                {
                    var player = priorityQueue.Dequeue();
                    
                    // Give Priority
                    _priorityPlayer = player;
                    PlayerGainedPriority?.Invoke(this, player);

                    while (!playersThatHavePassedPriority.Contains(player))
                    {
                        // The current player can only play sorcery-speed spells if the current player is the Active Player, and there is nothing on the stack, and it is currently their main phase
                        bool playerCanCastSorceries = (player == ActivePlayer) && (Stack.Count == 0) && mainPhase;

                        var chosenAction = player.GivePriority(this, playerCanCastSorceries);

                        switch (chosenAction.ActionType)
                        {
                            case ActionType.PassPriority:
                                // This player passed priority, add them to the list
                                playersThatHavePassedPriority.Add(player);
                                break;
                            case ActionType.PlayCard:
                                {
                                    var action = chosenAction as PlayCardAction;
                                    PlayCard(action, player, playerCanCastSorceries);

                                    // If this player put something on the stack, we'll need to make sure all the players that have already passed priority are put back into the queue
                                    // HINT: Playing a Land doesn't use the stack
                                    if (action.Card.UsesStack && playersThatHavePassedPriority.Count > 0)
                                    {
                                        foreach (var other in playersThatHavePassedPriority)
                                            priorityQueue.Enqueue(other);
                                        playersThatHavePassedPriority.Clear();
                                    }
                                }
                                break;
                            case ActionType.ActivateAbility:
                                {
                                    var action = chosenAction as ActivateAbilityAction;
                                    ActivateAbility(action, player, playerCanCastSorceries);

                                    // If this player put something on the stack, we'll need to make sure all the players that have already passed priority are put back into the queue
                                    // HINT: Mana Abilties don't use the stack
                                    if (!(action.Ability is ManaAbility) && playersThatHavePassedPriority.Count > 0)
                                    {
                                        foreach (var other in playersThatHavePassedPriority)
                                            priorityQueue.Enqueue(other);
                                        playersThatHavePassedPriority.Clear();
                                    }
                                }
                                break;
                        }
                    }
                }

                // All players have passed priority. Resolve the top of the stack, if necessary.
                if (Stack.Count > 0)
                {
                    ResolveTopOfStack();
                    repeatLoop = true;
                }

            } while (repeatLoop);
        }

        /// <summary>
        /// Play the specified Land card
        /// </summary>
        /// <param name="card"></param>
        private void PlayLand(Card card)
        {
            // Remove the card from the player's hand and place it on the battlefield
            card.Controller.Hand.Remove(card);
            PutPermanentOnBattlefield(card);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="player"></param>
        /// <param name="playerCanCastSorceries"></param>
        private void PlayCard(PlayCardAction action, Player player, bool playerCanCastSorceries)
        {
            // If it's a land card, it bypasses the stack
            if (action.Card.IsALand)
            {
                // Only play the land if the player is allowed to play a turn right now
                if (playerCanCastSorceries && player.LandsPlayedThisTurn < player.MaxLandsPlayedThisTurn)
                {
                    PlayLand(action.Card);
                    player.LandsPlayedThisTurn++;
                    CardHasChangedZones?.Invoke(this, action.Card, Common.Enums.Zone.Stack, Common.Enums.Zone.Battlefield);
                }
            }
            else
            {
                // Make player pay the cost of the Card
                if (action.Card.Cost.Pay())
                {
                    // Put the Card onto the stack
                    action.Card.OnCast?.Invoke(this);
                    player.Hand.Remove(action.Card);
                    PushOntoStack(action.Card, Common.Enums.Zone.Hand);
                }
            }

            CheckStateBasedActions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="player"></param>
        /// <param name="playerCanCastSorceries"></param>
        private void ActivateAbility(ActivateAbilityAction action, Player player, bool playerCanCastSorceries)
        {
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
                if(activatedAbility is ITargeting)
                {
                    // This ability needs a target, get one now
                    (activatedAbility as ITargeting).SelectTargets(this);
                }
                if (activatedAbility.Cost.CanPay())
                {
                    if (activatedAbility.Cost.Pay())
                    {
                        PushOntoStack(activatedAbility, Common.Enums.Zone.Exile);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolvable"></param>
        private void PushOntoStack(IResolvable resolvable, Common.Enums.Zone previousZone)
        {
            Stack.Push(resolvable);
            if (resolvable is Card)
                CardHasChangedZones(this, resolvable as Card, previousZone, Common.Enums.Zone.Stack);
            else if (resolvable is Ability)
                AbilityHasEnteredStack?.Invoke(this, resolvable as Ability);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permanent"></param>
        public void PutPermanentOnBattlefield(Card permanent)
        {
            permanent.Controller.Battlefield.Add(permanent);

            // Whenever a permanent enters the battlefield, it gets summoning sickness. This only affects whether creatures can attack or activate abilities that require them to tap/untap
            permanent.HasSummoningSickness = true;

            SubscribeToEvents(permanent);

            CardHasChangedZones?.Invoke(this, permanent, Common.Enums.Zone.Stack, Common.Enums.Zone.Battlefield);
        }

        public void RemovePermanentFromBattlefield(Card permanent)
        {
            permanent.Controller.Battlefield.Remove(permanent);
            UnsubscribeFromEvents(permanent);
        }

        private void SubscribeToEvents(Card card)
        {
            card.CountersCreated += permanent_CountersCreated;
            card.CountersRemoved += permanent_CountersRemoved;
            card.TookDamage += creature_tookDamage;

            foreach (EventTriggeredAbility ability in card.AbilitiesAfterModifiersApplied.Where(c => c is EventTriggeredAbility))
            {
                PlayerTookDamage += ability.PlayerTookDamage;
                CreatureTookDamage += ability.CreatureTookDamage;
                CardHasChangedZones += ability.CardHasChangedZones;
                CountersAddedToPermanent += ability.PermanentGotCounter;
                CountersAddedToPlayer += ability.PlayerGotCounter;
                PlayerDrewCards += ability.PlayerDrewCards;
                AttackerDeclared += ability.AttackerDeclared;
                BlockerDeclared += ability.BlockerDeclared;
            }
        }

        private void UnsubscribeFromEvents(Card card)
        {
            card.CountersCreated -= permanent_CountersCreated;
            card.CountersRemoved -= permanent_CountersRemoved;

            foreach (EventTriggeredAbility ability in card.AbilitiesAfterModifiersApplied.Where(c => c is EventTriggeredAbility))
            {
                PlayerTookDamage -= ability.PlayerTookDamage;
                CreatureTookDamage -= ability.CreatureTookDamage;
                CardHasChangedZones -= ability.CardHasChangedZones;
                CountersAddedToPermanent -= ability.PermanentGotCounter;
                CountersAddedToPlayer -= ability.PlayerGotCounter;
                PlayerDrewCards -= ability.PlayerDrewCards;
                AttackerDeclared -= ability.AttackerDeclared;
                BlockerDeclared -= ability.BlockerDeclared;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResolveTopOfStack()
        {
            if (Stack.Count > 0)
            {
                var obj = Stack.Pop();
                if (obj is Card)
                {
                    var card = obj as Card;
                    card.OnResolve(this);

                    if (card.IsAPermanent)
                        PutPermanentOnBattlefield(card);
                    else if (card.IsASpell)
                        card.Owner.Graveyard.Add(card);
                }
                else if (obj is Ability)
                {
                    var ability = obj as Ability;
                    ability.OnResolve(this);
                }
            }

            CheckStateBasedActions();
        }
    }
}
