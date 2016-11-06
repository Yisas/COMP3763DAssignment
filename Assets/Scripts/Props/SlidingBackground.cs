using UnityEngine;
using System.Collections;

public class SlidingBackground : MonoBehaviour {

    public float speed;
    public GameObject planeObject;
    public float translationTreshold;

    private GameObject currentPlane;
    private float planeExtentDistance;

	// Use this for initialization
	void Start () {
        // Instantiate first plane
        currentPlane = (GameObject)Instantiate(planeObject, transform);
        currentPlane.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1) * speed;
        
        Bounds tempBounds = currentPlane.GetComponent<SpriteRenderer>().bounds;
        planeExtentDistance = Mathf.Abs(tempBounds.max.z - tempBounds.center.z);
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(currentPlane.GetComponent<SpriteRenderer>().bounds.max);
        
        if (currentPlane.transform.position.z <= translationTreshold)
            InstantiatePlaneOject();
    }

    private void InstantiatePlaneOject()
    {
        Bounds bounds = currentPlane.GetComponent<SpriteRenderer>().bounds;
        Vector3 newPosition = new Vector3(
            currentPlane.transform.position.x,
            currentPlane.transform.position.y,
            bounds.center.z + (2 * planeExtentDistance)
            );
        currentPlane = (GameObject) Instantiate(planeObject, newPosition, currentPlane.transform.rotation, transform);
        currentPlane.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1) * speed;

    }
}
