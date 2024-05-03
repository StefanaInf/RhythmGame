using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressspace : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (this.transform.GetChild (0).gameObject.activeSelf == true) {
				this.transform.GetChild (0).gameObject.SetActive (false);
				return;
			}
			if (this.transform.GetChild (0).gameObject.activeSelf == false) {
				this.transform.GetChild (0).gameObject.SetActive (true);
				return;
			}
		}
		
	}
}
