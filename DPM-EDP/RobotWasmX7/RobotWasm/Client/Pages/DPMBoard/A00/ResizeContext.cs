using System;
namespace RobotWasm.Client.Pages.DPMBoard.A00 {
    public class ResizeContext {
        public event EventHandler OnResizeInvoked;

        public void NotifyResizeInvoked() {
            OnResizeInvoked.Invoke(null, new EventArgs());
        }
    }
}
