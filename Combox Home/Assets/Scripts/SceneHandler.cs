using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneHandler : MonoBehaviour {

	public void LoadMain(){
		SceneManager.LoadScene ("MainMenu");
	}

	public void loadInGame(){
		SceneManager.LoadScene ("InGame");
	}

	public void quit(){
		Application.Quit ();
	}
}
