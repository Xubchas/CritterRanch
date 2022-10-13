 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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

    private float _nibs;
    public float nibs{
        get;
        set;
    }

    private float _cash;
    public float cash{
        get;
        set;
    }

    private int _maxSlots;
    public int maxSlots{
        get;
        private set;
    }

    private float _time;
    public float time{
        get;
        set;
    }

    public List<CritterStats> critterMasterList;

    // Start is called before the first frame update
    void Awake()
    {
        //singleton check
        if(instance != null && instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    [Serializable]
    private class SaveGame{
        public string playerName;
        public float nibs;
        public float cash;
        public int maxSlots;
        public float time;
        public List<Critter> critters;

    }

    
}

[Serializable]
public class Critter{
        public string type;
        public string name;
        public int hunger;
        public float age;
}
