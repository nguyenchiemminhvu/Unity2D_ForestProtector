using UnityEngine;
using System.Collections;

public class MapItems {

    public int numberOfBirdFood;
    public int numberOfTurtleFood;
    public int keys;
    public MapItems(int keys)
    {
        this.keys = keys;
        numberOfBirdFood = 0;
        numberOfTurtleFood = 0;
    }
}
