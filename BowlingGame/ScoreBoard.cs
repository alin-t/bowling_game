namespace BowlingGame
{
    using System;
    using System.Collections.Generic;

    public class ScoreBoard
    {
        private readonly Dictionary<int, int> scoresPerFrames = new Dictionary<int, int>();

        public void AddScore(int frame, int score)
        {
            scoresPerFrames.Add(frame, score);
        }

        public int ScoreForFrame(int frame)
        {
            return scoresPerFrames.ContainsKey(frame) ? scoresPerFrames[frame] : 0;
        }

        public void Display()
        {
            foreach (var scoresPerFrame in scoresPerFrames)
            {
                Console.WriteLine(string.Format("Frame {0} has score {1}", scoresPerFrame.Key, scoresPerFrame.Value));
            }
        }
    }
}
