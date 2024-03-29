﻿using UnityEngine;
using System.Collections;

public class DestroyWhenBehindPlayer : MonoBehaviour {

	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.z < player.position.z) 
		{
			Destroy (gameObject);
		}
	}
}
