// VU PHAN HOANG AN - 104775412.
// An interface for the BoostObserver class to inherit from and update the
// boosts' countdown times.

namespace IceMissileArena
{
	public interface IBoostObserver
	{
		public void UpdateBoostCountdown(BoostType boostType);
	}
}

