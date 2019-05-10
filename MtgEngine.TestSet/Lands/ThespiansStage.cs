using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.TestSet.Lands
{
    [MtgCard("Thespian's Stage", "TestSet", "", "", Text = "{T}: Add {C}\n{2},{T}: Thespian's Stage becomes a copy of target land, except it has this ability.", FlavorText = "Amid rumors of war, the third act of The Absolution of the Guildpact was quickly rewritten as a tragedy.")]
    public class ThespiansStage : LandCard
    {
        private string copiedCardName = null;
        public override string Name
        {
            get
            {
                if (copiedCardName != null)
                    return copiedCardName;
                return base.Name;
            }
        }

        private Cost copiedCardCost = null;
        public override Cost Cost
        {
            get
            {
                if (copiedCardCost != null)
                    return copiedCardCost;
                return base.Cost;
            }
            protected set => base.Cost = value;
        }

        private string copiedCardText = null;
        public override string Text
        {
            get
            {
                if (copiedCardText != null)
                    return copiedCardText;
                return base.Text;
            }
        }

        private string copiedCardFlavorText = null;
        public override string FlavorText
        {
            get
            {
                if (copiedCardFlavorText != null)
                    return copiedCardFlavorText;
                return base.FlavorText;
            }
        }

        public override List<Ability> Abilities
        {
            get
            {
                if (_copiedCardAbilities == null)
                    return base.Abilities;

                var list = new List<Ability>();
                list.AddRange(base.Abilities.Where(c => c is CopyAbility));
                if (_copiedCardAbilities != null)
                    list.AddRange(_copiedCardAbilities);
                return list;
            }
        }

        private bool? copiedCardIsLegendary = null;
        public override bool IsLegendary {
            get
            {
                if (copiedCardIsLegendary.HasValue)
                    return copiedCardIsLegendary.Value;
                return base.IsLegendary;
            }
        }

        private CardType[] copiedCardTypes = null;
        public override CardType[] Types
        {
            get
            {
                if (copiedCardTypes != null)
                    return copiedCardTypes;
                return base.Types;
            }
        }

        private string[] copiedCardSubtypes = null;
        public override string[] Subtypes
        {
            get
            {
                if (copiedCardSubtypes != null)
                    return copiedCardSubtypes;
                return base.Subtypes;
            }
        }

        public ThespiansStage(Player owner) : base(owner, new[] { CardType.Land }, null, false, false, false)
        {
            base.Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Colorless), "{T}: Add {C}"));
            base.Abilities.Add(new CopyAbility(this));
        }

        public void Copy(PermanentCard other)
        {
            copiedCardName = other.Name;
            copiedCardText = other.Text;
            copiedCardFlavorText = other.FlavorText;
            copiedCardCost = other.Cost.Copy(this);
            copiedCardTypes = other.Types;
            copiedCardSubtypes = other.Subtypes;
            copiedCardIsLegendary = other.IsLegendary;
            _copiedCardAbilities = other.Abilities.Select(a => a.Copy(this)).ToList();
            _copiedCardBasePower = other.BasePower;
            _copiedCardBaseToughness = other.BaseToughness;
        }

        class CopyAbility : ActivatedAbility, ITargeting
        {
            PermanentCard targetLand = null;

            public CopyAbility(PermanentCard source) : base(source, null, "{2},{T}: Thespian's Stage becomes a copy of target land, except it has this ability.")
            {
                Cost = new AggregateCost(this, ManaCost.Parse(this, "{2}"), new TapCost(source));
            }

            public override Ability Copy(PermanentCard newSource)
            {
                return new CopyAbility(newSource);
            }

            public override void OnResolve(Game game)
            {
                (Source as ThespiansStage).Copy(targetLand);
            }

            public void SelectTargets(Game game)
            {
                List<ITarget> Lands = new List<ITarget>();
                foreach (var player in game.Players())
                    Lands.AddRange(player.Battlefield.Lands);

                targetLand = (PermanentCard)Source.Controller.ChooseTarget(this, Lands);
            }
        }
    }
}
