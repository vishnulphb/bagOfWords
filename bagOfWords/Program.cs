using Iveonik.Stemmers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bagOfWords
{
    class Program
    {
        static void Main(string[] args)
        {
            string cleanLine1 = removeSpecialChar("doc1.txt");
            string cleanLine2 = removeSpecialChar("doc2.txt");
            Dictionary<string,documentFreq> doc1Dict = createDictionary(cleanLine1);
            Dictionary<string, documentFreq> doc2Dict = updateDictionary(doc1Dict,cleanLine2);
            printFinalDict(doc2Dict);
            
        }

        public static string removeSpecialChar(string docPath)
        {
            StreamReader sr = new StreamReader(docPath);
            string line = sr.ReadLine();
            line = line.Replace("'", "");
            string cleanLine = Regex.Replace(line, "[^0-9a-zA-Z]+", ",");

            return cleanLine.Trim();
        }

        public static Dictionary<string, documentFreq> createDictionary(string cleanLine)
        {
            Dictionary<string, documentFreq> doc1Dict = new Dictionary<string, documentFreq>();
            documentFreq docsFreqObj = new documentFreq();
            string[] words = cleanLine.Split(',');
            foreach (string input in words)
            {
                string word = input;
                if (!doc1Dict.ContainsKey(word))
                {
                    docsFreqObj.doc1Freq = 1;

                    doc1Dict.Add(word, docsFreqObj);
                }

                else if (doc1Dict.ContainsKey(word))
                {
                    int newFreq = docsFreqObj.doc1Freq++;
                    doc1Dict[word].doc1Freq = newFreq;
                }
            }
            return doc1Dict;
        }

        public static Dictionary<string, documentFreq> updateDictionary(Dictionary<string, documentFreq> doc1Dict, string cleanLine2)
        {
            Dictionary<string, documentFreq> doc2Dict = doc1Dict;
           documentFreq doc2FreqObj = new documentFreq(); 
            
            string[] words = cleanLine2.Split(',');
            
            foreach(string input in words)
            {

                string word = input;
                if (!doc2Dict.ContainsKey(word))
                {
                    doc2FreqObj.doc1Freq = 0;
                    doc2FreqObj.doc2Freq = 1;
                    doc2Dict.Add(word, doc2FreqObj);
                }

                else if (doc2Dict.ContainsKey(word))
                {
                    doc2FreqObj.doc1Freq = doc2Dict[word].doc1Freq;
                    doc2FreqObj.doc2Freq++;
                    doc2Dict[word] = doc2FreqObj;
                }
            }

            return doc2Dict;
        }

        public static void printFinalDict(Dictionary<string, documentFreq> doc2Dict)
        {
            StreamWriter sw = new StreamWriter(@"output1.csv");
            var list = doc2Dict.Keys.ToList();
            list.Sort();
            //foreach (KeyValuePair<string, documentFreq> pair in doc2Dict)
            //{
            //    string output = pair.Key + "|" + pair.Value.doc1Freq +"|"+ pair.Value.doc2Freq;
            //    sw.WriteLine(output);
            //    Console.WriteLine(output);


            //}
            sw.WriteLine("DOCID | DOC1 |DOC2");
            foreach(var key in list)
            {
                sw.WriteLine(key+"|"+doc2Dict[key].doc1Freq+"|"+doc2Dict[key].doc2Freq);
            }
            sw.Close();
        }
    }
}
