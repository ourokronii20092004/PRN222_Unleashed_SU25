using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.ViewModels.Menu
{
    // Represents all the data needed to render the main navbar
    public class NavbarViewModel
    {
        public bool IsAuthenticated { get; set; }
        public int CartItemCount { get; set; }
        public List<NavigationLinkViewModel> Links { get; set; }
    }
}