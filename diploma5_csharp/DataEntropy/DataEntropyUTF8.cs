﻿using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.DataEntropy
{
    // Sources:
    // Shannon entropy - https://en.wiktionary.org/wiki/Shannon_entropy
    // Inspired code - http://www.csharpprogramming.tips/2013/07/Data-Entropy.html

    /// <summary>
    /// Calculates Shannon entropy for a file or an image
    /// </summary>
    public class DataEntropyUTF8
    {
        // Stores the number of times each symbol appears
        SortedList<byte, int> distributionDict;
        // Stores the entropy for each character
        SortedList<byte, double> probabilityDict;
        // Stores the last calculated entropy
        double overalEntropy;
        // Used for preventing unnecessary processing
        bool isDirty;
        // Bytes of data processed
        int dataSize;

        public int DataSampleSize
        {
            get { return dataSize; }
            private set { dataSize = value; }
        }

        public int UniqueSymbols
        {
            get { return distributionDict.Count; }
        }

        public double Entropy
        {
            get { return GetEntropy(); }
        }

        public Dictionary<byte, int> Distribution
        {
            get { return GetSortedDistribution(); }
        }

        public Dictionary<byte, double> Probability
        {
            get { return GetSortedProbability(); }
        }

        public byte GetGreatestDistribution()
        {
            return distributionDict.Keys[0];
        }

        public byte GetGreatestProbability()
        {
            return probabilityDict.Keys[0];
        }

        public double GetSymbolDistribution(byte symbol)
        {
            return distributionDict[symbol];
        }

        public double GetSymbolEntropy(byte symbol)
        {
            return probabilityDict[symbol];
        }

        Dictionary<byte, int> GetSortedDistribution()
        {
            List<Tuple<int, byte>> entryList = new List<Tuple<int, byte>>();
            foreach (KeyValuePair<byte, int> entry in distributionDict)
            {
                entryList.Add(new Tuple<int, byte>(entry.Value, entry.Key));
            }
            entryList.Sort();
            entryList.Reverse();

            Dictionary<byte, int> result = new Dictionary<byte, int>();
            foreach (Tuple<int, byte> entry in entryList)
            {
                result.Add(entry.Item2, entry.Item1);
            }
            return result;
        }

        Dictionary<byte, double> GetSortedProbability()
        {
            List<Tuple<double, byte>> entryList = new List<Tuple<double, byte>>();
            foreach (KeyValuePair<byte, double> entry in probabilityDict)
            {
                entryList.Add(new Tuple<double, byte>(entry.Value, entry.Key));
            }
            entryList.Sort();
            entryList.Reverse();

            Dictionary<byte, double> result = new Dictionary<byte, double>();
            foreach (Tuple<double, byte> entry in entryList)
            {
                result.Add(entry.Item2, entry.Item1);
            }
            return result;
        }

        double GetEntropy()
        {
            // If nothing has changed, dont recalculate
            if (!isDirty)
            {
                return overalEntropy;
            }
            // Reset values
            overalEntropy = 0;
            probabilityDict = new SortedList<byte, double>();

            foreach (KeyValuePair<byte, int> entry in distributionDict)
            {
                // Probability = Freq of symbol / # symbols examined thus far
                probabilityDict.Add(
                    entry.Key,
                    (double)distributionDict[entry.Key] / (double)dataSize
                );
            }

            foreach (KeyValuePair<byte, double> entry in probabilityDict)
            {
                // Entropy = probability * Log2(1/probability)
                overalEntropy += entry.Value * Math.Log((1 / entry.Value), 2);
            }

            isDirty = false;
            return overalEntropy;
        }

        public void ExamineChunk(byte[] chunk)
        {
            if (chunk.Length < 1 || chunk == null)
            {
                return;
            }

            isDirty = true;
            dataSize += chunk.Length;

            foreach (byte bite in chunk)
            {
                if (!distributionDict.ContainsKey(bite))
                {
                    distributionDict.Add(bite, 1);
                    continue;
                }
                distributionDict[bite]++;
            }
        }

        public void ExamineChunk(string chunk)
        {
            ExamineChunk(StringToByteArray(chunk));
        }

        byte[] StringToByteArray(string inputString)
        {
            char[] c = inputString.ToCharArray();
            IEnumerable<byte> b = c.Cast<byte>();
            return b.ToArray();
        }

        void Clear()
        {
            isDirty = true;
            overalEntropy = 0;
            dataSize = 0;
            distributionDict = new SortedList<byte, int>();
            probabilityDict = new SortedList<byte, double>();
        }

        void InitFromBytes(byte[] bytes)
        {
            ExamineChunk(bytes);
            GetEntropy();
            GetSortedDistribution();
        }

        public DataEntropyUTF8(string fileName)
        {
            this.Clear();
            if (File.Exists(fileName))
            {
                ExamineChunk(File.ReadAllBytes(fileName));
                GetEntropy();
                GetSortedDistribution();
            }
        }

        public DataEntropyUTF8(Image<Gray, byte> image)
        {
            this.Clear();
            if (image != null)
            {
                this.InitFromBytes(image.Bytes);
            }
        }

        public DataEntropyUTF8(Image<Gray, double> image)
        {
            this.Clear();
            if (image != null)
            {
                this.InitFromBytes(image.Bytes);
            }
        }

        public DataEntropyUTF8(Image<Bgr, byte> image)
        {
            this.Clear();
            if (image != null)
            {
                this.InitFromBytes(image.Bytes);
            }
        }

        public DataEntropyUTF8(Image<Bgr, double> image)
        {
            this.Clear();
            if (image != null)
            {
                this.InitFromBytes(image.Bytes);
            }
        }

        public DataEntropyUTF8(byte[] data)
        {
            this.Clear();
            ExamineChunk(data);
            GetEntropy();
            GetSortedDistribution();
        }

        public DataEntropyUTF8()
        {
            this.Clear();
        }
    }
}
