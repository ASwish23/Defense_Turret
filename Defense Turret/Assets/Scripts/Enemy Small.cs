using UnityEngine;

public class EnemySmall : MonoBehaviour
{
    [Header("Mișcare")]
    public float speed = 2f;          // viteza asteroidului
    public GameObject creditPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.3f;

    [Header("Stats")]
    public int hp = 1;                // viața asteroidului

    private Transform turretTarget;

    void Start()
    {
        // găsește turret automat dacă nu e setat
        GameObject turretGO = GameObject.FindGameObjectWithTag("Turret");
        if (turretGO != null)
            turretTarget = turretGO.transform;
        else
            Debug.LogError($"{gameObject.name} nu a găsit turret cu tag 'Turret'");
    }

    void Update()
    {
        if (turretTarget == null) return;

        // direcția către turetă
        Vector3 direction = (turretTarget.position - transform.position).normalized;

        // mișcare
        transform.position += direction * speed * Time.deltaTime;

        // rotire către turetă
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    // metoda pentru a primi damage
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} a primit {damage} damage. HP: {hp}");

        if (hp <= 0)
        {
            // drop de credit
            if (Random.value < dropChance && creditPrefab != null)
            {
                Instantiate(creditPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    // opțional: debug vizual în editor
    void OnDrawGizmos()
    {
        if (turretTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, turretTarget.position);
        }
    }
}
