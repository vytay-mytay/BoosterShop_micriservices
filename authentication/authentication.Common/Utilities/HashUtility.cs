using System;
using System.Text;

namespace authentication.Common.Utilities
{
    public class HashUtility
    {
        public string GetHash(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";

            byte[] data = Encoding.ASCII.GetBytes(inputString);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = Encoding.ASCII.GetString(data);
            return hash;
        }
    }
}
