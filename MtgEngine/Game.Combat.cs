using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using MtgEngine.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine
{
    public partial class Game
    {
        #region Combat Events

        delegate void AttackerDeclarationEvent(Game game, PermanentCard attacker, Player defendingPlayer);
        private event AttackerDeclarationEvent AttackerDeclared;

        delegate void BlockerDeclarationEvent(Game game, PermanentCard blocker, PermanentCard attacker);
        private event BlockerDeclarationEvent BlockerDeclared;

        delegate void CreatureTookDamageEvent(Game game, PermanentCard creature, Card source, int DamageTaken);
        private event CreatureTookDamageEvent CreatureTookDamage;

        delegate void PlayertookDamageEvent(Game game, Player player, Card source, int DamageTaken);
        private event PlayertookDamageEvent PlayerTookDamage;

        #endregion Combat Events

        /// <summary>
        /// 
        /// </summary>
        private void BeginningOfCombatStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Beginning of Combat");

            CheckForTriggeredAbilities();

            ApNapLoop(false);

            DrainManaPools();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DeclareAttackersStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Declare Attackers Step");

            // Ask the Active Player to declare attackers
            var attackers = ActivePlayer.DeclareAttackers(_players.Except(new[] { ActivePlayer }).ToList());
            if (attackers != null)
            {
                foreach (var declaration in attackers)
                {
                    declaration.AttackingCreature.DefendingPlayer = declaration.DefendingPlayer;

                    // Tap all attacking creatures that don't have vigilance
                    if (!declaration.AttackingCreature.StaticAbilities.Contains(StaticAbility.Vigilance))
                        declaration.AttackingCreature.Tap();

                    AttackerDeclared?.Invoke(this, declaration.AttackingCreature, declaration.DefendingPlayer);
                }
            }

            // Any "When this creature attacks" triggers get put onto the stack
            CheckForTriggeredAbilities();

            // Resolve the stack (this happens even if there's nothing already on the stack)
            ApNapLoop(false);

            DrainManaPools();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DeclareBlockersStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Declare Blockers Step");

            // Ask the Defending Players, in ApNap order, to declare Blockers
            var defendingPlayers = _players.StartAt(ActivePlayer)
                .Where(p => ActivePlayer.Battlefield.Creatures.Any(c => c.DefendingPlayer == p));

            // Iterate over the defending players in turn order
            foreach (var player in defendingPlayers)
            {
                // Have defending players declare blockers
                var blockers = player.DeclareBlockers(ActivePlayer.Battlefield.Creatures.Where(c => c.DefendingPlayer == player).ToList());
                if (blockers != null)
                {
                    foreach (var blocker in blockers)
                    {
                        blocker.Blocker.Blocking = blocker.Attacker;
                        BlockerDeclared?.Invoke(this, blocker.Blocker, blocker.Attacker);
                    }
                }
            }

            // Put any triggers on the stack
            CheckForTriggeredAbilities();

            // Resolve the stack (this happens even if there's nothing already on the stack)
            ApNapLoop(false);
            
            // At the end of each phase, mana pools are emptied
            DrainManaPools();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DamageStep()
        {
            CurrentStepHasChanged?.Invoke(this, "Damage Step");

            // These creatures have been blocked
            var blockedCreatures = ActivePlayer.Battlefield.Creatures.Where(c => c.IsAttacking && c.DefendingPlayer.Battlefield.Creatures.Any(d => d.Blocking == c)).ToList();

            // Have the active player sort blockers for each of their attackers
            Dictionary<PermanentCard, List<PermanentCard>> blockerMap = new Dictionary<PermanentCard, List<PermanentCard>>(blockedCreatures.Count);
            foreach (var attacker in blockedCreatures)
            {
                var blockers = attacker.DefendingPlayer.Battlefield.Creatures.Where(c => c.Blocking == attacker).ToList();

                if (blockers.Count > 1)
                    blockerMap.Add(attacker, ActivePlayer.SortBlockers(attacker, blockers).ToList());
                else
                    blockerMap.Add(attacker, blockers);
            }

            // If any attackers have firststrike or doublestrike
            if (ActivePlayer.Battlefield.Creatures.Any(c => c.IsAttacking && (doesFirstStrikeDamage(c) || takesFirstStrikeDamage(c))))
            {
                // TODO: Deal First Strike Damage
                foreach (var attacker in ActivePlayer.Battlefield.Creatures.Where(c => c.IsAttacking && (doesFirstStrikeDamage(c) || takesFirstStrikeDamage(c))))
                {
                    // Deal combat damage to, and take combat damage from, blockers                    
                    CombatDamage(attacker,
                        blockedCreatures.Contains(attacker),
                        blockerMap.ContainsKey(attacker) ? blockerMap[attacker].Where(c => doesFirstStrikeDamage(attacker) || doesFirstStrikeDamage(c)) : null,
                        true);
                }

                CheckStateBasedActionsAndResolveStack();
            }

            // Remove all blockers from the blocker map that have left the defending player's battlefield after firststrike damage before applying normal combat damage
            foreach (var attacker in blockerMap.Keys)
            {
                var blockers = blockerMap[attacker];
                blockers.RemoveAll(c => !c.Controller.Battlefield.Contains(c));
                blockerMap[attacker] = blockers;
            }

            // If any remaining attackers have doublestrike or don't have firststrike
            if (ActivePlayer.Battlefield.Creatures.Any(c => c.IsAttacking && (doesNormalDamage(c) || takesNormalDamage(c))))
            {
                foreach (PermanentCard attacker in ActivePlayer.Battlefield.Creatures.Where(c => c.IsAttacking && doesNormalDamage(c)))
                {
                    // Deal combat damage to, and take combat damage from, blockers
                    CombatDamage(attacker,
                        blockedCreatures.Contains(attacker),
                        blockerMap.ContainsKey(attacker) ? blockerMap[attacker].Where(c => doesNormalDamage(attacker) || doesNormalDamage(c)) : null,
                        false);
                }

                CheckStateBasedActionsAndResolveStack();
            }

            DrainManaPools();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="blocked"></param>
        /// <param name="blockers"></param>
        /// <param name="firstStrike"></param>
        private void CombatDamage(PermanentCard attacker, bool blocked, IEnumerable<PermanentCard> blockers, bool firstStrike)
        {
            // If the defending player didn't block (we might not have blockers right now)
            if (!blocked)
            {
                ApplyDamage(attacker.DefendingPlayer, attacker, attacker.Power);
            }
            else if (blockers != null)
            {
                // Deal damage to the defending player's creatures
                var damageOutput = attacker.Power;

                // Have the attacking creatures apply damage to and take damage from blocking creatures
                foreach (var blocker in blockers)
                {
                    // The attacker hits
                    if ((firstStrike && doesFirstStrikeDamage(attacker)) || (!firstStrike && doesNormalDamage(attacker)))
                    {
                        if (damageOutput > 0)
                        {
                            int damageDealt = Math.Min(blocker.Toughness, damageOutput);
                            blocker.TakeDamage(damageDealt, attacker);
                            damageOutput -= damageDealt;
                        }
                    }

                    // The blocker hits back
                    if ((firstStrike && doesFirstStrikeDamage(blocker)) || (!firstStrike && doesNormalDamage(blocker)))
                        attacker.TakeDamage(blocker.Power, blocker);
                }

                // Trample Damage to defending player
                if (attacker.HasTrample && damageOutput > 0)
                {
                    ApplyDamage(attacker.DefendingPlayer, attacker, damageOutput);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="amount"></param>
        public void ApplyDamage(IDamageable target, Card source, int amount)
        {
            target.TakeDamage(amount, source);
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndOfCombatStep()
        {
            CurrentStepHasChanged?.Invoke(this, "End of Combat Step");

            // Give Priority to Players
            CheckStateBasedActions();
            ApNapLoop(false);

            // TODO: Add any "End of Combat" triggers to the stack

            // Remove all creatures from combat
            foreach (var creature in ActivePlayer.Battlefield.Creatures)
                creature.DefendingPlayer = null;
            foreach (var player in _players)
            {
                foreach (var creature in player.Battlefield.Creatures)
                    creature.Blocking = null;
            }

            DrainManaPools();
        }
    }
}
