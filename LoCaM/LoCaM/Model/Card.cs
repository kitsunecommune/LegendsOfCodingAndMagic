using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaM.Model
{
    public class Card
    {
        public readonly int CardNumber;
        public readonly int InstanceId;
        public readonly int Location;
        public readonly int CardType;
        public readonly int Cost;
        public readonly int Attack;
        public readonly int Defense;
        public readonly string Abilities;
        public readonly int MyHealthChange;
        public readonly int OpponentHealthChange;
        public readonly int CardDraw;
        private readonly string _abilities = "BCDGLW";

        public Card(string[] inputs)
        {
            CardNumber = int.Parse(inputs[0]);
            InstanceId = int.Parse(inputs[1]);
            Location = int.Parse(inputs[2]);
            CardType = int.Parse(inputs[3]);
            Cost = int.Parse(inputs[4]);
            Attack = int.Parse(inputs[5]);
            Defense = int.Parse(inputs[6]);
            Abilities = inputs[7];
            MyHealthChange = int.Parse(inputs[8]);
            OpponentHealthChange = int.Parse(inputs[9]);
            CardDraw = int.Parse(inputs[10]);
        }

        public override string ToString()
        {
            var temp = $"Card Info: {CardNumber} {InstanceId} {Location} {CardType} Stats: {Cost} {Attack} {Defense}";
            return $"{temp} Abilities: {Abilities} {MyHealthChange} {OpponentHealthChange} {CardDraw}";
        }

        public double DetermineWorth()
        {
            double score = 2*Defense + 5*Attack;
            //give rating to items vs creatures

            if (Abilities[0] == 'B') //Breakthrough AKA trample
            {
                score += 0.9 * (Attack-1);
            }
            if (Abilities[1] == 'C') //Charge AKA haste
            {
                score += 1;
            }
            if (Abilities[2] == 'D')
            {

            }
            if (Abilities[3] == 'G') //Guard
            {
                score += 2.25*Defense;
            }
            if (Abilities[4] == 'L')
            {
                
            }
            if (Abilities[5] == 'W')
            {

            }
            if (MyHealthChange > 0)
            {
                score += MyHealthChange;
            }
            if (OpponentHealthChange > 0)
            {
                score += 2 * OpponentHealthChange;
            }
            if (CardDraw > 0)
            {
                score += 4*CardDraw;
            }
            /*if (this.drain)
            {
                score += 0.7 * (double)this.attack;
            }
            if (this.lethal)
            {
                score += 20.0 - (double)this.attack;
            }
            if (this.ward)
            {
                score += (double)this.attack;
                score += 4;
            }
            if (this.ward && this.lethal)
            {
                score += 20.0 - (double)this.attack;
            }
            if (opponentHp < 10 && this.drain && n == 1)
            {
                score += (double)this.attack;
            }*/
            return score;
        }

        public double DetermineCurrentPlayability()
        {
            /*double score = 2 * (double)this.defense + 5 * (double)this.attack;

            if (opponentHp <= this.hpChangeOpponent && n == 0)
            {
                score += 1000;
            }

            if (this.guard && mineHp < 20 && n == 0)
            {
                score += 1.5 * (double)this.defense;
            }
            if (this.CardDraw > 0 && mineHp < 15 && n == 0)
            {
                score += 4;
            }
            if (this.breaktrough)
            {
                score += ((double)this.attack - 1.0) * 0.9;
            }
            if (this.drain)
            {
                score += 0.7 * (double)this.attack;
            }
            if (this.lethal)
            {
                score += 20.0 - (double)this.attack;
            }
            if (this.ward)
            {
                score += (double)this.attack;
                score += 4;
            }
            if (this.ward && this.lethal)
            {
                score += 20.0 - (double)this.attack;
            }

            if (mineHp < 10 && this.guard && n == 0)
            {
                score += 3 * (double)this.defense;
            }
            if (opponentHp < 10 && this.breaktrough && n == 0)
            {
                score += (double)this.attack;
            }
            if (opponentHp < 10 && this.drain && n == 1)
            {
                score += (double)this.attack;
            }
            return score;*/
            return 0;
        }
    }
}
