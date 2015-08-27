using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading;

namespace SharpWeld.Utilities
{
    public static class RandomString
    {
        private const string ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static String Generate()
        {
            string retVal = "";
            byte [] bytes = new byte[10];
            RandomNumberGenerator.Create().GetBytes(bytes);
           
            for (int i = 0; i < 10; i++)
            {
                int val = bytes[i] % ALLOWED_CHARS.Length;
                retVal += ALLOWED_CHARS.Substring(val, 1);
            }

            return retVal;
        }
    }
}
