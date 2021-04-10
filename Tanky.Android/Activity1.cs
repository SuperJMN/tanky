using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace Tanky.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 game;
        private View view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            game = new Game1();
            view = game.Services.GetService(typeof(View)) as View;

            SetContentView(view);
            game.Run();
        }
    }
}
