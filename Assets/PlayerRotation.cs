using UnityEngine;
using System.Collections;

public class PlayerRotation : MonoBehaviour {
	public Transform target;
	Vector3 rotationDelta;
	Vector3 startPosition;
	Vector3 startScale;
	// Use this for initialization
	void Start () {
		rotationDelta = new Vector3(
			Random.Range(-10,10),
			Random.Range(-10,10),
			Random.Range(1,10));
		startPosition = transform.position;
		startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate( rotationDelta * Time.deltaTime);
		if(GameManager.gameInProgress){
			transform.position = target.position;//startPosition;
			transform.localScale = startScale;
			
		}
	}
}
