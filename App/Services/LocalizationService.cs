using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcards.Services
{
    public class Localization
    {
        [JsonProperty("nav.cards")] public string NavCards { get; set; } = "{NO-LOC-LOADED}";
        [JsonProperty("nav.settings")] public string NavSettings { get; set; } = "{NO-LOC-LOADED}";
        [JsonProperty("nav.debug")] public string NavDebug { get; set; } = "{NO-LOC-LOADED}";
    }

    public class LocalizationService
    {
        public static Dictionary<string, string> LocCodes = new Dictionary<string, string>()
        {
            { "en-US", "English (United States)" },
        };

        public string Language = "";
        public Localization Localization = new Localization();

        public async Task SetLanguage(string language, bool reload = true)
        {
            if (LocCodes.TryGetValue(language, out _))Language = language;
            else Language = "en-US";

            if (reload) await LoadLoc();
        }

        public async Task LoadLoc()
        {
            try
            {
                var stream = await IO.OpenPackageFile($"wwwroot/localization/{Language}.json");
                var reader = new StreamReader(stream);

                Localization = JsonConvert.DeserializeObject<Localization>(reader.ReadToEnd());

                await CallbackService.LocUpdateCallbacks.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
