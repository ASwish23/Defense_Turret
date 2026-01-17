using UnityEngine;

public class CreditPickup : MonoBehaviour
{
    public int value = 1;
    public float moveSpeed = 2f;

    Transform turret;

    void Start()
    {
        turret = GameObject.Find("Turret").transform;
    }

    void Update()
    {
        if (turret == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            turret.position,
            moveSpeed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Turret"))
        {
            CurrencyManager.Instance.AddCredits(value);
            Destroy(gameObject);
        }
    }
}
