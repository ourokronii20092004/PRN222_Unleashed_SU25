using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.ViewModels.Menu
{
    public class SideMenuItemViewModel
    {
        public string Label { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }
    }
}