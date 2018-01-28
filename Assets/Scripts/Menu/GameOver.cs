using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public GameObject winScreen;
	public GameObject lossScreen;

	void start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void Update()
	{
		if(Manager.won)
		{
			winScreen.SetActive (true);
			lossScreen.SetActive (false);
		} else if (Manager.lost)
		{
			winScreen.SetActive (false);
			lossScreen.SetActive (true);
		}
	}

	public void Menu()
	{
		Application.Quit ();
	}
}
