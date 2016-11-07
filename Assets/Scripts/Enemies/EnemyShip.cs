using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour {

    public float speed;

	public GameObject deathEffect;
    public AudioClip[] deathAudio;

    private GameController gameController;
    protected Rigidbody rb;
    private EnemyFormation enemyFormation;
    private AudioSource audioSource;

    private bool hitByPlayer = false;                   // Whether this ship has been hit by a player

    // Use this for initialization
    protected void Start () {
        // Set up refereces
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        enemyFormation = GetComponentInParent<EnemyFormation>();
        audioSource = GameObject.FindGameObjectWithTag("MainAudioSource").GetComponent<AudioSource>();

        // Set speed at start
        rb.velocity = new Vector3(0, 0, -speed);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
	}
	
	// Not being used for now but explicitly declared for child classes in case something gets added here
	protected void Update () {
	    
	}

	// Not being used for now but explicitly declared for child classes in case something gets added here
	protected void FixedUpdate(){

	}

    // To be called when hit by an enemy bullet
    public void HitByPlayer()
    {
		Instantiate (deathEffect, transform.position, transform.rotation);
        hitByPlayer = true;
        PlayerDeathAudioClip();
        gameController.IncreaseScore(100);
        Destroy(gameObject);
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
	}

    private void PlayerDeathAudioClip()
    {
        audioSource.PlayOneShot(deathAudio[Random.Range(0, deathAudio.Length)]);
    }
}
