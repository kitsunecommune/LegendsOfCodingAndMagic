using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaM.Model
{
    public class Legend
    {
        public readonly int Health;
        public readonly int Mana;
        public readonly int Deck;
        public readonly int Rune;
        public readonly int Draw;

        public Legend(string[] inputs)
        {
            Health = int.Parse(inputs[0]);
            Mana = int.Parse(inputs[1]);
            Deck = int.Parse(inputs[2]);
            Rune = int.Parse(inputs[3]);
            Draw = int.Parse(inputs[4]);
        }

        public override string ToString()
        {
            return $"{Health} {Mana} {Deck} {Rune} {Draw}";
        }
    }
}
