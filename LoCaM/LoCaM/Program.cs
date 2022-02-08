using System;

namespace LoCaM
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameState = new GameState();
            var action = new Action();
            var phase = "Draft";

            // game loop
            while (true)
            {
                gameState.ReadDataIn();
                Console.Error.WriteLine(gameState);

                action.GameState = gameState;

                if (gameState.Player.Mana > 0)
                {
                    phase = "Battle";
                }
                Console.Error.WriteLine($"Entering gameState phase: {phase}");
                string turnAction = action.TakeTurn(phase);
                Console.WriteLine(turnAction);
            }
        }
    }
}
