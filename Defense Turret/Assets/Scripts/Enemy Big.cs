using UnityEngine;

public class EnemyBig : MonoBehaviour
{
    public float speed = 1.5f;
    public GameObject creditPrefab;

    [Header("Effects")]
    public int currencyReward = 50;
    public GameObject deathEffect; // <-- AICI TRAGI PREFABUL EnemyExplosion

    [Range(0f, 1f)] public float dropChance = 0.5f;
    public int hp = 5;
    public GameObject enemySmallPrefab;
    private Transform turretTarget;

    void Start() { FindTarget(); }

    void Update()
    {
        if (turretTarget == null) FindTarget();
        if (turretTarget == null) return;

        Vector3 direction = (turretTarget.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 180);
    }

    void FindTarget()
    {
        GameObject turretGO = GameObject.FindGameObjectWithTag("Turret");
        if (turretGO != null) turretTarget = turretGO.transform;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        // Nu explodeaza inca...

        if (hp <= 0)
        {
            Die(); // ACUM explodeaza!
        }
    }

    void Die()
    {
        // --- EXPLOZIA ---
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            effect.transform.localScale = Vector3.one * 1.5f; // O facem putin mai mare pt Big Enemy
            Destroy(effect, 1f);
        }
        if (LevelManager.instance != null)
            LevelManager.instance.AddCurrency(currencyReward);
        if (Random.value < dropChance && creditPrefab != null)
            Instantiate(creditPrefab, transform.position, Quaternion.identity);

        if (enemySmallPrefab != null)
        {
            Instantiate(enemySmallPrefab, transform.position + Vector3.left * 0.5f, Quaternion.identity);
            Instantiate(enemySmallPrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}