using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaM.Model
{
    public class Card
    {
        public readonly int Number;
        public readonly int InstanceId;
        public readonly int Location;
        public readonly CardType Type;
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
            Number = int.Parse(inputs[0]);
            InstanceId = int.Parse(inputs[1]);
            Location = int.Parse(inputs[2]);
            Type = (CardType)int.Parse(inputs[3]);
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
            var temp = $"Card Info: {Number} {InstanceId} {Location} {Type} Stats: {Cost} {Attack} {Defense}";
            return $"{temp} Abilities: {Abilities} {MyHealthChange} {OpponentHealthChange} {CardDraw}";
        }

        public double DetermineWorth(double avgCostOffset)//Need to workshop this and possibly make cost factor in a lot more (esp for creatures) ALSO make items get calculated better!
        {
            //give rating to items vs creatures...

            double score = Math.Abs(Defense + 2*Attack); // assuming that nothing is a mix since I didn't see anything like that in the card list

            if (Abilities[0] == 'B') //Breakthrough AKA trample
            {
                score += (Type == CardType.Creature ? 0.9 * (Attack-1) : 1); // atk = 1 is useless 2 is...just ok. when we get up to 5+ trample starts getting good
            }
            if (Abilities[1] == 'C') //Charge AKA haste 
            {
                score += 1;//don't much care about it as it just comes out quick (this will be higher in battle though based on the situation)
            }
            if (Abilities[2] == 'D')//Drain AKA lifelink
            {
                score += (Type == CardType.Creature ? 0.7 * Attack : 1); //lifelink is better than etb self heal but based on attack way more
            }
            if (Abilities[3] == 'G') //Guard
            {
                score += (Type == CardType.Creature ? 1.25 *Defense : 1); // can save your life and will be higher rated in battle but for drafting it isn't that that good
            }
            if (Abilities[4] == 'L')//Lethal AKA Deathtouch
            {
                score += (Type == CardType.Creature ? 2 * (12 - Attack) : 1);
            }
            if (Abilities[5] == 'W')//Ward AKA Divine Shield (ehhhh hearthstone instead of magic, but w/e, bite me)
            {
                score += (Type == CardType.Creature ? Attack+Defense : 1);
                if (Abilities[4] == 'L') score += 10;//Ward+Lethal are just too good a combo
            }
            if (MyHealthChange > 0) //etb self health effects mostly healing
            {
                score += 0.5*MyHealthChange;
            }
            if (OpponentHealthChange > 0) //etb one time enemy hits mostly damage
            {
                score += -OpponentHealthChange; //since it's usually negative want to add it correctly
            }
            if (CardDraw > 0)
            {
                score += 2*CardDraw; //card draw is amazing always, but especially in drafting
            }

            Console.Error.WriteLine($"{score} with offset {avgCostOffset} translates to:");
            if (avgCostOffset >= 0)
            {
                score *= 1 + (Cost - avgCostOffset)/(Cost+1);//+1 keeps divide by zero from happening
                //the higher the offset the more we want high cost cards
            }
            else
            {
                score *= 1 - Cost / (Cost - avgCostOffset);
                //the more negative the offset the more we want low cost cards
            }
            Console.Error.WriteLine($"{score} for card with cost {Cost}");
            return score;
        }

        public double DetermineThreatLevel()
        {
            //I want to only base it on current def and atk and constant abilities they have
            //this will end up being used to determine priority for my attacks and priority of enemies to kill
            return 1;
        }

        public int DetermineWinner()
        {
            //will return -1 for enemy win, 0 for both die, 1 for outright win (and -1 simply won't be an option if it's an item
            return 1;
        }

        public double DetermineCurrentPlayability()
        {
            //I want to base this on how much health 

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
            return 1;
        }
    }
}
