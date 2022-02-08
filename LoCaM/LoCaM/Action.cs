using LoCaM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaM
{
    public class Action
    {
        private GameState _gameState;
        public Action(GameState gameState)
        {
            _gameState = gameState;
        }

        public string TakeTurn(string phase)
        {
            if (phase == "Draft")
            {
                return DraftCard();
            }
            else
            {
                var commands = new List<string>();

                commands.AddRange(SummonCreature());
                //commands.Add(UseItem());
                commands.AddRange(Attack());
                //split up summons so that you can summon more after attacking and only summon charge creatures before

                if (commands != null && commands.Any())
                {
                    return String.Join(';', commands);
                }
                else
                {
                    return "PASS";
                }
            }
        }

        public string DraftCard()
        {
            //update this so that it creates a solid mana curve at the very least
            //ideally develop some sort of rarity scheme or something?
            //maybe value specific keywords over others, maybe value groupings too? tribal decks and such
            var choices = _gameState.PlayerHand;
            var bestChoice = 0;
            double bestWorth = 0;
            for (var cardPosition = 0; cardPosition < choices.Count; cardPosition++){
                
                var worth = choices[cardPosition].DetermineWorth();
                Console.Error.WriteLine(worth);
                if (worth > bestWorth)
                {
                    bestWorth = worth;
                    bestChoice = cardPosition;
                }
            }

            return $"PICK {bestChoice}";
        }

        public List<string> Attack()
        {
            //prioritize enemies that have low health and won't outweigh my own damange
            //also prioritize killing enemies with breakthrough (and maybe Guard? if that's not automatically done)
            var commands = new List<string>();
            var enemiesThatHaveGuard = new List<Card>();

            foreach (var card in _gameState.EnemyField)
            {
                if (card.Abilities.Contains("G") || card.Abilities.Contains("B"))
                {
                    enemiesThatHaveGuard.Add(card);
                }
            }
            var enemyToAttack = -1;

            foreach (var card in _gameState.PlayerField)
            {
                if (enemiesThatHaveGuard != null && enemiesThatHaveGuard.Any())
                {
                    enemyToAttack = enemiesThatHaveGuard[0].InstanceId;
                    Console.Error.WriteLine(enemiesThatHaveGuard[0]);
                    if (enemiesThatHaveGuard[0].Defense < card.Attack)
                    {
                        enemiesThatHaveGuard.RemoveAt(0);
                    }
                }
                switch (card.Abilities)
                {
                    case "B":

                        break;
                    default:
                        commands.Add($"ATTACK {card.InstanceId} {enemyToAttack}");
                        break;
                }
            }
            return commands;
        }

        public List<string> SummonCreature()
        {
            //prioritize playing on curve and such, maybe shift from list to hash to help with hitting mana curve
            //prioritize things that are relevant to enemies and such
            var commands = new List<string>();
            var currentMana = _gameState.Player.Mana;
            foreach (var card in _gameState.PlayerHand)
            {
                if (card.Cost <= currentMana)
                {
                    if (!(card.MyHealthChange > 0 && _gameState.Player.Health > (30 - card.MyHealthChange)))
                    {
                        commands.Add($"SUMMON {card.InstanceId}");
                        currentMana -= card.Cost;
                        Console.Error.WriteLine($"Summoned Card's Abilities:{card.Abilities}");
                        if (card.Abilities.Contains("C"))
                        {
                            _gameState.PlayerField.Add(card);
                        }
                    }
                }
            }
            return commands;
        }

        public string UseItem()
        {
            return "";
        }

    }
}
