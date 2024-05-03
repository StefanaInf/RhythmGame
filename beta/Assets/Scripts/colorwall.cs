using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class colorwall : MonoBehaviour {

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

    float[] intervalBall = new float[8];
	public float ballIntervalTime;
	public bool spawnballs;
	//object pool
	public float minTimeScale, audioTimeScale;
	
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
	}

    bool _setTimeScale;
	// Update is called once per frame
	void Update () {

		//if (_setTimeScale) { Time.timeScale = minTimeScale + (audioPeer.AmplitudeBuffer * audioTimeScale); } else { _setTimeScale = true; } 

		Time.timeScale = 2.0f;

		for (int i = 0; i < 8; i++) {

			materialBall[i].SetColor("_EmissionColor", color[i] * ballEmissionMultiplier); 
			if (spawnballs) { 
				if ((audioPeer.audioBand [i] > tresholdBallSpawn) && (intervalBall [i] <= 0)) { 
					for (int g = 0; g < ballsPool.Count; g++) {
						if (!ballsPool [g].activeInHierarchy) { 
							ballsPool [g].transform.position = new Vector3 (spawnPosBalls.position.x, spawnPosBalls.position.y, lineTransform[i].position.z); 
							ballMeshRenderer [g].material = materialBall [i]; 
							ballsPool [g].SetActive (true); 
							ballRigidbody[g].AddForce (0, -5000, 0); 
							break; 
						}
					}

					intervalBall [i] = ballIntervalTime; 
				}

			}

			if (intervalBall[i] > 0f)
			{
				intervalBall[i] -= Time.deltaTime;
			}
        }
    }

    public void SaveToFile()
    {
        string path = "Assets/Debug2.txt";

        using (StreamWriter sw = new StreamWriter(path, true))
        {

            sw.WriteLine(audioPeer.Amplitude);
            sw.WriteLine(Time.time);
            for (int i = 0; i < 8; i++)
            {
                sw.WriteLine("Audio Band Index: " + i + ", Value: " + audioPeer.audioBand[i]);
            }
        }
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(new BeatListWrapper(beatList));
        string path = Application.persistentDataPath + "/beatData.json";
        File.WriteAllText(path, json);
        Debug.Log("Beat data written to: " + path);
    }

}

