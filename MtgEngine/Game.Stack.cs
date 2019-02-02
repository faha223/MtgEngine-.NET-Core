using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Utilities;
using System.Linq;

namespace MtgEngine
{
    public partial class Game
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startingPlayer"></param>
        /// <param name="startingPlayerCanCastSorceries"></param>
        private void ApNapLoop(Player startingPlayer, bool startingPlayerCanCastSorceries)
        {
            bool repeatLoop;
            do
            {
                repeatLoop = false;
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
                                break;
                            case ActionType.PlayCard:
                                PlayCard(chosenAction as PlayCardAction, player, player == startingPlayer && startingPlayerCanCastSorceries);
                                break;
                            case ActionType.ActivateAbility:
                                ActivateAbility(chosenAction as ActivateAbilityAction, player, player == startingPlayer && startingPlayerCanCastSorceries);
                                break;
                        }
                    } while (!playerHasPassedPriority);
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
        private void PlayLand(LandCard card)
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
            if (action.Card is LandCard)
            {
                // Only play the land if the player is allowed to play a turn right now
                if (playerCanCastSorceries && player.LandsPlayedThisTurn < player.MaxLandsPlayedThisTurn)
                {
                    PlayLand(action.Card as LandCard);
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
                    action.Card.OnCast(this);
                    player.Hand.Remove(action.Card);
                    PushOntoStack(action.Card, Common.Enums.Zone.Hand);
                }
            }
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
        public void PutPermanentOnBattlefield(PermanentCard permanent)
        {
            permanent.Controller.Battlefield.Add(permanent);
            if (permanent.IsACreature)
                permanent.HasSummoningSickness = true;

            SubscribeToEvents(permanent);

            CardHasChangedZones?.Invoke(this, permanent, Common.Enums.Zone.Stack, Common.Enums.Zone.Battlefield);
        }

        public void RemovePermanentFromBattlefield(PermanentCard permanent)
        {
            permanent.Controller.Battlefield.Remove(permanent);
            UnsubscribeFromEvents(permanent);
        }

        private void SubscribeToEvents(PermanentCard card)
        {
            card.CountersCreated += permanent_CountersCreated;
            card.CountersRemoved += permanent_CountersRemoved;

            foreach (EventTriggeredAbility ability in card.Abilities.Where(c => c is EventTriggeredAbility))
            {
                PlayerTookDamage += ability.PlayerTookDamage;
                CreatureTookDamage += ability.CreatureTookDamage;
                CardHasChangedZones += ability.CardHasChangedZones;
                CountersAddedToPermanent += ability.PermanentGotCounter;
                CountersAddedToPlayer += ability.PlayerGotCounter;
                PlayerDrewCards += ability.PlayerDrewCards;
            }
        }

        private void UnsubscribeFromEvents(PermanentCard card)
        {
            card.CountersCreated -= permanent_CountersCreated;
            card.CountersRemoved -= permanent_CountersRemoved;

            foreach (EventTriggeredAbility ability in card.Abilities.Where(c => c is EventTriggeredAbility))
            {
                PlayerTookDamage -= ability.PlayerTookDamage;
                CreatureTookDamage -= ability.CreatureTookDamage;
                CardHasChangedZones -= ability.CardHasChangedZones;
                CountersAddedToPermanent -= ability.PermanentGotCounter;
                CountersAddedToPlayer -= ability.PlayerGotCounter;
                PlayerDrewCards -= ability.PlayerDrewCards;
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
            if (Stack.Count > 0)
                ApNapLoop(ActivePlayer, false);
        }
    }
}
