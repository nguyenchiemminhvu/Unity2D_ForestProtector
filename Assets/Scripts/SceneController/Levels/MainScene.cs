using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainScene : MonoBehaviour
{
    public static MainScene instance;

    public Player player;
    public GameObject instructionPanel;
    public Text warningText;
    [HideInInspector] public int[] numberOfKeys;
    public Dictionary<fruits, Transform> fruits;
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        numberOfKeys = new int[10] { 2, 3, 3, 4, 1, 3, 4, 3, 10, 0 };
        instance = this;
    }

    public void moveObjectToPosition(GameObject obj, Vector3 pos)
    {
        obj.transform.position = new Vector3(pos.x, pos.y, obj.transform.position.z);
    }

    public IEnumerator birdCarryPlayer(Bird bird)
    {
        player.interaction = false;
        player.transform.position = new Vector3(
                                                bird.transform.position.x, 
                                                player.transform.position.y,
                                                player.transform.position.z
                                                );
        player.transform.SetParent(bird.transform);
        
        bird.flying();
        while (!bird.isArrived)
        {
            bird.transform.Translate(new Vector3(Time.deltaTime * bird.speed * bird.direction, 0.0f, 0.0f));
            yield return null;
        }

        bird.reverseDirection();
        bird.flip();
        bird.standing();
        bird.isArrived = false;

        player.transform.SetParent(null);
        player.interaction = true;
    }

    public IEnumerator turtleCarryPlayer(Turtle turtle)
    {
        player.interaction = false;
        player.transform.position = new Vector3(
                                                turtle.transform.position.x, 
                                                player.transform.position.y,
                                                player.transform.position.z
                                                );
        player.transform.SetParent(turtle.transform);

        turtle.wakeUp();
        while (turtle.isWakingUp())
        {
            yield return null;
        }

        while (!turtle.isArrived)
        {
            turtle.transform.Translate(new Vector3(Time.deltaTime * turtle.speed * turtle.direction, 0.0f, 0.0f));
            yield return new WaitForEndOfFrame();
        }
        turtle.reverseDirection();
        turtle.flip();
        turtle.sleep();
        turtle.isArrived = false;

        player.transform.SetParent(null);
        player.interaction = true;
    }

    public IEnumerator spiderMoving(Spider spider)
    {
        while (!spider.isArrived)
        {
            spider.transform.Translate(new Vector3(Time.deltaTime * spider.speed * spider.direction, 0.0f, 0.0f));
            yield return new WaitForEndOfFrame();
        }

        spider.reverseDirection();
        spider.isArrived = false;
    }

    public int GetCurrentLevelKeys(int level)
    {
        return numberOfKeys[level - 1];
    }

    public void stopAllAnimation()
    {
        Time.timeScale = 0.0f;
    }

    public void resumeAllAnimation()
    {
        Time.timeScale = 1.0f;
    }

    public void giveInstruction(string text)
    {
        if (instructionPanel)
        {
            stopAllAnimation();
            instructionPanel.GetComponentInChildren<Text>().text = " " + text;
            instructionPanel.SetActive(true);
        }
    }

    public void showWarning()
    {
        StartCoroutine(warning());
    }

    IEnumerator warning()
    {
        warningText.enabled = true;
        yield return new WaitForSeconds(0.5f);

        warningText.enabled = false;
        yield return new WaitForSeconds(0.5f);

        warningText.enabled = true;
        yield return new WaitForSeconds(0.5f);

        warningText.enabled = false;
        yield return new WaitForSeconds(0.5f);

        warningText.enabled = true;
        yield return new WaitForSeconds(0.5f);

        warningText.enabled = false;
        yield return new WaitForSeconds(0.5f);
    }
}
