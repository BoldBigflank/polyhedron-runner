using UnityEngine;
using System.Collections;

public class ClearPointScript : MonoBehaviour {
	bool passed;
	// Use this for initialization
	void Start () {
		passed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player" && !passed){
			GameManager.score++;

			transform.parent.renderer.material.SetColor ("_Color", Color.white);
			transform.parent.renderer.material.SetColor ("_SpecColor", Color.white);
			passed = true;
		}
	}

}
