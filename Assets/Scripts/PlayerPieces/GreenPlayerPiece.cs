using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerPiece : PlayerPiece
{
    RollingDice greenHomeRollingDice;
    void Start()
    {
        greenHomeRollingDice = GetComponentInParent<GreenHome>().rollingDice;
    }

    public void OnMouseDown()
    {
        if(GameManager.gm.rollingDice != null)
        {
            if(!isReady)
            {
                if(GameManager.gm.rollingDice == greenHomeRollingDice && GameManager.gm.numberOfStepsToMove == 6)
                {
                    GameManager.gm.greenOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.greenPathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }
            if(GameManager.gm.rollingDice == greenHomeRollingDice && isReady)
            {
                MoveSteps(pathParent.greenPathPoint);
            }
        }
    }
}
