using System;
using System.Collections.Generic;
using LoCaM.Model;

namespace LoCaM
{
    public class GameState
    {
        public Legend Player;
        public Legend Enemy;
        public List<Card> PlayerHand;//doubles as drafting area
        public List<Card> PlayerField;
        public List<Card> EnemyField;
        public int EnemyHandSize;
        public int EnemyTurnActions;

        public void ReadDataIn()
        {
            string[] inputs;
            //read in players
            var players = new List<Legend>();
            for (int i = 0; i < 2; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                var player = new Legend(inputs);
                players.Add(player);
            }
            Player = players[0];
            Enemy = players[1];

            //gonna be a while before I use any of these values but this reads in enemy actions
            inputs = Console.ReadLine().Split(' ');
            EnemyHandSize = int.Parse(inputs[0]);
            EnemyTurnActions = int.Parse(inputs[1]);

            for (int i = 0; i < EnemyTurnActions; i++)
            {
                string cardNumberAndAction = Console.ReadLine();
                Console.Error.WriteLine($"Opponent Action: {cardNumberAndAction}");
            }
            //add cardNumberAndAction to GameState at some point
            //opponent actions are really only useful for debugging at this point

            //read in each of the cards
            int cardCount = int.Parse(Console.ReadLine());
            EnemyField = new List<Card>();
            PlayerField= new List<Card>();
            PlayerHand = new List<Card>();

            for (int i = 0; i < cardCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                var cardInfo = new Card(inputs);
                if (cardInfo.Location == 0)
                {
                    PlayerHand.Add(cardInfo);
                }
                else if (cardInfo.Location == 1)
                {
                    PlayerField.Add(cardInfo);
                }
                else
                {
                    EnemyField.Add(cardInfo);
                }
            }
        }

        public override string ToString()
        {
            var playersInfo = $"Player: {Player} Enemy: {Enemy}, {EnemyHandSize} {EnemyTurnActions}{Environment.NewLine}";
            var playerHand = $"Cards in hand:{Environment.NewLine}{string.Join(Environment.NewLine, PlayerHand)}{Environment.NewLine}";
            var playerField = $"Cards on player's field:{string.Join(Environment.NewLine, PlayerField)}{Environment.NewLine}";
            var enemyField = $"Cards on enemy's field:{string.Join(Environment.NewLine, EnemyField)}";

            return $"{playersInfo}{playerHand}{playerField}{enemyField}";
        }
    }
}