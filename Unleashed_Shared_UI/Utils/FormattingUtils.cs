using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.Utils
{
    public static class FormattingUtils
    {
        // Formats the price as Vietnamese Dong (VND)
        public static string FormatPrice(decimal price)
        {
            var cultureInfo = new CultureInfo("vi-VN");
            return string.Format(cultureInfo, "{0:C0}", price); // C0 removes decimals
        }
    }
}
