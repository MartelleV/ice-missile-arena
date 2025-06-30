// VU PHAN HOANG AN - 104775412.
// A Singleton class to manage the player's rotating missile launcher head.

namespace IceMissileArena
{
    public class LauncherHead
    {
        // Properties of a Launcher head.
        private static LauncherHead instance = null!; // Static Instance.
        private Sprite _launcherHeadSprite; // Its sprite.
        private readonly Point2D _launcherHeadXY; // Its position.
        private int _reloadCountdown; // Its reload countdown.
        private readonly Bitmap _launcherHeadBitmap; // Its bitmap.

        // Constructor.
        private LauncherHead()
        {
            _launcherHeadBitmap = SplashKit.LoadBitmap("LauncherHead", "resources/images/launcher-head.png");
            _launcherHeadSprite = SplashKit.CreateSprite(_launcherHeadBitmap); // Create the sprite from the bitmap.

            // Set the head's sprite position and initial rotation.
            _launcherHeadXY = new Point2D() { X = (SplashKit.ScreenWidth() / 2) - (SplashKit.BitmapHeight(_launcherHeadBitmap) / 2), Y = SplashKit.ScreenHeight() - 150 };
            SplashKit.SpriteSetPosition(_launcherHeadSprite, _launcherHeadXY);
            SplashKit.SpriteSetRotation(_launcherHeadSprite, -90);

            // Set the new position to the center point of the head.
            float centerX = (float)(SplashKit.SpriteX(_launcherHeadSprite) + SplashKit.SpriteWidth(_launcherHeadSprite) / 2);
            float centerY = (float)(SplashKit.SpriteY(_launcherHeadSprite) + SplashKit.SpriteHeight(_launcherHeadSprite) / 2);
            _launcherHeadXY = new Point2D { X = centerX, Y = centerY };
            _reloadCountdown = 0; // Initial reload countdown: ready to fire.
        }

        // Instance method.
        public static LauncherHead GetInstance()
        {
            instance ??= new LauncherHead();
            return instance;
        }

        // Getters and Setters.
        public Point2D LauncherHeadXY
        {
            get => _launcherHeadXY;
        }

        public Sprite LauncherHeadSprite
        {
            get => _launcherHeadSprite;
            set => _launcherHeadSprite = value;
        }

        public int ReloadCountdown
        {
            get => _reloadCountdown;
            set => _reloadCountdown = value;
        }

        public Bitmap LauncherHeadBitmap
        {
            get => _launcherHeadBitmap;
        }

        // Helper Static Method to find the angle of the launcher head.
        public static double FindAngle(Point2D originXY, Point2D targetXY) // Find angle for launcher head's sprite.
        {
            // Delta X and Y coordinates (between the original X-Y and the target's X-Y).
            double deltaX = originXY.X - targetXY.X;
            double deltaY = targetXY.Y - originXY.Y;

            // Calculate angle in radians, then convert to degrees.
            double result = Math.Atan2(deltaX, deltaY) * 180 / Math.PI;
            result += 90; // Compensate 90d to be relative from the vertical axis.
            return result;
        }

        // Methods.
        public void DrawLauncherHead() // Draw the launcher head sprite and checks if it's ready to fire.
        {
            SplashKit.DrawSprite(LauncherHeadSprite);

            if (ReloadCountdown == 0) // If launcher is ready to fire:
            {
                SplashKit.FillCircle(Color.BrightGreen, LauncherHeadXY.X - 18, LauncherHeadXY.Y + 37, 3); // draw a green circle signal.
            }
            else
            {
                SplashKit.FillCircle(Color.Red, LauncherHeadXY.X - 18, LauncherHeadXY.Y + 37, 3); // Else, draw a red circle signal.
            }
        }

        // Update the launcher head's rotation based on mouse movements and positions,
        // and update the launcher's reload countdown.
        public void UpdateLauncherHead()
        {
            Point2D mouseXY = SplashKit.PointAt(SplashKit.MouseX(), SplashKit.MouseY()); // Find mouse cursor location.
            double rotation = FindAngle(LauncherHeadXY, mouseXY); // Find angle to rotate launcher head.
            SplashKit.SpriteSetRotation(LauncherHeadSprite, (float)rotation); // Rotate launcher head.

            // Decrement countdown.
            if (ReloadCountdown > 0)
            {
                ReloadCountdown--;
            }
        }
    }
}
