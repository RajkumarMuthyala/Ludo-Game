using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PathPoint : MonoBehaviour
{
    PathPoint[] pathPointsToMoveOn;
    public PathObjectParent pathObjectParent;
    public List<PlayerPiece> playerPiecesList = new List<PlayerPiece>();
    public GameObject[] winnerScreen;
    public TextMeshProUGUI[] texts;
    public int WinCount = 0;

    void Start()
    {
        pathObjectParent = GetComponentInParent<PathObjectParent>();
    }

    public bool AddPlayerPiece(PlayerPiece playerPiece)
    {
        if(this.name == "CenterPath") { Completed(playerPiece);}
        if(this.name != "PathPoint" &&this.name!="PathPoint (8)"&&this.name!="PathPoint (13)"&&this.name!="PathPoint (21)"&&this.name!="PathPoint (26)"&&this.name!="PathPoint (35)"&&this.name!="PathPoint (40)"&&this.name!="PathPoint (48)"&&this.name!="CenterPath")
        {
            if(playerPiecesList.Count == 1)
            {
                string prevPlayerPieceName = playerPiecesList[0].name;
                string curPlayerPieceName = playerPiece.name;
                curPlayerPieceName = curPlayerPieceName.Substring(0,curPlayerPieceName.Length-4);

                if(!prevPlayerPieceName.Contains(curPlayerPieceName))
                {
                    playerPiecesList[0].isReady = false;
                    StartCoroutine(revertOnStart(playerPiecesList[0]));
                    playerPiecesList[0].numberOfStepsAlreadyMoved = 0;
                    RemovePlayerPiece(playerPiecesList[0]);
                    playerPiecesList.Add(playerPiece);
                    return false;
                }
            }
        }
        addPlayer(playerPiece);
        return true;
    }

    IEnumerator revertOnStart(PlayerPiece playerPiece)
    {
        if(playerPiece.name.Contains("Blue")){  GameManager.gm.blueOutPlayers -= 1; pathPointsToMoveOn = pathObjectParent.bluePathPoint;}
        else if(playerPiece.name.Contains("Red")){  GameManager.gm.redOutPlayers -= 1; pathPointsToMoveOn = pathObjectParent.redPathPoint;GameManager.gm.redOut.Remove(playerPiece);}
        else if(playerPiece.name.Contains("Green")){  GameManager.gm.greenOutPlayers -= 1; pathPointsToMoveOn = pathObjectParent.greenPathPoint;}
        else if(playerPiece.name.Contains("Yellow")){  GameManager.gm.yellowOutPlayers -= 1; pathPointsToMoveOn = pathObjectParent.yellowPathPoint;}

        for(int i=playerPiece.numberOfStepsAlreadyMoved-1; i>=0;i--)
        {
            playerPiece.transform.position = pathPointsToMoveOn[i].transform.position;
            yield return new WaitForSeconds(0.02f);
        }

        playerPiece.transform.position = pathObjectParent.basePoint[BasePointPosition(playerPiece.name)].transform.position;
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

    private void Completed(PlayerPiece playerPiece)
    {
        if(GameManager.gm.bluePlayerPiece.Contains(playerPiece)){  GameManager.gm.blueCompletePlayers += 1;GameManager.gm.blueOutPlayers -= 1;if(GameManager.gm.blueCompletePlayers == 4){ ShowCellebration("Blue");}}
        else if(GameManager.gm.redPlayerPiece.Contains(playerPiece)){  GameManager.gm.redCompletePlayers += 1;GameManager.gm.redOutPlayers -= 1;GameManager.gm.redOut.Remove(playerPiece);if(GameManager.gm.redCompletePlayers == 4){ ShowCellebration("Red");}}
        else if(GameManager.gm.greenPlayerPiece.Contains(playerPiece)){  GameManager.gm.greenCompletePlayers += 1;GameManager.gm.greenOutPlayers -= 1;if(GameManager.gm.greenCompletePlayers == 4){ ShowCellebration("Green");}}
        else if(GameManager.gm.yellowPlayerPiece.Contains(playerPiece)){  GameManager.gm.yellowCompletePlayers += 1;GameManager.gm.yellowOutPlayers -= 1;if(GameManager.gm.yellowCompletePlayers == 4){ ShowCellebration("Yellow");}}
        if(GameManager.gm.CheckGameOver()){GameOver();}
    }
    void ShowCellebration(String color)
    {
        WinCount++;
        if(color == "Red"){winnerScreen[0].SetActive(true);texts[0].text=WinCount.ToString();}
        else if(color == "Green"){winnerScreen[1].SetActive(true);texts[1].text=WinCount.ToString();}
        else if(color == "Yellow"){winnerScreen[2].SetActive(true);texts[2].text=WinCount.ToString();}
        else if(color == "Blue"){winnerScreen[3].SetActive(true);texts[3].text=WinCount.ToString();}
    }
    public void GameOver()
    {
        GameManager.gm.gameOver.SetActive(true);
    }
    void addPlayer(PlayerPiece playerPiece)
    {
        playerPiecesList.Add(playerPiece);
        RescaleandRepositionAllPlayerPiece();
    }
    public void RemovePlayerPiece(PlayerPiece playerPiece)
    {
        if(playerPiecesList.Contains(playerPiece))
        {
            playerPiecesList.Remove(playerPiece);
            RescaleandRepositionAllPlayerPiece();
        }
    }
    public void RescaleandRepositionAllPlayerPiece()
    {
        int placeCount = playerPiecesList.Count;
        bool isOdd = (placeCount%2)==0?false:true;

        int extent = placeCount /2 ;
        int counter = 0;
        int spriteLayer =0;
        if(isOdd)
        {
            for(int i=-extent; i<=extent; i++)
            {
                playerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scale[placeCount-1],pathObjectParent.scale[placeCount-1],1f);
                playerPiecesList[counter].transform.position = new Vector3(transform.position.x+(i*pathObjectParent.positionDifference[placeCount-1]),transform.position.y,0f);
                counter++;
            }
        }
        else
        {
            for(int i=-extent; i<extent; i++)
            {
                playerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scale[placeCount-1],pathObjectParent.scale[placeCount-1],1f);
                playerPiecesList[counter].transform.position = new Vector3(transform.position.x+(i*pathObjectParent.positionDifference[placeCount-1]),transform.position.y,0f);
                counter++;
            }
        }
        for(int i=0; i<playerPiecesList.Count;i++)
        {
            playerPiecesList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder=spriteLayer;
            spriteLayer++;
        }
    }
}
