// VU PHAN HOANG AN - 104775412.
// A static class to only contain the game constants.

namespace IceMissileArena
{
    public static class Constants
    {
        // Missiles and explosions.
        public const int PlayerMissileSpeed = 6;
        public const int EnemyMissileSpeed = 2;
        public const int ReloadTimerCountdown = 60;          // Time for launcher to reload (60 = 1 second).
        public const int TimeBetweenEnemyMissiles = 300;     // Time between each enemy missiles launching, at the beginning of the game (60 = 1 second).
        public const double ExplosionShrinkRate = 0.04;      // How fast explosions shrink and disappear (0.01 = 1%, 60 times/second).
        public const int ExplosionRotationRate = 8;          // How fast explosions' sprite rotates (5 = 5 degrees, 60 times/second).

        // Gameplay and Score.
        public const int PointsForEachMissileKill = 75;
        public const int NewGameStartingDifficulty = 1;      // Which difficulty level to start a new game at.
        public const int EnemyLaunchesInEachLevel = 6;       // Number of launches of enemy missiles in each level.
        public const int BreatherTimeBeforeNextLevel = 600;  // Time of breather between waves (60 = 1 second).
        public const int PointsForTurretsStandingAfterEachLevel = 200;   // (multiplied by level).

        // Boosts.
        public const double ChanceOfBoostAppearance = 0.3;   // Chance of a bonus appearing (1.0 = 100%).
        public const int BoostSpriteSpeed = 3;
        public const int AppliedBoostTime = 1200;            // How long a boost's effect lasts (60 = 1 second).
        public const int ReloadTimerCountdownBoost = 6;
        public const int PlayerMissileSpeedBoost = 18;
        public const double PlayerMissileSizeBoost = 1.5;    // How much bigger the large missiles explosions are (1.0 = 100%).
    }
}
