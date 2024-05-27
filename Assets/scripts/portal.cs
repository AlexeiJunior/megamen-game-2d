using UnityEngine;
using System.Collections;

public class portal : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D colisor) {
		if(colisor.gameObject.tag == "Player"){
			transform.localScale = new Vector2(transform.localScale.x * 1.5f,transform.localScale.x * 1.5f);
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
