namespace BowlingGame
{
    using System.Collections.Generic;
    using System.Linq;

    public class ScoreCalculator
    {
        public static ScoreBoard CalculateScore(Player player)
        {
            var scoreBoard = new ScoreBoard();

            for (int i = 1; i <= Rules.FramesPerGame; i++)
            {
                if (player.ThrowsForRound(i).Count == 0) continue;

                int previousFrameScore = i > 0 ? scoreBoard.ScoreForFrame(i - 1) : 0;

                int frameScore = previousFrameScore + player.TotalPinsForRound(i);


                if (ShouldCalculateSpareBonus(player, i))
                {
                    frameScore += SpareExtraBonusForRound(player, i);
                }

                if (ShouldCalculateStrikeBonus(player, i))
                {
                    frameScore += StrikeExtraBonusForRound(player, i);
                }

                scoreBoard.AddScore(i, frameScore);
            }

            return scoreBoard;
        }

        private static int SpareExtraBonusForRound(Player player, int round)
        {
            var lastShot = player.ThrowsForRound(round)[1];
            var nextShot = ThrowForIndex(player, lastShot.Index + 1);
            if (nextShot != null)
                return nextShot.Pins;

            return 0;
        }

        private static int StrikeExtraBonusForRound(Player player, int round)
        {
            var score = 0;
            var lastShot = player.ThrowsForRound(round)[0];
            var nextShot = ThrowForIndex(player, lastShot.Index + 1);

            if (nextShot != null)
                score += nextShot.Pins;

            var nextNextShot = ThrowForIndex(player, lastShot.Index + 2);

            if (nextNextShot != null)
                score += nextNextShot.Pins;

            return score;
        }

        private static Throw ThrowForIndex(Player player, int index)
        {
            return player.Throws.Where(shot => shot.Index == index).FirstOrDefault();
        }

        private static int NextThrowValueForRound(Player player, int round)
        {
            List<Throw> throws = player.ThrowsForRound(round);

            if (throws.Count == 0) return 0;

            Throw lastThrowFromRound = throws[throws.Count - 1];

            return NextThrowValueForIndex(player, lastThrowFromRound.Index);
        }

        private static int NextThrowValueForIndex(Player player, int index)
        {
            Throw nextShot = ThrowForIndex(player, index + 1);

            if (nextShot == null) return 0;

            return nextShot.Pins;
        }

        private static bool ShouldCalculateSpareBonus(Player player, int round)
        {
            // no need to calculate extra bonus for last round since the bonus is calculate in the TotalPinsForRound
            if (round == Rules.FramesPerGame) return false;

            var throwsList = player.ThrowsForRound(round);

            if (throwsList.Count < 2) return false;

            return (throwsList[0].Pins + throwsList[1].Pins) == Rules.PinsPerFrame;
        }

        private static bool ShouldCalculateStrikeBonus(Player player, int round)
        {
            // no need to calculate extra bonus for last round since the bonus is calculate in the TotalPinsForRound
            if (round == Rules.FramesPerGame) return false;

            var throwsList = player.ThrowsForRound(round);

            if (throwsList.Count == 0) return false;

            return throwsList[0].Pins == Rules.PinsPerFrame;
        }
    }
}
