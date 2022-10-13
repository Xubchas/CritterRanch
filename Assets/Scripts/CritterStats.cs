using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CritterStats : ScriptableObject
{

    //Name
    public string type;

    //how often this critter moves (value from 0 to 100)
    public int moveFrequency;

    //Nibs Stats
    public float nibsMakeMin;
    public float nibsMakeMax;
    public float nibsEat;

    //Cash Stats
    public float cashMakeMin;
    public float cashMakeMax;
    public float cashEat;

    //Aging stats
    public float maxAge;
    public float minSell;
    public float maxSell;

    //Shop Stats
    public int cost;
}