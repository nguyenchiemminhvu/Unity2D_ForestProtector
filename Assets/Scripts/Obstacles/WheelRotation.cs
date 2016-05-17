using UnityEngine;
using System.Collections;

public class WheelRotation : MonoBehaviour {

    [Range(-1, 1)]
    public int direction;

    [Range(1.0f, 2.0f)]
    public float rotationSpeed;

	void Start () {
        if (direction == 0)
            direction = 1;
	}
	
	void Update () {
        gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), direction * rotationSpeed);
	}
}
