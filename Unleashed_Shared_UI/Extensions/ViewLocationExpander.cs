using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.Extensions
{
    // This class tells the Razor View Engine where to find our component views.
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        // This method is called by the framework to add new search paths.
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // The {0} is a placeholder for the view name (e.g., "Default").
            // The {1} is a placeholder for the component name (e.g., "Navbar").
            // This tells the engine: "Also look in /Components/{1}/{0}.cshtml".
            string[] customLocations = new string[] { "/Components/{1}/{0}.cshtml" };

            // We return the custom locations first, then the default ones.
            return customLocations.Concat(viewLocations);
        }

        // This method is required by the interface but we don't need to use it.
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // No implementation needed.
        }
    }
}