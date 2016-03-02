using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class start : MonoBehaviour {
	public GameObject controlsText;
	public GameObject audio;
	public Text HighScore;
	public bool mainMenu = false;

	// Use this for initialization
	void Start () {
		if (mainMenu) {
			DontDestroyOnLoad (audio);
			AudioSource[] audios = FindObjectsOfType<AudioSource>();
			if (audios.Length > 1) {
				Destroy(audio);
			}
			if (HighScore != null) {
				HighScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
			}
		}
	}

	public void something(){
		Application.LoadLevel ("Game"); 
	}

	public void menu(){
		Application.LoadLevel ("MainMenu");
	}

	public void quit(){
		Application.Quit ();
	}

	public void instruction(){
		controlsText.SetActive (!controlsText.activeInHierarchy);
	}

}
