using UnityEngine;
using System.Collections;

public class BossController3D : MonoBehaviour {

	public GameObject hitEffect;
	public GameObject[] weakSpots;
	public GameObject beamShot;

	public SpriteRenderer healthBar; 

	public int weakSpotHitsBeforeDeath;
	public int bodyHitsBeforeExposingWeakSpots;
	public float exposedWeakSpotTimer;

	private int initWeakSpotHitsBeforeDeath;
	private int initBodyHitsBeforeExposingWeakSpots;
	private bool weakSpotHit;

    // Use this for initialization
    void Start () {
       // Setup variables
		initWeakSpotHitsBeforeDeath = weakSpotHitsBeforeDeath;
		initBodyHitsBeforeExposingWeakSpots = bodyHitsBeforeExposingWeakSpots;
	}
	
	// Update is called once per frame
	void Update () {

  
    }

	// Invulnerable part of the boss is hit (so basically just instantiate explosion)
	public void BodyHit(Vector3 position)
	{
		Instantiate(hitEffect, position, new Quaternion(0, 0, 0, 0));

		bodyHitsBeforeExposingWeakSpots--;
		if (bodyHitsBeforeExposingWeakSpots <= 0) 
		{
			bodyHitsBeforeExposingWeakSpots = initBodyHitsBeforeExposingWeakSpots;
			StartCoroutine(ExposeWeakSpots ());
		}
	}

	private IEnumerator ExposeWeakSpots()
	{

		weakSpotHit = false;

		foreach (GameObject go in weakSpots) 
		{
			go.SetActive (true);
		}

		yield return new WaitForSeconds(exposedWeakSpotTimer);

		if (!weakSpotHit) 
		{
			BossShoot ();
		}

		foreach (GameObject go in weakSpots) 
		{
			go.SetActive (false);
		}
	}

	public void WeakSpotHit()
	{
		weakSpotHitsBeforeDeath--;

		float newScaleMultiplier = (float)weakSpotHitsBeforeDeath / (float)initWeakSpotHitsBeforeDeath;

		healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x * newScaleMultiplier, healthBar.transform.localScale.y);

		weakSpotHit = true;

		if (weakSpotHitsBeforeDeath <= 0)
			Die();
	}

	private void BossShoot()
	{
		foreach (GameObject go in weakSpots) 
		{
			Instantiate(beamShot,go.transform.position, beamShot.transform.rotation);
		}
	}

	private void Die()
	{
		// Turn off enemies, harmful objects and enemy spawners
		EnemySpawnerHoming[] spawners = transform.GetComponentsInChildren<EnemySpawnerHoming>();
		foreach (EnemySpawnerHoming esp in spawners)
			esp.enabled = false;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject go in enemies)
			go.GetComponent<EnemyShipHoming>().HitByPlayer();

		AuxFunctions.DestroyGameObjectsWithTag("BossBeam");
		AuxFunctions.DestroyGameObjectsWithTag("BossWeakSpot");

		StartCoroutine(AuxFunctions.ShakeCamera(1, 3));

		// You win animation
		GameObject.FindGameObjectWithTag("GameController").GetComponent<Animator>().SetTrigger("win");

		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WaitAndReload(4);

		// Stop this script
		this.enabled = false;
	}
}
