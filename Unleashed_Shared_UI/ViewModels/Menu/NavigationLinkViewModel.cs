using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.ViewModels.Menu
{
    // Represents a single link in the main navigation
    public class NavigationLinkViewModel
    {
        public string Label { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }
    }
}