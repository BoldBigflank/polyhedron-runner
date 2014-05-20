using UnityEngine;
using System.Collections;

public class CameraTouchScript : MonoBehaviour {
	float damping = 1.0F;
	Vector3 restPosition;
	Vector3 lastMousePosition;

	// Use this for initialization
	void Start () {
		restPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.gameInProgress) { return;}
		if(Input.touchCount == 1){
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Moved){
//				h = horizontalSpeed * touch.deltaPosition.x;
//				transform.Rotate (0, -h, 0, Space.World);
				Vector3 newPosition = transform.position - restPosition + (Vector3)touch.deltaPosition ;
				if (newPosition.magnitude > 1.0F) newPosition.Normalize();
				transform.position = Vector3.Lerp(transform.position, restPosition - newPosition, Time.deltaTime * damping);

				
//				v = verticalSpeed * touch.deltaPosition.y;
//				transform.Rotate (v, 0, 0, Space.World);

				GameManager.r = transform.rotation.eulerAngles;
			} else if (touch.phase == TouchPhase.Stationary){
				transform.position = Vector3.Lerp(transform.position, restPosition, Time.deltaTime * damping);
			}
		} else if (Input.GetMouseButton(0) == true) { // Mouse control
			if(Input.GetMouseButtonDown(0)){ lastMousePosition = Input.mousePosition;}
			
			transform.Rotate(Vector3.down * (Input.mousePosition.x - lastMousePosition.x), Space.World);
			transform.Rotate(Vector3.right * (Input.mousePosition.y - lastMousePosition.y), Space.World);
			lastMousePosition = Input.mousePosition;
//			GameManager.r = transform.rotation.eulerAngles;
		} else {
			transform.position = Vector3.Lerp(transform.position, restPosition, Time.deltaTime * damping);
		}
	}
}
