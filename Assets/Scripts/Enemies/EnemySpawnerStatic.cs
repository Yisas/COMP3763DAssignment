using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnerStatic : MonoBehaviour
{
	// The amount of time between each spawn.
	public float spawnTime;
	// The amount of time before spawning starts.
	public float spawnDelay;
	public bool spawnOnAwake;

	// Array of enemy prefabs.
	public GameObject[] enemies;

    private GameController gameController;

	private float spawnTimer;
	protected int nextEnemyIndex;
    protected bool notAFormation = false;

	protected void Awake ()
	{
        // Setup references
        gameController = GameObject.FindObjectOfType<GameController>();

        // Setup variables
		spawnTimer = spawnTime;
		if (spawnOnAwake)
			Spawn ();
	}

	void Update ()
	{
		// Waiting for delay
		if (spawnDelay > 0)
			spawnDelay -= Time.deltaTime;
		// Else start spawning.
		else {
			// Check for spawnTimer
			if (spawnTimer <= 0 && gameController.numberOfEnemyFormations < gameController.maxNumberOfEnemyFormations) {

				Spawn();

                if(!notAFormation)
				    gameController.numberOfEnemyFormations++;

				// Reset timer and flags
				spawnTimer = spawnTime;
			}
			spawnTimer -= Time.deltaTime;
		}
	}

    public virtual void Spawn()
    {
		// Chose enemy type to spawn
		nextEnemyIndex = Random.Range (0, enemies.Length);

        // Final instantiation
        GameObject enemy = (GameObject)Instantiate(enemies[nextEnemyIndex], transform.position, transform.rotation);
    }
}
