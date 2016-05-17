using UnityEngine;
using System.Collections.Generic;

public class AlternativeHidden : MonoBehaviour {

    public const float VISIBLE_DURATION = 1.5f;
    
    public List<GameObject> chainGround;
    private int currentIndex;

    private AudioSource audioSource;

	void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
        disableAll();
        currentIndex = 0;

        InvokeRepeating("activated", VISIBLE_DURATION, VISIBLE_DURATION);
	}

    public void activated()
    {
        int preIndex = currentIndex;
        preIndex = previousIndex(preIndex);
        preIndex = previousIndex(preIndex);

        enableIndex(currentIndex);
        disableIndex(preIndex);

        currentIndex = nextIndex(currentIndex);
    }

    private int nextIndex(int index)
    {
        index++;
        if (index >= chainGround.Count)
            index = 0;

        return index;
    }

    private int previousIndex(int index)
    {
        index--;
        if (index < 0)
            index = chainGround.Count - 1;

        return index;
    }

    public void disableAll()
    {
        for (int i = 0; i < chainGround.Count; i++)
        {
            chainGround[i].SetActive(false);
        }
    }

    public void enableAll()
    {
        for (int i = 0; i < chainGround.Count; i++)
        {
            chainGround[i].SetActive(true);
        }
    }

    public void disableIndex(int index)
    {
        if (index < 0 || index >= chainGround.Count)
            return;

        chainGround[index].SetActive(false);
    }

    public void enableIndex(int index)
    {
        if (index < 0 || index >= chainGround.Count)
            return;

        chainGround[index].SetActive(true);
        playSoundEachTurn();
    }

    public void playSoundEachTurn()
    {
        if (audioSource.clip != null)
            audioSource.PlayOneShot(audioSource.clip);
    }
}
