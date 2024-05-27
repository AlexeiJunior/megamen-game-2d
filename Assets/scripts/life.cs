using UnityEngine;
using System.Collections;

public class life : MonoBehaviour {
	private _PC player;
	public int health = 10;
	private UnityEngine.UI.Text txt;
	private bool alive = true;
	public AudioClip explodeSound;
	public AudioClip danoSound;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<_PC>();
		txt = GameObject.FindGameObjectWithTag("lifepoints").GetComponent<UnityEngine.UI.Text>();
		txt.text = health.ToString();
	}

	void Update () {
		fallTime();
		updateLife();
	}

	void updateLife(){
		txt.text = health.ToString();
	}

	void fallTime(){
		if(player.getPositionY() <= -15.5f && alive){ //it's just temporary
			alive = false;
			StartCoroutine(die());
		}
	}

	public void damage(int dano){
		player.play(danoSound);
		if(health - dano <= 0){
			StartCoroutine(die());
		}else{
			health -= dano;
		}
	}

	IEnumerator die(){
		player.play(explodeSound);
		player.setKinematicRb(true);
		player.setCanControl(false);
		player.explodePlayer();
		yield return new WaitForSeconds(0.7f);
		player.setScore(0); //se morrer setar pontos a 0
		Application.LoadLevel(Application.loadedLevel);
	}
}
