using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    private Snake snake;

    private void Awake()
    {
        snake = FindObjectOfType<Snake>();
    }

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;

        //Sinirlarin icinde rastgele bir konum secildi
        //Grid'e hizalanmasi icin deger yuvarlandi
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        //Yiyecegin yilan uzerinde uretilmesi engellendi
        while (snake.Occupies(x, y))
        {
            x++;

            if (x > bounds.max.x)
            {
                x = Mathf.RoundToInt(bounds.min.x);
                y++;

                if (y > bounds.max.y)
                {
                    y = Mathf.RoundToInt(bounds.min.y);
                }
            }
        }

        //yiyecegin konumunu bu koordinatlara atandi
        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //yiyecek ile snake carpistiginda yiyecek yeni bir konumda uretildi
        RandomizePosition();
    }
}