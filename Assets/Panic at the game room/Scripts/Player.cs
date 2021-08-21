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
       Debug.Log("player start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision enter");
        //collision with crumbs
        if (other.gameObject.CompareTag("Item"))
        {
            Debug.Log("collision item");
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
        
    }
    
}