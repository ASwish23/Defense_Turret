using UnityEngine;

public class AsteroidRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // grade/sec

    Rigidbody2D rb;

    void Start()
    {
        rotationSpeed = Random.Range(-100f, 100f);
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
