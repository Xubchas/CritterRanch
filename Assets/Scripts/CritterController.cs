using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterController : MonoBehaviour
{
    public int m_x;
    public int m_y;

    private readonly int[] UP = new int[] {-1,0};
    private readonly int[] DOWN = new int[] {1,0};
    private readonly int[] LEFT = new int[] {-1,0};
    private readonly int[] RIGHT = new int[] {1,0};

    private List<int[]> directions;

    //stats for this critter
    public CritterStats m_stats;

    //how many ticks left to wait til moving again
    public int tickCooldown = 0;
    //tick cooldown max
    public const int COOLDOWN = 5;

    public RanchController m_controller;


    // Start is called before the first frame update
    void Start()
    {
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<RanchController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TickCritter(){
        if(willMove()){
            AttemptMove();
        }
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
            Move(randomDir);
        }
    }

    //actually translates the critter
    void Move(int[] direction){
        m_x += direction[1];
        m_y += direction[0];
    }
}
