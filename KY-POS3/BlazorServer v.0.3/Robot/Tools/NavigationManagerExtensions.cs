using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Robot.Tools {
    public static class NavigationManagerExtensions {
        public static async Task   NavigationToNewWindow(this NavigationManager navigation, IJSRuntime jSRuntime, string url, string content) {
            await jSRuntime.InvokeAsync<object>("NavigationManagerExtensions.openInNewWindow", url,content);
        }

    }
}
