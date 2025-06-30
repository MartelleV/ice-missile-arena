// VU PHAN HOANG AN - 104775412.
// A Strategy class to determine the behavior of the player's and the enemy missiles.

namespace IceMissileArena
{
    // For the Enemy Missiles.
    public class EnemyMissileBehavior : IMissileBehavior
    {
        public void InitializeMissile(Missiles missile, BoostObserver boostObserver)
        {
            // Set the missile's bitmaps, transparent target and speed.
            Bitmap targetBitmap = SplashKit.LoadBitmap("TransparentTarget", "resources/images/target-transparent.png");
            Bitmap missileBitmap = SplashKit.LoadBitmap("RedEnemyMissile", "resources/images/enemy-missile.png");
            int missileSpeed = Constants.EnemyMissileSpeed;

            missile.SetMissileProperties(missileSpeed, missileBitmap, targetBitmap);
        }
    }

    // For the Player Missiles.
    public class PlayerMissileBehavior : IMissileBehavior
    {
        public void InitializeMissile(Missiles missile, BoostObserver boostObserver)
        {
            // Set the missile's bitmaps and size/speed boosts (if applied).
            Bitmap targetBitmap = SplashKit.LoadBitmap("BlueTargetCursor", "resources/images/target-cursor.png");
            Bitmap missileBitmap = boostObserver.IncreaseMissileSizeCountdown > 0
                                ? SplashKit.LoadBitmap("BoostedMissile", "resources/images/boosted-missile.png")
                                : SplashKit.LoadBitmap("NormalMissile", "resources/images/normal-missile.png");

            int missileSpeed = boostObserver.IncreaseMissileSpeedCountdown > 0
                               ? Constants.PlayerMissileSpeedBoost
                               : Constants.PlayerMissileSpeed;

            missile.SetMissileProperties(missileSpeed, missileBitmap, targetBitmap);
        }
    }
}
