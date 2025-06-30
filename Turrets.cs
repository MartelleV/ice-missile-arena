// VU PHAN HOANG AN - 104775412.
// A class to manage the turrets.

namespace IceMissileArena
{
    public class Turrets
    {
        // Properties of a turret.
        private int _location; // Its location.
        private readonly Bitmap _turretBitmap; // Its bitmap.
        private Sprite _turretSprite; // Its sprite.
        private Sprite _destroyedTurretSprite; // Its sprite when destroyed.
        private bool _isDestroyed; // If it is destroyed or not.

        // Constructor.
        public Turrets(int location)
        {
            _destroyedTurretSprite = null!; // Ensure that this is not null during runtime.
            _turretBitmap = SplashKit.LoadBitmap($"TurretNo.{location}", $"resources/images/turret-{location}.png");

            // Place all turrets at bottom of screen, with 64 pixel buffer for the ground.
            float y = SplashKit.ScreenHeight() - SplashKit.BitmapHeight(_turretBitmap) - 64;

            // Set location and create sprite based on bitmap.
            _location = location;
            _turretSprite = SplashKit.CreateSprite(_turretBitmap);

            // Allocate location for each turret based on their number.
            float x = location switch
            {
                1 => 75,
                2 => 250,
                3 => 400,
                4 => SplashKit.ScreenWidth() - SplashKit.BitmapWidth(_turretBitmap) - 400,
                5 => SplashKit.ScreenWidth() - SplashKit.BitmapWidth(_turretBitmap) - 250,
                _ => SplashKit.ScreenWidth() - SplashKit.BitmapWidth(_turretBitmap) - 75,
            };

            // Set the sprite X and Y, and the initial state to NOT destroyed.
            SplashKit.SpriteSetX(_turretSprite, x);
            SplashKit.SpriteSetY(_turretSprite, y);
            _isDestroyed = false;
        }

        // Getters - Setters.
        public int Location
        {
            get => _location;
            set => _location = value;
        }

        public Sprite TurretSprite
        {
            get => _turretSprite;
            set => _turretSprite = value;
        }

        public Sprite DestroyedTurretSprite
        {
            get => _destroyedTurretSprite;
            set => _destroyedTurretSprite = value;
        }

        public bool IsDestroyed
        {
            get => _isDestroyed;
            set => _isDestroyed = value;
        }

        public Bitmap TurretBitmap
        {
            get => _turretBitmap;
        }

        // Methods.
        public static void DrawTurrets(List<Turrets> turrets)
        { // Draw the turrets.
            foreach (Turrets turret in turrets)
            {
                SplashKit.DrawSprite(turret.TurretSprite);
            }
        }

        public void TurretSpriteDestroyed()
        { // Replace turret sprite with turret destroyed sprite once it is destroyed.
            if (!IsDestroyed)
            {
                Bitmap turretDestroyedBitmap = SplashKit.LoadBitmap($"DestroyedTurretNo.{Location}", $"resources/images/turret-{Location}-destroyed.png");
                // Create destroyed sprite from bitmap.
                Sprite turretDestroyedSprite = SplashKit.CreateSprite(turretDestroyedBitmap);
                Point2D turretXY = SplashKit.SpritePosition(TurretSprite);
                // Compensate for y axis position, since the turret destroyed sprites are shorter than the original turret sprites.
                turretXY.Y = SplashKit.ScreenHeight() - SplashKit.BitmapHeight(turretDestroyedBitmap) - 64;
                SplashKit.SpriteSetPosition(turretDestroyedSprite, turretXY);

                TurretSprite = turretDestroyedSprite;
            }
        }

        // Count how many turrets are left.
        public static int TurretsStillStanding(List<Turrets> turrets)
        {
            int count = 0;

            foreach (Turrets turret in turrets)
            {
                if (!turret.IsDestroyed)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
