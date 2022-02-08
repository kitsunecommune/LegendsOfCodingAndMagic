using System;
using System.IO;
using System.Text;
using System.Collections;
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
        public int OpponentHandSize;
        public int OpponentTurnActions;

        public void ReadDataIn()
        {
            string[] inputs;

            var players = new List<Legend>();
            for (int i = 0; i < 2; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                var player = new Legend(inputs);
                players.Add(player);
            }
            Player = players[0];
            Enemy = players[1];

            //gonna be a while before I use any of these values
            //
            inputs = Console.ReadLine().Split(' ');
            OpponentHandSize = int.Parse(inputs[0]);
            OpponentTurnActions = int.Parse(inputs[1]);

            for (int i = 0; i < OpponentTurnActions; i++)
            {
                string cardNumberAndAction = Console.ReadLine();
                Console.Error.WriteLine($"Opponent Action: {cardNumberAndAction}");
            }
            //add cardNumberAndAction to gameInfo at some point
            //
            //opponent actions are really only useful for debugging at this point

            int cardCount = int.Parse(Console.ReadLine());
            Console.Error.WriteLine($"cardCount is {cardCount}");
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
            var playersInfo = $"Player: {Player} Enemy: {Enemy} {OpponentHandSize} {OpponentTurnActions}";
            var playerHand = string.Join(Environment.NewLine, PlayerHand);
            var playerField = string.Join(Environment.NewLine, PlayerField);
            var enemyField = string.Join(Environment.NewLine, EnemyField);
            return $"{playersInfo} Cards in hand: {playerHand}, Cards on player's field: {playerField}, Cards on enemy's field: {enemyField}";
        }
    }
}