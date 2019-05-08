using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MtgEngine;
using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Players.Gameplay;
using MtgEngine.Common.Utilities;

namespace MtgEngineTest
{
    public class ConsolePlayer : Player
    {
        private bool passTurn = false;
        private bool passToYourTurn = false;
        private string currentStep = null;

        public ConsolePlayer(string name, int startingLifeTotal, string deckList) : base(name, startingLifeTotal, deckList)
        {
        }

        public override ActionBase GivePriority(Game game, bool canPlaySorcerySpeedSpells)
        {
            // Don't skip priority if there's something on the stack
            var stack = game.CardsOnStack();
            if (stack.Count == 0)
            {
                if (passTurn)
                    return new PassPriorityAction();

                if (game.ActivePlayer == this)
                    passToYourTurn = false;

                if (passToYourTurn && game.ActivePlayer != this)
                    return new PassPriorityAction();
            }

            // TODO: Print to the Console a list of possible actions, and allow the user to select one
            PrintBattlefield();
            PrintManaPool();
            PrintHand();
            Console.WriteLine($"You Have Priority (Active Player: {game.ActivePlayer.Name})");
            int i = 1;
            var potentialActions = new List<ActionBase>();

            foreach (var card in Hand)
            {
                if (canPlayCardThisTurn(card, game, canPlaySorcerySpeedSpells))
                {
                    Console.WriteLine($"{i++}: Play {card.Name}");
                    potentialActions.Add(new PlayCardAction(card));
                }
            }

            // Add each ability the player can activate to the 
            foreach(PermanentCard card in Battlefield)
            {
                foreach(ActivatedAbility ability in card.Abilities.Where(c => c is ActivatedAbility))
                {
                    if(ability.Cost.CanPay())
                    {
                        Console.WriteLine($"{i++}: {card.Name}: {ability.Text}");
                        potentialActions.Add(new ActivateAbilityAction(ability));
                    }
                }
            }

            Console.WriteLine($"{i++}: Pass Priority");
            potentialActions.Add(new PassPriorityAction());
            Console.WriteLine($"{i++}: Next Turn");
            Console.WriteLine($"{i++}: To Your Turn");

            // Ask the user for their choice of action
            int selection = -1;
            do
            {
                Console.Write("What do you want to do? ");
                var parsed = ParseChoice(Console.ReadLine(), 1, i);
                Console.WriteLine();

                if (!parsed.HasValue)
                {
                    Console.WriteLine("I don't understand your selection. Try Again.");
                    Console.WriteLine();
                }
                else
                    selection = parsed.Value;
            } while (selection == -1);

            // Return the action the user chose
            if (selection <= potentialActions.Count)
                return potentialActions[selection - 1];
            else
            {
                if (selection - potentialActions.Count == 1)
                    passTurn = true;
                else
                {
                    passTurn = true;
                    passToYourTurn = true;
                }
                return new PassPriorityAction();
            }
        }

        private void PrintManaPool()
        {
            Console.WriteLine("Mana Pool:");
            Console.WriteLine($"White: {ManaPool[ManaColor.White]}");
            Console.WriteLine($"Blue: {ManaPool[ManaColor.Blue]}");
            Console.WriteLine($"Black: {ManaPool[ManaColor.Black]}");
            Console.WriteLine($"Red: {ManaPool[ManaColor.Red]}");
            Console.WriteLine($"Green: {ManaPool[ManaColor.Green]}");
            Console.WriteLine($"Colorless: {ManaPool[ManaColor.Colorless]}");
            Console.WriteLine();
        }

        private void PrintHand()
        {
            Console.WriteLine("Your Hand:");
            Hand.ForEach(card => Console.WriteLine(card.Name));
            Console.WriteLine();
        }

        private void PrintBattlefield()
        {
            Console.WriteLine("Battlefield:");
            foreach(PermanentCard card in Battlefield)
            {
                string counters = null;
                if(card.Counters.Count > 0)
                {
                    var counterTypes = card.Counters.Distinct();
                    counters = string.Join(", ", counterTypes.Select(c => $"{c}: {card.Counters.Count(d => d == c)}"));
                }
                if (card.IsACreature)
                {
                    if (counters != null)
                        Console.WriteLine($"{(card.IsTapped ? "(tapped) ": string.Empty)}{card.Name} ({card.Power}/{card.Toughness}): {counters}");
                    else
                        Console.WriteLine($"{(card.IsTapped ? "(tapped) " : string.Empty)}{card.Name} ({card.Power}/{card.Toughness})");

                }
                else
                {
                    if(counters != null)
                        Console.WriteLine($"{(card.IsTapped ? "(tapped) " : string.Empty)}{card.Name}: {counters}");
                    else
                        Console.WriteLine($"{(card.IsTapped ? "(tapped) " : string.Empty)}{card.Name}");
                }
            }
            Console.WriteLine();
        }

        public override void Draw(int howMany)
        {
            var cardsDrawn = Library.Take(howMany).ToList();
            Library.RemoveRange(0, howMany);
            cardsDrawn.ForEach(card =>
            {
                Console.WriteLine($"You Drew: {card.Name}");
                Hand.Add(card);
            });
        }

        public override void Discard()
        {
            Console.WriteLine("Your Hand:");
            for(int i = 0; i < Hand.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {Hand[i].Name}");
            }

            Console.Write("Choose a card to discard: ");
            string choice = Console.ReadLine();
            int index = -1;
            if(int.TryParse(choice, out index))
            {
                if(index > 0 && index <= Hand.Count)
                {
                    Discard(Hand[index - 1]);
                }
            }
        }

        public override bool OfferMulligan()
        {
            // We can't mulligan anymore
            if (Hand.Count == 0)
                return false;

            PrintHand();
            
            while (true)
            {
                Console.Write("Do you wish to take a mulligan (Y/N)? ");
                string choice = Console.ReadLine();
                if (choice.ToLower().StartsWith("y"))
                    return true;
                if (choice.ToLower().StartsWith("n"))
                    return false;
                Console.WriteLine();
            }
        }

        public override Card SelectTarget(string message, Func<Card, bool> targetSelector)
        {
            // TODO: Print a list of possible targets to the console and allow the user to select one, or none;
            var possibleTargets = Battlefield.Where(c => targetSelector(c)).ToList();
            int i = 0;
            int? selection = null;

            do
            {
                Console.WriteLine(message);    
                for (i = 0; i < possibleTargets.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {possibleTargets[i].Name}");
                }
                Console.WriteLine($"{++i}: Cancel");
                selection = ParseChoice(Console.ReadLine(), 1, i);
                Console.WriteLine();

            } while (!selection.HasValue);

            if (selection.Value == i)
                return null;
            return possibleTargets[selection.Value - 1];
        }

        public override void ScryChoice(List<Card> scryedCards, out IEnumerable<Card> cardsOnTop, out IEnumerable<Card> cardsOnBottom)
        {
            Console.WriteLine($"Scry {scryedCards.Count}");
            for(int i = 0; i < scryedCards.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {scryedCards[i].Name}");
            }

            List<int> choice = null;
            do
            {
                Console.Write("Which cards would you like to put on the bottom of your library (in the order you wish to draw them)." + Environment.NewLine
                    + "Don't type anything and just press enter if you wish to keep all on top: ");
                choice = ParseChoice(Console.ReadLine(), 0, scryedCards.Count, 1, scryedCards.Count, true);

                // If we have to repeat this, then lets put a blank line since the last request to make it easier to see
                if (choice == null)
                    Console.WriteLine();
            } while (choice == null);

            cardsOnBottom = scryedCards.OrderByIndexList(choice.Select(c => c - 1));
            
            scryedCards = scryedCards.Except(cardsOnBottom).ToList();
            if (scryedCards.Count > 1)
            {
                Console.WriteLine("Cards to be put on top:");
                for (int i = 0; i < scryedCards.Count; i++)
                    Console.WriteLine($"{i + 1}: {scryedCards[i].Name}");

                choice = null;
                do
                {
                    Console.Write("Choose the order the remaining cards should be placed on top of the library. First card is the next card you draw.");
                    choice = ParseChoice(Console.ReadLine(), scryedCards.Count, scryedCards.Count, 1, scryedCards.Count, true);

                    // If we have to repeat this, then lets put a blank line since the last request to make it easier to see
                    if (choice == null)
                        Console.WriteLine();
                } while (choice == null);

                cardsOnTop = scryedCards.OrderByIndexList(choice.Select(c => c-1));
            }
            else
                cardsOnTop = scryedCards;
        }

        public override ITarget ChooseTarget(IResolvable source, List<ITarget> possibleTargets)
        {
            if (possibleTargets.Count == 1)
                return possibleTargets[0];

            int? selection = null;
            while (!selection.HasValue)
            {
                if (source is Card)
                    Console.WriteLine((source as Card).Text);
                else if (source is Ability)
                    Console.WriteLine((source as Ability).Text);

                int i = 0;
                foreach (var target in possibleTargets)
                {
                    if (target is Player)
                        Console.WriteLine($"{++i}: {(target as Player).Name}");
                    else if (target is Card)
                        Console.WriteLine($"{++i}: {(target as Card).Name}");
                }

                selection = ParseChoice(Console.ReadLine(), 1, i);
            }

            return possibleTargets[selection.Value - 1];
        }

        #region Combat

        public override List<AttackerDeclaration> DeclareAttackers(List<Player> opponents)
        {
            var attackers = new List<AttackerDeclaration>(Battlefield.Count);
            var creatures = Battlefield.Creatures.Where(c => c.CanAttack).ToList();
            if (creatures.Count > 0)
            {
                bool done = false;
                while (!done)
                {
                    int i = 1;

                    Console.WriteLine("Declare Attackers");
                    foreach (var creature in creatures)
                    {
                        if (creature.IsAttacking)
                        {
                            Console.WriteLine($"{i++}: Withdraw {creature.Name} from combat");
                        }
                        else
                            Console.WriteLine($"{i++}: Declare {creature.Name} as attacker");
                    }
                    Console.WriteLine($"{i}: Pass Priority");

                    var selection = ParseChoice(Console.ReadLine(), 1, i);
                    Console.WriteLine();

                    if (selection.HasValue)
                    {
                        if (selection.Value <= creatures.Count)
                        {
                            var creature = creatures[selection.Value - 1];
                            if (creature.IsAttacking)
                            {
                                attackers.RemoveAll(c => c.AttackingCreature == creature);
                                creature.DefendingPlayer = null;
                            }
                            else
                            {
                                if (opponents.Count == 1)
                                {
                                    creature.DefendingPlayer = opponents[0];
                                    attackers.Add(new AttackerDeclaration() { AttackingCreature = creature, DefendingPlayer = opponents[0] });
                                }
                                else
                                {
                                    selection = -1;
                                    do
                                    {
                                        Console.WriteLine("Choose Opponent:");
                                        i = 1;
                                        foreach (var opponent in opponents)
                                            Console.WriteLine($"{i++}: {opponent.Name}");
                                        Console.WriteLine($"{i}: Cancel");

                                        Console.Write("What is your selection? ");
                                        selection = ParseChoice(Console.ReadLine(), 1, i);
                                        Console.WriteLine();

                                        if (selection.HasValue)
                                        {
                                            creature.DefendingPlayer = opponents[selection.Value - 1];
                                            attackers.Add(new AttackerDeclaration() { AttackingCreature = creature, DefendingPlayer = creature.DefendingPlayer });
                                        }
                                    } while (selection == -1);
                                }
                            }
                        }
                        else if (selection == i)
                            done = true;
                    }
                    else
                        Console.WriteLine();
                }
            }
            return attackers;
        }

        public override List<BlockerDeclaration> DeclareBlockers(List<PermanentCard> attackingCreatures)
        {
            var availableBlockers = Battlefield.Creatures.Where(c => !c.IsTapped).ToList();
            var blockers = new List<BlockerDeclaration>(availableBlockers.Count);

            Console.WriteLine("You are being attacked");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Declare Blockers");

                int i = 1;
                foreach (var creature in attackingCreatures)
                {
                    Console.WriteLine($"{i++}: Block {creature.Name}");
                }
                foreach (var creature in Battlefield.Creatures.Where(c => c.Blocking != null))
                {
                    Console.WriteLine($"{i++}: Withdraw {creature.Name} from combat");
                }
                Console.WriteLine($"{i}: Pass Priority");

                Console.Write("What do you choose? ");
                var selection = ParseChoice(Console.ReadLine(), 1, i);
                Console.WriteLine();

                if (selection.HasValue)
                {
                    if (selection.Value <= attackingCreatures.Count)
                    {
                        var attacker = attackingCreatures[selection.Value - 1];

                        // Available blockers are creatures that are not currently blocking anything, and which can block the selected attacker
                        var _availBlockers = availableBlockers.Where(c => c.Blocking == null && c.CanBlock(attacker)).ToList();

                        // declare blocker
                        while (true)
                        {
                            Console.WriteLine("Choose a blocker:");
                            i = 1;
                            foreach (var creature in _availBlockers)
                            {
                                Console.WriteLine($"{i++}: {creature.Name}");
                            }
                            Console.WriteLine($"{i}: Cancel");
                            Console.Write("Which do you choose? ");
                            selection = ParseChoice(Console.ReadLine(), 1, i);
                            Console.WriteLine();

                            if (selection.HasValue)
                            {
                                if (selection.Value == i)
                                    break;
                                else
                                {
                                    var blocker = _availBlockers[selection.Value - 1];
                                    blocker.Blocking = attacker;
                                    blockers.Add(new BlockerDeclaration()
                                    {
                                        Attacker = attacker,
                                        Blocker = blocker
                                    });
                                    break;
                                }
                            }
                            else
                                Console.WriteLine();
                        }
                    }
                    else if (selection == i)
                        break;
                    else
                    {
                        // remove from combat
                        var blockingCreatures = Battlefield.Creatures.Where(c => c.Blocking != null).ToList();
                        var blocker = blockingCreatures[selection.Value - attackingCreatures.Count];
                        blockers.RemoveAll(c => c.Blocker == blocker);
                        blocker.Blocking = null;
                    }
                }
            }

            return blockers;
        }

        public override IEnumerable<PermanentCard> SortBlockers(PermanentCard attacker, IEnumerable<PermanentCard> blockers)
        {
            if (blockers.Count() == 1)
                return blockers;

            while(true)
            {
                Console.WriteLine($"{attacker.Name} is being blocked by {blockers.Count()} creatures");
                for(int i = 0; i < blockers.Count(); i++)
                    Console.WriteLine($"{i}: {blockers.ElementAt(i).Name}");
                Console.Write($"In what order would you like {attacker.Name} to deal damage to blockers?");
                var response = ParseChoice(Console.ReadLine(), blockers.Count(), blockers.Count(), 1, blockers.Count(), true);
                if (response != null && response.Count == blockers.Count())
                    return blockers.ToList().OrderByIndexList(response);
                Console.WriteLine();
            }
        }

        #endregion Combat

        #region Game State Updates

        public override void GameStepChanged(Game game, string currentStep)
        {
            this.currentStep = currentStep;

            if (currentStep == "Untap Step")
                passTurn = false;

            Console.WriteLine();
            Console.WriteLine($"CurrentStep: {currentStep}");
            Console.WriteLine();
        }

        public override void CardHasChangedZones(Game game, Card card, MtgEngine.Common.Enums.Zone oldZone, MtgEngine.Common.Enums.Zone newZone)
        {
            switch(newZone)
            {
                case MtgEngine.Common.Enums.Zone.Battlefield:
                    CardHasEnteredBattlefield(game, card);
                    break;
                case MtgEngine.Common.Enums.Zone.Stack:
                    CardHasEnteredStack(game, card);
                    break;
            }
        }

        public void CardHasEnteredBattlefield(Game game, Card card)
        {
            Console.WriteLine($"{card.Name} has entered the battlefield under the control of {card.Controller.Name}.");
            Console.WriteLine();
        }

        public void CardHasEnteredStack(Game game, Card card)
        {
            Console.WriteLine($"{card.Name} is now on the stack.");
            Console.WriteLine();
        }

        public override void CountersAddedToPlayer(Game game, Player player, CounterType counterType, int count)
        {
            Console.WriteLine($"{player.Name} got {count} {counterType} counter{(count == 1 ? string.Empty : "s")}");
            int accumulated = player.Counters.ToList().Count(c => c == counterType);
            Console.WriteLine($"{player.Name} now has {accumulated} {counterType} counter{(accumulated == 1 ? string.Empty : "s")}");
        }

        public override void AbilityHasEnteredStack(Game game, Ability ability)
        {
            Console.WriteLine($"\"{ability.Text}\" is now on the stack.");
            Console.WriteLine();
        }

        public override void PlayerTookDamage(Game game, Player player, int damageDealt)
        {
            Console.WriteLine($"{player.Name} took {damageDealt} damage.");
            Console.WriteLine($"{player.Name}'s life total is now {player.LifeTotal}");
            Console.WriteLine();
        }

        public override void PlayerWonTheGame(Game game, Player player, string reason)
        {
            Console.WriteLine($"{player.Name} Wins the Game: {reason}");
            Console.WriteLine();
        }

        public override void PlayerLostTheGame(Game game, Player player, string reason)
        {
            Console.WriteLine($"{player.Name} Lost the Game: {reason}");
            Console.WriteLine();
        }

        public override void GameEndedInDraw(Game game)
        {
            Console.WriteLine("Game Ended in Draw");
            Console.WriteLine();
        }

        #endregion Game State Updates

        public override int GetValueForX(string cost)
        {
            do
            {
                Console.WriteLine($"Pay {cost}");
                Console.Write("What value would you like for X? ");
                var selection = ParseChoice(Console.ReadLine(), 0, int.MaxValue);
                Console.WriteLine();

                if (selection.HasValue)
                    return selection.Value;
            } while (true);
        }

        public override ManaColor? PayManaCost(string cost)
        {
            do
            {
                Console.WriteLine($"Pay {cost}");
                int i = 1;
                List<ManaColor> colorOptions = new List<ManaColor>(6);
                foreach (var color in new[] { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green, ManaColor.Colorless })
                {
                    if (ManaPool[color] > 0)
                    {
                        colorOptions.Add(color);
                        Console.WriteLine($"{i++}: Pay {color}");
                    }
                }
                Console.WriteLine($"{i}: Cancel");

                var selection = ParseChoice(Console.ReadLine(), 1, i);
                Console.WriteLine();

                if (selection.HasValue)
                {
                    if (selection.Value == i)
                        return null;
                    else if (selection.Value <= colorOptions.Count)
                    {
                        var color = colorOptions[selection.Value - 1];
                        ManaPool[color]--;
                        return color;
                    }
                }
                Console.WriteLine();
            } while (true);
        }

        public override List<PermanentCard> MakeChoice(string message, int count, List<PermanentCard> options)
        {
            do
            {
                Console.WriteLine(message);
                int i = 0;
                foreach (var card in options)
                {
                    if (card.IsACreature)
                        Console.WriteLine($"{++i}: {card.Name} ({card.Power}/{card.Power})");
                    else
                        Console.WriteLine($"{++i}: {card.Name}");
                }

                Console.Write("Choose: ");

                var selection = ParseChoice(Console.ReadLine(), count, count, 1, i, true);
                if(selection != null)
                {
                    return options.OrderByIndexList(selection);
                }
            } while (true);
        }

        public override List<Card> MakeChoice(string message, int count, List<Card> options)
        {
            do
            {
                Console.WriteLine(message);
                int i = 0;
                foreach (var card in options)
                {
                    if (card.IsACreature)
                        Console.WriteLine($"{++i}: {card.Name} ({(card as PermanentCard).Power}/{(card as PermanentCard).Power})");
                    else
                        Console.WriteLine($"{++i}: {card.Name}");
                }

                Console.Write("Choose: ");

                var selection = ParseChoice(Console.ReadLine(), count, count, 1, i, true);
                if (selection != null)
                {
                    return options.OrderByIndexList(selection);
                }
            } while (true);
        }

        public override List<PermanentCard> Sort(string message, List<PermanentCard> options)
        {
            do
            {
                Console.WriteLine(message);
                int i = 0;
                foreach(var card in options)
                {
                    if (card.IsACreature)
                        Console.WriteLine($"{++i}: {card.Name} ({card.Power}/{card.Power})");
                    else
                        Console.WriteLine($"{++i}: {card.Name}");
                }
                Console.Write("Sort: ");
                var selection = ParseChoice(Console.ReadLine(), options.Count, options.Count, 1, i, true);
                if (selection != null)
                {
                    return options.OrderByIndexList(selection);
                }
            } while (true);
        }

        public override List<Card> Sort(string message, List<Card> options)
        {
            do
            {
                Console.WriteLine(message);
                int i = 0;
                foreach (var card in options)
                {
                    if (card.IsACreature)
                        Console.WriteLine($"{++i}: {card.Name} ({(card as PermanentCard).Power}/{(card as PermanentCard).Power})");
                    else
                        Console.WriteLine($"{++i}: {card.Name}");
                }
                Console.Write("Sort: ");
                var selection = ParseChoice(Console.ReadLine(), options.Count, options.Count, 1, i, true);
                if (selection != null)
                {
                    return options.OrderByIndexList(selection);
                }
            } while (true);
        }

        public override ManaColor ChooseColor()
        {
            var options = new[] { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green };
            while(true)
            {
                Console.WriteLine("Choose a Color:");
                for(int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {options[i]}");
                }
                Console.Write("Which Color do you choose? ");
                var choice = ParseChoice(Console.ReadLine(), 1, options.Length);
                Console.WriteLine();

                if (choice.HasValue)
                    return options[choice.Value - 1];
            }
        }

        private int? ParseChoice(string userText, int minResponseValue, int maxResponseValue)
        {
            var selections = ParseChoice(userText, 1, 1, minResponseValue, maxResponseValue, false);
            if (selections != null && selections.Count == 1)
                return selections[0];
            return null;
        }

        public static List<int> ParseChoice(string userText, int minResponseCount, int maxResponseCount, int minResponseValue, int maxResponseValue, bool noDuplicates)
        {
            if (string.IsNullOrWhiteSpace(userText))
            {
                if (minResponseCount <= 0)
                    return new List<int>();
                else
                    Console.WriteLine($"You must choose at least {minResponseCount}");
                    return null;
            }
            // This regex expects a list of integers, separated by a minimum of one comma or space
            userText = Regex.Replace(userText, @"[,\s]\s*", ","); // Clean up the commas
            userText = Regex.Replace(userText, ",+", ",");              // Each selection split by only 1 comma
            userText = Regex.Replace(userText, @"^,", string.Empty);    // No leading commas    
            userText = Regex.Replace(userText, @",$", string.Empty);    // No trailing commas

            // Check that it matches the regex we have rigorously tried to shoehorn the user text into
            if(Regex.IsMatch(userText, @"\d+(,\d+)*"))
            {
                try
                {
                    // Split the list and parse each integer. This will throw an exception if an integer doesn't parse
                    List<int> selection = userText.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToList();

                    // Check that the user selected at least the minimum number of items
                    if (selection.Count > maxResponseCount)
                    {
                        Console.WriteLine($"You may only choose {maxResponseCount} items");
                        return null;
                    }
                    else if (selection.Count < minResponseCount)
                    {
                        Console.WriteLine($"You must choose at least {minResponseCount} items");
                        return null;
                    }

                    // Check the validity of each response
                    foreach(var item in selection)
                    {
                        if (item < minResponseValue || item > maxResponseValue)
                        {
                            Console.WriteLine($"Invalid Response: {item}");
                            return null;
                        }
                        else if(noDuplicates && selection.Count(c => c == item) > 1)
                        {
                            Console.WriteLine($"You chose {item} more than once");
                            return null;
                        }
                    }

                    return selection;
                }
                catch
                {
                    Console.WriteLine("I don't understand your response.");
                }
            }
            else
            {
                Console.WriteLine("I don't understand your response.");
            }

            return null;
        }
    }
}
