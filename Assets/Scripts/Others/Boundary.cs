using UnityEngine;
using System.Collections;

public class Boundary <Type> {

    public Type min;
    public Type max;

    public Boundary(Type min, Type max) {
        this.min = min;
        this.max = max;
    }
}
