// VU PHAN HOANG AN - 104775412.
// A class to handle the explosions in the game, using the Strategy pattern
// of the IExplosionBehavior interface and the Explosion Behavior classes.

namespace IceMissileArena
{
    public class Explosions
    {
        // Properties of an Explosion.
        private Sprite _explosionSprite; // Its sprite.
        private Bitmap _explosionBitmap; // Its bitmap.
        private IExplosionBehavior _explosionBehavior; // Its behavior.

        // Constructor.
        public Explosions(MissileTeam missileTeam, Point2D centerXY, int increaseMissileSize, IExplosionBehavior explosionBehavior)
        {
            _explosionBitmap = SplashKit.LoadBitmap("Explosion", "resources/images/explosion.png");
            _explosionSprite = SplashKit.CreateSprite(_explosionBitmap); // Create sprite from explosion bitmap.
            _explosionBehavior = explosionBehavior; // Set explosion's behavior.

            // Set explosion's origin X and Y coordinates (with compensation to the bitmap size).
            SplashKit.SpriteSetX(_explosionSprite, (float)(centerXY.X - (SplashKit.BitmapWidth(_explosionBitmap) / 2)));
            SplashKit.SpriteSetY(_explosionSprite, (float)(centerXY.Y - (SplashKit.BitmapHeight(_explosionBitmap) / 2)));

            // Set Size Boost (scale a bigger explosion) only if the player has it.
            if (missileTeam == MissileTeam.Player && increaseMissileSize > 0)
            {
                SplashKit.SpriteSetScale(_explosionSprite, (float)Constants.PlayerMissileSizeBoost);
            }
        }

        // A series of Get-Set.
        public Sprite ExplosionSprite
        {
            get => _explosionSprite;
            set => _explosionSprite = value;
        }

        public Bitmap ExplosionBitmap
        {
            get => _explosionBitmap;
            set => _explosionBitmap = value;
        }

        public IExplosionBehavior ExplosionBehavior
        {
            get => _explosionBehavior;
            set => _explosionBehavior = value;
        }

        // Methods.
        public static void DrawExplosions(List<Explosions> explosions) // Draw the explosion sprites.
        {
            foreach (Explosions explosion in explosions)
            {
                SplashKit.DrawSprite(explosion.ExplosionSprite);
            }
        }

        // Update the explosions' existence. Once an explosion shrinks to 0, delete it.
        public static void UpdateExplosion(List<Explosions> explosions, BoostObserver boostObserver)
        {
            for (int i = 0; i < explosions.Count; i++)
            {
                SplashKit.UpdateSprite(explosions[i].ExplosionSprite);

                float rotation = SplashKit.SpriteRotation(explosions[i].ExplosionSprite);
                SplashKit.SpriteSetRotation(explosions[i].ExplosionSprite, rotation - Constants.ExplosionRotationRate); // Set default rotation rate for all explosions.
                float scale = SplashKit.SpriteScale(explosions[i].ExplosionSprite);

                if (scale > 0) // If explosion is still existing
                {
                    explosions[i].ExplosionBehavior.HandleExplosions(explosions[i].ExplosionSprite, boostObserver); // Set shrink rate based on explosion strategy.
                }
                else // The explosion has shrunk and disappeared
                {
                    explosions.RemoveAt(i); // Remove it.
                }
            }
        }
    }
}
