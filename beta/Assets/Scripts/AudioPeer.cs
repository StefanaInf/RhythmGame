using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.IO;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour {
	AudioSource audioSource;

	//FFT values
	private float[] samplesLeft = new float[512];
	private float[] samplesRight = new float[512];

	private float[] freqBand = new float[8];
	private float[] bandBuffer = new float[8];
	private float[] bufferDecrease = new float[8];
	private float[] freqBandHighest = new float[8];

	//audio band values
	[HideInInspector]
    [System.NonSerialized] public float[] audioBand, audioBandBuffer;

	//Amplitude variables
	[HideInInspector]
	public float Amplitude, AmplitudeBuffer;
	private float AmplitudeHighest;
	//audio profile
	public float audioProfile;

	//stereo channels
	public enum _channel { Stereo, Left, Right };
	public _channel channel = new _channel();

	//Audio64
	float[] freqBand64 = new float[64];
	float[] bandBuffer64 = new float[64];
	float[] bufferDecrease64 = new float[64];
	float[] freqBandHighest64 = new float[64];
	//audio band64 values
	[HideInInspector]
	//public float[] audioBand64, audioBandBuffer64;

	SavePointList savePointList = new SavePointList();
	static float[] array = {-1f, -1f, -1f , -1f , -1f , -1f , -1f , -1f };
	PointData lastSavedPoint = new PointData(array,-1f,-1f);
	float lastSavedPointTime = -1f;

	
	void Start () {
		audioBand = new float[8];
		audioBandBuffer = new float[8];
		//audioBand64 = new float[64];
		//audioBandBuffer64 = new float[64];
		audioSource = GetComponent<AudioSource> ();
		AudioProfile (audioProfile);
        AmplitudeBuffer = 0;
        Amplitude = 0;

    }

	// Update is called once per frame
	void Update () {
		GetSpectrumAudioSource();
		MakeFrequencyBands();
		BandBuffer();
		CreateAudioBands();
		GetAmplitude();
		
		if (lastSavedPointTime != audioSource.time)
        {

			if (!lastSavedPoint.Equals(new PointData(audioBand, Amplitude, audioSource.time))) 
			{
                //Debug.Log(audioSource.time + "\n");
				float[] temp =new float[8];
				for (int i = 0; i < 8; i++)
				{
                    //Debug.Log(audioBand[i]);
					temp[i] = audioBand[i];
                }

				//Debug.Log("_________________");
                savePointList.AddPoint(new PointData(temp, Amplitude, audioSource.time));
                lastSavedPointTime = audioSource.time;
				lastSavedPoint = new PointData(temp, Amplitude, audioSource.time); ;
            }
            
        }
    }


	void AudioProfile(float audioProfile)
	{
		for (int i = 0; i < 8; i++) {
			freqBandHighest [i] = audioProfile;
		}
	}

	void GetAmplitude()
	{
		float CurrentAmplitude = 0;
		float CurrentAmplitudeBuffer = 0;
		for (int i = 0; i < 8; i++) {
			CurrentAmplitude += audioBand [i];
			CurrentAmplitudeBuffer += audioBandBuffer [i];
		}
		if (CurrentAmplitude > AmplitudeHighest) {
			AmplitudeHighest = CurrentAmplitude;
		}
		Amplitude = CurrentAmplitude / AmplitudeHighest;
		AmplitudeBuffer = CurrentAmplitudeBuffer / AmplitudeHighest;
	}

	void CreateAudioBands()
	{
		for (int i = 0; i < 8; i++) 
		{
			if (freqBand [i] > freqBandHighest [i]) {
				freqBandHighest [i] = freqBand [i];
			}
			audioBand [i] = Mathf.Clamp((freqBand [i] / freqBandHighest [i]), 0, 1);
			audioBandBuffer [i] = Mathf.Clamp((bandBuffer [i] / freqBandHighest [i]), 0, 1);
        }
	}

	void GetSpectrumAudioSource()
	{
		audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.BlackmanHarris);
		audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.BlackmanHarris);
	}


	void BandBuffer()
	{
		for (int g = 0; g < 8; ++g) {
			if (freqBand [g] > bandBuffer [g]) {
				bandBuffer [g] = freqBand [g];
				bufferDecrease [g] = 0.005f;
			}

			if ((freqBand [g] < bandBuffer [g]) && (freqBand [g] > 0)) {
				bandBuffer[g] -= bufferDecrease [g];
				bufferDecrease [g] *= 1.2f;
			}

		}
	}

	void MakeFrequencyBands()
	{
		int count = 0;

		for (int i = 0; i < 8; i++) {


			float average = 0;
			int sampleCount = (int)Mathf.Pow (2, i) * 2;

			if (i == 7) {
				sampleCount += 2;
			}
			for (int j = 0; j < sampleCount; j++) {
				if (channel == _channel.Stereo) {
					average += (samplesLeft [count] + samplesRight [count]) * (count + 1);
				}
				if (channel == _channel.Left) {
					average += samplesLeft [count] * (count + 1);
				}
				if (channel == _channel.Right) {
					average += samplesRight [count] * (count + 1);
				}
				count++;

			}

			average /= count;

			freqBand [i] = average * 10;

		}
	}

    public void SaveToFile()
    {
        string path = "Assets/Debug.json";
        string json = JsonUtility.ToJson(savePointList);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var sr = File.CreateText(path);
		sr.WriteLine(json);
		sr.Close();

        //     string path = "Assets/Debug.txt";

        //     using (StreamWriter sw = new StreamWriter(path, true))
        //     {

        //sw.WriteLine(Amplitude);
        //         sw.WriteLine(audioSource.time);
        //         for (int i = 0; i < 8; i++)
        //         {
        //             sw.WriteLine("Audio Band Index: " + i + ", Value: " + audioBand[i]);
        //         }
        //     }
    }


}