using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using static NeoCortexApiSample.MultiSequenceLearning;
using NeoCortexApiSample;
using System.Reflection.Metadata.Ecma335;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;
using System.Collections;

namespace NeoCortexApiSample
{
    class CSVFileReader
    {
        private string _filePathToCSV;
        private int _columnIndex;


        //data from https://github.com/numenta/NAB/blob/master/data/realTraffic/speed_7578.csv

        public CSVFileReader(string filePathToCSV = "", int columnIndex = 0)
        {
            _filePathToCSV = filePathToCSV;
            _columnIndex = columnIndex;
        }

        public List<double> ReadFile()
        {
            // method implementation
            List<double> inputnumbers = new List<double>();
            string[] csvLines = File.ReadAllLines(_filePathToCSV);
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] columns = csvLines[i].Split(new char[] { ',', '"' });
                inputnumbers.Add((double.Parse(columns[_columnIndex]) / 10));
            }
            return inputnumbers;
        }

        public void SequenceConsoleOutput()
        {

            foreach (double k in ReadFile())
            {
                Console.WriteLine(k);
            }

        }
        public void OutSeq()
        {
            List<double> testsequence = new List<double>();

            testsequence.AddRange(ReadFile());

            List<double> finaltestsequence1 = testsequence.GetRange(0, 50);
            List<double> finaltestsequence2 = testsequence.GetRange(400, 50);
            List<double> finaltestsequence3 = testsequence.GetRange(600, 50);

            foreach (double i in finaltestsequence1)
            {
                Console.Write(i + " ");

            }
            Console.WriteLine();
            Console.WriteLine("Next Sequence");

            foreach (double i in finaltestsequence2)
            {
                Console.Write(i + " ");

            }

            Console.WriteLine();
            Console.WriteLine("Next Sequence");

            foreach (double i in finaltestsequence3)
            {
                Console.Write(i + " ");

            }
        }
    }

    class Program
    {
        /// <summary>
        /// This sample shows a typical experiment code for SP and TM.
        /// You must start this code in debugger to follow the trace.
        /// and TM.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //
            // Starts experiment that demonstrates how to learn spatial patterns.
            //SpatialPatternLearning experiment = new SpatialPatternLearning();
            //experiment.Run();
            Console.WriteLine("Number Of Logical Processors: {0}", Environment.ProcessorCount);
            //
            // Starts experiment that demonstrates how to learn spatial patterns.
            //SequenceLearning experiment = new SequenceLearning();
            //experiment.Run();

            //RunModifiedMultiSimpleSequenceLearningExperiment();
            //RunMultiSimpleSequenceLearningExperiment();
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //RunMultiSequenceLearningExperiment();
            TestLogMultisequenceExperiment();
            //CSVFileReader cv = new CSVFileReader(@"D:\general\test_file2.csv", 2);
            //cv.SequenceConsoleOutput();
            //CSVFileReader cv = new CSVFileReader(@"D:\general\test_file2.csv", 2);
            //cv.OutSeq();

            // stopwatch.Stop();
            // Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            

        }

        private static void RunMultiSimpleSequenceLearningExperiment()
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            List<double> testsequence = new List<double>();

            CSVFileReader cv = new CSVFileReader("test_file2.csv", 2);

            testsequence.AddRange(cv.ReadFile());

            List<double> finaltestsequence1 = testsequence.GetRange(0, 50);
            List<double> finaltestsequence2 = testsequence.GetRange(50, 50);


            //sequences.Add("S1", new List<double>(new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, }));
            //sequences.Add("S1", new List<double>(finaltestsequence1));
            sequences.Add("S1", finaltestsequence1);
            //sequences.Add("S2", new List<double>(new double[] { 10.0, 11.0, 12.0, 13.0, 14.0, 15.0, 16.0 }));

            sequences.Add("S2", finaltestsequence2);

            //
            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);

        }

        private static void RunModifiedMultiSimpleSequenceLearningExperiment()
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            List<double> inputsequence = new List<double>();

            List<List<double>> listofsequences = new List<List<double>>();

            CSVFileReader cv = new CSVFileReader("test_file2.csv", 2);

            inputsequence.AddRange(cv.ReadFile());

            //int[] rIndex = { 10, 15, 20, 25, 30, 35, 40, 45, 50 };
            //int[] rCount = { 5, 5, 5, 5, 5, 5, 5, 5, 5 };

            int step = 5;//step increase in rIndex
            
            int rStart = 0;//rIndex start
            
            int rNum = 25; //number of elements in rindex, rCount
            
            int rStat = 5; //staticvalue in rCount
            int[] rIndex = Enumerable.Range(rStart, rNum).Select(x => x * step).ToArray();
            int[] rCount = Enumerable.Repeat(rStat, rNum).ToArray();

            for (int i = 0; i < rIndex.Length; i++)
            {
                List<double> singleseq = inputsequence.GetRange(rIndex[i], rCount[i]);
                listofsequences.Add(singleseq);
            }

            int countString = listofsequences.Count;


            List<string> stringstream = new List<string>();

            for (int i = 1; i <= countString; i++)
            {
                stringstream.Add("S" + i);
            }


            for (int i = 0; i < stringstream.Count; i++)
            {
                sequences.Add(stringstream[i], listofsequences[i]);
            }

            foreach (KeyValuePair<string, List<double>> item in sequences)
            {
                Console.WriteLine("Key: {0}", item.Key);
                Console.WriteLine("Values:");
                foreach (double value in item.Value)
                {
                    Console.WriteLine("  {0}", value);
                }
            }
            Console.WriteLine("Sequences fed into experiment");

            //
            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);

        }
        private static void RunMultiSequenceLearningExperiment()
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            //sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0, 12.0, 11.0, 12.0, 13.0, 14.0, 11.0, 12.0, 14.0, 5.0, 7.0, 6.0, 9.0, 3.0, 4.0, 3.0, 4.0, 3.0, 4.0 }));
            //sequences.Add("S2", new List<double>(new double[] { 0.8, 2.0, 0.0, 3.0, 3.0, 4.0, 5.0, 6.0, 5.0, 7.0, 2.0, 7.0, 1.0, 9.0, 11.0, 11.0, 10.0, 13.0, 14.0, 11.0, 7.0, 6.0, 5.0, 7.0, 6.0, 5.0, 3.0, 2.0, 3.0, 4.0, 3.0, 4.0 }));

            //sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 3.0, 4.0, 2.0, 5.0, }));
            //sequences.Add("S2", new List<double>(new double[] { 8.0, 1.0, 2.0, 9.0, 10.0, 7.0, 11.00 }));

            List<double> testsequence = new List<double>();

            CSVFileReader cv = new CSVFileReader("test_file2.csv", 2);

            testsequence.AddRange(cv.ReadFile());

            List<double> finaltestsequence1 = testsequence.GetRange(0, 10);
            //List<double> finaltestsequence2 = testsequence.GetRange(600, 30);


            sequences.Add("S1", finaltestsequence1);
            //sequences.Add("S2", finaltestsequence2);
            



            //
            // Prototype for building the prediction engine.

            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);

            //
            // These list are used to see how the prediction works.
            // Predictor is traversing the list element by element. 
            // By providing more elements to the prediction, the predictor delivers more precise result.
            /*var list1 = new double[] { 1.0, 2.0, 3.0, 4.0, 2.0, 5.0 };
            var list2 = new double[] { 2.0, 3.0, 4.0 };
            var list3 = new double[] { 8.0, 1.0, 2.0 };*/

            //var list1_d = testsequence.GetRange(400, 30);
            //double[] list1 = list1_d.ToArray();

            //var list2_d = testsequence.GetRange(250, 6);
            //double[] list2 = list2_d.ToArray();
            //var list3_d = testsequence.GetRange(350, 6);
            //double[] list3 = list3_d.ToArray();



            double[] list1 = TestAnomaly(1000, 10);

            predictor.Reset();
            PredictNextElement(predictor, list1);


            /*predictor.Reset();
            PredictNextElement(predictor, list2);

            predictor.Reset();
            PredictNextElement(predictor, list3);*/
        }


        private static void TestLogMultisequenceExperiment()
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            List<double> testsequence = new List<double>();

            CSVFileReader cv = new CSVFileReader("test_file2.csv", 2);

            testsequence.AddRange(cv.ReadFile());

            List<string> stringstream = new List<string>();

             for (int i = 1; i <= 1000; i++)
            {
                stringstream.Add("S" + i);
            }
            

            for (int i = 10; i <= 30; i += 5)
            {
                Stopwatch swh = new Stopwatch();

                swh.Start();

                List<double> finaltestsequence1 = testsequence.GetRange(0, i);

                sequences.Add(stringstream[i], finaltestsequence1);

                //
                // Prototype for building the prediction engine.

                MultiSequenceLearning experiment = new MultiSequenceLearning();
                var predictor = experiment.Run(sequences);
                
                /*double[] list1 = TestAnomaly(1000, i);

                predictor.Reset();
                PredictNextElement(predictor, list1);*/

                swh.Stop();

                TimeSpan runtime = swh.Elapsed;

                Console.WriteLine("Runtime for training experiment with " + i + " elements in sequence is: " + runtime);
                

            }




        }
        private static void PredictNextElement(Predictor predictor, double[] list)
        {
            Debug.WriteLine("------------------------------");

            foreach (var item in list)
            {
                var res = predictor.Predict(item);

                if (res.Count > 0)
                {
                    foreach (var pred in res)
                    {
                        Debug.WriteLine($"{pred.PredictedInput} - {pred.Similarity}");
                    }

                    var tokens = res.First().PredictedInput.Split('_');
                    var tokens2 = res.First().PredictedInput.Split('-');
                    Debug.WriteLine($"Predicted Sequence: {tokens[0]}, predicted next element {tokens2.Last()}");
                }
                else
                    Debug.WriteLine("Nothing predicted :(");
            }

            Debug.WriteLine("------------------------------");
        }


        private static double[] TestAnomaly(int a, int b)
        {
            List<double> anomalytestsequence = new List<double>();

            CSVFileReader cv = new CSVFileReader("test_file2.csv", 2);

            anomalytestsequence.AddRange(cv.ReadFile());

            var anomalytestlist1_d = anomalytestsequence.GetRange(a, b);

            double[] anomalytestlist1 = anomalytestlist1_d.ToArray();

            return anomalytestlist1;

        }
    }
}