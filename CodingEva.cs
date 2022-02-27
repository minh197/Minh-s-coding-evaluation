using System;
using System.Collections.Generic;

namespace BerwynGroup
{
    class Program
    {
        static void Main(string[] args)

        {

            if (args.Length > 0)
            {
                if (System.IO.File.Exists(args[0]))
                {
                    ProcessData(args[0]);
                }
            }
            else
            {
               
                 System.Console.WriteLine("No arguments found!!");
              
               
            }
        }
        //Set up instream, uniqueID set for duplication detection
        //We use HashSet to store the unique GUID as well as the duplicated one
        static void ProcessData(string filepath)
        {
            System.IO.StreamReader infile = new System.IO.StreamReader(filepath);
            HashSet<string> uniqueGUINumber = new HashSet<string>();
            HashSet<string> duplicateGUINumber = new HashSet<string>();
            List<Record> processedRecord = new List<Record>();

            //Create new counters for the statistical computation
            int numRecord = 0;
            string maxV1V2Guid = "N/A";
            int maxV1V2Sum = 0;
            int SumLengthV3 = 0;
            //read in each line of the input file
            string headerLine;
            //skip the CSV header line

            headerLine = infile.ReadLine();
            while ((headerLine = infile.ReadLine()) != null)
            {
                string[] record = headerLine.Replace("\"", "").Split(',');
                Record result = new Record();

                //First we will process the GUID
                result.GUID = record[0];
                //check for duplicated GUID
                if (!uniqueGUINumber.Add(result.GUID))
                {
                    result.IsDuplicatedGUID = true;
                    duplicateGUINumber.Add(result.GUID);
                }

                //Find the sum of V1 and V2
                result.V1V2Sum = Int32.Parse(record[1]) + Int32.Parse(record[2]);

                //Check to see if this is the maximum Val1Val2 sum
                if (result.V1V2Sum > maxV1V2Sum)
                {
                    maxV1V2Sum = result.V1V2Sum;
                    maxV1V2Guid = result.GUID;
                }
                //process the length of Val3

                result.Val3 = record[3].Length;
                SumLengthV3 += result.Val3;

                //add the result to the processedRecord
                processedRecord.Add(result);
                numRecord++;
            }
            infile.Close();
            //Go on to compute the sum of V1 and V2
            double val3SumAverage = SumLengthV3;
            val3SumAverage /= numRecord;
            //output the processed result
            // Result filepath is the input path + "-processed"
            string outPutPath = filepath.Insert(filepath.LastIndexOf('.'), "-processed");
            System.IO.StreamWriter outfile = new System.IO.StreamWriter(outPutPath);

            //Output header info to console
            System.Console.Out.WriteLine(numRecord);
            System.Console.Out.Write(maxV1V2Guid);
            System.Console.Write(",");
            System.Console.WriteLine(maxV1V2Sum);

            foreach (string duplicateGUI in duplicateGUINumber)
            {
                System.Console.Out.Write(duplicateGUI);
                System.Console.Out.Write(",");
            }
            System.Console.Out.WriteLine();
            System.Console.Out.WriteLine(val3SumAverage);

            //output each record's proccessed result
            foreach (Record result in processedRecord)
            {
                outfile.Write(result.GUID);
                outfile.Write(",");

                outfile.Write(result.V1V2Sum);
                outfile.Write(",");

                outfile.Write((result.IsDuplicatedGUID ? "Y" : "N"));
                outfile.Write(",");

                outfile.WriteLine((result.Val3 > val3SumAverage ? "Y" : "N"));
            }
            outfile.Close();
        }

            class Record
        {
           
            public string GUID;
            public int V1V2Sum;
            public bool IsDuplicatedGUID;
            public int Val3;
        }

        }
    }
