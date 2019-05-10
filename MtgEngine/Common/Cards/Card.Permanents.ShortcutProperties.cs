using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract partial class Card : IDamageable
    {
        public bool HasFlying => StaticAbilities.Contains(StaticAbility.Flying);

        public bool HasReach => StaticAbilities.Contains(StaticAbility.Reach);

        public bool HasHaste => StaticAbilities.Contains(StaticAbility.Haste);

        public bool HasDefender => StaticAbilities.Contains(StaticAbility.Defender);

        public bool HasIndestructible => StaticAbilities.Contains(StaticAbility.Indestructible);

        public bool HasFear => StaticAbilities.Contains(StaticAbility.Fear);

        public bool HasFirstStrike => StaticAbilities.Contains(StaticAbility.FirstStrike);

        public bool HasDoubleStrike => StaticAbilities.Contains(StaticAbility.DoubleStrike);

        public bool HasVigilance => StaticAbilities.Contains(StaticAbility.Vigilance);

        public bool HasUnblockable => StaticAbilities.Contains(StaticAbility.Unblockable);

        public bool HasDevoid => StaticAbilities.Contains(StaticAbility.Devoid);

        public bool HasShadow => StaticAbilities.Contains(StaticAbility.Shadow);

        public bool HasHorsemanship => StaticAbilities.Contains(StaticAbility.Horsemanship);

        public bool HasPlainswalk => StaticAbilities.Contains(StaticAbility.Plainswalk);

        public bool HasIslandwalk => StaticAbilities.Contains(StaticAbility.Islandwalk);

        public bool HasSwampwalk => StaticAbilities.Contains(StaticAbility.Swampwalk);

        public bool HasMountainwalk => StaticAbilities.Contains(StaticAbility.Mountainwalk);

        public bool HasForestwalk => StaticAbilities.Contains(StaticAbility.Forestwalk);

        public bool HasFlash => StaticAbilities.Contains(StaticAbility.Flash);

        public bool HasShroud => StaticAbilities.Contains(StaticAbility.Shroud);

        public bool HasHexproof => StaticAbilities.Contains(StaticAbility.Hexproof);

        public bool HasTrample => StaticAbilities.Contains(StaticAbility.Trample);

        public bool HasInfect => StaticAbilities.Contains(StaticAbility.Infect);
    }
}
