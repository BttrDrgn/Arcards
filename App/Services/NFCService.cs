using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcards.Services
{
    internal class NFCService
    {
        public bool Initialized = false;

#if !WINDOWS
        public bool AvailableAndEnabled => CrossNFC.Current.IsAvailable && CrossNFC.Current.IsEnabled;
#else
        public bool AvailableAndEnabled => false;
#endif

        public delegate Task NFCReadEventHandler(ITagInfo tag);
        public event NFCReadEventHandler OnRead;
        public delegate void NFCDiscoveredEventHandler();
        public event NFCDiscoveredEventHandler OnDiscovered;


        public void Initialize()
        {
            if (Initialized) return;

            if (AvailableAndEnabled)
            {
                CrossNFC.Current.OnMessageReceived += OnNFCRead;
            }

            Initialized = true;
        }

        public void StartListening()
        {
            if (AvailableAndEnabled) CrossNFC.Current.StartListening();
        }

        public void StopListening()
        {
            if (AvailableAndEnabled) CrossNFC.Current.StopListening();
        }

        public void OnNFCRead(ITagInfo tag)
        {
            OnRead?.Invoke(tag);
        }
    }
}
