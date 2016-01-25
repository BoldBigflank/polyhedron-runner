using UnityEngine;
using System.Collections;

public class RotateOnTouch : MonoBehaviour {

	float h;
	float v;
	float horizontalSpeed = 1.0F;
	float verticalSpeed = 1.0F;

	Vector3 lastMousePosition;
	float mouseX = 0;
	float mouseY = 0;
	float mouseZ = 0;
	
	GameObject mainCamera;

	// Use this for initialization
	void Start () {
		lastMousePosition = Vector3.zero;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion rot;
//		if(!GameManager.current.gameInProgress) { return;}
		transform.rotation = GameManager.current.cubeRotation;
		if(Input.touchCount == 1){
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Moved){
				h = horizontalSpeed * touch.deltaPosition.x * ((float)GameManager.current.sensitivity / 4.0F);
				transform.RotateAround(transform.position, mainCamera.transform.up, -h);

				v = verticalSpeed * touch.deltaPosition.y * ((float)GameManager.current.sensitivity / 4.0F);
				transform.RotateAround (transform.position, mainCamera.transform.right, v);
			}
		} else if (Input.GetMouseButton(0) == true) { // Mouse control
			if(Input.GetMouseButtonDown(0)){ lastMousePosition = Input.mousePosition;}
//			transform.Rotate(Vector3.down * (Input.mousePosition.x - lastMousePosition.x), Space.World);
//			transform.Rotate(Vector3.right * (Input.mousePosition.y - lastMousePosition.y), Space.World);
			lastMousePosition = Input.mousePosition;
			
			mouseX += Input.GetAxis("Mouse X") * 5;
			if (mouseX <= -180) {
				mouseX += 360;
			} else if (mouseX > 180) {
				mouseX -= 360;
			}
			mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
			mouseY = Mathf.Clamp(mouseY, -85, 85);
			
			
		} 
		rot = Quaternion.Euler(mouseY, mouseX, mouseZ);
		
		GameManager.current.cubeRotation = transform.rotation;

	}
}