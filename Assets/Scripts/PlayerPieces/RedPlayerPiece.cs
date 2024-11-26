using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerPiece : PlayerPiece
{
    RollingDice redHomeRollingDice;
    void Start()
    {
        redHomeRollingDice = GetComponentInParent<RedHome>().rollingDice;
    }

    public void OnMouseDown()
    {
        if(GameManager.gm.rollingDice != null)
        {
            if(!isReady)
            {
                if(GameManager.gm.rollingDice == redHomeRollingDice && GameManager.gm.numberOfStepsToMove == 6)
                {
                    GameManager.gm.redOutPlayers += 1;
                    PlayerPiece playerPiece = this;
                    GameManager.gm.redOut.Add(playerPiece);
                    MakePlayerReadyToMove(pathParent.redPathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }
            if(GameManager.gm.rollingDice == redHomeRollingDice && isReady)
            {
                MoveSteps(pathParent.redPathPoint);
            }
        }
    }

    
}
