﻿using UnityEngine;
using System.Collections;

public class EnemyFormation : MonoBehaviour {

	public GameObject powerupDrop;
   
    private ArrayList enemies = new ArrayList();
	private GameController gameController;

    private int initialNumOfEnemies;
    private int numOfEnemies;
    private int enemiesKilledByPlayer = 0;                  // How many ships where destroyed by the player.
    private bool powerupDropped = false;

    // Use this for initialization
    protected void Start () {
        // Setup references
        // Get all enemies in this formation
        foreach (GameObject enemy in AuxFunctions.FindChildrenWithTag(transform.FindChild("Ships").transform, "Enemy"))
            enemies.Add(enemy);

		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();


        // Setup variables
        numOfEnemies = enemies.Count;
        initialNumOfEnemies = enemies.Count;
    }

    // To be called by the enemy when it has died
	public void EnemyDied(Vector3 enemyPosition, bool hitByPlayer)
    {
        numOfEnemies--;

        if (hitByPlayer)
            enemiesKilledByPlayer++;

        // If all players are dead, destroy formation object
		if (numOfEnemies <= 0) {
            // If all enemies where killed by player, drop powerup
			if (enemiesKilledByPlayer == initialNumOfEnemies) 
			{
				DropPowerUp (enemyPosition);
				gameController.BonusScore(2);
			}

			Destroy (gameObject);
		}
    }
	
	// Update is called once per frame
	void Update () {
    }

	void DropPowerUp(Vector3 enemyPosition)
    {
        if (!powerupDropped)
        {
			enemyPosition = new Vector3 (enemyPosition.x, enemyPosition.y, enemyPosition.z + 20);
            powerupDropped = true;
			Instantiate (powerupDrop, enemyPosition, powerupDrop.transform.rotation);
        }
    }

    void OnDestroy()
    {
		if (GameObject.FindObjectOfType<GameController> ())
			GameObject.FindObjectOfType<GameController> ().numberOfEnemyFormations--;
		else
			Debug.Log ("Enemy formation can't find game controller");
    }
}
