using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = true;

    private void Update()
    {
        // if the r key is press 
        // restsrt the game 
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
