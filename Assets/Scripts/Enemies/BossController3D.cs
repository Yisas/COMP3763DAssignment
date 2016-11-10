using UnityEngine;
using System.Collections;

public class BossController3D : MonoBehaviour {

    public float speed;

    public int armHitsToOpenCore;
    public int weakSpotHitsBeforeDeath;

    public GameObject hitEffect;
    public GameObject beam;
    public GameObject deathEffect;

    public AudioClip deathAudio;
	public AudioClip shotAudio;

    public SpriteRenderer healthBar; 

    private Animator animator;
    private AudioSource audioSource;
    private Transform beamSpawn;
    private GameObject beamCharge;
    private GameObject player;
    private Rigidbody2D rb;

    private int directionSign = 0;                      // Direction sign of relative position to the player. Positive 1 is to the right
    private int armHits = 0;
    private bool coreIsOpen = false;
    private bool weakspotHit = false;                   // Flag for when the weakspot is hit before the boss shoots from the core
    private int initWeakSpotHitsBeforeDeath;

    // Use this for initialization
    void Start () {
        // Set up references
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        beamCharge = transform.FindChild("Ray Charge").gameObject;
        beamSpawn = transform.FindChild("beamSpawn");
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Setup variables
        initWeakSpotHitsBeforeDeath = weakSpotHitsBeforeDeath;
	}
	
	// Update is called once per frame
	void Update () {

        directionSign = FindPlayer();

        // Set the enemy's velocity to moveSpeed in the x direction.
        rb.velocity = new Vector2(speed * directionSign, rb.velocity.y);
    }

    // Uses horizontal position diference to return the sign of the offset to the player.
    protected int FindPlayer()
    {
        int direction = 0;

        Vector3 tempFloat = transform.position - player.transform.position;

        if (tempFloat.x <= 0)
            direction = 1;
        else
            direction = -1;

        return direction;
    }

    // One of the arms is hit by a bullet
    public void ArmHit(GameObject armHit, Vector3 position)
    {
        // While core is open nothing happens when hitting the arms...
        if (!coreIsOpen)
        {
			Debug.Log ("Arm hit while core is closed: " + Time.time);

            //... else count to see if we should open core and expose weak spot
            armHits++;

            animator.SetTrigger("armHit");

            if (armHits >= armHitsToOpenCore)
            {
                armHits = 0;

                Instantiate(hitEffect, position, new Quaternion(0, 0, 0, 0));

                OpenCore();
            }
        }
    }

    void OpenCore()
    {
		coreIsOpen = true;
		Debug.Log ("Core open: " + Time.time);

        animator.SetTrigger("open");
		Debug.Log ("Open animation triggered: " + Time.deltaTime);
    }

    void CloseCore()
    {
		coreIsOpen = false;
		Debug.Log ("Core closed: " + Time.time);

        // Disable charge effect collider
        beamCharge.GetComponent<CircleCollider2D>().enabled = false;

        animator.SetTrigger("close");
		Debug.Log ("Close animation triggered: " + Time.deltaTime);

        // Reset flags
        weakspotHit = false;
    }

    public void ChargeShotFromCore()
    {
		if (!weakspotHit) 
		{
			Debug.Log ("Charging shot: " + Time.time);
			beamCharge.GetComponent<Animator> ().SetTrigger ("charge");
			beamCharge.GetComponent<CircleCollider2D> ().enabled = true;
		}
    }

    public void ShootFromCore()
    {
		Debug.Log ("Shooting: " + Time.time);
		if (!weakspotHit) {
			// Shoot ...
			Instantiate (beam, beamSpawn.position, beamSpawn.rotation);
			audioSource.PlayOneShot (shotAudio);
		}

        CloseCore();
    }

    // Invulnerable part of the boss is hit (so basically just instantiate explosion)
    public void BodyHit(Vector3 position)
    {
        Instantiate(hitEffect, position, new Quaternion(0, 0, 0, 0));
    }

    // Bullet hit the weak spot while it was active
    public void WeakSpotHit()
    {
        weakSpotHitsBeforeDeath--;

        float newScaleMultiplier = (float)weakSpotHitsBeforeDeath / (float)initWeakSpotHitsBeforeDeath;
        
        healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x * newScaleMultiplier, healthBar.transform.localScale.y);

        if (weakSpotHitsBeforeDeath <= 0)
            Die();
        else
        // Play hit animation feedback
        {
            animator.SetTrigger("weakspotHit");
            // Stop charging animation
            beamCharge.GetComponent<Animator>().SetTrigger("interruptCharge");

			Debug.Log ("Weakspot hit: " + Time.time);
            // Set flag so the boss won't shoot
            weakspotHit = true;

            CloseCore();
        }
    }

    void Die()
    {
        // Stop moving
        rb.velocity = new Vector2(0, 0);

        // Turn off enemies, harmful objects and enemy spawners
        EnemySpawnerHoming[] spawners = transform.GetComponentsInChildren<EnemySpawnerHoming>();
        foreach (EnemySpawnerHoming esp in spawners)
            esp.enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in enemies)
            go.GetComponent<EnemyShipHoming>().HitByPlayer();

        AuxFunctions.DestroyGameObjectsWithTag("BossBeam");
        AuxFunctions.DestroyGameObjectsWithTag("BossWeakSpot");

        // Animate death
        animator.SetTrigger("close");
        audioSource.PlayOneShot(deathAudio);
        Instantiate(deathEffect, transform.position, new Quaternion());
        StartCoroutine(AuxFunctions.ShakeCamera(1, 3));

        // You win animation
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Animator>().SetTrigger("win");

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WaitAndReload(4);

        // Stop this script
        this.enabled = false;
    }
}
