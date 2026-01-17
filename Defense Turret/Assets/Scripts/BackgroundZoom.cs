using UnityEngine;

public class BackgroundZoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;       // cât de repede face zoom
    public float minScale = 1f;          // dimensiunea minimă
    public float maxScale = 1.2f;        // dimensiunea maximă

    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float scale = minScale + (maxScale - minScale) * 0.5f * (1 + Mathf.Sin(Time.time * zoomSpeed));
        transform.localScale = startScale * scale;
    }
}
