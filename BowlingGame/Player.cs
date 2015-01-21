namespace BowlingGame
{
    using System.Collections.Generic;
    using System.Linq;

    public class Player
    {
        public string Name { get; set; }
        private readonly List<Throw> throws = new List<Throw>();
        private int throwIndex;

        public void ScoreShot(int round, int pins)
        {
            throws.Add(new Throw() { Index = throwIndex++, Round = round, Pins = pins });
        }

        public List<Throw> ThrowsForRound(int round)
        {
            return throws.Where(shot => shot.Round == round).OrderBy(shot => shot.Index).ToList();
        }

        public int TotalPinsForRound(int round)
        {
            int totalForRound = 0;
            ThrowsForRound(round).ForEach(shot => totalForRound += shot.Pins);

            return totalForRound;
        }

        public List<Throw> Throws
        {
            get
            {
                return throws;
            }
        }        
    }
}
