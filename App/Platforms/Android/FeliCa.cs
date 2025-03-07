using Arcards.Platforms.Android;
using Arcards.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Dependency(typeof(FeliCa))]
namespace Arcards.Platforms.Android
{
    public class FeliCa : IFeliCa
    {
        public void SetNfcId2(string uid)
        {
            FeliCaService.SetNfcId2(uid);
        }
    }
}
