using UnityEngine;
using System.Collections;

public class pause : MonoBehaviour {
	public GameObject PauseUI;
	private bool paused = false;
	private _PC playerController;
	public AudioClip menuSound;

	void Start () {
		PauseUI.SetActive(false);
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<_PC>();
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			playerController.play(menuSound);
			paused = !paused;
		}

		if(paused){
			pauseGame();
		}
		else{
			play ();
		}
	}

	void pauseGame(){
		PauseUI.SetActive(true);
		Time.timeScale = 0;
	}

	void play(){
		PauseUI.SetActive(false);
		Time.timeScale = 1;
	}

	public void resume(){
		paused = false;
	}

	public void restart(){
		Application.LoadLevel(Application.loadedLevel);
	}

	public void quit(){
		Application.Quit();
	}
}
