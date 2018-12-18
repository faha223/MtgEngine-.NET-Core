using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Players.Gameplay
{
    // TODO: Add Planeswalker Cards
    public class AttackerDeclaration
    {
        public Player DefendingPlayer { get; set; }
        
        public CreatureCard AttackingCreature { get; set; }
    }
}
