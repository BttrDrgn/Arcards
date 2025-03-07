using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcards
{
    internal class Env
    {
        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        public static bool IsIOS()
        {
#if IOS
            return true;
#else
            return false;
#endif
        }
    }
}
