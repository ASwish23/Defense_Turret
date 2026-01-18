using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float speed = 0.5f;     // viteza de mișcare
    public float spriteHeight = 10f; // înălțimea sprite-ului pentru looping

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Repeat(Time.time * speed, spriteHeight);
        transform.position = startPos + Vector3.down * newY;
    }
}