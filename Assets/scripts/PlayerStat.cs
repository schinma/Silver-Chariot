using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public int rescued = 1;
    public float food = 40f;
    public int distance = 0;
    public float foodStart = 40f;

    public void initialize()
    {
        rescued = 1;
        food = foodStart;
        distance = 0;
    }
}
