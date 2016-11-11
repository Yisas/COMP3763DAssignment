using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    [HideInInspector]
    public static bool hitsPlayer = false;                  // Set to true during bullethell mode, allowing the bullet to hit its own player.
	public int respawnsUntilDestroy;

	private int timesRespawned = 0;

	void OnTriggerEnter(Collider col)
    {
		if(col.transform.tag == "EnemyBulletTarget")
        {
            col.gameObject.GetComponentInParent<EnemyShip>().HitByPlayer();
            Destroy(gameObject);
        }

        if(col.transform.tag == "BossArm")
        {
            col.transform.parent.transform.parent.GetComponent<BossController>().ArmHit(col.gameObject, transform.FindChild("bulletTop").transform.position);
            Destroy(gameObject);
        }

        if (col.transform.tag == "BossInvulnerable")
        {
			Vector3 temp = new Vector3 (transform.position.x, transform.position.y, transform.position.z-2);
			col.GetComponent<BossController3D>().BodyHit(temp);
            Destroy(gameObject);
        }

        if(col.transform.tag == "BossWeakSpot")
        {
			col.transform.parent.parent.GetComponent<BossController3D>().WeakSpotHit();
            Destroy(gameObject);
        }

        if(col.transform.tag == "Player" && hitsPlayer)
        {
            col.transform.GetComponent<PlayerHealth>().TakeDamage();
            Destroy(gameObject);
        }
    }

	// To be called be respawner when it happens
	public void Respawned()
	{
		timesRespawned++;

		if (timesRespawned > respawnsUntilDestroy)
			Destroy (gameObject);
	}
}
