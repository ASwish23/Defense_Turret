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

        // Dacă tot nu am găsit ținta, ieșim
        if (turretTarget == null) return;
        // ------------------------------------

        // direcția către turetă
        Vector3 direction = (turretTarget.position - transform.position).normalized;

        // mișcare
        transform.position += direction * speed * Time.deltaTime;

        // rotire către turetă
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
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

    // metoda pentru a primi damage
    public void TakeDamage(int damage)
    {
        hp -= damage;
        // Debug.Log($"{gameObject.name} a primit {damage} damage. HP: {hp}"); // Poți decomenta pentru debug

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