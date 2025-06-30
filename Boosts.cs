// VU PHAN HOANG AN - 104775412.
// A class and an Enumeration to manage boosts. It is observed and notified
// by the BoostObserver class to track and apply the changes.

namespace IceMissileArena
{
    // Enumeration of Boost types.
    public enum BoostType
    {
        IncreaseReloadSpeed,  // Increase firing rate (Reload Boost).
        IncreaseMissileSpeed, // Increase missile flying speed (Speed Boost).
        IncreaseMissileSize,  // Increase missile's and explosion's sizes (Size Boost).
        ClearScreen,          // Clears all missiles on the screen with normal explosions (Clear Screen Boost).
    }

    // Boosts class.
    public class Boosts
    {
        // Properties of a boost.
        private Sprite _boostSprite; // Sprite of the boost.
        private BoostType _type; // Its type.
        private readonly Bitmap _boostBitmap; // Its bitmap.
        private Point2D _boostStartingXY; // Its starting position (coordinates).
        private readonly List<IBoostObserver> _observers; // Observers to attach to the boosts.

        // Constructor.
        public Boosts()
        {
            _observers = new();
            _boostStartingXY = new Point2D() { X = -100, Y = SplashKit.ScreenHeight() / 4 }; // Starting point of bonuses: off screen, to the left.

            // !TODO: The documentation says that Rnd is inclusive with both ends.
            // However, when testing the app, it turns out that the maximum is not included, so
            // I had to increment 1.
            switch (SplashKit.Rnd(1, 5))
            {
                case 4:
                    _type = BoostType.IncreaseReloadSpeed;
                    _boostBitmap = SplashKit.LoadBitmap("ReloadBoost", "resources/images/reload-boost.png");
                    break;
                case 3:
                    _type = BoostType.IncreaseMissileSpeed;
                    _boostBitmap = SplashKit.LoadBitmap("SpeedBoost", "resources/images/speed-boost.png");
                    break;
                case 2:
                    _type = BoostType.IncreaseMissileSize;
                    _boostBitmap = SplashKit.LoadBitmap("SizeBoost", "resources/images/size-boost.png");
                    break;
                default:
                    _type = BoostType.ClearScreen;
                    _boostBitmap = SplashKit.LoadBitmap("ClearScreenBoost", "resources/images/clearscreen-boost.png");
                    break;
            }

            // Create sprite from boost bitmap.
            _boostSprite = SplashKit.CreateSprite(_boostBitmap);
            SplashKit.SpriteSetPosition(_boostSprite, _boostStartingXY); // Start position of the boost sprite.
            SplashKit.SpriteSetDx(_boostSprite, Constants.BoostSpriteSpeed); // Move the boost sprite along X axis.
        }

        // Getters and Setters.
        public Bitmap BoostBitmap
        {
            get => _boostBitmap;
        }

        public BoostType Type
        {
            get => _type;
            set => _type = value;
        }

        public Sprite BoostSprite
        {
            get => _boostSprite;
            set => _boostSprite = value;
        }

        public Point2D BoostStartingXY
        {
            get => _boostStartingXY;
        }

        public List<IBoostObserver> Observers
        {
            get => _observers;
        }

        // Method to help connect with the Observers.
        private void NotifyObservers()
        {
            foreach (var observer in Observers)
            {
                observer.UpdateBoostCountdown(Type); // For each observer, update its boost's countdown time based on type.
            }
        }

        // Other methods.
        public void ApplyBoost()
        {
            NotifyObservers(); // Notify the Observer to update boost countdowns if the player hits a boost.
        }

        // Draw the moving boost sprites on the screen.
        public static void DrawBoostsMoving(List<Boosts> boosts)
        {
            foreach (Boosts boost in boosts)
            {
                SplashKit.DrawSprite(boost.BoostSprite);
            }
        }

        // Checks if boost sprites go out of screen bound.
        public static void CheckBoostsMoving(List<Boosts> boosts)
        {
            int xBoundaryRight = SplashKit.ScreenWidth() + 100; // 100 Pixels off screen, to the right.

            for (int i = 0; i < boosts.Count; i++)
            {
                SplashKit.UpdateSprite(boosts[i].BoostSprite);

                if (SplashKit.SpriteX(boosts[i].BoostSprite) > xBoundaryRight) // If boost goes off screen:
                {
                    boosts.RemoveAt(i); // delete boost.
                }
            }
        }
    }
}
