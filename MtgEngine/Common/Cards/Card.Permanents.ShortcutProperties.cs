using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : IDamageable
    {
        public bool HasFlying => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Flying);

        public bool HasReach => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Reach);

        public bool HasHaste => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Haste);

        public bool HasDefender => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Defender);

        public bool HasIndestructible => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Indestructible);

        public bool HasFear => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Fear);

        public bool HasFirstStrike => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.FirstStrike);

        public bool HasDeathtouch => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Deathtouch);

        public bool HasDoubleStrike => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.DoubleStrike);

        public bool HasVigilance => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Vigilance);

        public bool HasUnblockable => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Unblockable);

        public bool HasDevoid => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Devoid);

        public bool HasShadow => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Shadow);

        public bool HasHorsemanship => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Horsemanship);

        public bool HasPlainswalk => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Plainswalk);

        public bool HasIslandwalk => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Islandwalk);

        public bool HasSwampwalk => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Swampwalk);

        public bool HasMountainwalk => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Mountainwalk);

        public bool HasForestwalk => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Forestwalk);

        public bool HasFlash => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Flash);

        public bool HasShroud => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Shroud);

        public bool HasHexproof => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Hexproof);

        public bool HasTrample => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Trample);

        public bool HasInfect => StaticAbilitiesAfterModifiersApplied.Contains(StaticAbility.Infect);
    }
}
