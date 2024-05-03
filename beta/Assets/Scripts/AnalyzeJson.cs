using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

public class AnalyzeJson : MonoBehaviour
{

    public static List<PointData> beatList = new List<PointData>();
    SavePointList savePointList = new SavePointList();
    public List<List<float>> bandThresholdsList = new List<List<float>>();

    public float[] bandThresholds = new float[8];
    public float multiplier;
    public int batchSize = 100;

    private void Start()
    {
        LoadFromJson("Assets/Debug.json");
        CalculateThresholdsDynamically();
        //CalculateThresholdsDynamicallyBatching();
    }

    public void CalculateThresholdsDynamicallyBatching()
    {
        bandThresholdsList = new List<List<float>>();
        int numBatches = Mathf.CeilToInt((float)beatList.Count / batchSize);

        for (int bandIndex = 0; bandIndex < 8; bandIndex++)
        {
            List<float> bandThresholds = new List<float>();

            for (int batchIndex = 0; batchIndex < numBatches; batchIndex++)
            {
                int startIndex = batchIndex * batchSize;
                int endIndex = Mathf.Min((batchIndex + 1) * batchSize, beatList.Count);

                List<float> bandValues = new List<float>();

                for (int i = startIndex; i < endIndex; i++)
                {
                    bandValues.Add(beatList[i].bandValues[bandIndex]);
                }

                float bandMean = bandValues.Average();
                float bandStdDev = 0f;

                foreach (float value in bandValues)
                {
                    bandStdDev += (value - bandMean) * (value - bandMean);
                }

                bandStdDev = Mathf.Sqrt(bandStdDev / bandValues.Count);

                float threshold = bandMean + multiplier * bandStdDev;
                bandThresholds.Add(threshold);
            }

            bandThresholdsList.Add(bandThresholds);
        }

        for (int batchIndex = 0; batchIndex < numBatches; batchIndex++)
        {
            string output = "Batch " + batchIndex + " Thresholds: ";
            for (int bandIndex = 0; bandIndex < 8; bandIndex++)
            {
                output += bandThresholdsList[bandIndex][batchIndex].ToString("F2");
                if (bandIndex < 7)
                {
                    output += ", ";
                }
            }
            Debug.Log(output);
        }
    }

    public void CalculateThresholdsDynamically()
    {
        float[] bandMeans = new float[8];
        float[] bandStdDevs = new float[8];

        for (int i = 0; i < beatList.Count; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                bandMeans[j] += beatList[i].bandValues[j];
            }
        }

        for (int j = 0; j < 8; j++)
        {
            bandMeans[j] /= beatList.Count;
        }

        for (int i = 0; i < beatList.Count; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                float deviation = beatList[i].bandValues[j] - bandMeans[j];
                bandStdDevs[j] += deviation * deviation;
            }
        }

        for (int j = 0; j < 8; j++)
        {
            bandStdDevs[j] = Mathf.Sqrt(bandStdDevs[j] / beatList.Count);
        }

        for (int j = 0; j < 8; j++)
        {
            bandThresholds[j] = bandMeans[j] + multiplier * bandStdDevs[j];
        }

        for (int i = 0; i < 8; i++)
            Debug.Log(bandThresholds[i]);
    }

    public void LoadFromJson(string path)
    {
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            savePointList = JsonUtility.FromJson<SavePointList>(jsonString);

            Debug.Log("OK!" + " " + savePointList.points.Count);
            beatList = savePointList.points;

        }
        else { Debug.Log(path); }

    }
}


//public void CalculateThresholdsDynamically()
//{
//    int numBatches = Mathf.CeilToInt((float)beatList.Count / batchSize);


//    for (int bandIndex = 0; bandIndex < 8; bandIndex++)
//    {
//        List<float> bandThresholds = new List<float>();
//        for (int batchIndex = 0; batchIndex < numBatches; batchIndex++)
//        {
//            List<PointData> batch = beatList.GetRange(batchIndex * batchSize, Mathf.Min(batchSize, beatList.Count - batchIndex * batchSize));
//            List<float> bandValues = new List<float>();
//            foreach (PointData point in batch)
//            {
//                bandValues.Add(point.bandValues[bandIndex]);
//            }

//            bandValues.Sort((a, b) => b.CompareTo(a));

//            int thresholdIndex = Mathf.CeilToInt(0.01f * bandValues.Count);
//            float threshold = bandValues[thresholdIndex];
//            bandThresholds.Add(threshold);
//        }
//        bandThresholdsList.Add(bandThresholds);
//    }

//    for (int i = 0; i < numBatches; i++)
//    {
//        string output = "BATCH " + i + " : ";
//        for (int j = 0; j < 8; j++)
//        {
//            output += bandThresholdsList[j][i].ToString("F2"); 
//            if (j < 7)
//            {
//                output += ",";
//            }
//        }
//        Debug.Log(output);
//    }
//}