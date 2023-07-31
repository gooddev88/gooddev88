using System;
namespace RobotWasm.Client.Pages.DPMBoard.CustomBoard {
    public class ResizeContext {
        public event EventHandler OnResizeInvoked;

        public void NotifyResizeInvoked() {
            OnResizeInvoked.Invoke(null, new EventArgs());
        }
    }
}
