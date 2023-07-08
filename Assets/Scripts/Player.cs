using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int position = 0;
    private int nextRoll = 0;

    public int GetBoardPosition()
    {
        return position;
    }

    public void SetBoardPosition(int pos)
    {
        position = pos;
    }

    public int GetNextRoll()
    {
        return nextRoll;
    }

    public void SetNextRoll(int roll)
    {
        nextRoll = roll;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
