using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region variables
    /// <summary>
    /// group of all the crumbs, used for enabling/disabling the objects from the scene
    /// instead of destroying
    /// </summary>
    [SerializeField]
    private GameObject[] _crumbs;
    /// <summary>
    /// reference to the player
    /// </summary>
    [SerializeField]
    private GameObject _playerGameObject;
    
    /// <summary>
    /// gameover UI to show or hide
    /// </summary>
    [SerializeField]
    private GameObject _gameOverUI;
    
    /// <summary>
    /// status text to show game over or congratulation
    /// </summary>
    [SerializeField]
    private Text _statusText;
    
    /// <summary>
    /// text object of the score for ui updates
    /// </summary>
    [SerializeField]
    private Text _scoreText;

    /// <summary>
    /// score of the game
    /// </summary>
    public int score { get; private set; }
    /// <summary>
    /// is it menu displayed now?
    /// </summary>
    public bool isMenu;
    
    public bool isPlayerSpotted = false;
    public bool isCountdown = false;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// show or hide the gameover menu
    /// </summary>
    /// <param name="message">the message to show, game over or congratulation</param>
    /// <param name="show">show or hide the menu</param>
    public void ShowMenu(string message, bool show)
    {
        if (show)
        {
            _playerGameObject.SetActive(false);
            _gameOverUI.SetActive(true);
            _statusText.text = message;
            isMenu = true;
        }
        else
        {
            _gameOverUI.SetActive(false);
            _playerGameObject.SetActive(true);
            isMenu = false;
        }
    }

    /// <summary>
    /// increments score
    /// </summary>
    public void AddScore()
    {
        score++;
        UpdateHUD();
    }

    /// <summary>
    /// updates the HUD on the screen
    /// </summary>
    public void UpdateHUD()
    {
        _scoreText.text = score.ToString();
    }

    /// <summary>
    /// reset state and starts a new game
    /// </summary>
    public void StartNewGame()
    {
        ResetScore();
        ResetItems();
        setSpotted(false, false);
        ShowMenu("", false);
    }

    /// <summary>
    /// resets score to zero
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        UpdateHUD();
    }

    /// <summary>
    /// enables all crumb objects
    /// </summary>
    public void ResetItems()
    {
        foreach (var c in _crumbs)
        {
            c.SetActive(true);
        }
    }
    
    public void setSpotted(bool spotted, bool countdown)
    {
        isPlayerSpotted = spotted;
        isCountdown = countdown;
    }
}
