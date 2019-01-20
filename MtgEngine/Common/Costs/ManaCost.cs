using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtgEngine.Common.Costs
{
    // TODO: Make constructors private, add a static method that parses mana cost
    public class ManaCost : Cost
    {
        private ManaAmount[] _manaAmounts;

        public delegate void IntegerEvent(int X);
        public event IntegerEvent ValueforXChosen;

        private Player _controller
        {
            get
            {
                Player controller = null;
                if (_source is ActivatedAbility)
                    controller = (_source as ActivatedAbility).Controller;
                else if (_source is Card)
                    controller = (_source as Card).Controller;
                return controller;
            }
        }

        private ManaCost(IResolvable source, params ManaAmount[] manaAmounts) : base(source)
        {
            _manaAmounts = manaAmounts;
        }

        /// <summary>
        /// Returns the Converted mana cost
        /// </summary>
        /// <returns></returns>
        public int Convert()
        {
            return _manaAmounts.Sum(c => c.Convert());
        }

        public static Cost Parse(IResolvable source, string manaCost)
        {
            if (manaCost == "{1}{G}")
                return new ManaCost(source, new ManaAmount(1, ManaColor.Generic), new ManaAmount(1, ManaColor.Green));
            else if (manaCost == "{X}{X}{X}")
                return new ManaCost(source, new ManaAmount(3, ManaColor.GenericX));
            else if (manaCost == "{U}{U}")
                return new ManaCost(source, new ManaAmount(2, ManaColor.Blue));
            else if (manaCost == "{W}{U}{B}{R}{G}")
                return new ManaCost(source, new ManaAmount(1, ManaColor.White), new ManaAmount(1, ManaColor.Blue), new ManaAmount(1, ManaColor.Black), new ManaAmount(1, ManaColor.Red), new ManaAmount(1, ManaColor.Green));

            var manaAmounts = ManaParser.Parse(manaCost);
            if (manaAmounts == null)
                return new NoCost(source);
            return new ManaCost(source, manaAmounts);
        }

        public override bool CanPay()
        {
            // TODO: Think of a better way...?
            return true;
        }

        public override bool Pay()
        {
            var controller = _controller;
            if (controller == null)
                return false;

            var temp = new List<ManaAmount>(_manaAmounts.Select(c => new ManaAmount(c.Amount, c.Color)));
            if(temp.Any(c => c.Color == ManaColor.GenericX))
            {
                int x = controller.GetValueForX(stringify(temp));
                ValueforXChosen?.Invoke(x);
                var amt = temp.Where(c => c.Color == ManaColor.GenericX).Sum(c => c.Amount);
                temp.RemoveAll(c => c.Color == ManaColor.GenericX);
                if(temp.Any(c => c.Color == ManaColor.Generic))
                {
                    var generic = temp.First(c => c.Color == ManaColor.Generic);
                    temp.Remove(generic);
                    temp.Add(new ManaAmount(generic.Amount + (amt * x), ManaColor.Generic));
                }
                else
                {
                    temp.Add(new ManaAmount(amt * x, ManaColor.Generic));
                }
            }
            List<ManaColor> manaPaid = new List<ManaColor>();
            while(temp.Count > 0)
            {
                var colorPaid = controller.PayManaCost(stringify(temp));
                if (!colorPaid.HasValue)
                {
                    // If the player cancelled, return the paid mana to their mana pool
                    foreach(var mana in manaPaid)
                    {
                        controller.ManaPool.Add(new ManaAmount(1, mana));
                    }
                    return false;
                }

                manaPaid.Add(colorPaid.Value);

                switch(colorPaid)
                {
                    case ManaColor.White:
                    case ManaColor.Blue:
                    case ManaColor.Black:
                    case ManaColor.Red:
                    case ManaColor.Green:
                    case ManaColor.Colorless:
                        ManaAmount amount = null;
                        if (temp.Any(c => c.Color == colorPaid))
                            amount = temp.First(c => c.Color == colorPaid);
                        else if (temp.Any(c => c.Color == ManaColor.Generic))
                            amount = temp.First(c => c.Color == ManaColor.Generic);
                            
                        temp.Remove(amount);
                        if (amount.Amount == 1)
                            continue;
                        temp.Add(new ManaAmount(amount.Amount - 1, amount.Color));
                        break;
                }
            }
            return true;
        }

        string stringify(IEnumerable<ManaAmount> cost)
        {
            var sb = new StringBuilder();
            foreach (var amount in cost)
                sb.Append(amount.ToString());
            return sb.ToString();
        }

        public override string ToString()
        {
            return stringify(_manaAmounts);
        }

        private ManaAmount[] _costSortedByColorPriority()
        {
            var colorPriority = new List<ManaColor>(new ManaColor[] {
                ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green, ManaColor.Colorless,
                ManaColor.WhiteBlue, ManaColor.WhiteBlack, ManaColor.BlueBlack, ManaColor.BlueRed, ManaColor.BlackRed,
                ManaColor.BlackGreen, ManaColor.RedGreen, ManaColor.RedWhite, ManaColor.GreenWhite, ManaColor.GreenBlue,
                ManaColor.TwoOrWhite, ManaColor.TwoOrBlue, ManaColor.TwoOrBlack, ManaColor.TwoOrRed, ManaColor.TwoOrGreen,
                ManaColor.PhyrexianWhite, ManaColor.PhyrexianBlue, ManaColor.PhyrexianBlack, ManaColor.PhyrexianRed, ManaColor.PhyrexianGreen,
                ManaColor.Generic
            });

            return _manaAmounts.OrderBy(c => colorPriority.IndexOf(c.Color)).ToArray();
        }
    }
}
