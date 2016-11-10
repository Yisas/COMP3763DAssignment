using UnityEngine;
using System.Collections;

public class EnemyShipShooter : EnemyShip {

	public GameObject bullet;
	public float shotForce;
	public float intervalBetweenShots;

	private Transform shotSpawn;
	private Animator animator;

	private float shotTimer;						// Countdown to when the enemy is allowed to shoot again
	bool step;

	new void Start()
	{
		base.Start ();

		// Setup references
		shotSpawn = transform.FindChild ("shotSpawn");
		animator = GetComponent<Animator> ();

		// Setup variables
		StartCoroutine (Step ());
	}

	new void Update()
	{
		base.Update ();
		shotTimer -= Time.deltaTime;

		if (step)
			StartCoroutine (Step ());
	}


	new void FixedUpdate(){
		base.FixedUpdate ();

		Ray ray = new Ray (shotSpawn.position, -Vector3.forward);
		RaycastHit rayHit;

		LayerMask playerLayerMask = 1 << LayerMask.NameToLayer ("Player");
		playerLayerMask = ~playerLayerMask;

		// Hitting only player layer
		bool  hit = Physics.Raycast(ray, out rayHit, Mathf.Infinity);

		if(hit)
			if (rayHit.collider != null) 
				if (rayHit.collider.tag == "Player")
					Shoot ();
		
	}

	public void Shoot()
	{
		if (shotTimer <= 0 && transform.position.y >= 0) {
				GameObject tempBullet = (GameObject)Instantiate (bullet, shotSpawn.transform.position, bullet.transform.rotation);
			tempBullet.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, -shotForce);

				shotTimer = intervalBetweenShots;
			} 
	}

	private IEnumerator Step()
	{
		Debug.Log ("here");
		step = false;
		yield return new WaitForSeconds(2);
		animator.SetTrigger ("step");
		step = true;
	}
}
