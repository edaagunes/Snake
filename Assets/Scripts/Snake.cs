using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Snake : MonoBehaviour
{
    
    private Vector2 _direction;

    private void Start()
    {
        ResetMove();
    }
    
    private void ResetMove()
    {
        //Default olarak saga hareket edecek
        _direction=Vector2.right;
        Time.timeScale = 0.1f;
    }

    private void Update()
    {
        GetUserInput();
    }

    void GetUserInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;
        }else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction=Vector2.left;
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction=Vector2.right;
        }
    }
    private void FixedUpdate()
    {
        SnakeMove();   
    }

    void SnakeMove()
    {
        //yilani her zaman bir izgaraya hizalamaliyiz bu nedenle tam sayi degerlerine yuvarladik
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            0.0f);
    }
}
