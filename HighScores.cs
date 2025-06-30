// VU PHAN HOANG AN - 104775412.
// A singleton class to manage the highscores in the game.

namespace IceMissileArena
{
	public class HighScores
	{
        // Static instance.
        private static HighScores instance = null!;

        // Private constructor.
        private HighScores()
        {
            // Empty.
        }

        // Instance method.
        public static HighScores GetInstance()
        {
            instance ??= new HighScores(); // If null, create a new instance.
            return instance;
        }

        // Static Methods. They act as helper methods to the other methods below.
        private static void QuickSort(List<int> values, int left, int right)
        { // Sort 10 high scores using Quick Sort algorithm - O(n.logn) average time complexity.
            if (left < right)
            {
                int pivotIndex = Partition(values, left, right);
                QuickSort(values, left, pivotIndex - 1);
                QuickSort(values, pivotIndex + 1, right);
            }
        }

        // Split the array of high scores with a pivot index.
        private static int Partition(List<int> values, int left, int right)
        {
            int pivot = values[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (values[j] >= pivot) // Change comparison for descending order.
                {
                    i++;
                    Swap(values, i, j);
                }
            }
            Swap(values, i + 1, right);
            return i + 1;
        }

        // Swap two values using tuples.
        private static void Swap(List<int> values, int a, int b)
        {
            (values[b], values[a]) = (values[a], values[b]);
        }

        // Trim the list of high scores if it exceeds 10.
        private static void TrimHighScores(List<int> highscores)
        {
            while (highscores.Count > 10)
            {
                highscores.RemoveAt(highscores.Count - 1);
            }
        }

        // Public methods that will be called on the Singleton instance.
        public void ReadHighScoresFromFile(List<int> highscores)
        {
            StreamReader fileReader = new("highscores.csv");
            using (fileReader)
            {
                string? inputString;
                while ((inputString = fileReader.ReadLine()) != null)
                {
                    if (int.TryParse(inputString, out _))
                    {
                        highscores.Add(int.Parse(inputString));
                    }
                }
            }
            fileReader.Close();
            QuickSort(highscores, 0, highscores.Count - 1);
            TrimHighScores(highscores);
        }

        public void WriteHighScoresToFile(List<int> highscores)
        {
            StreamWriter fileWriter = new("highscores.csv");
            using (fileWriter)
            {
                foreach (int score in highscores)
                {
                    fileWriter.WriteLine(score);
                }
            }
            fileWriter.Close();
        }

        // Checks if player has reached a new highscore (greater than the lowest/10th high score).
        public void CheckHighScores(List<int> highscores, int newHighscore)
        {
            if ((highscores.Count < 10) || (newHighscore > highscores[9]))
            {
                Font gameFont = SplashKit.LoadFont("BarlowBold", "resources/fonts/font.ttf");
                SplashKit.DisplayDialog("Congratulations!", "You've got a new highscore!", gameFont, 15);

                highscores.Add(newHighscore);

                QuickSort(highscores, 0, highscores.Count - 1);
                TrimHighScores(highscores);
                WriteHighScoresToFile(highscores);
            }
        }

        // Draw the high scores on the screen.
        public void DrawHighScores(List<int> highscores)
        {
            // Draw the font and the high score box.
            Font gameFont = SplashKit.LoadFont("BarlowBold", "resources/fonts/font.ttf");
            Color boxColor = Color.RGBAColor(0, 0, 0, 0.1);

            // Draw the box.
            SplashKit.FillRectangle(boxColor, 1030, 140, 200, 300);
            SplashKit.DrawRectangle(Color.DarkRed, 1030, 140, 200, 300);
            // Draw the HighScore title.
            SplashKit.DrawText("HIGHSCORES", Color.Black, gameFont, 25, 1057, 150); // Dropback shadow.
            SplashKit.DrawText("HIGHSCORES", Color.DarkRed, gameFont, 25, 1058, 151);

            int yPosition = 180; // Position of the first high score.
            // For every high score in the file:
            for (int i = 0; i < highscores.Count; i++)
            {
                // Draw the ranking.
                SplashKit.DrawText((i + 1).ToString(), Color.Black, gameFont, 25, 1040, yPosition);
                SplashKit.DrawText((i + 1).ToString(), Color.DarkRed, gameFont, 25, 1041, yPosition + 1); // Dropback shadow.

                // Draw the high score.
                SplashKit.DrawText(highscores[i].ToString(), Color.DarkRed, gameFont, 25, 1090, yPosition); // Dropback shadow.
                SplashKit.DrawText(highscores[i].ToString(), Color.Black, gameFont, 25, 1091, yPosition + 1);
                yPosition += 25;
            }
        }
	}
}
