using UnityEngine;
using System.Collections;

public class CardboardOrbit : MonoBehaviour {
	public GameObject player;
	
	public Transform target;
	public float distance = 10.0f;
	
	float xSpeed = 250.0f;
	float ySpeed = 120.0f;
	
	float yMinLimit = -20.0f;
	float yMaxLimit = 80.0f;
	
	private float x = 0.0f;
	private float y = 0.0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void LateUpdate () {
		if (target) {
			//			x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
			//			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
			//			
			//			y = ClampAngle(y, yMinLimit, yMaxLimit);
			//			
			Quaternion rotation = transform.rotation;//Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * (distance * Vector3.back) + target.position;
			
			//			transform.rotation = rotation;
			transform.position = position;
//			Vector3 playerPosition = player.transform.localPosition;
//			if(GameManager.vrMode)
//				player.transform.localPosition = new Vector3(playerPosition.x, playerPosition.y, 3.0F);
//			else {
//				player.transform.localPosition = Vector3.zero;
//			}
		}
	}
}
