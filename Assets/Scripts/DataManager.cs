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


    //ENCAPSULATION: Data manager's fields are all encapsulated
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
    public const float START_CASH = 5000;
    public const float START_NIBS = 0;
    public const int START_SLOTS = 5;
    public const int MAX_HUNGER = 150;

    public const int DEAD_SELL = 10;
    //volume tracking
    public float volume;

    // Start is called before the first frame update
    void Awake()
    {
        save = null;
        volume = 10f/15f;
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

    public bool hasSaveGame(){
        return save != null;
    }

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
