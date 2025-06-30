// VU PHAN HOANG AN - 104775412.
// An interface for the Explosion Behavior classes to inherit from and
// implement different explosion strategies based on the type of boost.

namespace IceMissileArena
{
    public interface IExplosionBehavior
    {
        void HandleExplosions(Sprite explosionSprite, BoostObserver boostObserver);
    }
}