using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public bool isReady;
    public bool moveNow;
    public int numberOfStepsToMove;
    public int numberOfStepsAlreadyMoved;
    public PathObjectParent pathParent;
    Coroutine movePlayerPiece;
    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;

    private void Awake()
    {
        pathParent = FindObjectOfType<PathObjectParent>();
    }
    public void MoveSteps(PathPoint[] pathPointsToMoveOn)
    {
        movePlayerPiece = StartCoroutine(MoveSteps_Enum(pathPointsToMoveOn));
    }
    public void MakePlayerReadyToMove(PathPoint[] pathPointsToMoveOn)
    {
        isReady=true;
        transform.position = pathPointsToMoveOn[0].transform.position;
        numberOfStepsAlreadyMoved = 1;
        previousPathPoint = pathPointsToMoveOn[0];
        currentPathPoint = pathPointsToMoveOn[0];
        currentPathPoint.AddPlayerPiece(this);
        GameManager.gm.AddPathPoint(currentPathPoint);
        GameManager.gm.canDiceRoll = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferDice = false;
    }
    IEnumerator MoveSteps_Enum(PathPoint[] pathPointsToMoveOn)
    {
        GameManager.gm.transferDice = false;
        yield return new WaitForSeconds(0.25f);
        numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;
        for(int i=numberOfStepsAlreadyMoved; i<(numberOfStepsAlreadyMoved+numberOfStepsToMove); i++)
        {
            currentPathPoint.RescaleandRepositionAllPlayerPiece();
            if(isPathPoitsAvailable(numberOfStepsToMove,numberOfStepsAlreadyMoved,pathPointsToMoveOn))
            {
                transform.position = pathPointsToMoveOn[i].transform.position;
                GameManager.gm.ads.Play();
                yield return new WaitForSeconds(.35f);
            }
        }
        if(isPathPoitsAvailable(numberOfStepsToMove,numberOfStepsAlreadyMoved,pathPointsToMoveOn))
        {
            numberOfStepsAlreadyMoved += numberOfStepsToMove;
            GameManager.gm.RemovePathPoint(previousPathPoint);
            previousPathPoint.RemovePlayerPiece(this);
            currentPathPoint = pathPointsToMoveOn[numberOfStepsAlreadyMoved - 1];

            if(currentPathPoint.AddPlayerPiece(this)){
                if(numberOfStepsAlreadyMoved == 57)
                {
                    GameManager.gm.selfDice = true;
                }
                else
                {
                    if(GameManager.gm.numberOfStepsToMove != 6)
                    {
                        GameManager.gm.transferDice = true;
                    }
                    else
                    {
                        GameManager.gm.selfDice = true;

                    }
                }
            }
            else{
                GameManager.gm.selfDice = true;
            }
            
            GameManager.gm.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            
            GameManager.gm.numberOfStepsToMove = 0;

        }
        
        GameManager.gm.canPlayerMove = true;

        GameManager.gm.RollingDiceManager();

        if(movePlayerPiece != null)
        {
            StopCoroutine("MoveSteps_Enum");
        }
    }

    bool isPathPoitsAvailable(int numOfStepsToMove, int numOfStepsAlreadyMoved,PathPoint[] pathPointToMove)
    {
        if(numOfStepsToMove == 0)
        {
            return false;
        }
        int leftNumOfPath = pathPointToMove.Length - numOfStepsAlreadyMoved;
        if(leftNumOfPath >= numOfStepsToMove)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
