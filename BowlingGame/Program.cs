namespace BowlingGame
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var bowlingGame = new BowlingGame(new Player() { Name = "Player 1" }, new Player() { Name = "Player 2" });

            bowlingGame.ScoreMultipleThrows("1:1,4-2:10;1:4,5-2:10;1:6,4-2:10;1:5,5-2:10;1:10-2:10;1:0,1-2:10;1:7,3-2:10;1:6,4-2:10;1:10-2:10;1:2,8,6-2:10,10,10");

            bowlingGame.DisplayScore();

            Console.WriteLine("finish");
            Console.In.Read();
        }
    }
}
