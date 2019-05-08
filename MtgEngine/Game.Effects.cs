using MtgEngine.Common;
using MtgEngine.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine
{
    public partial class Game
    {
        /// <summary>
        /// This method removes a spell from the stack and puts it in its owner's graveyard. If the given spell is not on the stack, then this effect does nothing.
        /// </summary>
        /// <param name="card">This spell is the spell that is to be countered.</param>
        public void Counter(IResolvable obj)
        {
            if (Stack.Contains(obj))
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
                    if (topmost is Card)
                    {
                        (topmost as Card).Owner.Graveyard.Add(topmost as Card);
                        CardHasChangedZones?.Invoke(this, topmost as Card, Common.Enums.Zone.Stack, Common.Enums.Zone.Graveyard);
                    }
                }
            }
            CheckStateBasedActions();
        }

        /// <summary>
        /// This spell destroys all lands that meet the selector's requirement
        /// </summary>
        /// <param name="selector"></param>
        public void DestroyLands(Func<PermanentCard, bool> selector)
        {
            List<PermanentCard> landsDestroyed = new List<PermanentCard>();
            foreach (var player in _players)
            {
                foreach (var land in player.Battlefield.Lands.Where(c => selector(c)).ToList())
                {
                    landsDestroyed.Add(land);
                }
            }

            foreach(var land in landsDestroyed)
            {
                DestroyPermanent(land);
            }

            // TODO: Add Land On LeaveBattlefield or OnDestroy triggers to the stack in order
        }

        public void DestroyPermanent(PermanentCard card)
        {
            if (card.Controller.Battlefield.Contains(card) && !card.HasIndestructible)
            {
                MoveFromBattlefieldToGraveyard(card);
            }
        }
    }
}
