using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValues : MonoBehaviour {
	public colorwall _colorwall;
	public GameObject lights;
	Slider slider;
	Light[] _spotlight = new Light[6];
	Light _directionalLight;

	public void SliderEmissionTreshold()
	{
		_colorwall.treshold = slider.value;
		_colorwall.tresholdBallSpawn = slider.value;
	}
	
	public void SliderMaxTimeScale()
	{
		_colorwall.audioTimeScale = slider.value;
	}
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
