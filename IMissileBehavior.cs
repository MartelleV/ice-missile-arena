// VU PHAN HOANG AN - 104775412.
// An interface for the Missile Behaviors classes to inherit from and determine
// the missile's behaviors based on team.

namespace IceMissileArena
{
    public interface IMissileBehavior
    {
        void InitializeMissile(Missiles missile, BoostObserver boostObserver);
    }
}

