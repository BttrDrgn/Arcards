using Arcards.Platforms.Windows;
using Arcards.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Dependency(typeof(FeliCa))]
namespace Arcards.Platforms.Windows
{
    public class FeliCa : IFeliCa
    {
        public void SetNfcId2(string uid)
        {
        }
    }
}
