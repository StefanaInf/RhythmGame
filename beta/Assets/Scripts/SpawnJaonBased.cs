using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnJaonBased : MonoBehaviour {

    public AnalyzeJson analyzeJson;
    public AudioPeer audioPeer; 
	Transform[] lineTransform = new Transform[8];
	public Transform spawnPosBalls;
	public Gradient colorGradient;
	Color[] color = new Color[8];
	Material[] materialLine = new Material[8];
	public Material materialsourceLine;
	Material[] materialBall = new Material[8];
	public Material materialsourceBall;
	public float diffuseColorMultiplier , emissionColorMultiplier;
	public float treshold, tresholdBallSpawn;
	public float ballEmissionMultiplier;
	public GameObject ball;
	List<GameObject> ballsPool;
	List <MeshRenderer> ballMeshRenderer;
	List<Rigidbody> ballRigidbody;
	public int pooledAmount;
    public static List<PointData> beatList = new List<PointData>();
	SavePointList savePointList = new SavePointList();

    float[] intervalBall = new float[8];
	public float ballIntervalTime;
	public bool spawnballs;
	//object pool
	public float minTimeScale, audioTimeScale;
    public int batchSize = 100;
    float[] ballSpawnCooldown = new float[8];

    public AudioSource audioSource;


    // Use this for initialization
    void Start () {
		
		ballsPool = new List<GameObject> ();
		ballMeshRenderer = new List<MeshRenderer> ();
		ballRigidbody = new List<Rigidbody> ();

		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (ball); 
			obj.SetActive(false); 
			ballsPool.Add (obj); 
			ballMeshRenderer.Add (obj.GetComponent<MeshRenderer> ()); 
			ballRigidbody.Add (obj.GetComponent<Rigidbody> ()); 
		}


		for (int i = 0; i < 8; i++) {
			intervalBall [i] = 5; 
			materialLine[i] = new Material(materialsourceLine); 
			materialBall [i] = new Material (materialsourceBall); 
			color[i] = colorGradient.Evaluate((1f / 8f) * i); 

			this.transform.GetChild (i).GetComponent<MeshRenderer> ().material = materialLine [i]; 
			lineTransform[i] = this.transform.GetChild (i).gameObject.transform; 

		}

		LoadFromJson("Assets/Debug.json");

		for(int i = 0;i < savePointList.points.Count; i++) {
			string str = "";
			for(int j = 0;j < 8; j++) {
				str += " " + savePointList.points[i].bandValues[j];
			}
			//Debug.Log(str);
		}

	}
    float lastProcessedTimestamp = 0f;
    bool _setTimeScale;
	void Update () {

        //if (audioSource.time >= beatList[0].timestamp)
        //{
        //    int numBatches = Mathf.CeilToInt((float)beatList.Count / batchSize);

        //    for (int batchIndex = 0; batchIndex < numBatches; batchIndex++)
        //    {
        //        int startIdx = batchIndex * batchSize;
        //        int endIdx = Mathf.Min((batchIndex + 1) * batchSize, beatList.Count);

        //        for (int bandIndex = 0; bandIndex < 8; bandIndex++)
        //        {
        //            float threshold = analyzeJson.bandThresholdsList[bandIndex][batchIndex];

        //            for (int i = startIdx; i < endIdx; i++)
        //            {
        //                PointData point = beatList[i];
        //                materialBall[bandIndex].SetColor("_EmissionColor", color[bandIndex] * ballEmissionMultiplier);

        //                if (spawnballs && point.bandValues[bandIndex] > threshold)
        //                {
        //                    for (int g = 0; g < ballsPool.Count; g++)
        //                    {
        //                        if (!ballsPool[g].activeInHierarchy)
        //                        {
        //                            ballsPool[g].transform.position = new Vector3(spawnPosBalls.position.x, spawnPosBalls.position.y, lineTransform[bandIndex].position.z);
        //                            ballMeshRenderer[g].material = materialBall[bandIndex];
        //                            ballsPool[g].SetActive(true);
        //                            ballRigidbody[g].AddForce(0, -5000, 0);
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        beatList.RemoveRange(startIdx, endIdx - startIdx);
        //    }
        //}

       

        Time.timeScale = 2.0f;
        //Debug.Log(audioSource.time + " " + beatList[0].timestamp);
        if (audioSource.time >= beatList[0].timestamp)
        {
            //Debug.Log(beatList.Count);
            for (int i = 0; i < 8; i++)
            {

                materialBall[i].SetColor("_EmissionColor", color[i] * ballEmissionMultiplier);
                if (spawnballs)
                {
                    // analyzeJson.bandThresholds[i]
                    if (ballSpawnCooldown[i] <= 0f && (beatList[0].bandValues[i] > analyzeJson.bandThresholds[i]))
                    {
                        //Debug.Log(beatList[0].bandValues[i]);
                        for (int g = 0; g < ballsPool.Count; g++)
                        {
                            if (!ballsPool[g].activeInHierarchy)
                            {
                                ballsPool[g].transform.position = new Vector3(spawnPosBalls.position.x, spawnPosBalls.position.y, lineTransform[i].position.z);
                                ballMeshRenderer[g].material = materialBall[i];
                                ballsPool[g].SetActive(true);
                                ballRigidbody[g].AddForce(0, -5000, 0);
                                ballSpawnCooldown[i] = 0.05f;
                                break;
                            }
                        }
                    }

                }
                ballSpawnCooldown[i] -= Time.deltaTime;
            }
            beatList.RemoveAt(0);
        }
    }

    //public void SaveToFile()
    //{
    //    string path = "Assets/Debug2.txt";

    //    using (StreamWriter sw = new StreamWriter(path, true))
    //    {

    //        sw.WriteLine(audioPeer.Amplitude);
    //        sw.WriteLine(Time.time);
    //        for (int i = 0; i < 8; i++)
    //        {
    //            sw.WriteLine("Audio Band Index: " + i + ", Value: " + audioPeer.audioBand[i]);
    //        }
    //    }
    //}

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
    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(new BeatListWrapper(beatList));
        string path = Application.persistentDataPath + "/beatData.json";
        File.WriteAllText(path, json);
        Debug.Log("Beat data written to: " + path);
    }

}

// Wrapper class to serialize list
[System.Serializable]
public class BeatListWrapper
{
    public List<PointData> beats;

    public BeatListWrapper(List<PointData> beats)
    {
        this.beats = beats;
    }
}
