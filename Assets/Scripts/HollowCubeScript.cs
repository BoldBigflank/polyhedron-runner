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
	ParticleSystem p;

	void Start () {
		passed = false;
		Reset ();
		maxScale = 1600.0F;
		isModifiedColor = false;
		gameController = GameObject.FindGameObjectWithTag("GameController");
		p = gameObject.GetComponentInChildren<ParticleSystem>();
		p.Stop ();
	}

	void Reset(){
		aliveTime = 0.0F;
		transform.localScale = Vector3.zero;
//		float x = Random.Range(0,360);
		float y = Random.Range(0,360);
		float z = Random.Range(-45,45);
		rotationOffset = Quaternion.Euler (new Vector3(0.0F, y, z));
		collided = false;
	}
	
	// Update is called once per frame
	void Update () {
		aliveTime += Time.deltaTime;
		float scale =  Mathf.Pow(2, 1.6F*(GameManager.current.timer - timeOffset) );
		if(scale > maxScale) scale = 0.0F;
//		scale = Mathf.Min(scale, maxScale);
		transform.localScale = new Vector3(scale, scale, scale);
		transform.rotation =  GameManager.current.cubeRotation * rotationOffset;
		transform.position = Vector3.zero;

		if(scale > 0.05F && scale < 10.0F && !p.isPlaying){
			p.Play ();
			p.startColor = GetComponent<Renderer>().material.color;
		}

		if(scale > 2.0F && !passed){ // Past the 
			GameManager.current.score++;
			originalColor = GetComponent<Renderer>().material.color;
			GetComponent<Renderer>().material.SetColor ("_Color", Color.white);
			GetComponent<Renderer>().material.SetColor ("_SpecColor", Color.white);
//			gameObject.GetComponent<Collider>().enabled = false;
			isModifiedColor = true;
			passed = true;
			GameManager.current.numberOfCubes--; // Currently not deleting cubes, so subtract when they've passed
			p.Stop ();
		}
		if(GameManager.current.rewind && isModifiedColor){
			GetComponent<Renderer>().material.SetColor("_Color", originalColor);
			GetComponent<Renderer>().material.SetColor("_SpecColor", originalColor);
//			gameObject.GetComponent<Collider>().enabled = true;
			isModifiedColor = false;
		}

	}


	void OnTriggerEnter(Collider other){
//		Debug.Log ("HollowCube OnTriggerEnter" + other.gameObject.tag);
		if(other.gameObject.tag == "Player"){
			if(GameManager.current.gameInProgress){
//				other.transform.parent = transform; // Make it a child so it gets sucked in.
				other.GetComponent<AudioSource>().Play ();
				gameController.SendMessage ("Hit");
			}
		}
	}


}
