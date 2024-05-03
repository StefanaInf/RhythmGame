using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class ScaleOnAmplitude : MonoBehaviour
{
    public float startScale, maxScale;
    public bool useBuffer;

    void Update()
    {
        if (!useBuffer)
        {
            if (!float.IsNaN(MainMenuAudio.Amplitude) && !float.IsInfinity(MainMenuAudio.Amplitude))
            {
                transform.localScale = new Vector3((MainMenuAudio.Amplitude * maxScale) + startScale,
                                                    (MainMenuAudio.Amplitude * maxScale) + startScale,
                                                    (MainMenuAudio.Amplitude * maxScale) + startScale);
            }
        }
        else
        {
            if (!float.IsNaN(MainMenuAudio.AmplitudeBuffer) && !float.IsInfinity(MainMenuAudio.AmplitudeBuffer))
            {
                transform.localScale = new Vector3((MainMenuAudio.AmplitudeBuffer * maxScale) + startScale,
                                                    (MainMenuAudio.AmplitudeBuffer * maxScale) + startScale,
                                                    (MainMenuAudio.AmplitudeBuffer * maxScale) + startScale);
            }
        }
    }
}
