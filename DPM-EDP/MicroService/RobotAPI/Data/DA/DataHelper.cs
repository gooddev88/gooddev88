namespace RobotAPI.Data.DA {
    public class DataHelper {
 
        public static string CreateWhereIn(string input, char split) {
            var arr = input.Split(split);
            string ouput = "";
            if (!input.ToLower().Contains(',')) {
                return input;
            }
            if (arr.Length == 0) {
                return ouput;
            }
            if (arr.Length == 1) {
                if (string.IsNullOrEmpty(arr[0])) {
                    return ouput;
                }
            }

            int k = 0;
            foreach (var c in arr) {
                if (k == 0) {
                    ouput = ouput + @" '" + c + @"' ";
                } else {
                    ouput = ouput + @",'" + c + @"' ";
                }
                k++;
            }
            return ouput;
        }
    }
}
