// VU PHAN HOANG AN - 104775412.
// A Facade class to hide the complexities of the Game class against the main
// Program loop.

namespace IceMissileArena
{
    public class GameFacade
    {
        private Game _game;

        // Constructor.
        public GameFacade()
        {
            _game = new Game();
        }

        // Getter-Setter.
        public Game Game
        {
            get => _game;
            set => _game = value;
        }

        // Methods.
        public void DrawGame() // Draw the game components.
        {
            Game.DrawGameBasedOnState();
        }

        public void HandleInputs() // Handle user inputs (i.e. mouse and key inputs).
        {
            Game.HandleInputs();
        }

        public void UpdateGame() // Update the game's state (collisions, boosts, etc.).
        {
            Game.UpdateGame();
        }

        public bool IsGameOver() // Check for GameOver.
        {
            return Game.State == GameStates.GameOver;
        }

        public void RestartGame() // Restart the game.
        {
            Game = new Game();
        }
    }
}