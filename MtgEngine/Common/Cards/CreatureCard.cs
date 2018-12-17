using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract class CreatureCard : PermanentCard
    {
        private int _basePower { get; }
        public virtual int BasePower
        {
            get { return _basePower; }
        }
        
        private int _baseToughness { get; }
        public virtual int BaseToughness
        {
            get { return _baseToughness; }
        }

        public Player DefendingPlayer { get; set; } = null;

        public bool IsAttacking { get { return DefendingPlayer != null; } }

        public Card Blocking { get; set; } = null;

        protected CreatureCard(Player owner, string name, string image, string cardId, Cost cost, CardType[] types, string[] subtypes, int basePower, int baseToughness, bool isLegendary) : 
            base(owner, name, image, cardId, true, cost, types, subtypes, false, isLegendary)
        {
            _basePower = basePower;
            _baseToughness = baseToughness;
        }
    }
}
