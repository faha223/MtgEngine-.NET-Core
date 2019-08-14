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

        public override Cost Copy(IResolvable newSource)
        {
            return new ManaCost(newSource, _manaAmounts);
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

        /// <summary>
        /// Returns the color identity computed based on the mana cost
        /// </summary>
        public ManaColor[] Colors
        {
            get
            {
                List<ManaColor> colors = new List<ManaColor>(5);
                if (_manaAmounts.Any(c =>   c.Color == ManaColor.White || 
                                            c.Color == ManaColor.TwoOrWhite || 
                                            c.Color == ManaColor.PhyrexianWhite ||
                                            c.Color == ManaColor.WhiteBlue || 
                                            c.Color == ManaColor.WhiteBlack || 
                                            c.Color == ManaColor.RedWhite || 
                                            c.Color == ManaColor.GreenWhite))
                    colors.Add(ManaColor.White);

                if (_manaAmounts.Any(c => c.Color == ManaColor.Blue ||
                                            c.Color == ManaColor.TwoOrBlue ||
                                            c.Color == ManaColor.PhyrexianBlue ||
                                            c.Color == ManaColor.WhiteBlue ||
                                            c.Color == ManaColor.BlueBlack ||
                                            c.Color == ManaColor.BlueRed ||
                                            c.Color == ManaColor.GreenBlue))
                    colors.Add(ManaColor.Blue);

                if (_manaAmounts.Any(c => c.Color == ManaColor.Black ||
                                            c.Color == ManaColor.TwoOrBlack ||
                                            c.Color == ManaColor.PhyrexianBlack ||
                                            c.Color == ManaColor.WhiteBlack ||
                                            c.Color == ManaColor.BlueBlack ||
                                            c.Color == ManaColor.BlackRed ||
                                            c.Color == ManaColor.BlackGreen))
                    colors.Add(ManaColor.Black);

                if (_manaAmounts.Any(c => c.Color == ManaColor.Red ||
                                            c.Color == ManaColor.TwoOrRed ||
                                            c.Color == ManaColor.PhyrexianRed ||
                                            c.Color == ManaColor.RedWhite ||
                                            c.Color == ManaColor.BlueRed ||
                                            c.Color == ManaColor.BlackRed ||
                                            c.Color == ManaColor.RedGreen))
                    colors.Add(ManaColor.Red);

                if (_manaAmounts.Any(c => c.Color == ManaColor.Green ||
                                            c.Color == ManaColor.TwoOrGreen ||
                                            c.Color == ManaColor.PhyrexianGreen ||
                                            c.Color == ManaColor.GreenWhite ||
                                            c.Color == ManaColor.BlackGreen ||
                                            c.Color == ManaColor.RedGreen ||
                                            c.Color == ManaColor.GreenBlue))
                    colors.Add(ManaColor.Green);

                if (colors.Count == 0)
                    colors.Add(ManaColor.Colorless);

                return colors.ToArray();
            }
        }
    }
}
