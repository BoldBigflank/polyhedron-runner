using UnityEngine;
using System.Collections;

public class PlayerCollisionScript : MonoBehaviour {
	GameObject gameController;
	float firstScale;
	// Use this for initialization
	void Start () {
		firstScale = Mathf.Log (Mathf.Abs (transform.position.z), 2);
		gameController = GameObject.FindGameObjectWithTag("GameController");
		Debug.Log ("firstscale" + firstScale.ToString());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!GameManager.gameInProgress || GameManager.rewind) return;
//		Debug.Log (transform.position);

		// Check for collisions
		Vector3 direction = new Vector3(transform.position.x, transform.position.y, -1.0F * Mathf.Pow(2, 1.59F - Time.fixedDeltaTime)) - transform.position;

//		Debug.Log (direction);
//		Debug.DrawRay(transform.position, direction.normalized, Color.red);
//
//		RaycastHit hit;
//		if(Physics.SphereCast (transform.position, GetComponent<SphereCollider>().radius * transform.localScale.x, direction, out hit, direction.magnitude ))
//		{
//			Debug.Log (hit.transform.tag);
//			if(hit.transform.tag == "HollowCube"){
//				Debug.Log (hit.transform.tag);
//				Debug.Break();
//				transform.parent = hit.transform;
//				audio.Play ();
//				gameController.SendMessage ("GameOver");
//			}
//		}

	}
}
