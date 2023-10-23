using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public Transform segmentPrefab;
    private Vector2Int _direction = Vector2Int.right;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;


    private List<Transform> _segments = new();
    private Vector2Int input;
    private float nextUpdate;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        GetUserInput();
    }

    void GetUserInput()
    {
        // X ekseninde hareket ederken yalnızca yukarı veya aşağı dönmeye izin verir
        if (_direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                input = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                input = Vector2Int.down;
            }
        }

        // Y ekseninde hareket ederken yalnızca sola veya sağa dönmeye izin verir
        else if (_direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                input = Vector2Int.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = Vector2Int.left;
            }
        }
    }

    private void FixedUpdate()
    {
        // Devam etmeden önce bir sonraki güncellemeye kadar beklesin
        if (Time.time < nextUpdate)
        {
            return;
        }

        // Girişe göre yeni yönü ayarlandı
        if (input != Vector2Int.zero)
        {
            _direction = input;
        }

        // Her parçanın konumunu takip ettiği parçayla aynı olacak şekilde ayarlandı.
        // Konumun önceki konuma ayarlanması için bunu ters sırada yapmalıyız,
        // aksi takdirde hepsi üst üste istiflenir.

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        // Yılanı baktığı yöne doğru hareket ettirildi
        // Izgaraya hizalandığından emin olmak için değerleri yuvarlandı
        int x = Mathf.RoundToInt(transform.position.x) + _direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + _direction.y;
        transform.position = new Vector2(x, y);

        // Hıza göre bir sonraki güncelleme zamanını ayarlandı
        nextUpdate = Time.time + 1f / (speed * speedMultiplier);
    }
    

    private void Grow()
    {
        // Yilan segmentleri turetilip listeye eklendi
        Transform segment = Instantiate(segmentPrefab);

        // Uretilen segment nesnesi yilanin sonuna eklenecek
        // Yeni segment listedeki son segmentin pozisyonunda uretilecek
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Yiyecek ile snake carpistiginda yiyecek yeni bir segment uretilecek 
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            ResetState();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            // Yılanın hızı 25f olduğunda duvarlardan geçebilsin.
            if (moveThroughWalls && speed == 25f )
            {
                Traverse(other.transform);
            }
            else
            {
                ResetState();
            }
        }
    }

    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in _segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetState()
    {
        _direction = Vector2Int.right;
        transform.position = Vector3.zero;

        //Kafayi yok etmeyi atlamak için 1'den başlatildi
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(transform);

        //-1 cunku kafa zaten listede
        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }

    private void Traverse(Transform wall)
    {
        //Duvarlardan gecmesi saglandi
        Vector3 position = transform.position;

        if (_direction.x != 0f)
        {
            position.x = Mathf.RoundToInt(-wall.position.x + _direction.x);
        }
        else if (_direction.y != 0f)
        {
            position.y = Mathf.RoundToInt(-wall.position.y + _direction.y);
        }

        transform.position = position;
    }
}