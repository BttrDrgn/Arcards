using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Nfc;
using Android.Nfc.Tech;
using System.Text;
using Plugin.NFC;
using Android.Nfc.CardEmulators;
using Arcards.Services;

namespace Arcards.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(new[] { NfcAdapter.ActionTechDiscovered, NfcAdapter.ActionTagDiscovered })]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossNFC.Init(this);

            FeliCaService.FeliCaEmulator = NfcFCardEmulation.GetInstance(NfcAdapter.GetDefaultAdapter(this));
            DependencyService.Register<IFeliCa, FeliCa>();

            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            CrossNFC.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            CrossNFC.OnNewIntent(intent);
        }
    }
}
