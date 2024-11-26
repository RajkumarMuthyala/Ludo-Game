using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject gamePanel;
    public PathObjectParent pathObjectParent;
    public RollingDice[] manageRollingDice;

    public PlayerPiece[] bluePlayerPiece;
    public PlayerPiece[] redPlayerPiece;
    public PlayerPiece[] greenPlayerPiece;
    public PlayerPiece[] yellowPlayerPiece;
    public PathPoint pathPoint;


    void Start()
    {
        pathObjectParent = GetComponentInParent<PathObjectParent>();
    }

   public void BackToPlayMenu()
   {
        gamePanel.gameObject.SetActive(false);
        resetGameState(redPlayerPiece);
        resetGameState(greenPlayerPiece);
        resetGameState(yellowPlayerPiece);
        resetGameState(bluePlayerPiece);
        mainPanel.gameObject.SetActive(true);

   }
   public void resetGameState(PlayerPiece[] playerPiece)
   {
        for(int i=0;i<playerPiece.Length;i++)
        {
            if(playerPiece[i].gameObject.activeSelf == false)
            {
                playerPiece[i].gameObject.SetActive(true);
            }
            
            playerPiece[i].transform.position = pathObjectParent.basePoint[BasePointPosition(playerPiece[i].name)].transform.position;
            pathPoint.playerPiecesList[0].numberOfStepsAlreadyMoved = 0;
            pathPoint.RemovePlayerPiece(pathPoint.playerPiecesList[0]);
            pathPoint.playerPiecesList.Add(playerPiece[i]);
        }

   }
   int BasePointPosition(string name)
    {
        for(int i =0;i<pathObjectParent.basePoint.Length;i++)
        {
            if(pathObjectParent.basePoint[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }
}
