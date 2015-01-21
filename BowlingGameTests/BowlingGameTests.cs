namespace BowlingGameTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BowlingGame;
    using System;

    [TestClass]
    public class BowlingGameTests
    {
        private BowlingGame bowlingGame;

        [TestInitialize]
        public void TestInitialize()
        {
            bowlingGame = new BowlingGame(new Player() { Name = "Player 1" }, new Player() { Name = "Player 2" });
        }

        [TestMethod]
        public void NormalGame_EndsAfter10Rounds()
        {
            bowlingGame.ScoreMultipleThrows("1:1,4-2:10;1:4,5-2:10;1:6,4-2:10;1:5,5-2:10;1:10-2:10;1:0,1-2:10;1:7,3-2:10;1:6,4-2:10;1:10-2:10;1:2,8,6-2:10,10,10");

            ValidateScore(bowlingGame.GetPlayerOneScore(), "5,14,29,49,60,61,77,97,117,133");
            ValidateScore(bowlingGame.GetPlayerTwoScore(), "30,60,90,120,150,180,210,240,270,300");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PlayerOneDoesInvalidExtraShot_ExceptionShouldOccur()
        {
            bowlingGame.ScoreMultipleThrows("1:1,1,1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PlayerTwoDoesInvalidExtraShot_ExceptionShouldOccur()
        {
            bowlingGame.ScoreMultipleThrows("1:1,1-2:1,1,1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidNumberOfPinsDownedIsEnteredForPlayerOne_ExceptionShoudOccur()
        {
            bowlingGame.ScoreMultipleThrows("1:1,10");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidNumberOfPinsDownedIsEnteredForPlayerTwo_ExceptionShoudOccur()
        {
            bowlingGame.ScoreMultipleThrows("1:1,5-2:4,10");
        }

        [TestMethod]
        public void SpareFrameDuringGame_IsCalculatedCorrectly()
        {
            bowlingGame.ScoreMultipleThrows("1:5,5-2:4,6;1:5,0-2:3,3");
            ValidateScore(bowlingGame.GetPlayerOneScore(), "15,20,0,0,0,0,0,0,0,0");
            ValidateScore(bowlingGame.GetPlayerTwoScore(), "13,19,0,0,0,0,0,0,0,0");
        }
    
        [TestMethod]
        public void SpareFrameAtTheEndOfGame_IsCalculatedCorrectly()
        {
            bowlingGame.ScoreMultipleThrows("1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:5,5,5-2:4,6,7");
            ValidateScore(bowlingGame.GetPlayerOneScore(), "0,0,0,0,0,0,0,0,0,15");
            ValidateScore(bowlingGame.GetPlayerTwoScore(), "0,0,0,0,0,0,0,0,0,17");
        }

        [TestMethod]
        public void StrikeFrameDuringGame_IsCalculatedCorrectly()
        {
            bowlingGame.ScoreMultipleThrows("1:10-2:10;1:7,2-2:4,4");
            ValidateScore(bowlingGame.GetPlayerOneScore(), "19,28,0,0,0,0,0,0,0,0");
            ValidateScore(bowlingGame.GetPlayerTwoScore(), "18,26,0,0,0,0,0,0,0,0");
        }

        [TestMethod]
        public void StrikeFrameAtTheEndOfGame_IsCalculatedCorrectly()
        {
            bowlingGame.ScoreMultipleThrows("1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:0,0-2:0,0;1:10,10,10-2:10,10,10");
            ValidateScore(bowlingGame.GetPlayerOneScore(), "0,0,0,0,0,0,0,0,0,30");
            ValidateScore(bowlingGame.GetPlayerTwoScore(), "0,0,0,0,0,0,0,0,0,30");
        }

        public void ValidateScore(ScoreBoard scoreBoard, string score)
        {
            var scorePerFrames = score.Split(new char[] { ',' });
            for (int i = 0; i < scorePerFrames.Length; i++)
            {
                Assert.AreEqual(
                    int.Parse(scorePerFrames[i]),
                    scoreBoard.ScoreForFrame(i + 1),
                    string.Format("Expected score for frame {0} is {1} but actual is {2}", i + 1, scorePerFrames[i], scoreBoard.ScoreForFrame(i + 1)));
            }
        }
    }
}
