using UnityEngine;
using System.Collections;

public class PlayerClearPointScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		transform.position = Vector3.up / 5.0F;
	}

	void OnCollisionEnter(Collision collision){
//		if(collision.transform.tag == "HollowCube")
		Debug.Log ("PlayerClearPoint OnCollisionEnter > " + collision.transform.tag);
	}

	void OnCollisionExit(Collision collision){
//		if(collision.transform.tag == "HollowCube")
		Debug.Log ("PlayerClearPoint OnCollisionExit > " + collision.transform.tag);
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "HollowCube"){
			other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
		}
		Debug.Log ("PlayerClearPoint OnTriggerEnter > "+ other.transform.tag);

	}

	void OnTriggerStay(Collider other){
		Debug.Log ("PlayerClearPoint OnTriggerStay > "+ other.transform.tag);

	}
	
	void OnTriggerExit(Collider other){
		Debug.Log ("PlayerClearPoint OnTriggerExit > "+ other.transform.tag);

	}
}
