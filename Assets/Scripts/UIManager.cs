using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject gamePanel;

    public void Game1()
    {
        GameManager.gm.totalplayerCanPlay = 2;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        Game1setting();

    }
        public void Game2()
    {
        GameManager.gm.totalplayerCanPlay = 3;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        Game2setting();
    }
        public void Game3()
    {
        GameManager.gm.totalplayerCanPlay = 4;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);

    }
        public void Game4()
    {
        GameManager.gm.totalplayerCanPlay = 1;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        Game1setting();
    }

    void Game1setting()
    {
        HidePlayers(GameManager.gm.greenPlayerPiece);
        HidePlayers(GameManager.gm.bluePlayerPiece);
    }
    void HidePlayers(PlayerPiece[] playerPieces)
    {
        for(int i=0;i<playerPieces.Length;i++)
        {
            playerPieces[i].gameObject.SetActive(false);
        }
    }
    void Game2setting()
    {
        HidePlayers(GameManager.gm.bluePlayerPiece);
    }
}
