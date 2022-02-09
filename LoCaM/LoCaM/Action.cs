using LoCaM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace LoCaM
{
    public class Action
    {
        public GameState GameState;

        private List<int> _cardCosts;
        private int _goalAvgCost;

        public Action()
        {
            _cardCosts = new List<int>();
            _goalAvgCost = 5;
        }

        public string TakeTurn(string phase)
        {
            if (phase == "Draft")
            {
                return DraftCard();
            }
            else
            {
                var watch = new Stopwatch();
                var commands = new List<string>();
                //set up a while loop or for loop to cycle through possible actions being able to attack or summon etc multiple times as individual actions

                //ok so first thing is determine the threat levels of all of the enemies and rank them in terms of priority. I need to set a default priority for enemy player
                //then I need to for each one determine if it's worth killing it or not. if it's not worth killing then don't bother looking at any more of the lower ranks and just hit player
                //if it is worth killing find the most efficient way of killing it, basically try taking it out with the minimum price loss

                //off topic but might be worth storing every copy of a card in a static dictionary? Though I guess that would make me have to add current health and a bunch of stuff so prob not



                //relic of the past, will be replaced by gamestate tracking
                //maybe check to see if we even have enough mana for anything in our hand first
                var enemiesThatHaveGuard = new List<Card>();
                var enemies = new SortedDictionary<double, Card>();
                foreach(var card in GameState.EnemyField)
                {
                    var threatLevel = card.DetermineThreatLevel();
                    if (card.Abilities[3] =='G')
                    {
                        enemiesThatHaveGuard.Add(card);
                        threatLevel += 100;
                    }
                    if(card.Abilities[0] == 'B') threatLevel += 20;
                    if (card.Abilities[0] == 'L') threatLevel -= 40;
                    enemies[threatLevel] = card;
                }
                foreach(var card in enemies){ Console.Error.Write($"{card.Key}: {card.Value}"); }
                foreach (var card in enemiesThatHaveGuard) { Console.Error.Write($"{card}"); }
                //look through PlayerHand, PlayerField. use Player and Enemy for health data

                var cardsInHand = new SortedDictionary<double, Card>();
                foreach (var card in GameState.PlayerHand)
                {
                    var threatLevel = card.DetermineCurrentPlayability();
                    cardsInHand[threatLevel] = card;
                }
                foreach (var card in enemies) { Console.Error.Write($"{card.Key}: {card.Value}"); }


                //sort cards by priority for attacking
                var myUnits = new SortedDictionary<double, Card>();
                foreach (var card in GameState.PlayerField)
                {
                    var threatLevel = card.DetermineThreatLevel();
                    myUnits[threatLevel] = card;
                }
                foreach (var card in enemies) { Console.Error.Write($"{card.Key}: {card.Value}"); }

                //play all the cards you can
                var currentMana = GameState.Player.Mana;
                foreach(var card in cardsInHand)
                {
                    if (card.Value.Cost <= currentMana)
                    {
                        currentMana -= card.Value.Cost;
                        commands.Add($"SUMMON {card.Value.InstanceId}");
                        if (card.Value.Abilities[1] == 'C')
                        {
                            myUnits.Add(card.Key,card.Value);
                        }
                    }
                }

                //attack with as many cards as you can
                foreach (var card in myUnits)
                {
                    KeyValuePair<double, Card>? potentialTarget = null;
                    if (enemies.Count > 0) { potentialTarget = enemies.First(); }

                    if (potentialTarget == null)
                    {
                        commands.Add($"ATTACK {card.Value.InstanceId} -1");
                    }
                    else
                    {
                        commands.Add($"ATTACK {card.Value.InstanceId} {Attack(potentialTarget)}");
                        if (card.Value.Attack > potentialTarget.Value.Value.Defense)
                        {
                            if (enemies.Count > 0)
                            {
                                enemies.Remove(enemies.Keys.First());
                            }
                        }
                    }
                }

                //now have access to runes so I should use them in calculations in the future

                //commands.Add(UseItem());
                //split up summons so that you can summon more after attacking and only summon charge creatures before

                if (commands != null && commands.Any())
                {
                    return String.Join(';', commands);
                }
                else
                {
                    return "PASS";
                }
                Console.Error.WriteLine(watch.ElapsedMilliseconds);
            }
        }

        private string DraftCard()
        {
            //update this so that it creates a solid mana curve at the very least
            //ideally develop some sort of rarity scheme or something?
            //maybe value specific keywords over others, maybe value groupings too? tribal decks and such
            var choices = GameState.PlayerHand;
            var bestChoice = 0;
            double bestWorth = 0;
            for (var cardPosition = 0; cardPosition < choices.Count; cardPosition++){
                
                var worth = choices[cardPosition].DetermineWorth(GetCostOffset());
                Console.Error.WriteLine(worth);
                if (worth > bestWorth)
                {
                    bestWorth = worth;
                    bestChoice = cardPosition;
                }
            }

            _cardCosts.Add(choices[bestChoice].Cost);
            return $"PICK {bestChoice}";
        }

        public int Attack(KeyValuePair<double, Card>? enemyCard)
        {
            if (enemyCard == null) { return enemyCard.Value.Value.InstanceId; }
            var enemyThreat = 50;
            return enemyCard.Value.Key > enemyThreat ? enemyCard.Value.Value.InstanceId : -1;
        }

        private string SummonCreature()
        {
            //prioritize playing on curve and such, maybe shift from list to hash to help with hitting mana curve
            //prioritize things that are relevant to enemies and such
            var commands = new List<string>();
            var currentMana = GameState.Player.Mana;
            foreach (var card in GameState.PlayerHand)
            {
                if (card.Cost <= currentMana)
                {
                    if (!(card.MyHealthChange > 0 && GameState.Player.Health > (30 - card.MyHealthChange)))
                    {
                        commands.Add($"SUMMON {card.InstanceId}");
                        currentMana -= card.Cost;
                        Console.Error.WriteLine($"Summoned Card's Abilities:{card.Abilities}");
                        if (card.Abilities.Contains("C"))
                        {
                            GameState.PlayerField.Add(card);
                        }
                    }
                }
            }
            return commands.FirstOrDefault();
        }

        private string UseItem()
        {
            return "";
        }

        private double GetCostOffset()
        {
            if (_cardCosts == null || _cardCosts.Count == 0) { return 0; }
            var currAvgCost =_cardCosts.Sum() / _cardCosts.Count;
            return _goalAvgCost - currAvgCost;
        }
    }
}
