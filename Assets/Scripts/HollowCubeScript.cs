using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HollowCubeScript : MonoBehaviour {
	float aliveTime;
	public float timeOffset;
	Quaternion rotationOffset;
	bool passed;
	float maxScale;
	Color originalColor;
	bool isModifiedColor;
	GameObject gameController;
	bool collided;

	void Start () {
		passed = false;
		Reset ();
		maxScale = 1600.0F;
		isModifiedColor = false;
		gameController = GameObject.FindGameObjectWithTag("GameController");

	}

	void Reset(){
		aliveTime = 0.0F;
		transform.localScale = Vector3.zero;
		float x = Random.Range(0,360);
		float y = Random.Range(0,360);
		float z = Random.Range(0,360);
		rotationOffset = Quaternion.Euler (new Vector3(x, y, z));
		collided = false;
	}
	
	// Update is called once per frame
	void Update () {
		aliveTime += Time.deltaTime;
		float scale =  Mathf.Pow(2, 1.6F*(GameManager.timer - timeOffset) );
		if(scale > maxScale) scale = 0.0F;
//		scale = Mathf.Min(scale, maxScale);
		transform.localScale = new Vector3(scale, scale, scale);
		transform.rotation =  GameManager.cubeRotation * rotationOffset;
		transform.position = Vector3.zero;


		if(scale > 3.0F && !passed){ // Past the 
			GameManager.score++;
			originalColor = GetComponent<Renderer>().material.color;
			GetComponent<Renderer>().material.SetColor ("_Color", Color.white);
			GetComponent<Renderer>().material.SetColor ("_SpecColor", Color.white);
			isModifiedColor = true;
			passed = true;
			GameManager.numberOfCubes--; // Currently not deleting cubes, so subtract when they've passed
		}
		if(GameManager.rewind && isModifiedColor){
			GetComponent<Renderer>().material.SetColor("_Color", originalColor);
			GetComponent<Renderer>().material.SetColor("_SpecColor", originalColor);
			isModifiedColor = false;
		}

//		if(scale > 2000.0F){
//			GameManager.numberOfCubes--;
//			Destroy(gameObject);
//		}
	}

//	void OnCollisionEnter(Collision collision){
//		Debug.Log ("Collision!" + collision.transform.tag);
//	}
//
//	void OnCollisionExit(Collision collision){
//		Debug.Log ("CollisionExit" + collision.transform.tag);
//	}

	void OnTriggerEnter(Collider other){
//		Debug.Log ("HollowCube OnTriggerEnter" + other.gameObject.tag);
		if(other.gameObject.tag == "Player"){
			if(GameManager.gameInProgress){
				other.transform.parent = transform; // Make it a child so it gets sucked in.
				other.GetComponent<AudioSource>().Play ();
				gameController.SendMessage ("Hit");
			}
		}
	}


}
