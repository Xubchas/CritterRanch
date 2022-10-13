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
        private set{
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

    private SaveGame save;

    public List<CritterStats> critterMasterList;
    
    private List<Critter> _critters;
    public List<Critter> critters{
        get{
            return _critters;
        }
        set{
            _critters = value;
        }
    }

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

    public void newGame(){
        cash = 50;
        nibs = 0;
        maxSlots = 3;
        time = 0;
        critters = new List<Critter>();
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
