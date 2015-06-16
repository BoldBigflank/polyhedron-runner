using UnityEngine;
using System.Collections;

public class RotateOnTouch : MonoBehaviour {

	float h;
	float v;
	float horizontalSpeed = 1.0F;
	float verticalSpeed = 1.0F;
//	float damping = 24.0F;

	Vector3 lastMousePosition;
	GameObject mainCamera;

	// Use this for initialization
	void Start () {
		lastMousePosition = Vector3.zero;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.gameInProgress) { return;}
		transform.rotation = GameManager.cubeRotation;
		if(Input.touchCount == 1){
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Moved){
				h = horizontalSpeed * touch.deltaPosition.x * GameManager.sensitivity;
//				transform.Rotate (0, -h, 0, Space.World);
				transform.RotateAround(transform.position, mainCamera.transform.up, -h);

				v = verticalSpeed * touch.deltaPosition.y * GameManager.sensitivity;
//				transform.Rotate (v, 0, 0, Space.World);
				transform.RotateAround (transform.position, mainCamera.transform.right, v);
			}
		} else if (Input.GetMouseButton(0) == true) { // Mouse control
			if(Input.GetMouseButtonDown(0)){ lastMousePosition = Input.mousePosition;}
			transform.Rotate(Vector3.down * (Input.mousePosition.x - lastMousePosition.x), Space.World);
			transform.Rotate(Vector3.right * (Input.mousePosition.y - lastMousePosition.y), Space.World);
			lastMousePosition = Input.mousePosition;
		} 
//		else {  // Square it away
//			Vector3 newTransform = new Vector3 ( 
//			                        Mathf.RoundToInt( transform.rotation.eulerAngles.x / 90.0F ) * 90.0F, 
//			                        Mathf.RoundToInt( transform.rotation.eulerAngles.y / 90.0F ) * 90.0F, 
//			                        Mathf.RoundToInt( transform.rotation.eulerAngles.z / 90.0F ) * 90.0F);
////			transform.localEulerAngles = newTransform;
//			Debug.Log("no touches" + transform.rotation.eulerAngles.x + " " + transform.rotation.eulerAngles.y + transform.rotation.eulerAngles.z);
//			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(newTransform), Time.deltaTime * damping);
//
//		}
		GameManager.cubeRotation = transform.rotation;

	}
}


//#pragma strict
//
//private var h : float;
//private var v : float;
//private var horozontalSpeed : float = 2.0;
//private var verticalSpeed : float = 2.0;
//
//function Update()
//{
//	if (Input.touchCount == 1)
//	{
//		var touch : Touch = Input.GetTouch(0);
//		
//		if (touch.phase == TouchPhase.Moved)
//		{
//			h = horozontalSpeed * touch.deltaPosition.x ;
//			transform.Rotate( 0, -h, 0, Space.World );
//			
//			v = verticalSpeed * touch.deltaPosition.y ;
//			transform.Rotate( v, 0, 0, Space.World );
//		}
//	}
//}