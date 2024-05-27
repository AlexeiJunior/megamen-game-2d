using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {
	private UnityEngine.UI.Text txt;
	private float time = 0;

	void Start () {
		txt = GameObject.FindGameObjectWithTag("timer").GetComponent<UnityEngine.UI.Text>();
	}
	

	void Update () {
		time += Time.deltaTime;
		string minutes = Mathf.Floor(time / 60).ToString("00");
		string seconds = Mathf.Floor(time % 60).ToString("00");
		if(time < 3600){
			txt.text = minutes + ":" + seconds;
		}else{
			txt.text = "O.o";
		}
	}
}
