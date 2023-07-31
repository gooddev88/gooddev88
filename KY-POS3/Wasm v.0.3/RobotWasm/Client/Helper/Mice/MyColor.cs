namespace RobotWasm.Client.Helper.Mice {
    public class MyColor {
        public static string RandomHtmlColor() {
            var random = new Random();
           return String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
        }
    }
}
