using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	public float spawnTime;
	// The amount of time between each spawn.
	public float spawnDelay;
	// The amount of time before spawning starts.
	public GameObject[] enemies;
	// Array of enemy prefabs.

	public bool sound;
	public AudioClip spawnSound;
	private AudioSource audioSource;

	private Transform leftmostSpawnEdge;                    	// Smallest value of x in world coordinates the delivery can happen at.
	private Transform rightmostSpawnEdge;                   	// Largest value of x in world coordinates the delivery can happen at.
    private GameController gameController;

	private float spawnTimer;
	private Vector3 nextSpawnPosition;
	private int nextEnemyIndex;

	void Awake ()
	{
        // Setup references
		leftmostSpawnEdge = transform.FindChild ("leftmostSpawnEdge");
		rightmostSpawnEdge = transform.FindChild ("rightmostSpawnEdge");
        gameController = GameObject.FindObjectOfType<GameController>();

        // Setup variables
		spawnTimer = spawnTime;

		if (sound)
			audioSource = GetComponent<AudioSource> ();
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
				// Chose enemy type to spawn
				nextEnemyIndex = Random.Range (0, enemies.Length);

				nextSpawnPosition = FindSpawnTarget ();

				// Final instantiation
				GameObject enemy = (GameObject)Instantiate (enemies [nextEnemyIndex], nextSpawnPosition, transform.rotation);

				if (sound) {
					
					audioSource.PlayOneShot (spawnSound);
				}

				gameController.numberOfEnemyFormations++;

				// Reset timer and flags
				spawnTimer = spawnTime;
			}
			spawnTimer -= Time.deltaTime;
		}
	}

	public int UniqueRandomInt (int min, int max, List<int> usedValues)
	{
		int i = 0;
		int val = Random.Range (min, max);
		while (usedValues.Contains (val) && i <= 1000) {
			val = Random.Range (min, max);
			i++;
		}
		return val;
	}

	private Vector3 FindSpawnTarget ()
	{
		RaycastHit2D hit = new RaycastHit2D ();
		Vector3 dropPos;
		List<int> usedValues = new List<int> ();

		GameObject[] enemies = AuxFunctions.FindGameObjectsWithLayer (LayerMask.NameToLayer ("EnemyFormation"));

		foreach (GameObject go in enemies) {
			for (int p = (int)go.transform.position.x - 2; p < (int)go.transform.position.x + 2; p++)
				usedValues.Add (p);
		}


		int i = 0;

		do {

			// Create a random x coordinate for the delivery in the drop range.
			int dropPosX = UniqueRandomInt ((int)leftmostSpawnEdge.position.x, (int)rightmostSpawnEdge.position.x, usedValues);
			//float dropPosX = Random.Range(dropRangeLeft.position.x,dropRangeRight.position.x);
			usedValues.Add (dropPosX);

			// Create a position with the random x coordinate.
			dropPos = new Vector3 (dropPosX, leftmostSpawnEdge.position.y, transform.position.z);

			hit = new RaycastHit2D ();
			// Raycasting Enemies layer, to try to spawn where an enemy is not
			hit = Physics2D.Raycast (dropPos, -Vector2.up, 10f, LayerMask.NameToLayer ("Enemies"));

			i++;

			if (i >= 100) {
				i = 0;
				//Debug.Log("Spawner error");
				hit = new RaycastHit2D ();
			}


		} while (hit.collider != null);

		return dropPos;
	}
}
