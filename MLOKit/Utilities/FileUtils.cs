using System;

namespace MLOKit.Utilities
{
    class FileUtils
    {

        // return random 8 characters
        public static string generateRandomName()
        {
            string stringToOutput = "";

            // create random directory name
            Random rd = new Random();
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            char[] chars = new char[8];

            for (int i = 0; i < 8; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            stringToOutput = new string(chars);
            

            return stringToOutput;
        }

    }

}