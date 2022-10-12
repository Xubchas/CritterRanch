using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CritterStats : ScriptableObject
{

    //how often this critter moves (value from 0 to 100)
    public string type;
    public int moveFrequency;
    public float nibsMakeMin;
    public float nibsMakeMax;
    public float nibsEat;
    public float cashMakeMin;
    public float cashMakeMax;
    public float cashEat;
    public float maxAge;
    public float minSell;
    public float maxSell;
}