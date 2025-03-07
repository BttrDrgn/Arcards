using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcards.Services
{
    internal interface IFeliCa
    {
        void SetNfcId2(string uid);
    }
}
