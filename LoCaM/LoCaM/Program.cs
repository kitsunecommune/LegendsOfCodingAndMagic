using System;

namespace LoCaM
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameState = new GameState();
            var phase = "Draft";

            // game loop
            while (true)
            {
                var turnAction = "PASS";
                gameState.ReadDataIn();
                Console.Error.WriteLine(gameState);

                var action = new Action(gameState);

                if (gameState.Player.Mana > 0)
                {
                    phase = "Battle";
                }
                turnAction = action.TakeTurn(phase);
                Console.WriteLine(turnAction);
            }
        }
    }
}
