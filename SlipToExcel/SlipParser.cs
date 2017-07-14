using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SlipToExcel
{
    //Takes a packing slip, which is a provided textfile with some basic formatting, and
    //extracts the portions that are relevant to creating an excel sheet
    class SlipParser
    {
        private string SlipFilename = "";
        private string SlipPath = "";

        private Regex SlipTrim = new Regex(@"\s+\d\s{5}\d.+(\n\s.+)*");     //Removes excess from slip
        private Regex SlipItemStart = new Regex(@"^\ {4}\d");             //Finds beginning of new item
        private Regex SlipItemDate = new Regex(@"\d{2}\-\d{2}");           //Finds date range in item desc

        List<string[]> excelData = null;

        public SlipParser() { }
        public void ReadFile(string filePath)
        {
            StreamReader reader = File.OpenText(filePath);
            string fileString = reader.ReadToEnd().Trim();
            reader.Close();
            MatchCollection matches = SlipTrim.Matches(fileString);
            List<string> matchedData = MatchesToString(matches);
            excelData = Parse(matchedData);
        }

        public List<string[]> ExcelData { get { return excelData; } }

        private List<string[]> Parse(List<string> list)
        {
            List<string[]> toExcel = new List<string[]>();
            bool secondNext = false;
            string[] dataRow = null;
            foreach (string s in list)
            {
                if (SlipItemStart.Match(s).Success)
                {                        //Found first row of new item
                    if (dataRow != null)
                        toExcel.Add(dataRow);
                    dataRow = new string[3] { "", "", "" };
                    dataRow[0] = s.Substring(0, 5).Trim();                //Number of items
                    dataRow[1] = " " + string.Format("{0,10} ", new string(s.Skip(12).Take(10).ToArray()));    //Model Name
                    secondNext = true;
                }
                else
                {
                    dataRow[1] += new string(s.Skip(24).Take(35).ToArray()).Trim();   //Item Desc
                    if (secondNext)
                    {
                        dataRow[1] = SlipItemDate.Match(s).Value + " " + dataRow[1];      //Item date range
                        dataRow[2] = new string(s.Skip(60).Take(9).ToArray()).Trim();  //Item Cost
                    }
                    secondNext = false;
                }
            }
            toExcel.Add(dataRow);
            return toExcel;
        }

        private List<string> MatchesToString(MatchCollection matches)
        {
            List<string> list = new List<string>();
            foreach (Match match in matches)
            {
                list.AddRange(match.Value.Split(new string[] { "\n", "\r\n" },
                                                StringSplitOptions.RemoveEmptyEntries));
            }
            return list;
        }
    }
}
