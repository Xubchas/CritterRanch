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
    public void TickCritter(){
        if(willMove()){
            AttemptMove();
        }
        Make();
        Age();
    }

    //returns whether the critter will move this tick
    public bool willMove(){
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
    void Make(){
        if(m_stats.makesNibs){
            m_controller.nibs += (m_stats.makesMin + ((m_stats.makesMax - m_stats.makesMin) * me.age/m_stats.maxAge))*TICK_LENGTH;
            m_controller.nibsPerSecond += (m_stats.makesMin + (m_stats.makesMax * me.age/m_stats.maxAge));
        }
        else if (m_stats.makesCash){
            m_controller.cash += (m_stats.makesMin + ((m_stats.makesMax - m_stats.makesMin) * me.age/m_stats.maxAge))*TICK_LENGTH;
            m_controller.cashPerSecond += (m_stats.makesMin + (m_stats.makesMax * me.age/m_stats.maxAge));
        }
    }

    //Increments Age of critter
    void Age(){
        me.age += RanchController.TICK_LENGTH;
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
}
