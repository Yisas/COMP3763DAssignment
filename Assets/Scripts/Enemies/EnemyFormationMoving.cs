using UnityEngine;
using System.Collections;

public class EnemyFormationMoving : EnemyFormation {

    public float speed;

	// Use this for initialization
	new void Start () {
        base.Start();

		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -speed);   
	}
}
