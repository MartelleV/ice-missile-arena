// VU PHAN HOANG AN - 104775412.
// Strategy classes to determine explosion behaviors based on the boost state.

namespace IceMissileArena
{
    // Class for Normal Explosions.
    public class NormalExplosionBehavior : IExplosionBehavior
    {
        // Handle Normal Explosion: set a quicker shrink rate.
        public void HandleExplosions(Sprite explosionSprite, BoostObserver boostObserver)
        {
            float scale = SplashKit.SpriteScale(explosionSprite);
            SplashKit.SpriteSetScale(explosionSprite, (float)(scale - Constants.ExplosionShrinkRate));
        }
    }

    // Class for Boosted Explosions.
    public class BoostedExplosionBehavior : IExplosionBehavior
    {
        // Handle Boosted Explosion: set a slower shrink rate (only if Size Boost is applied).
        public void HandleExplosions(Sprite explosionSprite, BoostObserver boostObserver)
        {
            float scale = SplashKit.SpriteScale(explosionSprite);
            if (boostObserver.IncreaseMissileSizeCountdown > 0)
            {
                SplashKit.SpriteSetScale(explosionSprite, (float)(scale - (Constants.ExplosionShrinkRate / 2)));
            }
            else
            {
                SplashKit.SpriteSetScale(explosionSprite, (float)(scale - Constants.ExplosionShrinkRate));
            }
        }
    }

    // More behaviors will be implemented in the future...
}
