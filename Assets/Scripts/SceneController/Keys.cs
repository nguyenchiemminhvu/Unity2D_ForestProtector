using UnityEngine;
using System.Collections;

public class Keys : MonoBehaviour {

	void Start () 
    {
	    
	}
	
	void Update () 
    {
	
	}

    public void OnGetKey()
    {
        Score.addScore(50);
        Destroy(gameObject);
    }
    
}
