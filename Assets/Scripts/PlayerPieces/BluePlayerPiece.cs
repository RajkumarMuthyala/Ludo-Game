using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BluePlayerPiece : PlayerPiece
{
    RollingDice blueHomeRollingDice;
    void Start()
    {
        blueHomeRollingDice = GetComponentInParent<BlueHome>().rollingDice;
    }

    public void OnMouseDown()
    {
        if(GameManager.gm.rollingDice != null)
        {
            if(GameManager.gm.rollingDice == blueHomeRollingDice && GameManager.gm.numberOfStepsToMove == 6)
            {
                if(!isReady)
                {
                    GameManager.gm.blueOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.bluePathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }
            if(GameManager.gm.rollingDice == blueHomeRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                MoveSteps(pathParent.bluePathPoint);
            }
        }
    }
}
