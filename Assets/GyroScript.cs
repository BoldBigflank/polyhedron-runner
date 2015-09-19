using UnityEngine;
using System.Collections;

public class GyroScript : MonoBehaviour {
	[SerializeField]
	GameObject constructor;
	// Use this for initialization

	Gyroscope gyro = Input.gyro;

	void Start () {
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
//		Input.gyro.attitude;
		Quaternion attitudeFix = new Quaternion(gyro.attitude.x, gyro.attitude.y, -gyro.attitude.z, -gyro.attitude.w);
		Debug.Log (Input.gyro.attitude);
		constructor.transform.rotation = attitudeFix;


	}
}
