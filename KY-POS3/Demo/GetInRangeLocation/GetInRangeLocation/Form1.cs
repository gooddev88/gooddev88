namespace GetInRangeLocation {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            CalPlease();
        }

        private void CalPlease() {
            double currentLatitude =Convert.ToDouble(txtLat.Text); // Current latitude (e.g., San Francisco)
            double currentLongitude =  Convert.ToDouble(txtLon.Text); // Current longitude (e.g., San Francisco)
             
            double targetLatitude = 13.707706330254576; // Target latitude (e.g., New York City)
            double targetLongitude = 100.3759570576719; // Target longitude (e.g., New York City)

            double distance = DistanceCalculator.CalculateDistance(currentLatitude, currentLongitude, targetLatitude, targetLongitude);
            textBox1.Text = distance.ToString();

        }
    }
}