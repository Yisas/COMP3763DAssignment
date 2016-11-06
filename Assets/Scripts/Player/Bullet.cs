using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    [HideInInspector]
    public static bool hitsPlayer = false;                  // Set to true during bullethell mode, allowing the bullet to hit its own player.
	public int respawnsUntilDestroy;

	private int timesRespawned = 0;

	void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Enemy")
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
            col.transform.parent.transform.parent.GetComponent<BossController>().BodyHit(transform.FindChild("bulletTop").transform.position);
            Destroy(gameObject);
        }

        if(col.transform.tag == "BossWeakSpot")
        {
            col.transform.parent.GetComponent<BossController>().WeakSpotHit();
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
