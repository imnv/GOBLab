using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOB.Logging
{
    class Toggle
    {
        private static bool _toggle = false;

        public static bool GetToggle
        {
            get
            {
                _toggle = !_toggle;
                return !_toggle;
            }
        }
    }
}
