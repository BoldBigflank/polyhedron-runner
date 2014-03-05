using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConstructorScript : MonoBehaviour {
	public GameObject cube;
	public GameObject[] shapes;
	
	int maxCubes = 5;

	List<Color> colors;

	// Use this for initialization
	void Start () {
		colors = new List<Color>();

		colors.Add(HexToColor("3399FF"));
		colors.Add(HexToColor("FF33CC"));
		colors.Add(HexToColor("FF0D00"));

		colors.Add(HexToColor("FFA500"));
		colors.Add(HexToColor("FFFF00"));
		colors.Add(HexToColor("CCFF00"));

		colors.Add(HexToColor("FFF52D"));
		colors.Add(HexToColor("A0F919"));
		colors.Add(HexToColor("EF0071"));
		colors.Add(HexToColor("7807DF"));
		colors.Add(HexToColor("16E0D2"));


//		colors.Add(Color.red);
//		colors.Add(Color.yellow);
//		colors.Add(new Color(205.0F/255.0F, 127.0F/255.0F, 50.0F/255.0F));
//		colors.Add(new Color(191.0F/255.0F, 193.0F/255.0F, 194.0F/255.0F));
//		colors.Add(Color.green);
//		colors.Add(Color.cyan);
//		colors.Add(Color.blue);
//		colors.Add(Color.magenta);
	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	void AddCube(){

		GameObject c = (GameObject)Instantiate(shapes[Random.Range(0, shapes.Length)]);
		c.GetComponent<HollowCubeScript>().timeOffset = (GameManager.cubeIndex) * (2.57F) + 4 ;
		Color cubeColor = colors[  GameManager.cubeIndex % colors.Count];
		c.GetComponent<Renderer>().material.SetColor ("_Color", cubeColor);
//		c.GetComponent<Renderer>().material.SetColor ("_SpecColor", cubeColor);
//		c.GetComponent<Renderer>().material.SetColor ("_ReflectColor", cubeColor);

		GameManager.cubeIndex++;
		GameManager.numberOfCubes++;
	}



	// Update is called once per frame
	void Update () {
		if(GameManager.gameInProgress){
			while(GameManager.numberOfCubes < maxCubes){
				AddCube();
			}
		}

	}
}
