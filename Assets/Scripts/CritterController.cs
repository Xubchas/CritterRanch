using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake(){
        //initialize directions
        directions.Add(UP);
        directions.Add(DOWN);
        directions.Add(LEFT);
        directions.Add(RIGHT);
        //find controller
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<RanchController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TickCritter(){
        if(willMove()){
            AttemptMove();
        }
        m_controller.nibs += m_stats.nibsMake/10f;
    }

    //returns whether the critter will move this tick
    public bool willMove(){
        //check whether cooling down
        if(tickCooldown > 0){
            tickCooldown--;
            return false;
        }
        return Random.Range(0,100) < m_stats.moveFrequency;
    }

    //attempts a move
    void AttemptMove(){
        tickCooldown = COOLDOWN;
        //moves the critter if it is allowed by ranch controller
        int[] randomDir = directions[Random.Range(0,4)];
        if(m_controller.Move(m_x,m_y,randomDir)){
            m_x += randomDir[1];
            m_y += randomDir[0];
            StartCoroutine(Lerp(m_x,m_y));
        }
    }

    IEnumerator Lerp(int destX, int destY){
        float timeElapsed = 0;
        Vector2 destination = new Vector2(X_ZERO + destX, Y_ZERO - destY);
        while(timeElapsed < MOVE_TIME){
            transform.position = Vector2.Lerp(transform.position, destination, timeElapsed * 1/(MOVE_TIME));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
    }
}
