using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static bool gameInProgress;
	bool paused;
	public static float timer;
	public static int numberOfCubes;
	public static int cubeIndex;
	public static int score;
	public static Quaternion cubeRotation;
	public static Vector3 r; // For camera rotation
	public static int sensitivity;
	public static bool rewind;

	public GUISkin guiSkin;
	public GUIStyle lightStyle;

	public Sprite soundOn;
	public Sprite soundOff;
	public Sprite play;
	public Sprite lowSensitivity;
	public Sprite highSensitivity;
	bool allowExitToHome;

	// UI Stuff
	public Canvas uiCanvas;
	public Image soundsImage;
	public Image controlsImage;
	public Button playButton;
	private EventSystem eventSystem;

	// PlayerPrefs settings
	string PlayerName;
	int highScore;
	bool sound;

	float swipeBuffer;
	Vector3 lastMousePosition;
	bool linedUp;

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
		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

		guiSkin.textField.fontSize = Mathf.Max (Screen.width, Screen.height) / 25;
		guiSkin.button.fontSize = Mathf.Max (Screen.width, Screen.height) / 25;
		guiSkin.label.fontSize = Mathf.Max (Screen.width, Screen.height) / 25;


		float sliderHeight = Screen.height * 0.7F;
		guiSkin.verticalSlider.fixedWidth = sliderHeight * 0.125F;
		guiSkin.verticalSlider.fixedHeight = sliderHeight;

		guiSkin.verticalSliderThumb.fixedWidth = sliderHeight * 0.125F;
		guiSkin.verticalSliderThumb.fixedHeight = sliderHeight * 0.125F;



		rewind = false;
		timer = 0.0F;
		gameInProgress = false;
		paused = false;
//		positions = new List<Quaternion>();
		rotationLog = new Stack<rotationEvent>();
		if(!PlayerPrefs.HasKey("sensitivity")){
			PlayerPrefs.SetInt ("sensitivity", 1);
		}

		sensitivity = PlayerPrefs.GetInt("sensitivity");

		if(!PlayerPrefs.HasKey("sound")){
			PlayerPrefs.SetInt ("sound", 1);
		}
		
		sound = PlayerPrefs.GetInt("sound") == 1;
		AudioListener.volume = (sound) ? 1.0F : 0.0F;


		if(!PlayerPrefs.HasKey("high")){
			PlayerPrefs.SetInt ("high", 0);
		}

		highScore = PlayerPrefs.GetInt ("high");
//		highScore = 0; // DEBUG PLEASE REMOVE

		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		// Start the music here
		mainCamera.GetComponent<AudioSource>().Play();
		logo = GameObject.FindGameObjectWithTag ("Logo");
		player = GameObject.FindGameObjectWithTag ("Player");

		soundsImage.sprite = (sound) ? soundOn : soundOff;
		controlsImage.sprite = (sensitivity == 1) ? lowSensitivity : highSensitivity;
		ShowMenu();

	}

	public void StartButton(){
		if(paused){
			HideMenu();
			Time.timeScale = 1.0F;
			paused = false;
		} else {
			NewGame();
		}
	}

	public void NewGame() {
		HideMenu();

		player.transform.parent = null;
		rewind = false;
		rotationLog.Clear ();
		rotationLog.Push (new rotationEvent(timer, cubeRotation));
		gameInProgress = true;
		paused = false;
		Time.timeScale = 1.0F;
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
		PlayerPrefs.SetInt("sensitivity", sensitivity);

		// Tutorial stuff
		if(highScore < 3){
			swipeBuffer = 250.0F;
			linedUp = false;
		} else {
			swipeBuffer = 0.0F;
		}
	}

	void ShowMenu(){
//		UnityEngine.Apple.TV.Remote.touchesEnabled = false; // For menu stuff
//		UnityEngine.Apple.TV.Remote.reportAbsoluteDpadValues = false; // For control based on position
		uiCanvas.enabled = true;
		eventSystem.sendNavigationEvents = true;

		logo.SetActive (true);
//		player.SetActive (false);

		playButton.Select();
		allowExitToHome = true;
		StartCoroutine( "EnableExitOnMenu");
	}

	void HideMenu(){
//		UnityEngine.Apple.TV.Remote.touchesEnabled = true; // For rotating stuff
		uiCanvas.enabled = false;
		eventSystem.sendNavigationEvents = false;
		logo.SetActive (false);
		player.SetActive (true);

		DisableExitOnMenu();
	}

	IEnumerator EnableExitOnMenu(){
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + 0.1F) {
			yield return null;
		}
		
//		UnityEngine.Apple.TV.Remote.allowExitToHome = allowExitToHome;
	}

	void DisableExitOnMenu(){
		allowExitToHome = false;
//		UnityEngine.Apple.TV.Remote.allowExitToHome = allowExitToHome;
	}

	void Hit(){
		rewind = true;
		gameInProgress = false;
		if(score > highScore){
			highScore = score;
			PlayerPrefs.SetInt ("high", score);
		}
	}

	void GameOver(){
		Debug.Log ("GameOver called");
		rewind = false;
		mainCamera.GetComponent<AudioSource>().pitch = 1.0F;
		ShowMenu();
		player.transform.parent = null;
	}

	public void ToggleSound(){
		sound = !sound;
		PlayerPrefs.SetInt("sound", (sound) ? 1:0);
		AudioListener.volume = (sound) ? 1.0F:0.0F;
		soundsImage.sprite = (sound) ? soundOn : soundOff;
	}
	
	public void ToggleSensitivity(){
		sensitivity = (sensitivity == 1) ? 2: 1;
		PlayerPrefs.SetInt("sensitivity", sensitivity);
		controlsImage.sprite = (sensitivity == 1) ? lowSensitivity : highSensitivity;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 

		if(Input.GetButtonUp ("Menu")){
			
			if(gameInProgress && !paused){
				// Pause button AND Menu button
				paused = true;
				Time.timeScale = 0.0F;
				ShowMenu ();
			} 
		}
		
		if(Input.GetButtonUp ("Cancel")){
			
			if(gameInProgress && !paused){
				// Pause button AND Menu button
				paused = true;
				Time.timeScale = 0.0F;
				ShowMenu ();
			} else {
				StartButton ();
			}
		}

		if(!paused && gameInProgress){
			if(swipeBuffer > 0.0F && highScore < 3 && timer > 3.0F){
				// Tutorial stuff

			} else {
				timer += Time.deltaTime * 0.85F; // Increment the timer (15% slower now)
				timer += Time.deltaTime * Mathf.Min((score/8) * 0.25F, 0.85F); // Speed up the timer 1/4 every 8 cubes, capping at 0.85
				if(timer > rotationLog.Peek ().time + rotationEventInterval){
					rotationLog.Push (new rotationEvent(timer, cubeRotation));
				}
			}

			if(Input.GetKeyDown (KeyCode.JoystickButton15)){ // Down
				Time.timeScale = (Time.timeScale == 1.0F) ? 0.0F : 1.0F;
			}

		}
//		if(!rewind && gameInProgress) timer += Time.deltaTime;

		if(Input.touchCount == 1){
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Moved){
				if(timer > 2.0F)
					swipeBuffer -= touch.deltaPosition.magnitude;
			}
		} else if (Input.GetMouseButton(0) == true) { // Mouse control
			if(Input.GetMouseButtonDown(0)){ lastMousePosition = Input.mousePosition;}
			if(timer > 2.0F) swipeBuffer -= (Input.mousePosition - lastMousePosition).magnitude;
			lastMousePosition = Input.mousePosition;

		} 

		if(rewind == true){
			mainCamera.GetComponent<AudioSource>().pitch = -0.4F;

			while(rotationLog.Count > 0 && rotationLog.Peek().time > timer){
				rotationLog.Pop();
			}

			float rewindSpeed = (score+1) * 1.75F; //Mathf.Min((score+1) * 2.4F / 4.0F, 19.2F);
			timer -= rewindSpeed * Time.deltaTime;
			if(timer <= 0.0F){
				GameOver(); // Reset the menu and stuff
			} else {
				if(rotationLog.Count != 0)
					cubeRotation = Quaternion.Slerp(cubeRotation, rotationLog.Peek().rotation, Time.deltaTime/(timer - rotationLog.Peek ().time ));
			}
		}

	}

	void OnGUI(){
		Event e = Event.current;
		if (e.isKey)
			Debug.Log("e.keyCode: "+e.keyCode);

		GUI.skin = guiSkin;
		if(gameInProgress){
			// Show Time and score
			GUI.Label (new Rect(Screen.width/4,Screen.height*0.9F,Screen.width/2,Screen.height*0.1F), score.ToString());
			GUI.skin.label.normal.textColor = Color.white;
			if(highScore < 3 && timer < 3.1F){
				GUI.Label (new Rect(0.0F, 0.0F, Screen.width, Screen.height * 0.2F), "Swipe to Rotate Objects");
			}else if(highScore < 3 && timer < 4.5F){
				GUI.Label (new Rect(0.0F, 0.0F, Screen.width, Screen.height * 0.2F), "Find the Gap");
			}

		} else if (!rewind) {
			GUI.skin.label.normal.textColor = Color.black;
//			// New Game Button
//			if(GUI.Button(new Rect(Screen.width * 0.55F, Screen.height * 0.65F, Screen.width * 0.10F, Screen.height* 0.15F), play)){
//				NewGame ();
//			}

//			Texture soundTexture = (sound) ? soundOn : soundOff;
//			if(GUI.Button(new Rect(Screen.width * 0.05F, Screen.height * 0.85F, Screen.width * 0.05F, Screen.width* 0.05F), soundTexture)){
//				sound = !sound;
//				PlayerPrefs.SetInt("sound", (sound) ? 1:0);
//				AudioListener.volume = (sound) ? 1.0F:0.0F;
//			}
			
//			if(GUI.Button(new Rect(Screen.width * 0.65F, Screen.height * 0.85F, Screen.width * 0.35F, Screen.width* 0.05F), "More Games")){
//				Application.OpenURL("http://bold-it.com/games-by-alex-swan/");
//			}
			


//			string toggleText = (sound) ? "Sound On":"Sound Off";
//			sound = GUI.Toggle (new Rect(Screen.width * 0.05F, Screen.height * 0.85F, Screen.width * 0.10F, Screen.height* 0.15F), sound, toggleText);
//			

//			GUI.Label (new Rect(Screen.width*0.05F, Screen.height*0.1F , Screen.width*0.04F, Screen.height * 0.8F), "S\nE\nN\nS\nI\nT\nI\nV\nI\nT\nY", lightStyle);

//			sensitivity = GUI.VerticalSlider(new Rect(Screen.width*0.05F, Screen.height*0.05F , guiSkin.verticalSlider.fixedWidth, guiSkin.verticalSlider.fixedHeight), sensitivity, 4.0F, 0.5F);
			GUI.Label(new Rect(Screen.width * 0.6F, Screen.height * 0.35F, Screen.width * 0.15F, Screen.height* 0.2F), "Last\n"+score.ToString());
			GUI.Label(new Rect(Screen.width * 0.25F, Screen.height * 0.35F, Screen.width * 0.15F, Screen.height* 0.2F), "Best\n"+highScore.ToString());

		}
	}
}
