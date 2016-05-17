using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{

    bool isPress = false;
    GameObject player;
    Rigidbody2D playerRigid;
  // Use this for initialization
    void Start()

    { 
        player = GameObject.Find("Player");
        playerRigid = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPress)
        {
            switch (gameObject.name)
            {
                case "btn-right":
                    player.transform.position = new Vector3(player.transform.position.x + 5 * Time.deltaTime, player.transform.position.y, player.transform.position.z);
                    break;
                case "btn-left":
                    player.transform.position = new Vector3(player.transform.position.x - 5 * Time.deltaTime, player.transform.position.y, player.transform.position.z);
                    break;
            }
        }
    }
    public void OnMouseDown()
    {
        isPress = true;
        if (gameObject.name == "btn-jump") playerRigid.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
    }
    public void OnMouseUp()
    {
        isPress = false;
    }
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("btn-down");
    //    if (gameObject.name == "btn-jump") playerRigid.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
    //    isPress = true;
    //}
    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    Debug.Log("btn-up");
    //    isPress = false;
    //}
}
