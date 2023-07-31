using System;
using System.Collections.Generic;
using static Robot.Data.DA.Login.LogInService;

namespace Robot {
    public class StateContainer
    {
        private string savedString;
  
        //static LoginSet loginSet = null;
        public string Property
        {
            get => savedString;
            set
            {
                savedString = value;
                NotifyStateChanged();
            }
        }
        public List<string> XID { get; set; }
        public LoginSet loginSet {
            get; set;
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
