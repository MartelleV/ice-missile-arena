// VU PHAN HOANG AN - 104775412.
// A static Factory class to generate new missiles based on its type (behavior).

namespace IceMissileArena
{
    public static class MissileFactory
    {
        // Create a missile based on its Team, coordinates and boost.
        public static Missiles CreateMissile(MissileTeam missileTeam, Point2D missileXY, Point2D targetXY, BoostObserver boostObserver)
        {
            Missiles missile = new(missileTeam, missileXY, targetXY);
            IMissileBehavior missileBehavior;

            // Decide behavior based on the missile's team.
            missileBehavior = missileTeam == MissileTeam.Enemy
                ? new EnemyMissileBehavior()
                : new PlayerMissileBehavior();

            // Initialize the missile.
            missileBehavior.InitializeMissile(missile, boostObserver);
            return missile;
        }
    }
}