using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int powerups;
    public int maxNumberOfPickups;
    public GameObject deathEffect;

    public AudioClip hitAudio;                  // Audio that plays when the player is hit, not killed
    public AudioClip deathAudio;                // Audio that plays when the player is killed

    private PlayerController playerController;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        // Setup references
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Damage is an optional parameter, averageDamageAmount = 10 by default
    public void TakeDamage(int damage = 1)
    {
        powerups -= damage;

        powerups = Mathf.Clamp(powerups, -1, maxNumberOfPickups);

        playerController.numberOfShots--;
        playerController.numberOfShots = Mathf.Clamp(playerController.numberOfShots, 1, maxNumberOfPickups + 1);

        if (powerups < 0)
            Die();
        else
        {
            audioSource.PlayOneShot(hitAudio);
            StartCoroutine(AuxFunctions.ShakeCamera(0.5f, 0.5f));
        }
    }

    void PickupPowerup()
    {
        powerups += 1;

        powerups = Mathf.Clamp(powerups, 0, maxNumberOfPickups);

        playerController.numberOfShots++;
        playerController.numberOfShots = Mathf.Clamp(playerController.numberOfShots, 1, maxNumberOfPickups + 1);
    }

    void OntriggerEnter(Collider col)
    {
        if (col.transform.tag == "Powerup")
        {
            PickupPowerup();
            Destroy(col.gameObject);
        }
    }

    public void Die()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Collider>().enabled = false;

        Rigidbody[] rbs = GameObject.FindObjectsOfType<Rigidbody>();
        EnemySpawner[] espwnrs = GameObject.FindObjectsOfType<EnemySpawner>();
        /*
        foreach (Rigidbody2D rb in rbs)
        {
            // Bullets run their course, everything else stops
            if(rb.transform.tag!="Bullet")
            rb.velocity = new Vector2(0, 0);

        }
        */

        foreach (EnemySpawner es in espwnrs)
            es.enabled = false;

        // Deactivate meshes and instantiate death particle effect
        transform.FindChild("Meshes").gameObject.SetActive(false);
        Instantiate(deathEffect, transform.position, transform.rotation);

        audioSource.volume = 1.0f;
        audioSource.PlayOneShot(deathAudio);
        StartCoroutine(AuxFunctions.ShakeCamera(1, 3));

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WaitAndReload(4);
    }
}
