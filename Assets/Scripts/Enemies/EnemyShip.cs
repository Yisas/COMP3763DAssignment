using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour {

    public float speed;

	public GameObject deathEffect;
    public AudioClip[] deathAudio;
	public float deathRotationMin;
	public float deathRotationMax;

    private GameController gameController;
    protected Rigidbody rb;
    private EnemyFormation enemyFormation;
    private AudioSource audioSource;
	private GameObject smokeEffect;
	private float deathRotation;

    private bool hitByPlayer = false;                   // Whether this ship has been hit by a player

    // Use this for initialization
    protected void Start () {
        // Set up refereces
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        enemyFormation = GetComponentInParent<EnemyFormation>();
        audioSource = GameObject.FindGameObjectWithTag("MainAudioSource").GetComponent<AudioSource>();
		smokeEffect = transform.FindChild ("trail").gameObject;

        // Set speed at start
        rb.velocity = new Vector3(0, 0, -speed);
		deathRotation = Random.Range (deathRotationMin, deathRotationMax);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
	}
	
	// Not being used for now but explicitly declared for child classes in case something gets added here
	protected void Update () {
		if (hitByPlayer) {
			transform.Rotate (0, 0, deathRotation * Time.deltaTime); //rotates 50 degrees per second around z axis

		}
	}

	// Not being used for now but explicitly declared for child classes in case something gets added here
	protected void FixedUpdate(){

	}

    // To be called when hit by an enemy bullet
    public void HitByPlayer()
    {
		smokeEffect.SetActive (true);
        hitByPlayer = true;
        PlayerDeathAudioClip();
        gameController.IncreaseScore(100);
		GetComponent<Rigidbody> ().useGravity = true;

		if (!(GetComponentInParent<Animator>() == null)) 
		{
			Vector3 temp = transform.position;
			GetComponentInParent<Animator> ().enabled = false;
			transform.position = temp;
		}
    }

	// To be called when the enemy hits the player
	void HittingPlayer(){
		Instantiate (deathEffect, transform.position, transform.rotation);
        PlayerDeathAudioClip();
        Destroy(gameObject);
	}

	void OnDestroy(){
        if(enemyFormation)
            enemyFormation.EnemyDied(transform.position, hitByPlayer);
	}

	void OnTriggerEnter(Collider col){
		if (col.transform.tag == "Player") {
			col.gameObject.GetComponent<PlayerHealth> ().TakeDamage ();
			HittingPlayer ();
		}

		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && hitByPlayer) 
		{
			Instantiate (deathEffect, transform.position, transform.rotation);
			PlayerDeathAudioClip();
			Destroy(gameObject);
		}
	}

    private void PlayerDeathAudioClip()
    {
        audioSource.PlayOneShot(deathAudio[Random.Range(0, deathAudio.Length)]);
    }
}
