using Android.App;
using Android.Content;
using Android.Nfc.CardEmulators;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Android.App.Application;

namespace Arcards.Platforms.Android
{
    [Service(Exported = true, Permission = "android.permission.BIND_NFC_SERVICE")]
    [IntentFilter(new[] { "android.nfc.cardemulation.action.HOST_NFCF_SERVICE" })]
    [MetaData("android.nfc.cardemulation.host_nfcf_service", Resource = "@xml/nfcf_setting")]
    public class FeliCaService : HostNfcFService
    {
        public static NfcFCardEmulation FeliCaEmulator;
        public static ComponentName FeliCaComponentName = new ComponentName("com.drgn.arcards", "com.drgn.arcards.FeliCaService");

        public static void SetNfcId2(string uid)
        {
            FeliCaEmulator?.SetNfcid2ForService(FeliCaComponentName, uid.ToUpper());
        }

        public override byte[] ProcessNfcFPacket(byte[] commandPacket, Bundle extras)
        {
            if (commandPacket.Length < 1 + 1 + 8)
            {
                Toast.MakeText(Application.Context, "ProcessPacket: too short packet!", ToastLength.Short).Show();
                return null;
            }

            byte[] nfcid2 = new byte[8];
            Array.Copy(commandPacket, 2, nfcid2, 0, 8);
            string res = string.Join("", nfcid2.Select(b => b.ToString("x2")));

            Toast.MakeText(Application.Context, $"ProcessPacket: {res}", ToastLength.Short).Show();

            if (commandPacket[1] == 0x04)
            {
                byte[] resp = new byte[1 + 1 + 8 + 1];
                resp[0] = 11;
                resp[1] = 0x05;
                Array.Copy(nfcid2, 0, resp, 2, 8);
                resp[10] = 0;
                return resp;
            }
            else
            {
                return null;
            }
        }

        public override void OnDeactivated(DeactivationReasonF reason)
        {
        }
    }
}
