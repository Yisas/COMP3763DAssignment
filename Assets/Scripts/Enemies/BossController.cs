﻿using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {

    public float speed;

    public int armHitsToOpenCore;
    public int weakSpotHitsBeforeDeath;

    public GameObject hitEffect;
    public GameObject beam;
    public GameObject deathEffect;

    private Animator animator;
    private Transform beamSpawn;
    private GameObject beamCharge;
    private GameObject player;
    private Rigidbody2D rb;

    private int directionSign = 0;                      // Direction sign of relative position to the player. Positive 1 is to the right
    private int armHits = 0;
    private bool coreIsOpen = false;
    private bool weakspotHit = false;                   // Flag for when the weakspot is hit before the boss shoots from the core

	// Use this for initialization
	void Start () {
        // Set up references
        animator = GetComponent<Animator>();
        beamCharge = transform.FindChild("Ray Charge").gameObject;
        beamSpawn = transform.FindChild("beamSpawn");
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
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
            animator.SetTrigger("open");

            coreIsOpen = true;
    }

    void CloseCore()
    {
        // Disable charge effect collider
        beamCharge.GetComponent<CircleCollider2D>().enabled = false;

        animator.SetTrigger("close");

        // Reset flags
        weakspotHit = false;
       coreIsOpen = false;
    }

    public void ChargeShotFromCore()
    {
            beamCharge.GetComponent<Animator>().SetTrigger("charge");
            beamCharge.GetComponent<CircleCollider2D>().enabled = true;
    }

    public void ShootFromCore()
    {
        if (!weakspotHit)
            // Shoot ...
            Instantiate(beam, beamSpawn.position, beamSpawn.rotation);

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

        if (weakSpotHitsBeforeDeath <= 0)
            Die();
        else
        // Play hit animation feedback
        {
            animator.SetTrigger("weakspotHit");
            // Stop charging animation
            beamCharge.GetComponent<Animator>().SetTrigger("interruptCharge");

            // Set flag so the boss won't shoot
            weakspotHit = true;

            CloseCore();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform);
        StartCoroutine(AuxFunctions.ShakeCamera(1, 3));
    }
}
