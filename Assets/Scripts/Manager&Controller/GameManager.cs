using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public RollingDice rollingDice;
    public int numberOfStepsToMove;
    public bool canPlayerMove = true;
    public bool canDiceRoll = true;

    List<PathPoint> playerOnPathPointList = new List<PathPoint>();

    public bool transferDice = false;
    public bool selfDice =false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;
    public List<PlayerPiece> redOut = new List<PlayerPiece>();
    
    public int blueCompletePlayers;
    public int redCompletePlayers;
    public int greenCompletePlayers;
    public int yellowCompletePlayers;


    public RollingDice[] manageRollingDice;

    public PlayerPiece[] bluePlayerPiece;
    public PlayerPiece[] redPlayerPiece;
    public PlayerPiece[] greenPlayerPiece;
    public PlayerPiece[] yellowPlayerPiece;

    public int totalplayerCanPlay;
    public GameObject gameOver;

    public int totalSix = 0;
    public AudioSource ads;

    private void Awake()
    {
        gm = this;
        ads = GetComponent<AudioSource>();
    }
    public void AddPathPoint(PathPoint pathPoint)
    {
        playerOnPathPointList.Add(pathPoint);
    }
    public void RemovePathPoint(PathPoint pathPoint)
    {
        if(playerOnPathPointList.Contains(pathPoint))
        {
            playerOnPathPointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path Point Do Not Found To Be Remove");
        }
    }
    public void RollingDiceManager()
    {
        
        if(GameManager.gm.transferDice)
        {
            rollingDice.Skip();
            if(GameManager.gm.numberOfStepsToMove!=6)
            {
                ShiftDice();
            }
            GameManager.gm.canDiceRoll = true;
        }
        else
        {
            if(GameManager.gm.selfDice)
            {
                GameManager.gm.selfDice = false;
                GameManager.gm.canDiceRoll = true;
                GameManager.gm.SelfRoll();
            }
        }
    }
    public void SelfRoll()
    {
        if(GameManager.gm.totalplayerCanPlay == 1 && GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2])
        {
            Invoke("rolled",0.6f);
        }
        
    }
    void rolled()
    {
        GameManager.gm.manageRollingDice[2].mouseRoll();
    }
    void ShiftDice()
    {
        int nextDice;
        if(GameManager.gm.totalplayerCanPlay == 1)
        {
            if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);
                passout(0);
                Invoke("rolled",0.6f);
            }
            else
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);
                passout(2);
            }
        }
        else if(GameManager.gm.totalplayerCanPlay == 2)
        {
            if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);
                passout(0);
            }
            else
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);
                passout(2);
            }
        }
        else if(GameManager.gm.totalplayerCanPlay == 3)
        {
            for(int i =0;i<3;i++)
            {
                if(i==2){ nextDice = 0; }else{ nextDice = i + 1; }
                i = passout(i);
                if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[i])
                {
                    GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for(int i=0;i<4;i++)
            {
                if(i==3){ nextDice = 0; }else{ nextDice = i + 1; }
                i = passout(i);
                if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[i])
                {
                    GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }

    }
    int passout(int i)
    {
        if(i==0){ if(GameManager.gm.redCompletePlayers == 4){ return (i+1);}}
        else if(i==1){ if(GameManager.gm.greenCompletePlayers == 4){ return (i+1);}}
        else if(i==2){ if(GameManager.gm.yellowCompletePlayers == 4){ return (i+1);}}
        else if(i==3){ if(GameManager.gm.blueCompletePlayers == 4){ return (i+1);}}
        return i;
    }
    public bool CheckGameOver()
    {
        if(totalplayerCanPlay == 2)
        {
            if(redCompletePlayers == 4 && yellowCompletePlayers ==4)
            {
                return true;
            }
        }
        if(totalplayerCanPlay == 3)
        {
            if(redCompletePlayers == 4 && yellowCompletePlayers ==4 && greenCompletePlayers == 4)
            {
                return  true;
            }
        }
        if(totalplayerCanPlay == 4)
        {
            if(redCompletePlayers == 4 && yellowCompletePlayers ==4 && greenCompletePlayers == 4 && blueCompletePlayers ==4)
            {
                return  true;
            }
        }
        if(totalplayerCanPlay == 1)
        {
            if(redCompletePlayers == 4 && yellowCompletePlayers ==4)
            {
                return  true;
            }
        }
        return false;
    }

}
