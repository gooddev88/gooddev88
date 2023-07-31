using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class EncryptService {
        #region Class


        #endregion


        public static string hashPassword(string HashType, string pwd) {
            byte[] hash = null;
            StringBuilder digest = null;
            switch (HashType) {
                case "SHA1":
                    SHA1 sha1 = SHA1CryptoServiceProvider.Create();
                    hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(pwd));
                    digest = new StringBuilder();
                    foreach (byte n in hash) {
                        digest.Append(Convert.ToInt32(n + 256).ToString("x2"));
                    }
                    break;
                case "SHA256":
                    SHA256 sha256 = SHA256CryptoServiceProvider.Create();
                    hash = sha256.ComputeHash(Encoding.ASCII.GetBytes(pwd));
                    digest = new StringBuilder();
                    foreach (byte n in hash) {
                        digest.Append(Convert.ToInt32(n + 256).ToString("x2"));
                    }
                    break;
                case "SHA384":
                    SHA384 sha384 = SHA384CryptoServiceProvider.Create();
                    hash = sha384.ComputeHash(System.Text.Encoding.ASCII.GetBytes(pwd));
                    digest = new StringBuilder();
                    foreach (byte n in hash) {
                        digest.Append(Convert.ToInt32(n + 256).ToString("x2"));
                    }
                    break;
                case "SHA512":
                    SHA512 sha512 = SHA512CryptoServiceProvider.Create();
                    hash = sha512.ComputeHash(System.Text.Encoding.ASCII.GetBytes(pwd));
                    digest = new StringBuilder();
                    foreach (byte n in hash) {
                        digest.Append(Convert.ToInt32(n + 256).ToString("x2"));
                    }
                    break;
                case "MD5":
                    MD5 md5 = MD5CryptoServiceProvider.Create();
                    hash = md5.ComputeHash(Encoding.ASCII.GetBytes(pwd));
                    digest = new StringBuilder();
                    foreach (byte n in hash) {
                        digest.Append(Convert.ToInt32(n + 256).ToString("x2"));
                    }
                    break;
            }
            return digest.ToString();
        }
        private static Random random = new Random();
        public static string RandomString(int length) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}