// VU PHAN HOANG AN - 104775412.
// Main Game loop.

namespace IceMissileArena
{
	public class Program
	{
        public static void Main()
        {
            SplashKit.OpenWindow("Ice Missile Arena", 1200, 800); // Open game window.
            GameFacade gameFacade = new();

            gameFacade.DrawGame();
            // Game loop.
            while (!SplashKit.QuitRequested())
            {
                if (!gameFacade.IsGameOver())
                {
                    gameFacade.HandleInputs();
                    gameFacade.UpdateGame();
                    gameFacade.DrawGame();
                }
                else
                {
                    SplashKit.ProcessEvents();
                    if (SplashKit.KeyTyped(KeyCode.SpaceKey))
                    {
                        gameFacade.RestartGame();
                    }
                }
            }
        }
    }
}

