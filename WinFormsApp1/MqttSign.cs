using System;
using System.Security.Cryptography;

namespace WinFormsApp1
{
    class CryptoUtil
    {
        public static String Sha256(String plainText)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] plainTextBytes = encoding.GetBytes(plainText);

            SHA256 hmac = SHA256.Create();
            byte[] sign = hmac.ComputeHash(plainTextBytes);
            return BitConverter.ToString(sign).Replace("-", string.Empty);
        }
    }

    public class MqttSign
    {
        private String username = "";

        private String password = "";

        private String clientid = "";

        public String getUsername() { return this.username; }

        public String getPassword() { return this.password; }

        public String getClientid() { return this.clientid; }

        public bool calculate(String productKey, String deviceName)
        {
            if (productKey == null || deviceName == null)
            {
                return false;
            }

            this.username = "root";
            String plainPasswd = "clientId" + productKey + "."
                + deviceName + "deviceName" +  deviceName + "productKey" + productKey;
            this.password = CryptoUtil.Sha256("#$%^&!1441H$!");
            this.clientid = productKey + "." + deviceName;
            return true;
        }
    }
}