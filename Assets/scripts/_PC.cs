using UnityEngine;
using System.Collections;

public class _PC : MonoBehaviour {
	private float vel = 50;
	private float maxVel = 3;
	private float maxVelAux;
	private Animator anime;
	private Rigidbody2D rb;
	private BoxCollider2D col;
	private float force = 380;
	private bool isGrounded;
	private bool isWall;
	private bool jumped = false;
	private Transform land;
	private Transform wall;
	private float slideTime = 0.5f;
	private float slideDelay = 0.5f;
	private bool sli = false;
	private float tamOffsetOrigX, tamOffsetOrigY, tamSizeOrigX, tamSizeOrigY;
	private float tamOffsetY = 0.24f, tamSizex = 0.48f, tamSizey = 0.48f;
	private static int score = 0;
	private UnityEngine.UI.Text txt;
	private bool enableClimbWall = true;
	private bool facingRight;
	private bool canControl = true;
	private bool firstGS = false;

	public AudioSource efxSound;
	public AudioClip groundSound;
	public AudioClip jumpSound;
	public AudioClip slideSound;

	void Start () {
		maxVelAux = maxVel;
		anime = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
		land = GameObject.FindGameObjectWithTag("Gr").GetComponent<Transform>();
		wall = GameObject.FindGameObjectWithTag("wall").GetComponent<Transform>();
		tamOffsetOrigX = col.offset.x;
		tamOffsetOrigY = col.offset.y;
		tamSizeOrigX = col.size.x;
		tamSizeOrigY = col.size.y;
		txt = GameObject.FindGameObjectWithTag("score").GetComponent<UnityEngine.UI.Text>();
		scoreStr(score.ToString());
	}

	void FixedUpdate(){
		if(canControl){
			run();
			jump();
			slide();
			wallClimb();
		}
	}

	void slide(){
		if(Input.GetKeyDown(KeyCode.LeftShift) && !sli && isGrounded){
			play(slideSound);
			anime.SetBool ("slide", true);
			StartCoroutine(slideFaster());
			slideTime = slideDelay;
			sli = true;
			col.offset = new Vector2(tamOffsetOrigX,tamOffsetY);
			col.size = new Vector2(tamSizex,tamSizey);
		}
		slideTime -= Time.deltaTime;
		if(slideTime <= 0 && sli){
			maxVel = maxVelAux; //temporario
			anime.SetBool ("slide", false);
			sli = false;
			col.offset = new Vector2(tamOffsetOrigX,tamOffsetOrigY);
			col.size = new Vector2(tamSizeOrigX ,tamSizeOrigY);
		}
	}

	IEnumerator slideFaster(){
		maxVel *= 3;
		float h = Input.GetAxisRaw("Horizontal");
		rb.AddForce((Vector2.right * vel * 10) * h);
		yield return new WaitForSeconds(1);
		rb.velocity = new Vector2(rb.velocity.x,0);
		maxVel = maxVelAux;
	}

	void wallClimb(){
		if(enableClimbWall){
			isGrounded = Physics2D.OverlapCircle(land.position, 0.002f);
			isWall = Physics2D.OverlapCircle(wall.position, 0.002f); //cria um circulo de colisao de 0.002f para verificar se e vdd ou falso
			if(isWall && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))){
				rb.velocity = new Vector2(rb.velocity.x,-0.5f);
			}
			if(isWall && facingRight && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.D) && !isGrounded){
				play(jumpSound);
				rb.velocity = new Vector2(rb.velocity.x,0);
				rb.AddForce(new Vector2(-0.5f, 1.2f) * force);

				rb.velocity = new Vector2(rb.velocity.x,0); //cod do jump
				col.offset = new Vector2(tamOffsetOrigX,tamOffsetOrigY);
				col.size = new Vector2(tamSizeOrigX ,tamSizeOrigY);
				rb.AddForce(new Vector2(rb.velocity.x,force/6));
				jumped = true;
			}
			else if(isWall && !facingRight && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.A) && !isGrounded){
				play(jumpSound);
				rb.velocity = new Vector2(rb.velocity.x,0);
				rb.AddForce(new Vector2(0.5f, 1.2f) * force);

				rb.velocity = new Vector2(rb.velocity.x,0); //cod do jump
				col.offset = new Vector2(tamOffsetOrigX,tamOffsetOrigY);
				col.size = new Vector2(tamSizeOrigX ,tamSizeOrigY);
				rb.AddForce(new Vector2(rb.velocity.x,force/6));
				jumped = true;
			}
		}
	}

	void jump(){
		isGrounded = Physics2D.OverlapCircle(land.position, 0.02f); //cria um circulo de colisao de 0.02f para verificar se e vdd ou falso
		if(isGrounded){
			if(firstGS){
				play(groundSound);
				firstGS = false;
			}
			jumped = false;
			anime.SetTrigger("ground");
		}else{
			jumped = true;
			firstGS = true;
			anime.SetTrigger("jump");
		}

		if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !jumped){
			play(jumpSound);
			rb.velocity = new Vector2(rb.velocity.x,0);
			col.offset = new Vector2(tamOffsetOrigX,tamOffsetOrigY);
			col.size = new Vector2(tamSizeOrigX ,tamSizeOrigY);
			rb.AddForce(new Vector2(rb.velocity.x,force));
			jumped = true;
		}
	}

	void run(){
		fakeFriction();
		anime.SetFloat("run",Mathf.Abs(Input.GetAxisRaw("Horizontal")));
		float h = Input.GetAxisRaw("Horizontal");
		if(Input.GetAxisRaw("Horizontal") > 0 ){
			//transform.Translate(Vector2.right * vel * Time.deltaTime); OLD BUT GOLD
			transform.eulerAngles = new Vector2(0,0);
			facingRight = true;
		}
		if(Input.GetAxisRaw("Horizontal") < 0){
			transform.eulerAngles = new Vector2(0,180);
			facingRight = false;
		}

		//move player with force
		rb.AddForce((Vector2.right * vel)* h);

		//set limits to velocity
		if(rb.velocity.x > maxVel){
			rb.velocity = new Vector2(maxVel,rb.velocity.y);
		}
		if(rb.velocity.x < -maxVel){
			rb.velocity = new Vector2(-maxVel,rb.velocity.y);
		}
	}

	void fakeFriction(){
		Vector3 easeVel = rb.velocity;
		easeVel.x *= 0.75f; //reduce velocity a little faster
		if(isGrounded) rb.velocity = easeVel;
	}

	public void scoreStr(string score){
		txt.text = " " + score;
	}

	public void setScore(int s){
		score = s;
		scoreStr(score.ToString());
	}

	void OnTriggerEnter2D (Collider2D colisor) {
		if(colisor.gameObject.tag == "point"){
			score++;
			scoreStr(score.ToString());
		}
	}

	public IEnumerator knockback(float knockbackPwr){
		rb.velocity = new Vector2(rb.velocity.x,0);
		rb.AddForce(new Vector2(rb.velocity.x * -knockbackPwr, knockbackPwr)); //x(velocidade ao contrario) y(forca para o alto) e estou usando a mesma forca

		//RedFlash LOOK THIS *0*
		GetComponent<Renderer>().material.color = Color.white;
		yield return new WaitForSeconds(.08f);
		GetComponent<Renderer>().material.color = Color.red;
		yield return  new WaitForSeconds(.08f);
		GetComponent<Renderer>().material.color = Color.white;
		yield return  new WaitForSeconds(.08f);
		GetComponent<Renderer>().material.color = Color.red;
		yield return  new WaitForSeconds(.08f);
		GetComponent<Renderer>().material.color = Color.white;
	}

	public void explodePlayer(){
		gameObject.GetComponent<Animator>().Play("playerExploding");
	}
	
	public float getPositionY(){
		return transform.position.y;
	}

	public void play(AudioClip clip){
		efxSound.clip = clip;
		efxSound.Play();
	}

	public void setKinematicRb(bool kinematic){
		rb.isKinematic = kinematic;
	}

	public void setCanControl(bool canControl){
		this.canControl = canControl;
	}
}
