using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        //yiyecegin uretilecegi konumlar sinirlandirildi
        Bounds bounds = this.gridArea.bounds;

        //grid'lerden tasma olmamasi icin degerler yuvarlandi
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        //yiyecegin konumunu bu koordinatlara atandi
        transform.position = new Vector3(Mathf.Round(x),Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //yiyecek ile snake carpistiginda yiyecek yeni bir konumda uretilecek 
        if (other.tag=="Player")
        {
            RandomizePosition();
        }
        
    }
}
