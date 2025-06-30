// VU PHAN HOANG AN - 104775412.
// A class that contains simple data for the boost/buff tracker.

namespace IceMissileArena
{
	public class BoostObserver: IBoostObserver
	{
	    // How long each boost is applied (60 = 1s).
        private int _increaseReloadSpeedCountdown;
        private int _increaseMissileSpeedCountdown;
        private int _increaseMissileSizeCountdown;
        private bool _canClearScreen; // True, if player has the clear screen boost

        // Constructor.
        public BoostObserver()
        {
            // Initialize boosts.
            _increaseReloadSpeedCountdown = 0;
            _increaseMissileSpeedCountdown = 0;
            _increaseMissileSizeCountdown = 0;
            _canClearScreen = false;
        }

        // Getters and Setters.
        public int IncreaseReloadSpeedCountdown
        {
            get => _increaseReloadSpeedCountdown;
            set => _increaseReloadSpeedCountdown = value;
        }

        public int IncreaseMissileSpeedCountdown
        {
            get => _increaseMissileSpeedCountdown;
            set => _increaseMissileSpeedCountdown = value;
        }

        public int IncreaseMissileSizeCountdown
        {
            get => _increaseMissileSizeCountdown;
            set => _increaseMissileSizeCountdown = value;
        }

        public bool CanClearScreen
        {
            get => _canClearScreen;
            set => _canClearScreen = value;
        }

        // Methods.
        public void UpdateBoostCountdown(BoostType boostType) // Update the boost type and its countdown/boolean.
        {
            switch (boostType)
            {
                case BoostType.IncreaseReloadSpeed:
                    IncreaseReloadSpeedCountdown = Constants.AppliedBoostTime;
                    break;
                case BoostType.IncreaseMissileSpeed:
                    IncreaseMissileSpeedCountdown = Constants.AppliedBoostTime;
                    break;
                case BoostType.IncreaseMissileSize:
                    IncreaseMissileSizeCountdown = Constants.AppliedBoostTime;
                    break;
                case BoostType.ClearScreen:
                    CanClearScreen = true;
                    break;
            }
        }

        // Decrement boost countdowns as long as they are active.
        public void DecrementBoostCountdown()
        {
            if (IncreaseReloadSpeedCountdown > 0) IncreaseReloadSpeedCountdown--;
            if (IncreaseMissileSpeedCountdown > 0) IncreaseMissileSpeedCountdown--;
            if (IncreaseMissileSizeCountdown > 0) IncreaseMissileSizeCountdown--;
        }

        // Draw active boosts' icons on the game HUD to track them.
        public void DrawBoostIcons(Font gameFont)
        {
            // For Speed Boost.
            if (IncreaseMissileSpeedCountdown > 0)
            {
                SplashKit.DrawBitmap(SplashKit.LoadBitmap("SpeedBoostIcon", "resources/images/speed-boost-icon.png"), 30, 120);
                SplashKit.DrawText($"SPEED BOOST ACTIVE. TIME LEFT: {IncreaseMissileSpeedCountdown/60}s", Color.DarkRed, gameFont, 15, 92, 130);
                SplashKit.DrawText($"SPEED BOOST ACTIVE. TIME LEFT: {IncreaseMissileSpeedCountdown/60}s", Color.Black, gameFont, 15, 93, 131);
            }

            // For Size Boost.
            if (IncreaseMissileSizeCountdown > 0)
            {
                SplashKit.DrawBitmap(SplashKit.LoadBitmap("SizeBoostIcon", "resources/images/size-boost-icon.png"), 30, 165);
                SplashKit.DrawText($"SIZE BOOST ACTIVE. TIME LEFT: {IncreaseMissileSizeCountdown/60}s", Color.DarkRed, gameFont, 15, 92, 175);
                SplashKit.DrawText($"SIZE BOOST ACTIVE. TIME LEFT: {IncreaseMissileSizeCountdown/60}s", Color.Black, gameFont, 15, 93, 176);
            }

            // For Reload Boost.
            if (IncreaseReloadSpeedCountdown > 0)
            {
                SplashKit.DrawBitmap(SplashKit.LoadBitmap("ReloadBoostIcon", "resources/images/reload-boost-icon.png"), 30, 210);
                SplashKit.DrawText($"RELOAD BOOST ACTIVE. TIME LEFT: {IncreaseReloadSpeedCountdown/60}s", Color.DarkRed, gameFont, 15, 92, 220);
                SplashKit.DrawText($"RELOAD BOOST ACTIVE. TIME LEFT: {IncreaseReloadSpeedCountdown/60}s", Color.Black, gameFont, 15, 93, 221);
            }

            // For Clear Screen Boost.
            if (CanClearScreen == true)
            {
                SplashKit.DrawBitmap(SplashKit.LoadBitmap("ClearScreenIcon", "resources/images/clearscreen-boost-icon.png"), 30, 75);
                SplashKit.DrawText("RIGHT CLICK TO USE CLEAR SCREEN BOOST", Color.DarkRed, gameFont, 15, 92, 85);
                SplashKit.DrawText("RIGHT CLICK TO USE CLEAR SCREEN BOOST", Color.Black, gameFont, 15, 93, 86);
            }
        }
    }
}
