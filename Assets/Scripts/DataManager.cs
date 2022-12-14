 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//CLASS DESCRIPTION
//Data Manager manages all the game data(lol)
//it is primarily in charge of keeping the save game and the curretn list of critter types
//Data manager is a singleton and persists across scenes
public class DataManager : MonoBehaviour
{
    //singleton pattern
    private static DataManager _instance;
    public static DataManager instance{
        get
        {
            return _instance;
        }
    }


    //ENCAPSULATION
    //Key game metrics fields are all encapsulated
    private float _nibs;
    public float nibs{
        get{
            return _nibs;
        }
        set{
            _nibs = value;
        }
    }

    private float _cash;
    public float cash{
        get{
            return _cash;
        }
        set{
            _cash = value;
        }
    }

    private int _maxSlots;
    public int maxSlots{
        get{
            return _maxSlots;
        }
        set{
            _maxSlots = value;
        }
    }

    private float _time;
    public float time{
        get{
            return _time;
        }
        set{
            _time = value;
        }
    }

    public string playerName;

    //keeps the last saved/loaded game
    private SaveGame save;

    //Has all the critter types
    public List<CritterStats> critterMasterList;
    
    //holds the actual critters in this save
    private List<Critter> _critters;
    public List<Critter> critters{
        get{
            return _critters;
        }
        set{
            _critters = value;
        }
    }

    //Default constants
    public const float START_CASH = 50;
    public const float START_NIBS = 0;
    public const int START_SLOTS = 5;

    //how many ticks a critter can go without food
    public const int MAX_HUNGER = 150;
    //how much a dead critter sells for
    public const int DEAD_SELL = 10;

    public const float MAX_VOLUME = 15f;
    public const float START_VOLUME = 6f;

    //volume tracking across scenes
    public float volume;

    // Start is called before the first frame update
    void Awake()
    {
        save = null;
        volume = START_VOLUME/MAX_VOLUME;
        //singleton check
        if(instance != null && instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    //initializes data with new game
    public void newGame(string name){
        playerName = name;
        cash = START_CASH;
        nibs = START_NIBS;
        maxSlots = START_SLOTS;
        time = 0;
        critters = new List<Critter>();
    }

    //searches the master list for a type
    public CritterStats getStat(string typeSearch){
        foreach(CritterStats stat in critterMasterList){
            if (stat.type == typeSearch){
                return stat;
            }
        }

        return null;
    }

    //checks whether there is data
    public bool hasSaveGame(){
        return save != null;
    }

    //creates a new save game object and stores it in this class
    public void SaveSaveGame(){
        save = new SaveGame(playerName, nibs, cash, maxSlots, time, critters);
    }


    //used the game's save/load data structure
    [Serializable]
    private class SaveGame{
        public string playerName;
        public float nibs;
        public float cash;
        public int maxSlots;
        public float time;
        public List<Critter> critters;

        //constructor
        public SaveGame(string playerName, float nibs, float cash, int maxSlots, float time, List<Critter> critters){
            this.playerName = playerName;
            this.nibs = nibs;
            this.cash = cash;
            this.maxSlots = maxSlots;
            this.time = time;
            this.critters = critters;
        }

    }

    
}

//CLASS DESCRIPTION
//Keeps the mutable traits of a critter
[Serializable]
public class Critter{
        public string type;
        public string name;
        public int hunger;
        public float age;

        //Constructor always sets hunger and age to default
        public Critter(string type, string name){
            this.type = type;
            this.name = name;
            hunger = DataManager.MAX_HUNGER;
            age = 0;
        }
}
