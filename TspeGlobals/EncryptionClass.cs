using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace TspeGlobals
{
    public class EncryptionClass
    {
        private static string Salt = "TSP is unable to process config file. Please run the admin program and try again.";
        private static string Password = "Z@$FV%&HD*K(LE)0Q!#$";

        public EncryptionClass()
        {
        }

        private static string TwistString(string s)
        {
            Random r = new Random(198237);
            int idx = 0;
            while (idx < s.Length)
            {
                string tmp = "" + (char)r.Next(32, 126);
                s = s.Insert(idx, tmp);
                idx += r.Next(1, 4);
            }
            return s;
        }

        public static byte[] encryptStringToBytes_AES(string PlainText)
        {
            if ((PlainText == null) || (PlainText.Length < 1))
            {
                //Error
                return null;
            }

            string salt = TwistString(Salt);
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;
            RijndaelManaged rij = null;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Password, Encoding.ASCII.GetBytes(salt));
                rij = new RijndaelManaged();
                rij.Key = key.GetBytes(rij.KeySize / 8);
                rij.IV = key.GetBytes(rij.BlockSize / 8);
                rij.Padding = PaddingMode.None;

                ICryptoTransform encryptor = rij.CreateEncryptor(rij.Key, rij.IV);

                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);
                swEncrypt.Write(PlainText);
            }
            catch (Exception ex)
            {
                string errorstr = ex.Message;
            }
            finally
            {
                if (swEncrypt != null) swEncrypt.Close();
                if (csEncrypt != null) csEncrypt.Close();
                if (msEncrypt != null) msEncrypt.Close();
                if (rij != null) rij.Clear();
            }

            return msEncrypt.ToArray();
        }


        public static string decryptStringFromBytes_AES(byte[] cipherText)
        {
            if ((cipherText == null) || (cipherText.Length < 1))
            {
                return "";
                //ERROR
            }

            string salt = TwistString(Salt);
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;
            RijndaelManaged rij = null;
            string plaintext = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Password, Encoding.ASCII.GetBytes(salt));
                rij = new RijndaelManaged();
                rij.Key = key.GetBytes(rij.KeySize / 8);
                rij.IV = key.GetBytes(rij.BlockSize / 8);
                rij.Padding = PaddingMode.None;

                ICryptoTransform decryptor = rij.CreateDecryptor(rij.Key, rij.IV);

                msDecrypt = new MemoryStream(cipherText);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);
                plaintext = srDecrypt.ReadToEnd();
            }
            finally
            {
                if (srDecrypt != null) srDecrypt.Close();
                if (csDecrypt != null) csDecrypt.Close();
                if (msDecrypt != null) msDecrypt.Close();
                if (rij != null) rij.Clear();
            }
            return plaintext;
        }

    }
}
