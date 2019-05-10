using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine
{
    public partial class Game
    {
        /// <summary>
        /// /// This method shuffles the Players collection by having the players Roll for Initiative, similarly to how we play in Paper MTG, except they roll d100 instead of 2d6
        /// /// </summary>
        private void ShufflePlayers()
        {
            var temp = new Queue<Player>(_players.Count);
            foreach (var player in _players)
                temp.Enqueue(player);
            _players.Clear();

            Dictionary<int, Player> initiative = new Dictionary<int, Player>(temp.Count);
            while (temp.Count > 0)
            {
                var player = temp.Dequeue();
                int i = player.RollInitiative();

                if (initiative.ContainsKey(i))
                {
                    temp.Enqueue(initiative[i]);
                    temp.Enqueue(player);

                    initiative.Remove(i);
                }
                else
                {
                    initiative.Add(i, player);
                }
            }

            foreach (var pair in initiative.ToList().OrderBy(c => c.Key))
            {
                _players.Add(pair.Value);
            }
            initiative.Clear();
        }

        /// <summary>
        /// This method offers all players mulligans in Turn order, then gives them each a mulligan. The process repeats until no player has opted to take their hand
        /// </summary>
        private void OfferMulligans()
        {
            List<Player> playersTakingMulligan = new List<Player>(_players);
            do
            {
                // Offer each player a mulligan, note which ones chose to keep their current hand
                List<Player> playersKeeping = new List<Player>(playersTakingMulligan.Count);
                foreach (var player in playersTakingMulligan)
                {
                    if (!player.OfferMulligan())
                        playersKeeping.Add(player);
                }

                // Remove players who chose to keep their hand from the players taking mulligan
                playersTakingMulligan.RemoveAll(player => playersKeeping.Contains(player));

                // Apply the Vancouver Mulligan strategy to all players who elected to take a mulligan
                playersTakingMulligan.ForEach(player => VancouverMulligan(player));

            } while (playersTakingMulligan.Count > 0);

            // Players who are starting with a hand of less than 7 scry 1
            foreach (var player in _players)
            {
                if (player.Hand.Count < 7)
                {
                    var scryedCards = player.Library.Take(1).ToList();
                    player.Library.RemoveRange(0, 1);
                    player.ScryChoice(scryedCards, out var cardsOnTop, out var cardsOnBottom);
                    player.Library.AddRange(cardsOnBottom);
                    foreach (var card in cardsOnTop.Reverse())
                        player.Library.Insert(0, card);
                }
            }
        }

        /// <summary>
        /// Vancouver mulligan works by having the player return their hand to their library and 
        /// drawing 1 fewer cards. In Multiplayer, the player gets a free first mulligan.
        /// </summary>
        /// <param name="player">The player that is taking a mulligan</param>
        private void VancouverMulligan(Player player)
        {
            // The player Shuffles their current Hand into their library
            player.Library.AddRange(player.Hand);
            player.Hand.Clear();
            player.ShuffleLibrary();

            // Then draws a new hand, the number of cards is determined by how many mulligans the player has taken already
            if (_players.Count > 2) // Free first mulligan if multiplayer
            {
                player.Draw(Math.Max(0, 7 - player.MulligansTaken));
                player.MulligansTaken++;
            }
            else
            {
                player.Draw(Math.Max(0, 6 - player.MulligansTaken));
                player.MulligansTaken++;
            }
        }

        /// <summary>
        /// Resets the Player's lands played per turn and max lands played per turn
        /// </summary>
        /// <param name="player"></param>
        private void ResetLandsPlayed(Player player)
        {
            player.LandsPlayedThisTurn = 0;
            player.MaxLandsPlayedThisTurn = (player == ActivePlayer ? 1 : 0);
        }

        /// <summary>
        /// Resets each player's mana pool to zero
        /// </summary>
        private void DrainManaPools()
        {
            _players.ForEach(c => c.ManaPool.Clear());
        }

        public Common.Zone Battlefield
        {
            get
            {
                var bfield = new Common.Zone();
                foreach (var player in _players)
                    bfield.AddRange(player.Battlefield);
                return bfield;
            }
        }

        #region Combat Utilities

        /// <summary>
        /// Do first strike damage only if the creature has first strike or double strike
        /// </summary>
        private bool doesFirstStrikeDamage(Card creature)
        {
            if (creature.StaticAbilities.Contains(StaticAbility.FirstStrike) || creature.StaticAbilities.Contains(StaticAbility.DoubleStrike))
                return true;
            return false;
        }

        /// <summary>
        /// Take first strike damage if any blocking creatures do first strike damage
        /// </summary>
        /// <param name="attacker">The attacking creature</param>
        /// <returns>True if the attacker or any of its blockers have first strike or doublestrike. Otherwise false.</returns>
        private bool takesFirstStrikeDamage(Card attacker)
        {
            return attacker.DefendingPlayer.Battlefield.Creatures.Any(c => c.Blocking == attacker && doesFirstStrikeDamage(c));
        }

        /// <summary>
        /// Do normal damage unless the creature has first strike and NOT double strike
        /// </summary>
        /// <param name="creature">The creature</param>
        /// <returns>True if the creature does normal damage.</returns>
        private bool doesNormalDamage(Card creature)
        {
            if (creature.StaticAbilities.Contains(StaticAbility.DoubleStrike))
                return true;
            if (creature.StaticAbilities.Contains(StaticAbility.FirstStrike))
                return false;
            return true;
        }

        /// <summary>
        /// Take normal damage if any blocking creatures do normal damage
        /// </summary>
        /// <param name="attacker">The attacking creature</param>
        /// <returns>True if the attacking creature or any of the blocking creatures do normal damage.</returns>
        private bool takesNormalDamage(Card attacker)
        {
            return attacker.DefendingPlayer.Battlefield.Creatures.Any(c => c.Blocking == attacker && doesNormalDamage(c));
        }

        #endregion Combat Utilities
    }
}
