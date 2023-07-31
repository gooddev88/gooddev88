using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintMaster.Helper {
    [Serializable]
    public static class TextTHB {
        private static char cha1;
        private static string ProcessValue;
        public static string Process(string numberVar1) {
            string[] NumberWord;
            string[] NumberWord2;
            string Num3 = "";
            cha1 = '.';
            NumberWord = numberVar1.Split(cha1);
            cha1 = ',';
            NumberWord2 = NumberWord[0].Split(cha1);
            for (int i = 0; i <= NumberWord2.Length - 1; i++) {
                Num3 = Num3 + NumberWord2[i];
            }
            ProcessValue = SplitWord(Num3);
            if (NumberWord.Length > 1) {
                if (int.Parse(NumberWord[1]) > 0) {
                    ProcessValue = ProcessValue + "บาท" + SplitWord(NumberWord[1]) + "สตางค์";
                } else {
                    ProcessValue = ProcessValue + "บาทถ้วน";
                }
            } else {
                ProcessValue = ProcessValue + "บาทถ้วน";
            }
            return ProcessValue;
        }
        public static string SplitWord(string numberVar) {
            int i = numberVar.Length;
            int k = 0;
            int n = i;
            int m = i;
            int b = 6;
            //char value2;
            char[] value1;
            string CurrencyWord = "";
            value1 = numberVar.ToCharArray();
            for (int a = 0; a <= i; a = a + 7) {
                if (n <= a + 7 && n > 0) {
                    b = n - 1;
                    if (i > 7) {
                        k = 1;
                    }
                } else {
                    b = 6;
                }
                if (n > 0) {
                    for (int j = 0; j <= b; j++) {
                        n--;
                        k++;
                        CurrencyWord = GetWord(value1[n].ToString(), k) + CurrencyWord;
                    }
                }
            }
            return CurrencyWord;
        }
        public static string GetWord(string str1, int Num1) {
            string value1 = GetCurrency(Num1);
            switch (str1) {
                case "1":
                    if (Num1 == 1) {
                        value1 = value1 + "หนึ่ง";
                    } else if (Num1 > 2) {
                        value1 = "หนึ่ง" + value1;
                    }
                    break;
                case "2":
                    if (Num1 == 2) {
                        value1 = "ยี่" + value1;
                    } else {
                        value1 = "สอง" + value1;
                    }
                    break;
                case "3":
                    value1 = "สาม" + value1;
                    break;
                case "4":
                    value1 = "สี่" + value1;
                    break;
                case "5":
                    value1 = "ห้า" + value1;
                    break;
                case "6":
                    value1 = "หก" + value1;
                    break;
                case "7":
                    value1 = "เจ็ด" + value1;
                    break;
                case "8":
                    value1 = "แปด" + value1;
                    break;
                case "9":
                    value1 = "เก้า" + value1;
                    break;
                default:
                    value1 = "";
                    break;
            }
            return value1;
        }
        public static string GetCurrency(int Num2) {
            string value1;
            switch (Num2) {
                case 1:
                    value1 = "";
                    break;
                case 2:
                    value1 = "สิบ";
                    break;
                case 3:
                    value1 = "ร้อย";
                    break;
                case 4:
                    value1 = "พัน";
                    break;
                case 5:
                    value1 = "หมื่น";
                    break;
                case 6:
                    value1 = "แสน";
                    break;
                case 7:
                    value1 = "ล้าน";
                    break;
                default:
                    value1 = "";
                    break;
            }
            return value1;
        }



    }
}