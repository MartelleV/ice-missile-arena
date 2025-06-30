// VU PHAN HOANG AN - 104775412.
// A class that connects all the other classes in the game
// and get them to work together with a more simplified interface.

namespace IceMissileArena
{
    // Enum to manage different game states.
    public enum GameStates
    {
        Welcome,
        Playing,
        Paused,
        GameOver,
    }

    // Game class.
    public class Game
    {
        // Properties of a game.
        private GameStates _gameState; // Its current state.
        private LauncherHead _ingameLauncherHead = LauncherHead.GetInstance(); // Its launcher head.
        private List<Turrets> _ingameTurrets = new(); // Its turrets.
        private List<Missiles> _ingameMissiles = new(); // Its missiles.
        private List<Explosions> _ingameExplosions = new(); // Its explosions.
        private List<Boosts> _ingameBoosts = new(); // Its boosts.
        private BoostObserver _boostObserver = new(); // Its boost observer.
        // Other gameplay stuffs: levels, time, score and highscores.
        private int _difficultyLevel;
        private int _secondsUntilNextEnemyLaunch;
        private int _secondsUntilNextDifficultyLevel;
        private int _score;
        private List<int> _highScoreList = new();

        // Constructor.
        public Game()
        {
            _gameState = GameStates.Welcome; // Starting State is always Welcome.
            _score = 0;
            _secondsUntilNextEnemyLaunch = 180; // Launch the first missile 3 seconds after the game begins (60 = 1 second).
            _difficultyLevel = Constants.NewGameStartingDifficulty;
            _secondsUntilNextDifficultyLevel = Constants.EnemyLaunchesInEachLevel;

            // Add turrets with the corresponding locations.
            for (int i = 0; i < 6; i++)
            {
                _ingameTurrets.Add(new Turrets(i + 1));
            }

            // Ensure the BoostObserver is attached to observe boosts.
            foreach (Boosts boost in _ingameBoosts)
            {
                boost.Observers.Add(_boostObserver);
            }

            // Read the high scores from the file.
            HighScores.GetInstance().ReadHighScoresFromFile(_highScoreList);
        }

        // Getters-Setters.
        public GameStates State
        {
            get => _gameState;
            set => _gameState = value;
        }

        public LauncherHead IngameLauncherHead
        {
            get => _ingameLauncherHead;
            set => _ingameLauncherHead = value;
        }

        public List<Turrets> IngameTurrets
        {
            get => _ingameTurrets;
            set => _ingameTurrets = value;
        }

        public List<Missiles> IngameMissiles
        {
            get => _ingameMissiles;
            set => _ingameMissiles = value;
        }

        public List<Explosions> IngameExplosions
        {
            get => _ingameExplosions;
            set => _ingameExplosions = value;
        }

        public List<Boosts> IngameBoosts
        {
            get => _ingameBoosts;
            set => _ingameBoosts = value;
        }

        public BoostObserver BoostObserver
        {
            get => _boostObserver;
            set => _boostObserver = value;
        }

        public int DifficultyLevel
        {
            get => _difficultyLevel;
            set => _difficultyLevel = value;
        }

        public int SecondsUntilNextEnemyLaunch
        {
            get => _secondsUntilNextEnemyLaunch;
            set => _secondsUntilNextEnemyLaunch = value;
        }

        public int SecondsUntilNextDifficultyLevel
        {
            get => _secondsUntilNextDifficultyLevel;
            set => _secondsUntilNextDifficultyLevel = value;
        }

        public int Score
        {
            get => _score;
            set => _score = value;
        }

        public List<int> HighScoreList
        {
            get => _highScoreList;
            set => _highScoreList = value;
        }

        // Methods.
        public void DrawScenery() // Draw the game's scenery and backdrop.
        {
            // Load the bitmaps.
            Bitmap background = SplashKit.LoadBitmap("Background", "resources/images/background.png");
            Bitmap ground = SplashKit.LoadBitmap("Ground", "resources/images/ground.png");
            Bitmap launcherBase = SplashKit.LoadBitmap("LauncherBase", "resources/images/launcher-base.png");
            Bitmap button = State == GameStates.Paused
                ? SplashKit.LoadBitmap("PauseButton", "resources/images/pause-button.png")
                : SplashKit.LoadBitmap("PlayButton", "resources/images/play-button.png");

            // Draw background.
            SplashKit.DrawBitmap(background, 0, 0);
            // Draw Play/Pause Button on background's right corner.
            SplashKit.DrawBitmap(button, SplashKit.ScreenWidth() - button.Width, 5);

            // Draw continuous ground tiles with a loop (set groundX to 0 first).
            double groundX = 0;
            double groundY = SplashKit.ScreenHeight() - ground.Height;
            for (int i = 0; i < SplashKit.ScreenWidth(); i += ground.Width)
            {
                SplashKit.DrawBitmap(ground, groundX, groundY);
                groundX += ground.Width;
            }

            // Then change groundX to draw the static launcher base.
            groundX = (SplashKit.ScreenWidth() / 2) - (launcherBase.Height / 2);
            // Y coordinate for launcher base (right on the ground) tiles.
            groundY = SplashKit.ScreenHeight() - ground.Height - launcherBase.Height;
            // Draw the launcher base.
            SplashKit.DrawBitmap(launcherBase, groundX, groundY);
        }

        // Draw the welcome screen.
        public void DrawWelcomeScreen(List<int> highscores)
        {
            // Load font and bitmap.
            Font gameFont = SplashKit.LoadFont("BarlowBold", "resources/fonts/font.ttf");
            Bitmap welcome = SplashKit.LoadBitmap("Welcome", "resources/images/welcome.png");

            // Draw the Welcome bitmap.
            SplashKit.DrawBitmap(welcome, (SplashKit.ScreenWidth() - welcome.Width) / 2, 100);

            // Draw the texts to guide the user.
            SplashKit.DrawText("LEFT CLICK TO START PLAYING AND LAUNCH MISSILES", Color.DarkRed, gameFont, 30, 230, 460); // Dropback shadow.
            SplashKit.DrawText("LEFT CLICK TO START PLAYING AND LAUNCH MISSILES", Color.Black, gameFont, 30, 232, 462);

            SplashKit.DrawText("PRESS P TO PAUSE THE GAME", Color.DarkRed, gameFont, 30, 400, 500); // Dropback shadow.
            SplashKit.DrawText("PRESS P TO PAUSE THE GAME", Color.Black, gameFont, 30, 402, 502);

            // Draw the High scores and refresh the screen.
            HighScores.GetInstance().DrawHighScores(highscores);
            SplashKit.RefreshScreen();
        }

        // Draw the GameOver screen.
        public static void DrawGameOver(List<int> highscores, int score, int difficultyLevel)
        {
            // Load font and bitmap.
            Font gameFont = SplashKit.LoadFont("BarlowBold", "resources/fonts/font.ttf");
            Bitmap gameover = SplashKit.LoadBitmap("GameOver", "resources/images/gameover.png");

            // Draw the Gameover bitmap.
            SplashKit.DrawBitmap(gameover, (SplashKit.ScreenWidth() - gameover.Width) / 2, 100);

            // Draw the result of user's game.
            SplashKit.DrawText($"YOU DEFENDED UNTIL LEVEL {difficultyLevel}th!", Color.DarkRed, gameFont, 40, 300, 460); // Dropback shadow.
            SplashKit.DrawText($"YOU DEFENDED UNTIL LEVEL {difficultyLevel}th!", Color.Black, gameFont, 40, 302, 462);

            SplashKit.DrawText($"YOUR FINAL SCORE: {score}", Color.DarkRed, gameFont, 40, 380, 500); // Dropback shadow.
            SplashKit.DrawText($"YOUR FINAL SCORE: {score}", Color.Black, gameFont, 40, 382, 502);

            HighScores.GetInstance().DrawHighScores(highscores); // Draw High scores.
        }

        // Draw the Pause Game screen.
        public static void DrawPauseGame()
        {
            // Load bitmap.
            Bitmap paused = SplashKit.LoadBitmap("Paused", "resources/images/paused.png");

            // Draw the Paused bitmap and refresh the screen.
            SplashKit.DrawBitmap(paused, (SplashKit.ScreenWidth() - paused.Width) / 2, 100);
            SplashKit.RefreshScreen();
        }

        // Draw the Game HUD (Heads-up Display), including the current level, score and boost icons.
        public static void DrawHud(int score, int difficultyLevel, BoostObserver boostObserver)
        {
            // Load font.
            Font gameFont = SplashKit.LoadFont("BarlowBold", "resources/fonts/font.ttf");

            // Draw the HUD's texts.
            SplashKit.DrawText($"LEVEL NO.: {difficultyLevel}", Color.DarkRed, gameFont, 20, 30, 30); // Dropback shadow.
            SplashKit.DrawText($"LEVEL NO.: {difficultyLevel}", Color.Black, gameFont, 20, 31, 31);

            SplashKit.DrawText($"SCORE: {score}", Color.DarkRed, gameFont, 20, 30, 50); // Dropback shadow.
            SplashKit.DrawText($"SCORE: {score}", Color.Black, gameFont, 20, 31, 51);

            // If boosts are active, draw boosts icons on the Game HUD.
            boostObserver.DrawBoostIcons(gameFont);
        }

        // Draw all game components.
        public void DrawGameComponents()
        {
            // Draw Game Scenery.
            DrawScenery();

            // Draw the game HUD.
            DrawHud(Score, DifficultyLevel, BoostObserver);

            // Draw game components: turrets, missiles, launcher head, moving boosts, explosions.
            Turrets.DrawTurrets(IngameTurrets);
            Missiles.DrawMissiles(IngameMissiles);
            Boosts.DrawBoostsMoving(IngameBoosts);
            Explosions.DrawExplosions(IngameExplosions);
            IngameLauncherHead.DrawLauncherHead();
        }

        // Draw based on the game state.
        public void DrawGameBasedOnState()
        {
            SplashKit.ClearScreen(); // Clear the screen.

            // Draw Game based on its state.
            switch (State)
            {
                // If at the start of the game, draw welcome screen.
                case GameStates.Welcome:
                    DrawGameComponents();

                    // Draw the Welcome Screen.
                    DrawWelcomeScreen(HighScoreList);
                    break;

                // If game is playing, only draw its components.
                case GameStates.Playing:
                    DrawGameComponents();
                    break;

                // If game is paused, draw paused game screen.
                case GameStates.Paused:
                    DrawGameComponents();

                    // Draw the Paused screen.
                    DrawPauseGame();
                    break;

                // If game is over, draw gameover screen.
                case GameStates.GameOver:
                    DrawGameComponents();

                    // Draw the Game Over Screen.
                    DrawGameOver(HighScoreList, Score, DifficultyLevel);
                    break;
            }

            SplashKit.RefreshScreen(60); // Refresh screen with 60FPS rate.
        }

        // Handle the user inputs.
        public void HandleInputs()
        {
            SplashKit.ProcessEvents();

            // Track user's inputs based on 2 different states of the game.
            switch (State)
            {
                // Left mouse click to start playing.
                case GameStates.Welcome:
                    if (SplashKit.MouseClicked(MouseButton.LeftButton))
                    {
                        State = GameStates.Playing;
                    }
                    break;

                // Handle inputs while playing.
                case GameStates.Playing:
                    // Track user's left mouse clicks to launch missiles to the target click position
                    // (only if the launcher head is reloaded).
                    if (SplashKit.MouseClicked(MouseButton.LeftButton) && IngameLauncherHead.ReloadCountdown == 0)
                    {
                        Point2D mouseXY = SplashKit.MousePosition();
                        Missiles gameMissile = MissileFactory.CreateMissile(MissileTeam.Player, IngameLauncherHead.LauncherHeadXY, mouseXY, BoostObserver);
                        IngameMissiles.Add(gameMissile);

                        if (BoostObserver.IncreaseReloadSpeedCountdown > 0)
                        {
                            IngameLauncherHead.ReloadCountdown = Constants.ReloadTimerCountdownBoost;
                        }
                        else
                        {
                            IngameLauncherHead.ReloadCountdown = Constants.ReloadTimerCountdown;
                        }
                    }
                    // Track user's right mouse clicks to clear screen
                    // (only if user has the boost's ability).
                    if (SplashKit.MouseClicked(MouseButton.RightButton) && BoostObserver.CanClearScreen)
                    {
                        Score = ClearScreenBoost(IngameMissiles, IngameExplosions, Score);
                        BoostObserver.CanClearScreen = false;
                    }
                    // Track user's P key press to pause the game.
                    if (SplashKit.KeyTyped(KeyCode.PKey))
                    {
                        State = GameStates.Paused;
                    }
                    break;

                // Handle inputs if paused.
                case GameStates.Paused:
                    if (SplashKit.KeyTyped(KeyCode.PKey))
                    {
                        State = GameStates.Playing;
                    }
                    break;
            }
        }

        // Check missiles' collisions with other objects.
        public void CheckMissileCollisions()
        {
            int yGroundLevel = SplashKit.ScreenHeight() - 100; // Set ground level.

            // Iterate over missiles in reverse order to prevent errors.
            // Check missiles' collisions.
            for (int i = IngameMissiles.Count - 1; i >= 0; i--)
            {
                // If missile hasn't hit the ground yet (this is for enemy missiles).
                if (SplashKit.SpriteY(IngameMissiles[i].MissileSprite) > yGroundLevel)
                {
                    IExplosionBehavior explosionBehavior = new NormalExplosionBehavior(); // Default Explosion strategy for a normal missile.
                    if (BoostObserver.IncreaseMissileSizeCountdown > 0) // If Size Boost is active.
                    {
                        explosionBehavior = new BoostedExplosionBehavior(); // Boosted Explosion strategy applied.
                    }
                    // Create a new explosion and remove the missile.
                    Point2D missileCenter = new Point2D {
                        X = SplashKit.SpriteX(IngameMissiles[i].MissileSprite) + SplashKit.SpriteWidth(IngameMissiles[i].MissileSprite) / 2,
                        Y = SplashKit.SpriteY(IngameMissiles[i].MissileSprite) + SplashKit.SpriteHeight(IngameMissiles[i].MissileSprite) / 2
                    };
                    Explosions gameExplosion = new(IngameMissiles[i].MissileTeam, missileCenter, BoostObserver.IncreaseMissileSizeCountdown, explosionBehavior);
                    IngameExplosions.Add(gameExplosion);
                    IngameMissiles.RemoveAt(i);
                    continue;
                }

                // If missile hits its target point (this is for player's missiles).
                if (SplashKit.SpriteCollision(IngameMissiles[i].MissileSprite, IngameMissiles[i].TargetSprite))
                {
                    IExplosionBehavior explosionBehavior = new NormalExplosionBehavior(); // Default Explosion strategy.
                    if (BoostObserver.IncreaseMissileSizeCountdown > 0) // If Size Boost is active.
                    {
                        explosionBehavior = new BoostedExplosionBehavior();
                    }
                    // Create a new explosion and remove the missile.
                    Point2D targetCenter = new Point2D{
                        X = SplashKit.SpriteX(IngameMissiles[i].TargetSprite) + SplashKit.SpriteWidth(IngameMissiles[i].TargetSprite) / 2,
                        Y = SplashKit.SpriteY(IngameMissiles[i].TargetSprite) + SplashKit.SpriteHeight(IngameMissiles[i].TargetSprite) / 2
                    };
                    Explosions gameExplosion = new(IngameMissiles[i].MissileTeam, targetCenter, BoostObserver.IncreaseMissileSizeCountdown, explosionBehavior);
                    IngameExplosions.Add(gameExplosion);
                    IngameMissiles.RemoveAt(i);
                    continue;
                }

                // If any missile hits a turret.
                for (int j = 0; j < IngameTurrets.Count; j++)
                {
                    if (SplashKit.SpriteCollision(IngameMissiles[i].MissileSprite, IngameTurrets[j].TurretSprite))
                    {
                        IExplosionBehavior explosionBehavior = new NormalExplosionBehavior(); // Default Explosion strategy.
                        if (BoostObserver.IncreaseMissileSizeCountdown > 0) // If Size Boost is active.
                        {
                            explosionBehavior = new BoostedExplosionBehavior();
                        }
                        // Create a new explosion and remove the missile.
                        Point2D missileCenter = new Point2D {
                            X = SplashKit.SpriteX(IngameMissiles[i].MissileSprite) + SplashKit.SpriteWidth(IngameMissiles[i].MissileSprite) / 2,
                            Y = SplashKit.SpriteY(IngameMissiles[i].MissileSprite) + SplashKit.SpriteHeight(IngameMissiles[i].MissileSprite) / 2
                        };
                        Explosions gameExplosion = new(IngameMissiles[i].MissileTeam, missileCenter, BoostObserver.IncreaseMissileSizeCountdown, explosionBehavior);
                        IngameExplosions.Add(gameExplosion);
                        IngameMissiles.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        // Check explosions' collisions with other objects.
        public void CheckExplosionCollisions()
        {
            for (int i = 0; i < IngameExplosions.Count; i++)
            {
                // Check collision with turrets.
                for (int j = 0; j < IngameTurrets.Count; j++)
                {
                    if (SplashKit.SpriteCollision(IngameExplosions[i].ExplosionSprite, IngameTurrets[j].TurretSprite))
                    {
                        // Hit turret -> turret is destroyed.
                        IngameTurrets[j].TurretSpriteDestroyed();
                        IngameTurrets[j].IsDestroyed = true;
                    }
                }

                // Check collision with missiles.
                for (int j = 0; j < IngameMissiles.Count; j++)
                {
                    if (SplashKit.SpriteCollision(IngameExplosions[i].ExplosionSprite, IngameMissiles[j].MissileSprite))
                    {
                        if (IngameMissiles[j].MissileTeam == MissileTeam.Enemy)
                        { // If explosion hits an enemy missile -> increase points.
                            Score += Constants.PointsForEachMissileKill;
                        }
                        IngameMissiles.RemoveAt(j); // Remove the missile.
                    }
                }

                // Check collision with boosts.
                for (int j = 0; j < IngameBoosts.Count; j++)
                {
                    if (SplashKit.SpriteCollision(IngameExplosions[i].ExplosionSprite, IngameBoosts[j].BoostSprite))
                    { // Hits the boost -> apply it and delete the moving boost sprite.
                        IngameBoosts[j].ApplyBoost();
                        IngameBoosts.RemoveAt(j);
                    }
                }
            }
        }

        // Function for clear screen boost.
        public static int ClearScreenBoost(List<Missiles> missiles, List<Explosions> explosions, int score)
        {
            for (int i = missiles.Count - 1; i >= 0; i--)
            {
                // Create explosion at each missile.
                Point2D missileCenter = new Point2D {
                    X = SplashKit.SpriteX(missiles[i].MissileSprite) + SplashKit.SpriteWidth(missiles[i].MissileSprite) / 2,
                    Y = SplashKit.SpriteY(missiles[i].MissileSprite) + SplashKit.SpriteHeight(missiles[i].MissileSprite) / 2
                };
                Explosions gameExplosion = new(missiles[i].MissileTeam, missileCenter, 0, new NormalExplosionBehavior());
                explosions.Add(gameExplosion);
                // Remove enemy missile.
                missiles.RemoveAt(i);
                score += Constants.PointsForEachMissileKill; // Add points for each missile cleared.
            }

            return score;
        }

        // Update the game's state.
        public void UpdateGame()
        {
            if (State != GameStates.Playing) return; // If game is not playing, stop updating the game's state.

            // Check launcher head's reload time and head movements.
            IngameLauncherHead.UpdateLauncherHead();

            // Manage active explosions and missiles on screen.
            Explosions.UpdateExplosion(IngameExplosions, BoostObserver);
            Missiles.UpdateMissiles(IngameMissiles);

            // Check incoming boosts.
            Boosts.CheckBoostsMoving(IngameBoosts);

            // Check collisions between objects in the game.
            CheckMissileCollisions();
            CheckExplosionCollisions();

            // Update countdown time of boosts.
            BoostObserver.DecrementBoostCountdown();

            // Check for number of intact buildings.
            int turretsStanding = Turrets.TurretsStillStanding(IngameTurrets);
            if (turretsStanding < 1) // If standing turrets go below 1, game is over.
            {
                State = GameStates.GameOver;
                // Check player's current score to see if it's a high score.
                HighScores.GetInstance().CheckHighScores(HighScoreList, Score);
            }

            // If enemy still launches missiles.
            if (SecondsUntilNextEnemyLaunch < 0)
            {
                for (int i = 0; i < DifficultyLevel; i++)
                {
                    // Generate a new wave.
                    Missiles.GenerateEnemyMissiles(IngameMissiles, BoostObserver);
                }
                SecondsUntilNextEnemyLaunch = Constants.TimeBetweenEnemyMissiles;
                SecondsUntilNextDifficultyLevel--; // Decrement to gradually reach to the next level.

                // Randomly roll a new boost.
                if (SplashKit.Rnd() < Constants.ChanceOfBoostAppearance)
                {
                    Boosts boost = new();
                    boost.Observers.Add(BoostObserver);
                    IngameBoosts.Add(boost);
                }

                // Bonus points for turrets after each level.
                if (SecondsUntilNextDifficultyLevel <= 0)
                {
                    Score += turretsStanding * DifficultyLevel * Constants.PointsForTurretsStandingAfterEachLevel;
                    DifficultyLevel++; // Increase difficulty.
                    SecondsUntilNextDifficultyLevel = Constants.EnemyLaunchesInEachLevel;
                    SecondsUntilNextEnemyLaunch = Constants.BreatherTimeBeforeNextLevel;
                    Boosts boost = new();
                    boost.Observers.Add(BoostObserver);
                    IngameBoosts.Add(boost);
                }
            }
            SecondsUntilNextEnemyLaunch--;
        }
    }
}
