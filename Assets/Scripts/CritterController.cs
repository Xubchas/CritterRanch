using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CLASS DESCRIPTION
//This class manages critter movement and tick actions (make, eat)
public class CritterController : MonoBehaviour
{
    //position variables
    public int m_x;
    public int m_y;

    //world position references
    private const float Y_ZERO = 4.5f;
    private const float X_ZERO = -4.5f;

    //direction vectors
    private readonly int[] UP = new int[] {-1,0};
    private readonly int[] DOWN = new int[] {1,0};
    private readonly int[] LEFT = new int[] {0,-1};
    private readonly int[] RIGHT = new int[] {0,1};
    private List<int[]> directions = new List<int[]>();

    //stats for this critter
    public CritterStats m_stats;

    //how many ticks left to wait til moving again
    public int tickCooldown = 0;
    //tick cooldown max
    public const int COOLDOWN = 10;

    //controller
    public RanchController m_controller;

    //move speed
    private const float MOVE_TIME = 1f;

    //holds mutable stats
    public Critter me;

    //copy tick length const
    public const float TICK_LENGTH = RanchController.TICK_LENGTH;

    //my collider
    public Collider2D m_col;

    //my animator
    public Animator anim;

    //hunger stats
    public bool isHungry;
    public bool isDead;
    public const float HUNGER_THRESHOLD = 0.7f;

    void Awake(){
        //initialize directions
        directions.Add(UP);
        directions.Add(DOWN);
        directions.Add(LEFT);
        directions.Add(RIGHT);
        //find controller
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<RanchController>();
    }

    //Calls all tick methods
    public virtual void TickCritter(){
        if(willMove()){
            AttemptMove();
        }
        Eat();
        Make();
        CheckHunger();
        Age();
    }

    //returns whether the critter will move this tick
    public bool willMove(){
        if(isDead || isHungry){
            return false;
        }
        //check whether cooling down
        if(tickCooldown > 0){
            //countdown cooldown
            tickCooldown--;
            return false;
        }
        return Random.Range(0,100) < m_stats.moveFrequency;
    }

    //attempts a move
    void AttemptMove(){
        tickCooldown = COOLDOWN;
        //chooses a random direction to move in
        int[] randomDir = directions[Random.Range(0,directions.Count)];
        //moves the critter if it is allowed by ranch controller
        if(m_controller.Move(m_x,m_y,randomDir)){
            m_x += randomDir[1];
            m_y += randomDir[0];
            StartCoroutine(LerpMove(m_x,m_y));
        }
    }

    //Uses a lerp interposition to move the critter
    IEnumerator LerpMove(int destX, int destY){
        float timeElapsed = 0;
        Vector2 destination = new Vector2(X_ZERO + destX, Y_ZERO - destY);
        while(timeElapsed < MOVE_TIME){
            transform.position = Vector2.Lerp(transform.position, destination, timeElapsed * 1/(MOVE_TIME));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
    }

    //adds nibs and cash accorfing to critter stats
    protected virtual void Make(){
        if(isHungry || isDead){
            return;
        }
        if(m_stats.makesNibs){
            m_controller.nibs += (m_stats.makesMin + ((m_stats.makesMax - m_stats.makesMin) * me.age/m_stats.maxAge))*TICK_LENGTH;
            m_controller.nibsPerSecond += (m_stats.makesMin + ((m_stats.makesMax - m_stats.makesMin) * me.age/m_stats.maxAge));
        }
        else if (m_stats.makesCash){
            m_controller.cashThisTick += (m_stats.makesMin + ((m_stats.makesMax - m_stats.makesMin) * me.age/m_stats.maxAge))*TICK_LENGTH;
            m_controller.cashPerSecond += (m_stats.makesMin + ((m_stats.makesMax - m_stats.makesMin) * me.age/m_stats.maxAge));
        }
    }

    void Eat(){
        if(isDead){
            return;
        }
        if(m_stats.eatsNibs){
            if(m_controller.nibs < m_stats.eats){
                me.hunger--;
            }
            else if (me.hunger < DataManager.MAX_HUNGER){
                me.hunger++;
            }
            m_controller.nibs -= m_stats.eats * TICK_LENGTH;
            m_controller.nibsPerSecond -= m_stats.eats;
        }
        else if (m_stats.eatsCash){
            if(m_controller.cash < m_stats.eats){
                me.hunger--;
            }
            else if (me.hunger < DataManager.MAX_HUNGER){
                me.hunger++;
            }
            m_controller.cash -= m_stats.eats * TICK_LENGTH;
            m_controller.cashPerSecond -= m_stats.eats;
        }
    }

    //Increments Age of critter
    public void Age(int times = 1){
        if(isDead){
            return;
        }
        for(int i = 0; i < times; i++){
            if(me.age < m_stats.maxAge){
                me.age += RanchController.TICK_LENGTH;
            }
            else{
                break;
            }
        }
    }

    //Checks whether critter is hungry
    void CheckHunger(){
        isHungry = me.hunger < HUNGER_THRESHOLD * DataManager.MAX_HUNGER;
        isDead = me.hunger <= 0;
        anim.SetBool("isHungry", isHungry);
        anim.SetBool("isDead", isDead);
    }

    //Allows ranch controller to set a position
    //ONLY CALLED during critter instantiate
    public void SetPos(int x, int y){
        m_x = x;
        m_y = y;
        transform.position = new Vector2(X_ZERO + x, Y_ZERO - y);
    }

    //sets "me" data
    public void SetCritter(Critter critter){
        me = critter;
    }

    void OnMouseDown(){
        m_controller.OpenInfoPanel(this);
    }

    void Update(){
        if(m_controller.paused){
            m_col.enabled = false;
        }else{
            m_col.enabled = true;
        }
    }

}
