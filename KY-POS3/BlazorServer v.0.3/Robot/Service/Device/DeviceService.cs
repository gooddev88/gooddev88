using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Robot.Service.Device {
    public class DeviceService {

     public   IJSRuntime _jsRuntime;
        public DeviceService(IJSRuntime jsRuntime) {
            _jsRuntime = jsRuntime; 
        }

        private string isDevice { get; set; }
        private bool mobile { get; set; }
        public async Task FindResponsiveness() {
            mobile = await _jsRuntime.InvokeAsync<bool>("isDevice");
            isDevice = mobile ? "Mobile" : "Desktop";

        }
    }
}
