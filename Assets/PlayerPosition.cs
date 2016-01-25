using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour {
	// Camera
	public GameObject camera;
	
	// Spirograph variables
	public float bigR = 0.75F;
	public float r = 1.0F;
	public float a = 1.0F;
	public float moveSpeed = 1.0f;
	float t;
	
	// VR Mode variables
	public float normalDistance = 2.0F;
	public float vrDistance = 0.125F;
	
	// Use this for initialization
	void Start () {
		t = Random.Range(0.0f, 100.0f);
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(GameManager.current.vrMode || !GameManager.current.gameInProgress){
			// No spirograph
			camera.transform.localPosition = Vector3.back * vrDistance;
			
		} else {
			// Playing a game and not vr mode
			t += Time.deltaTime;
			
			camera.transform.localPosition = new Vector3( 
                 (bigR - r) * Mathf.Cos( r/bigR * t * moveSpeed ) - a * Mathf.Cos( (1 + r/bigR )  * t * moveSpeed ),
                 (bigR - r) * Mathf.Sin( r/bigR * t * moveSpeed ) - a * Mathf.Sin( (1 + r/bigR )  * t * moveSpeed ),
			     -normalDistance
			);
			
		}
		
	}
}
