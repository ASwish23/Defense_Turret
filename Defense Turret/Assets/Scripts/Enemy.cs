using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int maxHealth = 3;

    [Header("Effects")]
    public int currencyReward = 25;
    public GameObject deathEffect; // <-- AICI TRAGI PREFABUL EnemyExplosion

    private int currentHealth;
    private Transform turretTarget;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        FindTarget();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (turretTarget == null) FindTarget();
        if (turretTarget != null) MoveToTarget(turretTarget);
        if (anim != null) anim.SetFloat("Speed", speed);
    }

    void FindTarget()
    {
        GameObject turretGO = GameObject.FindGameObjectWithTag("Turret");
        if (turretGO != null) turretTarget = turretGO.transform;
    }

    void MoveToTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        // Daca inca are viata, NU se intampla nimic vizual.

        if (currentHealth <= 0)
        {
            Die(); // Abia cand viata e 0, apelam Die()
        }
    }

    void Die()
    {
        // --- EXPLOZIA ESTE AICI (LA MOARTE) ---
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }

        if (LevelManager.instance != null)
            LevelManager.instance.AddCurrency(currencyReward);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Turret"))
        {
            if (GameManager.instance != null) GameManager.instance.ReduceLives();
            Die();
        }
    }
}