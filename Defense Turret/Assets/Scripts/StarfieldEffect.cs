using UnityEngine;

public class StarfieldEffect : MonoBehaviour
{
    public float speed = 0.5f;       // viteza de mișcare
    public float flickerSpeed = 2f;  // cât de rapid clipesc stelele
    private SpriteRenderer sr;
    private Vector3 startPos;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    void Update()
    {
        // mișcare verticală continuă
        float newY = Mathf.Repeat(Time.time * speed, 10f); // 10 = înălțimea sprite-ului
        transform.position = startPos + Vector3.down * newY;

        // flicker ușor al alpha-ului
        if (sr != null)
        {
            float alpha = 0.5f + 0.5f * Mathf.Sin(Time.time * flickerSpeed + transform.position.x);
            sr.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
