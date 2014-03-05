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

	void Start () {
		passed = false;
		Reset ();
		maxScale = 2000.0F;
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
	}
	
	// Update is called once per frame
	void Update () {
		aliveTime += Time.deltaTime;
		float scale =  Mathf.Pow(2, 2.0F*(GameManager.timer - timeOffset) );
		scale = Mathf.Min(scale, maxScale);
		transform.localScale = new Vector3(scale, scale, scale);
		transform.rotation =  GameManager.cubeRotation * rotationOffset;

		if(scale > 6.0F && !passed){ // Past the 
			GameManager.score++;
			originalColor = renderer.material.color;
			renderer.material.SetColor ("_Color", Color.white);
			renderer.material.SetColor ("_SpecColor", Color.white);
			isModifiedColor = true;
			passed = true;
			GameManager.numberOfCubes--; // Currently not deleting cubes, so subtract when they've passed
		}
		if(GameManager.rewind && isModifiedColor){
			renderer.material.SetColor("_Color", originalColor);
			renderer.material.SetColor("_SpecColor", originalColor);
			isModifiedColor = false;
		}

//		if(scale > 2000.0F){
//			GameManager.numberOfCubes--;
//			Destroy(gameObject);
//		}
	}

	void OnCollisionEnter(Collision collision){
		Debug.Log ("Collision!");
	}

	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "Player"){
			if(GameManager.gameInProgress){
				other.transform.parent = transform; // Make it a child so it gets sucked in.
				other.audio.Play ();
				gameController.SendMessage ("GameOver");
//				GameManager.gameInProgress = false;
//				GameManager.rewind = true;
			}
		}
	}

}
