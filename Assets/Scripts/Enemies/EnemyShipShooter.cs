using UnityEngine;
using System.Collections;

public class EnemyShipShooter : EnemyShip {

	public GameObject bullet;
	public float shotForce;
	public float intervalBetweenShots;

	private Transform shotSpawn;

	private float shotTimer;						// Countdown to when the enemy is allowed to shoot again

	new void Start()
	{
		base.Start ();

		// Setup references
		shotSpawn = transform.FindChild ("shotSpawn");
	}

	new void Update()
	{
		base.Update ();
		shotTimer -= Time.deltaTime;
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

	private void Shoot()
	{
		Debug.Log ("here");
		if (shotTimer <= 0 && transform.position.y >= 0) {
			Debug.Log ("here2");
				GameObject tempBullet = (GameObject)Instantiate (bullet, shotSpawn.transform.position, bullet.transform.rotation);
			tempBullet.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, -shotForce);

				shotTimer = intervalBetweenShots;
			} 
	}
}
