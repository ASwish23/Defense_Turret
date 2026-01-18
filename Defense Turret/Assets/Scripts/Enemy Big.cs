using UnityEngine;

public class EnemyBig : MonoBehaviour
{
    [Header("Mișcare")]
    public float speed = 1.5f;            // mai lent decât mic
    public GameObject creditPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.5f;       // șansă mai mare de drop

    [Header("Stats")]
    public int hp = 5;                    // mai multă viață
    public GameObject enemySmallPrefab;   // prefab pentru split

    private Transform turretTarget;

    void Start()
    {
        // Încercăm să găsim ținta la start
        FindTarget();
    }

    void Update()
    {
        // --- MODIFICARE: RE-CĂUTARE ȚINTĂ ---
        // Dacă ținta e null (a fost distrusă în timpul upgrade-ului), o căutăm pe cea nouă
        if (turretTarget == null)
        {
            FindTarget();
        }

        // Dacă tot nu am găsit ținta (de ex: jocul s-a terminat), nu facem nimic
        if (turretTarget == null) return;
        // ------------------------------------

        // direcția către turret
        Vector3 direction = (turretTarget.position - transform.position).normalized;

        // mișcare
        transform.position += direction * speed * Time.deltaTime;

        // rotire către turret
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 180);
    }

    // Funcție separată pentru a găsi tureta
    void FindTarget()
    {
        GameObject turretGO = GameObject.FindGameObjectWithTag("Turret");
        if (turretGO != null)
        {
            turretTarget = turretGO.transform;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // drop de credit
        if (Random.value < dropChance && creditPrefab != null)
        {
            Instantiate(creditPrefab, transform.position, Quaternion.identity);
        }

        // split în 2 EnemySmall
        if (enemySmallPrefab != null)
        {
            Instantiate(enemySmallPrefab, transform.position + Vector3.left * 0.5f, Quaternion.identity);
            Instantiate(enemySmallPrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if (turretTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, turretTarget.position);
        }
    }
}