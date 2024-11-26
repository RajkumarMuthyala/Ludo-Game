using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject gamepanel;
    
    public void OnMouseDown()
    {
        mainPanel.SetActive(true);
        gamepanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToHome()
    {
        mainPanel.SetActive(true);
        gamepanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
