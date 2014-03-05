using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static bool gameInProgress;
	public static float timer;
	public static int numberOfCubes;
	public static int cubeIndex;
	public static int score;
	public static Quaternion cubeRotation;
	public static Vector3 r; // For camera rotation
	public static float sensitivity;
	public static bool rewind;

	public GUISkin guiSkin;
	public GUIStyle lightStyle;

	// PlayerPrefs settings
	string PlayerName;
	int highScore;

	struct rotationEvent {
		public float time;
		public Quaternion rotation;
		public rotationEvent(float t, Quaternion r){
			time = t;
			rotation = r;
		}
	};

	static Stack<rotationEvent> rotationLog;
	public float rotationEventInterval = .125F;
	GameObject mainCamera;
	GameObject logo;
	GameObject player;

	// Use this for initialization
	void Start () {
		guiSkin.textField.fontSize = Mathf.Max (Screen.width, Screen.height) / 25;
		guiSkin.button.fontSize = Mathf.Max (Screen.width, Screen.height) / 25;
		guiSkin.label.fontSize = Mathf.Max (Screen.width, Screen.height) / 25;

		guiSkin.verticalSlider.fixedWidth = Screen.width * 0.05F;
		guiSkin.verticalSliderThumb.fixedWidth = Screen.width * 0.05F;
		guiSkin.verticalSliderThumb.fixedHeight = Screen.width * 0.05F;



		rewind = false;
		timer = 0.0F;
		gameInProgress = false;
//		positions = new List<Quaternion>();
		rotationLog = new Stack<rotationEvent>();
		if(!PlayerPrefs.HasKey("sensitivity")){
			PlayerPrefs.SetFloat ("sensitivity", 2.0F);
		}

		sensitivity = PlayerPrefs.GetFloat("sensitivity");

		if(!PlayerPrefs.HasKey("high")){
			PlayerPrefs.SetInt ("high", 0);
		}

		highScore = PlayerPrefs.GetInt ("high");

		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		logo = GameObject.FindGameObjectWithTag ("Logo");
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void NewGame() {
		logo.SetActive (false);
		player.SetActive (true);
		player.transform.parent = null;
		rewind = false;
		rotationLog.Clear ();
		rotationLog.Push (new rotationEvent(timer, cubeRotation));
		gameInProgress = true;
		mainCamera.GetComponent<AudioSource>().pitch = 1.0F;
		timer = 0.0F;
		score = 0;

		numberOfCubes = 0;
		cubeIndex = 0;
		GameObject[] leftoverCubes = GameObject.FindGameObjectsWithTag("HollowCube");
		foreach(GameObject g in leftoverCubes){
			Destroy (g);
		}

		// Save the sensitivity
		PlayerPrefs.SetFloat("sensitivity", sensitivity);
	}

	void GameOver(){
		// Currently not used
		rewind = true;
		gameInProgress = false;
		if(score > highScore){
			highScore = score;
			PlayerPrefs.SetInt ("high", score);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 

		if(gameInProgress){
			timer += Time.deltaTime; // Increment the timer
			timer += Time.deltaTime * Mathf.Min((score/8)/3.0F, 1.0F); // Speed up the timer 1/3 every 8 cubes
			if(timer > rotationLog.Peek ().time + rotationEventInterval){
				rotationLog.Push (new rotationEvent(timer, cubeRotation));
			}
		}
//		if(!rewind && gameInProgress) timer += Time.deltaTime;


		if(rewind == true){
			mainCamera.GetComponent<AudioSource>().pitch = -0.4F;

			while(rotationLog.Count > 0 && rotationLog.Peek().time > timer){
				rotationLog.Pop();
			}

			float rewindSpeed = Mathf.Min((score+1) * 2.4F / 4.0F, 19.2F);
			timer -= rewindSpeed * Time.deltaTime;
			if(timer <= 0.0F){
				rewind = false;
				mainCamera.GetComponent<AudioSource>().pitch = 1.0F;
				logo.SetActive(true);
			} else {
				if(rotationLog.Count != 0)
					cubeRotation = Quaternion.Slerp(cubeRotation, rotationLog.Peek().rotation, Time.deltaTime/(timer - rotationLog.Peek ().time ));
			}
		}

	}

	void OnGUI(){
		GUI.skin = guiSkin;
		if(gameInProgress){
			// Show Time and score
//			GUI.Label (new Rect(10,10,100,100), timer.ToString());
			GUI.Label (new Rect(Screen.width/4,Screen.height*0.9F,Screen.width/2,Screen.height*0.1F), score.ToString());

		} else if (!rewind) {
			// New Game Button
			if(GUI.Button(new Rect(Screen.width * 0.55F, Screen.height * 0.65F, Screen.width * 0.10F, Screen.height* 0.15F), ">>")){
				NewGame ();
			}

			GUI.Label (new Rect(Screen.width*0.05F, Screen.height*0.1F , Screen.width*0.04F, Screen.height * 0.8F), "S\nE\nN\nS\nI\nT\nI\nV\nI\nT\nY", lightStyle);
			sensitivity = GUI.VerticalSlider(new Rect(Screen.width*0.05F, Screen.height*0.1F , Screen.width*0.05F, Screen.height * 0.8F), sensitivity, 4.0F, 0.5F);
			GUI.Label(new Rect(Screen.width * 0.6F, Screen.height * 0.35F, Screen.width * 0.15F, Screen.height* 0.2F), "Last\n"+score.ToString());
			GUI.Label(new Rect(Screen.width * 0.25F, Screen.height * 0.35F, Screen.width * 0.15F, Screen.height* 0.2F), "Best\n"+highScore.ToString());

		}
	}
}
