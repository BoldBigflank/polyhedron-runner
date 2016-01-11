
var damping = 6.0;
var smooth = true;
// The distance in the x-z plane to the target
var distance = 10.0;

var bigR = 0.75F;
var r = 1.0F;
var a = 1.0F;
private var t : float;

@script AddComponentMenu("Positioning/Spirograph")

function LateUpdate () {
	
		t += Time.deltaTime;
//		transform.localPosition.z = target.position.z - distance;
		transform.localPosition.x = (bigR - r) * Mathf.Cos( r/bigR * t ) - a * Mathf.Cos( (1 + r/bigR )  * t );
		transform.localPosition.y = (bigR - r) * Mathf.Sin( r/bigR * t ) - a * Mathf.Sin( (1 + r/bigR )  * t );		

//	transform.LookAt(Vector3(0,0,-1.5) );
}

function Start () {
	// Make the rigid body not change rotation
   	t = Random.Range(0.0, 100.0);
}