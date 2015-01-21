namespace BowlingGame
{
    using System;
    using System.Collections.Generic;

    public class BowlingGame
    {
        private readonly Player playerOne;
        private readonly Player playerTwo;

        private int currentRound = 1;

        public BowlingGame(Player playerOne, Player playerTwo)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
        }

        public ScoreBoard GetPlayerOneScore()
        {
            return ScoreCalculator.CalculateScore(playerOne);
        }

        public ScoreBoard GetPlayerTwoScore()
        {
            return ScoreCalculator.CalculateScore(playerTwo);
        }

        public void DisplayScore()
        {
            Console.WriteLine("Player one");
            ScoreBoard playerOneScore = ScoreCalculator.CalculateScore(playerOne);
            playerOneScore.Display();

            Console.WriteLine("Player two");
            ScoreBoard playerTwoScore = ScoreCalculator.CalculateScore(playerTwo);
            playerTwoScore.Display();
        }

        public void ScorePlayerOne(int pins)
        {
            if (HasMoreThrowsForRound(playerOne, currentRound) == false)
            {
                throw new InvalidOperationException("Player one has finished his round");
            }

            if (IsNumberOfPinsDownedValid(playerOne, pins) == false)
            {
                throw new InvalidOperationException("Invalid pins downed entered");
            }

            playerOne.ScoreShot(currentRound, pins);
        }

        public void ScorePlayerTwo(int pins)
        {
            if (HasMoreThrowsForRound(playerOne, currentRound))
            {
                throw new InvalidOperationException("Player one still has throws to do");
            }

            if (HasMoreThrowsForRound(playerTwo, currentRound) == false)
            {
                throw new InvalidOperationException("Player two has finished his round");
            }

            if (IsNumberOfPinsDownedValid(playerTwo, pins) == false)
            {
                throw new InvalidOperationException("Invalid pins downed entered");
            }

            playerTwo.ScoreShot(currentRound, pins);

            TryIncrementRound();
        }

        private bool IsNumberOfPinsDownedValid(Player player, int pins)
        {
            if (pins > Rules.PinsPerFrame) return false;
            if (currentRound < Rules.FramesPerGame && (player.TotalPinsForRound(currentRound) + pins) > Rules.PinsPerFrame) return false;
            return true;
        }

        private void TryIncrementRound()
        {
            if (HasMoreThrowsForRound(playerOne, currentRound)) return;
            if (HasMoreThrowsForRound(playerTwo, currentRound)) return;
            if (currentRound == Rules.FramesPerGame) return;

            currentRound++;
        }

        private static bool HasMoreThrowsForRound(Player player, int round)
        {
            List<Throw> throws = player.ThrowsForRound(round);
            int sum = 0;

            throws.ForEach(shot => sum += shot.Pins);
            if (sum < Rules.PinsPerFrame && throws.Count < 2)
            {
                return true;
            }

            // last round is spare
            if (round == Rules.FramesPerGame && sum == Rules.PinsPerFrame && throws.Count == 2)
            {
                return true;
            }

            // last round is strike
            if (round == Rules.FramesPerGame && throws.ToArray()[0].Pins == Rules.PinsPerFrame && throws.Count > 0 && throws.Count < 3)
            {
                return true;
            }

            return false;
        }
    }

    public static class BowlingGameExtensions
    {
        /// <summary>
        /// * Rounds are separated by ;
        /// * Player throws are separated by -
        /// * Throws are separated by ,
        /// </summary>
        /// <param name="bowlingGame"></param>
        /// <param name="throwsToScore"></param>
        public static void ScoreMultipleThrows(this BowlingGame bowlingGame, string throwsToScore)
        {
            var rounds = throwsToScore.Split(new char[] { ';' });

            foreach (var round in rounds)
            {
                var playerThrows = round.Split(new char[] { '-' });
                foreach (var playerThrow in playerThrows)
                {
                    var playerIndex = playerThrow.Split(new char[] { ':' })[0];
                    var throws = playerThrow.Split(new char[] { ':' })[1].Split(new char[] { ',' });

                    foreach (var throwValue in throws)
                    {
                        if (playerIndex.Equals("1"))
                        {
                            bowlingGame.ScorePlayerOne(int.Parse(throwValue));
                        }
                        else
                        {
                            bowlingGame.ScorePlayerTwo(int.Parse(throwValue));
                        }
                    }
                }
            }

        }
    }
}

