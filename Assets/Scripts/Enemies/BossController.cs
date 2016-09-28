﻿using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {

    public int armHitsToOpenCore;

    public GameObject hitEffect;

    private int armHits = 0;
    private bool coreIsOpen = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // One of the arms is hit by a bullet
    public void ArmHit(GameObject armHit, Vector3 position)
    {
        armHits++;

        armHit.GetComponent<Animator>().SetTrigger("Hit");

        if (armHits >= armHitsToOpenCore)
        {
            armHits = 0;

            Instantiate(hitEffect, position, new Quaternion(0, 0, 0, 0));

            OpenCore();
        }
    }

    void OpenCore()
    {
        //TODO
        if (!coreIsOpen)
        {
            coreIsOpen = true;
        }
    }

    // Invulnerable part of the boss is hit (so basically just instantiate explosion)
    public void BodyHit(Vector3 position)
    {
        Instantiate(hitEffect, position, new Quaternion(0, 0, 0, 0));
    }
}
