using System;
using System.IO;

namespace WordCounterFacade
{
    public class FileProcess
    {
        public string ReadTextFile(string filePath)
        {
            string text;
            try
            {
                // Read entire text file in one string
                /* ReadAllText() don't need to care about the encoding, because the function 
                 * detects the encoding by reading the BOM (Byte Order Mark). */
                text = File.ReadAllText(filePath);                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return text;
        }

        

    }
}
