using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SliderValues : MonoBehaviour {
	public colorwall _colorwall;
	public GameObject _lights;
    UnityEngine.UI.Slider _slider;
	Light[] _spotlight = new Light[6];
	Light _directionalLight;

	public void SliderEmissionTreshold()
	{
		_colorwall.treshold = _slider.value;
		_colorwall.tresholdBallSpawn = _slider.value;
	}
	
	public void SliderMaxTimeScale()
	{
		_colorwall.audioTimeScale = _slider.value;
	}
	
	void Start () {
        if (_lights != null)
        {

            for (int i = 0; i < 6; i++)
            {
                _spotlight[i] = _lights.transform.GetChild(i).GetComponent<Light>();
            }
            _directionalLight = _lights.transform.GetChild(6).GetComponent<Light>();
        }
        _slider = GetComponent<UnityEngine.UI.Slider>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
