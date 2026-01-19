using UnityEngine;

public class EnemySmall : MonoBehaviour
{
    public float speed = 2f;
    public GameObject creditPrefab;

    [Header("Effects")]
    public int currencyReward = 15;
    public GameObject deathEffect; // <-- AICI TRAGI PREFABUL EnemyExplosion

    [Range(0f, 1f)] public float dropChance = 0.3f;
    public int hp = 1;
    private Transform turretTarget;

    void Start() { FindTarget(); }

    void Update()
    {
        if (turretTarget == null) FindTarget();
        if (turretTarget == null) return;

        Vector3 direction = (turretTarget.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void FindTarget()
    {
        GameObject turretGO = GameObject.FindGameObjectWithTag("Turret");
        if (turretGO != null) turretTarget = turretGO.transform;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            // --- EXPLOZIA ---
            if (deathEffect != null)
            {
                GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
            }
            if (LevelManager.instance != null)
                LevelManager.instance.AddCurrency(currencyReward);
            if (Random.value < dropChance && creditPrefab != null)
                Instantiate(creditPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}