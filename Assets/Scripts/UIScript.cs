using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {

	public void MainMenuButton()
	{
		LoadScene (0);
	}
	public void PauseGameButton()
	{
		Time.timeScale = 0.0f;
	}
	public void ResumeGameButton()
	{
		Time.timeScale = 1.0f;
	}
	public void RestartGameButton()
	{
		LoadScene (1);
	}
	public void ExitButton()
	{
		Application.Quit();
	}
	void LoadScene(int numScene)
	{
		SceneManager.LoadScene (numScene);
	}
}
