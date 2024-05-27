using UnityEngine;
using System.Collections;

public class coin : MonoBehaviour {

	private _PC playerController;
	public AudioClip coinSound;

	void Start () {
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<_PC>();
	}

	/* Com RigidBody apenas para colisao sem trigger
	 * 
	 * void OnCollisionEnter2D (Collision2D colisor) {
		if(colisor.gameObject.tag == "Player"){
			GetComponent<Rigidbody2D>().AddForce(transform.up * 200);
			Destroy(gameObject,0.1f);
		}
	}*/

	void OnTriggerEnter2D (Collider2D colisor) {
		if(colisor.gameObject.tag == "Player"){
			playerController.play(coinSound);
			Destroy(gameObject,0.1f);
		}
	}
}
