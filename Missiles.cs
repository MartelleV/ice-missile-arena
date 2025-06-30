// VU PHAN HOANG AN - 104775412.
// A Missiles class and a MissileTeam enum that use the Factory and Strategy patterns
// to create missiles and determine their behaviors based on the team.

namespace IceMissileArena
{
    // Enum for two missile teams.
    public enum MissileTeam
    {
        Player,
        Enemy,
    }

    // Missiles class.
    public class Missiles
    {
        // Properties of a missile.
        private readonly MissileTeam _missileTeam; // Its team/side.
        private Sprite _missileSprite; // Its sprite.
        private Sprite _targetSprite; // Its target's sprite.
        private Point2D _missileXY; // Its starting coordinates.
        private Point2D _targetXY; // Its target/ending coordinates.

        // Constructor.
        public Missiles(MissileTeam missileTeam, Point2D missileXY, Point2D targetXY)
        {
            _missileTeam = missileTeam;
            _missileXY = missileXY;
            _targetXY = targetXY;
        }

        // Getters, Setters.
        public MissileTeam MissileTeam
        {
            get => _missileTeam;
        }

        public Sprite MissileSprite
        {
            get => _missileSprite;
            set => _missileSprite = value;
        }

        public Sprite TargetSprite
        {
            get => _targetSprite;
            set => _targetSprite = value;
        }

        public Point2D OriginalMissileXY
        {
            get => _missileXY;
            set => _missileXY = value;
        }

        public Point2D TargetMissileXY
        {
            get => _targetXY;
            set => _targetXY = value;
        }

        // Methods.
        public void SetMissileProperties(int missileSpeed, Bitmap missileBitmap, Bitmap targetBitmap)
        {   // Set the missile properties for the Missile Behavior strategy classes.

            // Create the target sprites for the missiles.
            _targetSprite = SplashKit.CreateSprite(targetBitmap);
            SplashKit.SpriteSetX(_targetSprite, (float)(_targetXY.X - (SplashKit.BitmapWidth(targetBitmap) / 2)));
            SplashKit.SpriteSetY(_targetSprite, (float)(_targetXY.Y - (SplashKit.BitmapHeight(targetBitmap) / 2)));

            // Create the missile sprite and find the angle between the missile and the target point
            // to launch the missile from the launcher head.
            _missileSprite = SplashKit.CreateSprite(missileBitmap);
            double rotation = LauncherHead.FindAngle(_missileXY, _targetXY);
            SplashKit.SpriteSetRotation(_missileSprite, (float)rotation);
            SplashKit.SpriteSetX(_missileSprite, (float)(_missileXY.X - (SplashKit.BitmapWidth(missileBitmap) / 2)));
            SplashKit.SpriteSetY(_missileSprite, (float)(_missileXY.Y - (SplashKit.BitmapHeight(missileBitmap) / 2)));

            // Set the missile sprite moving.
            SplashKit.SpriteSetDx(_missileSprite, missileSpeed);
        }

        // Draw the missiles and targets sprites.
        public static void DrawMissiles(List<Missiles> missiles)
        {
            foreach (Missiles missile in missiles)
            {
                SplashKit.DrawSprite(missile.MissileSprite);
                SplashKit.DrawSprite(missile.TargetSprite);
            }
        }

        // Make new enemy missiles.
        public static void GenerateEnemyMissiles(List<Missiles> missiles, BoostObserver boostObserver)
        {
            // Random starting/ending X and Y coordinates for enemy missiles.
            Point2D originXY = SplashKit.PointAt(SplashKit.Rnd(50, SplashKit.ScreenWidth() - 50), -20);
            Point2D targetXY = SplashKit.PointAt(SplashKit.Rnd(50, SplashKit.ScreenWidth() - 50), SplashKit.ScreenHeight() - 96);
            // Create new enemy missile using the Factory.
            Missiles newEnemyMissile = MissileFactory.CreateMissile(MissileTeam.Enemy, originXY, targetXY, boostObserver);
            missiles.Add(newEnemyMissile);
        }

        // Update the moving missiles.
        public static void UpdateMissiles(List<Missiles> missiles)
        {   // Set screen boundaries.
            int xBoundaryLeft = -100;
            int xBoundaryRight = SplashKit.ScreenWidth() + 100;
            int yBoundaryTop = -100;

            for (int i = 0; i < missiles.Count; i++)
            {
                SplashKit.UpdateSprite(missiles[i].MissileSprite);

                // If missiles go off screen.
                if (SplashKit.SpriteX(missiles[i].MissileSprite) < xBoundaryLeft ||
                    SplashKit.SpriteX(missiles[i].MissileSprite) > xBoundaryRight ||
                    SplashKit.SpriteY(missiles[i].MissileSprite) < yBoundaryTop)
                {
                    missiles.RemoveAt(i); // Delete them.
                    i--;
                }
            }
        }
    }
}
