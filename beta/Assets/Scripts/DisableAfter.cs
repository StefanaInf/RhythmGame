using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfter : MonoBehaviour {
	public float _timeInSeconds;
	void OnEnable()
	{
		Invoke ("Destroy", _timeInSeconds);
		GetComponent<Rigidbody> ().isKinematic = false;
	}

	void Destroy()
	{
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		CancelInvoke ();
		GetComponent<Rigidbody> ().isKinematic = true;
	}
}
