using UnityEngine;
using System.Collections;

public class spike : MonoBehaviour {
	private life lifePlayer;
	private int spikeDano = 1;
	private _PC player;
	private int knockbackPwr = 300;
	public bool turnDown = false; //if spike is turn upsidedown

	void Start () {
		lifePlayer = GameObject.FindGameObjectWithTag("life").GetComponent<life>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<_PC>();
	}

	void OnTriggerEnter2D (Collider2D colisor) {
		if(colisor.CompareTag("Player")){
			lifePlayer.damage(spikeDano);
			if(turnDown){
				StartCoroutine(player.knockback(-knockbackPwr));
			}
			else{
				StartCoroutine(player.knockback(knockbackPwr));
			}
		}
	}
}
