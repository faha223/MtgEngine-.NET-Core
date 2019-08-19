using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card
    {
        Card CreateATokenCopy(Player owner)
        {
            var copy = new Card(owner, this, true);

            // The token copy is the same as the original, except it's a token
            var originalTypes = new List<CardType>(_types);
            originalTypes.Add(CardType.Token);
            copy._types = originalTypes.ToArray();

            foreach(var ability in _abilities)
                copy.AddAbility(ability.Copy(copy));

            foreach (var ability in _staticAbilities)
                copy._staticAbilities.Add(ability);

            foreach(var effect in _effects)
            {
            }
            
            return copy;
        }
    }
}
