using System;
using System.Collections.Generic;
using System.Linq;

namespace WordCounterFacade
{
    public class WordProcess
    {        
        private string Words = string.Empty;

        //Constructor of the WordProcess class
        public WordProcess(string words)
        {
            this.Words = words;
        }

        #region Process words

        // Count and return the word dictionary.
        public Dictionary<string, int> CountWords()
        {
            Dictionary<string, int> objWordDictionary = new Dictionary<string, int>();
            
            try
            {
                // Pre-process the text.
                Words = PreprocessText(Words);
                
                // Split the text into words.
                string[] wordsArray = Words.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in wordsArray)
                {
                    // Filter null and white space words.
                    if (!string.IsNullOrWhiteSpace(word))
                    {
                        // Check the word already exists. If exists update the occurrence by one.
                        if (objWordDictionary.ContainsKey(word))
                        {
                            objWordDictionary[word] += 1;
                        }
                        else // if it's new add to the dictionary.
                        {
                            objWordDictionary.Add(word, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objWordDictionary;
        }

        // preprocess the text.
        public string PreprocessText(string text)
        {
            string processedText;
            try
            {
                // preprocess the text.
                processedText = text.Trim().Replace(Environment.NewLine, " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return processedText;
        }

        #endregion

        #region Sort words

        public Dictionary<string, int> SortWordDictionary(Dictionary<string, int> objWordDictionary)
        {
            try
            {
                // Sort the word dictionary descending order by the occurrence.
                objWordDictionary = objWordDictionary.OrderByDescending(word => word.Value).ToDictionary(word => word.Key, word => word.Value);
            }            
            catch (Exception ex)
            {
                throw ex;
            }

            return objWordDictionary;
        }

        #endregion
    }
}
