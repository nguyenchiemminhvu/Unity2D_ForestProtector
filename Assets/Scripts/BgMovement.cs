using UnityEngine;
using System.Collections;

public class BgMovement : MonoBehaviour
{
	// create smooth movement background via pos of camera
	GameObject cam;
	GameObject sceneSize;
	float camStartPosX, camStartPosY;
	float deltaX, deltaY;
	float bgStartPosX, bgStartPosY;
	float ratioX, ratioY;
	BoxCollider2D sceneCol;
	// Use this for initialization
	void Start ()
	{
		sceneSize = GameObject.Find ("SceneSize");
		sceneCol = sceneSize.GetComponent<BoxCollider2D> ();
		cam = GameObject.Find ("Main Camera");

		// camera start position
		camStartPosX = cam.transform.position.x;
		camStartPosY = cam.transform.position.y;
		// background start position
		bgStartPosX = transform.position.x;
		bgStartPosY = transform.position.y;
		// calculate ratio (bg - cam / scene size)
		ratioX = (2 * (transform.position.x - cam.transform.position.x)) / sceneCol.size.x;
		ratioY = (2 * (transform.position.y - cam.transform.position.y)) / sceneCol.size.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		deltaX = ratioX * (cam.transform.position.x - camStartPosX);
		deltaY = ratioY * (cam.transform.position.y - camStartPosY);
	}

	void LateUpdate ()
	{
		transform.position = new Vector3 (
			bgStartPosX + (cam.transform.position.x - camStartPosX) - deltaX,
			bgStartPosY + (cam.transform.position.y - camStartPosY) - deltaY,
			transform.position.z);
	}
}
