using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {

        if (GameObject.FindObjectOfType<PlayerHealth>().powerups >= GameObject.FindObjectOfType<PlayerHealth>().maxNumberOfPickups)
            Destroy(gameObject);

        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -speed);
	}
}
