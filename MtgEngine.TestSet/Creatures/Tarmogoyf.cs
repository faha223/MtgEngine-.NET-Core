using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Tarmogoyf", "TestSet", "", "", "Tarmogoyf's power is equal to the number of card types among cards in all graveyards and its toughness is equal to that number plus 1.")]
    public class Tarmogoyf : CardSource
    {
        public override Card GetCard(Player owner)
        {
            Func<Game, Card, int> basePowerFunc = (g, c) =>
            {
                var cardTypes = new List<CardType>();
                foreach (var graveyard in g.Players().Select(p => p.Graveyard))
                {
                    foreach (var c2 in graveyard)
                    {
                        foreach (var cardType in c2.Types)
                            if (!cardTypes.Contains(cardType))
                                cardTypes.Add(cardType);
                    }
                }
                return cardTypes.Count;
            };

            Func<Game, Card, int> baseToughnessFunc = (g, c) =>
            {
                return 1 + c.BasePowerFunc(g, c);
            };

            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Lhurgoyf" }, false, basePowerFunc, baseToughnessFunc, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
            return card;
        }
    }
}
