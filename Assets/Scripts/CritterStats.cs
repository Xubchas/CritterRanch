using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CLASS DESCRIPTION
//This class keeps the immutable characteristics of critter types
[CreateAssetMenu]
public class CritterStats : ScriptableObject
{

    //CRITICALLY IMPORTANT
    //Critter type name
    public string type;

    //how often this critter moves (value from 0 to 100)
    public int moveFrequency;

    //Make Stats
    public bool makesNibs;
    public bool makesCash;
    public float makesMin;
    public float makesMax;

    //Special make info
    public bool makesSpecial;
    public bool specialHasUnits;
    public string makesSpecialText;

    //Eat Stats
    public bool eatsNibs;
    public bool eatsCash;
    public float eats;

    //Aging stats
    public float maxAge;
    public float minSell;
    public float maxSell;

    //Shop Stats
    public int cost;

    //VERY IMPORTANT
    //Critter Prefab
    public GameObject prefab;

    //Criter Shop Icon
    public Texture idleIcon;
    public Texture hungryIcon;
    public Texture deadIcon;

}