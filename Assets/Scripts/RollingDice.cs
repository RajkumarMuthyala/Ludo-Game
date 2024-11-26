using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField]
    Sprite[] numberSprite;
    [SerializeField]
    SpriteRenderer numberSpriteHolder;
    [SerializeField]
    SpriteRenderer rollingDiceAnimation;
    [SerializeField]
    int numberGot;
    
    Coroutine generateRandomNumber;
    public int outPieces;
    public int completePieces;
    public PathObjectParent pathParent;
    PlayerPiece[] currentPlayerPieces;
    //List<PlayerPiece> movablePieces = new List<PlayerPiece>();
    //public Dictionary<PlayerPiece,int> nearestKill = new Dictionary<PlayerPiece,int>();
   
    PathPoint[] pathPointToMoveOn;
    Coroutine movePlayerPiece;
    PlayerPiece outPlayerPiece;
    public Dice diceSound;
    int maxNumber = 6;

    private void Awake()
    {
        pathParent = FindObjectOfType<PathObjectParent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        generateRandomNumber = StartCoroutine(RollDice());
    }
    public void mouseRoll()
    {
        generateRandomNumber = StartCoroutine(RollDice());
    }
    IEnumerator RollDice()
    {
        if(GameManager.gm.canDiceRoll)
        {
            diceSound.PlaySound();
            GameManager.gm.canDiceRoll = false;
            yield return new WaitForEndOfFrame();
            numberSpriteHolder.gameObject.SetActive(false);
            rollingDiceAnimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            if(GameManager.gm.totalSix == 2){ maxNumber = 5;}else{ maxNumber = 6;}
            numberGot = Random.Range(0,maxNumber);
            if(numberGot == 6)
            {
                GameManager.gm.totalSix += 1;
            }else{
                GameManager.gm.totalSix = 0;
            }
            numberSpriteHolder.sprite = numberSprite[numberGot];
            numberGot += 1;

            GameManager.gm.numberOfStepsToMove = numberGot;
            GameManager.gm.rollingDice = this;

            numberSpriteHolder.gameObject.SetActive(true);
            rollingDiceAnimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            int numGot = GameManager.gm.numberOfStepsToMove;
            if(PlayerCannotMove())
            {
                yield return new WaitForSeconds(0.5f);
                if(numGot != 6){ GameManager.gm.transferDice = true;}
                else{ GameManager.gm.selfDice = true;}

            }
            else
            {
                if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]){ outPieces = GameManager.gm.redOutPlayers;}
                else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]){ outPieces = GameManager.gm.greenOutPlayers;}
                else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]){ outPieces = GameManager.gm.yellowOutPlayers;}
                else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]){ outPieces = GameManager.gm.blueOutPlayers;}

                if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]){ completePieces = GameManager.gm.redCompletePlayers;}
                else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]){ completePieces = GameManager.gm.greenCompletePlayers;}
                else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]){ completePieces = GameManager.gm.yellowCompletePlayers;}
                else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]){ completePieces = GameManager.gm.blueCompletePlayers;}


                if(outPieces == 0 && numGot != 6)
                {
                    yield return new WaitForSeconds(0.5f);
                    GameManager.gm.transferDice = true;
                }
                else
                {
                    if(outPieces == 0 && numGot == 6 && completePieces < 4) 
                    {
                        MakePlayerReadyToMove(0);
                    }
                    else if(outPieces == 1 && numGot != 6 && GameManager.gm.canPlayerMove)
                    {
                        int playerPiecePos = CheckOutPlayer();
                        if(playerPiecePos >= 0)
                        {
                        GameManager.gm.canPlayerMove = false;
                        movePlayerPiece = StartCoroutine(MoveSteps_Enum(playerPiecePos));
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                            if(numGot != 6){ GameManager.gm.transferDice = true;}
                            else{ GameManager.gm.selfDice = true;}
                            
                        }
                    }
                    else if(GameManager.gm.totalplayerCanPlay == 1 && GameManager.gm.rollingDice==GameManager.gm.manageRollingDice[2])
                    {
                        if(numberGot == 6 && outPieces < 4 && completePieces!= 4)
                        {
                            MakePlayerReadyToMove(OutPlayerToMove());
                        }
                        else
                        {
                            int playerPiecePos = CheckOutPlayer();
                            if(playerPiecePos >= 0)
                            {
                                GameManager.gm.canPlayerMove = false;
                                movePlayerPiece = StartCoroutine(MoveSteps_Enum(playerPiecePos));
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.5f);
                                if(numGot != 6){ GameManager.gm.transferDice = true;}
                                else{ GameManager.gm.selfDice = true;}

                            }
                        }
                    }
                    else if(outPieces>0){yield return new WaitForSeconds(10f);}
                    else if(outPieces == 1 && completePieces==3 && numGot == 6){GameManager.gm.transferDice = true;}
                    else
                    {
                        if(CheckOutPlayer() < 0)
                        {
                            yield return new WaitForSeconds(0.5f);
                            if(numGot != 6){ GameManager.gm.transferDice = true;}
                            else{ GameManager.gm.selfDice = true;}
                        }
                    }
                }

            }
            GameManager.gm.RollingDiceManager();
            if(generateRandomNumber != null)
            {
                StopCoroutine(RollDice());
            }
        }
    }
    int OutPlayerToMove()
    {
        for(int i = 0; i<4;i++)
        {
            if(!GameManager.gm.yellowPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return 0;
    }
    int CheckOutPlayer()
    {
        if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]){ currentPlayerPieces = GameManager.gm.redPlayerPiece;pathPointToMoveOn=pathParent.redPathPoint;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]){ currentPlayerPieces = GameManager.gm.greenPlayerPiece;pathPointToMoveOn=pathParent.greenPathPoint;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]){ currentPlayerPieces = GameManager.gm.yellowPlayerPiece;pathPointToMoveOn=pathParent.yellowPathPoint;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]){ currentPlayerPieces = GameManager.gm.bluePlayerPiece;pathPointToMoveOn=pathParent.bluePathPoint;}
        
        for(int i=0;i<currentPlayerPieces.Length;i++)
        {
            if(currentPlayerPieces[i].isReady && isPathPoitsAvailable(GameManager.gm.numberOfStepsToMove,currentPlayerPieces[i].numberOfStepsAlreadyMoved,pathPointToMoveOn))
            {
                return i;
            }
        }
        return -1;
    }
    // int CheckOutPlayerForKill(){
    //     if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]){ currentPlayerPieces = GameManager.gm.redPlayerPiece;pathPointToMoveOn=pathParent.redPathPoint;}
    //     else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]){ currentPlayerPieces = GameManager.gm.greenPlayerPiece;pathPointToMoveOn=pathParent.greenPathPoint;}
    //     else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]){ currentPlayerPieces = GameManager.gm.yellowPlayerPiece;pathPointToMoveOn=pathParent.yellowPathPoint;}
    //     else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]){ currentPlayerPieces = GameManager.gm.bluePlayerPiece;pathPointToMoveOn=pathParent.bluePathPoint;}
    //     int pos = 0;
    //     for(int i=0;i<currentPlayerPieces.Length;i++)
    //     {
    //         if(currentPlayerPieces[i].isReady && isPathPoitsAvailable(GameManager.gm.numberOfStepsToMove,currentPlayerPieces[i].numberOfStepsAlreadyMoved,pathPointToMoveOn))
    //         {
    //             movablePieces.Add(currentPlayerPieces[i]);
    //             pos++;
        
    //         }
    //     }
    //     if(movablePieces.Count==1){
    //         for(int i=0;i<currentPlayerPieces.Length;i++){
    //             if(movablePieces[0]==currentPlayerPieces[i]){
    //                 return i;
    //             }
    //         }
    //     }else if(movablePieces.Count!=0){
    //         PlayerPiece playerPiece = CheckForKill(movablePieces);
    //         for(int i=0;i<currentPlayerPieces.Length;i++){
    //             if(playerPiece==currentPlayerPieces[i]){
    //                 return i;
    //             }
    //         }
    //     }
    //     return -1;
    // }
    // PlayerPiece CheckForKill(List<PlayerPiece> playerPieces){
    //     foreach(PlayerPiece playerPiece in playerPieces){
    //         foreach(PlayerPiece redPiece in GameManager.gm.redOut){
    //             int redDis = redPiece.numberOfStepsAlreadyMoved+26;
    //             if(redDis>=52){redDis=redDis-52;}
    //             int Dis = redDis - playerPiece.numberOfStepsAlreadyMoved;
    //             if(!nearestKill.ContainsKey(playerPiece)){
    //                 nearestKill.Add(playerPiece, Dis);
    //             }
    //             else if(nearestKill[playerPiece]>Dis){
    //                 nearestKill[playerPiece] = Dis;
    //             }
    //         }
    //     }
    //     PlayerPiece minKey = nearestKill.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;

    //     return minKey;
    // }
    public bool PlayerCannotMove()
    {
        if(outPieces>0)
        {
            bool cannotMove = false;

            if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]){ currentPlayerPieces = GameManager.gm.redPlayerPiece;pathPointToMoveOn=pathParent.redPathPoint;}
            else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]){ currentPlayerPieces = GameManager.gm.greenPlayerPiece;pathPointToMoveOn=pathParent.greenPathPoint;}
            else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]){ currentPlayerPieces = GameManager.gm.yellowPlayerPiece;pathPointToMoveOn=pathParent.yellowPathPoint;}
            else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]){ currentPlayerPieces = GameManager.gm.bluePlayerPiece;pathPointToMoveOn=pathParent.bluePathPoint;}

            for(int i=0;i<currentPlayerPieces.Length;i++)
            {
                if(currentPlayerPieces[i].isReady)
                {
                    if(isPathPoitsAvailable(GameManager.gm.numberOfStepsToMove,currentPlayerPieces[i].numberOfStepsAlreadyMoved,pathPointToMoveOn))
                    {
                        return false;
                    }
                }
                else
                {
                    if(!cannotMove)
                    {
                        cannotMove = true;
                    }
                }
            }
            if(cannotMove)
            {
                return true;
            }
        }
        return false;
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
    public void MakePlayerReadyToMove(int outPlayer)
    {

        if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]){ outPlayerPiece = GameManager.gm.redPlayerPiece[outPlayer];pathPointToMoveOn=pathParent.redPathPoint;GameManager.gm.redOutPlayers += 1;if(!GameManager.gm.redOut.Contains(outPlayerPiece)){GameManager.gm.redOut.Add(outPlayerPiece);}}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]){ outPlayerPiece = GameManager.gm.greenPlayerPiece[outPlayer];pathPointToMoveOn=pathParent.greenPathPoint;GameManager.gm.greenOutPlayers += 1;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]){ outPlayerPiece = GameManager.gm.yellowPlayerPiece[outPlayer];pathPointToMoveOn=pathParent.yellowPathPoint;GameManager.gm.yellowOutPlayers += 1;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]){ outPlayerPiece = GameManager.gm.bluePlayerPiece[outPlayer];pathPointToMoveOn=pathParent.bluePathPoint;GameManager.gm.blueOutPlayers += 1;}


        outPlayerPiece.isReady=true;
        outPlayerPiece.transform.position = pathPointToMoveOn[0].transform.position;
        outPlayerPiece.numberOfStepsAlreadyMoved = 1;
        outPlayerPiece.previousPathPoint = pathPointToMoveOn[0];
        outPlayerPiece.currentPathPoint = pathPointToMoveOn[0];
        outPlayerPiece.currentPathPoint.AddPlayerPiece(outPlayerPiece);
        GameManager.gm.AddPathPoint(outPlayerPiece.currentPathPoint);
        GameManager.gm.canDiceRoll = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferDice = false;
        GameManager.gm.numberOfStepsToMove = 0;
        GameManager.gm.SelfRoll();
    }
    IEnumerator MoveSteps_Enum(int movePlayer)
    {
        if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]){ outPlayerPiece = GameManager.gm.redPlayerPiece[movePlayer];pathPointToMoveOn=pathParent.redPathPoint;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]){ outPlayerPiece = GameManager.gm.greenPlayerPiece[movePlayer];pathPointToMoveOn=pathParent.greenPathPoint;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]){ outPlayerPiece = GameManager.gm.yellowPlayerPiece[movePlayer];pathPointToMoveOn=pathParent.yellowPathPoint;}
        else if(GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]){ outPlayerPiece = GameManager.gm.bluePlayerPiece[movePlayer];pathPointToMoveOn=pathParent.bluePathPoint;}

        GameManager.gm.transferDice = false;
        yield return new WaitForSeconds(0.25f);
        int numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;
        for(int i=outPlayerPiece.numberOfStepsAlreadyMoved; i<(outPlayerPiece.numberOfStepsAlreadyMoved+numberOfStepsToMove); i++)
        {
            if(isPathPoitsAvailable(numberOfStepsToMove,outPlayerPiece.numberOfStepsAlreadyMoved,pathPointToMoveOn))
            {
                outPlayerPiece.transform.position = pathPointToMoveOn[i].transform.position;
                GameManager.gm.ads.Play();
                yield return new WaitForSeconds(.35f);
            }
        }
        if(isPathPoitsAvailable(numberOfStepsToMove,outPlayerPiece.numberOfStepsAlreadyMoved,pathPointToMoveOn))
        {
            outPlayerPiece.numberOfStepsAlreadyMoved += numberOfStepsToMove;
            GameManager.gm.RemovePathPoint(outPlayerPiece.previousPathPoint);
            outPlayerPiece.previousPathPoint.RemovePlayerPiece(outPlayerPiece);
            outPlayerPiece.currentPathPoint = pathPointToMoveOn[outPlayerPiece.numberOfStepsAlreadyMoved - 1];

            if(outPlayerPiece.currentPathPoint.AddPlayerPiece(outPlayerPiece)){
                if(outPlayerPiece.numberOfStepsAlreadyMoved == 57)
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
            
            GameManager.gm.AddPathPoint(outPlayerPiece.currentPathPoint);
            outPlayerPiece.previousPathPoint = outPlayerPiece.currentPathPoint;

            
            GameManager.gm.numberOfStepsToMove = 0;

        }
        
        GameManager.gm.canPlayerMove = true;

        GameManager.gm.RollingDiceManager();

        if(movePlayerPiece != null)
        {
            StopCoroutine("MoveSteps_Enum");
        }
    }

    public void Skip()
    {
        if(completePieces == 4)
        {
            GameManager.gm.transferDice = true;
        }
    }

}
