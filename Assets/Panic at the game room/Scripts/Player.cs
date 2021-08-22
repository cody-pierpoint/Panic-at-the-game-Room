using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("collision enter");
        //collision with crumbs
        if (other.gameObject.CompareTag("Item"))
        {
            //deactivate to remove from scene but don't destroy
            other.gameObject.SetActive(false);
            _gameManager.AddScore();
        }
        
        //collision with enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            //game over, deactivate self
            _gameManager.ShowMenu("Game Over", true);
        }
        
        //collision with exit point
        if (other.gameObject.CompareTag("Exit Point"))
        {
            //Debug.Log("collision exit");
            //game clear
            _gameManager.ShowMenu($"RAD\nYou escaped with {_gameManager.score} crumbs", true);
        }

    }
    
}