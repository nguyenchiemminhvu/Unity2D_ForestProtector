using UnityEngine;
using System.Collections;

public class VerticalMovement : MonoBehaviour {

    public Boundary<float> boundary;
    public float speed;
    private bool keepMoving;

	// Use this for initialization
	void Start () {
        keepMoving = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!keepMoving)
        {
            StartCoroutine(moving());
        }
    }

    IEnumerator moving()
    {
        keepMoving = !keepMoving;
        while (gameObject.transform.position.y < boundary.max)
        {
            gameObject.transform.Translate(new Vector3(0, Time.deltaTime * speed, 0));
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (gameObject.transform.position.y > boundary.min)
        {
            gameObject.transform.Translate(new Vector3(0, - Time.deltaTime * speed, 0));
            yield return null;
        }

        yield return new WaitForSeconds(1);
        keepMoving = !keepMoving;
    }
}
