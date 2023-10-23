using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Snake : MonoBehaviour
{
    private Vector2 _direction;
    private List<Transform> _segments;
    public Transform segmentPrefab;

    private void Awake()
    {
        //yilanin segmentlerini listede tutuldu
        _segments = new List<Transform>();
        _segments.Add(transform);
    }

    private void Start()
    {
        ResetMove();
    }

    private void ResetMove()
    {
        //Default olarak saga hareket edecek
        _direction = Vector2.right;
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
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        SnakeMove();
        SortingOfSegments();
    }

    private void SnakeMove()
    {
        //yilani her zaman bir izgaraya hizalamaliyiz bu nedenle tam sayi degerlerine yuvarladik
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            0.0f);
    }

    private void Grow()
    {
        //yilan segmentleri turetilip listeye eklendi
        Transform segment = Instantiate(this.segmentPrefab);

        //uretilen segment nesnesi yilanin sonuna eklenecek
        //yeni segment listedeki son segmentin pozisyonunda uretilecek
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //yiyecek ile snake carpistiginda yiyecek yeni bir segment uretilecek 
        if (other.tag == "Food")
            Grow();
    }

    private void SortingOfSegments()
    {
        //Her bir segment onundeki segmenti takip edecek ve basi en son guncellenecek
        //her segment kuyruktaki bir onceki segmentin pozisyonuna gececek
        //yilanin son segmentinden baslayarak bir bir geriye dogru ilerler
        
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

    }
}