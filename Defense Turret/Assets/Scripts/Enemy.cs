using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 3f;
    public int maxHealth = 3;

    // Private variables
    private int currentHealth;
    private Transform turretTarget = null;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        FindTarget();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (turretTarget != null)
        {
            MoveToTarget(turretTarget);
        }

        // Update Animation
        if (anim != null)
        {
            anim.SetFloat("Speed", speed);
        }
    }

    void FindTarget()
    {
        GameObject turretGO = GameObject.FindGameObjectWithTag("Turret");

        if (turretGO != null)
        {
            turretTarget = turretGO.transform;
        }
        else
        {
            Debug.LogWarning("Enemy: Nu am găsit niciun obiect cu tag-ul 'Turret'!");
        }
    }

    void MoveToTarget(Transform target)
    {
        // Mișcare
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotație (pentru 2D, ca inamicul să privească spre turelă)
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    // Această funcție este apelată când inamicul este lovit
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Opțional: Poți adăuga un efect de "hit" aici (sunet sau particule)

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Adăugăm bani (verificăm dacă LevelManager există ca să nu primim eroare)
        if (LevelManager.instance != null)
        {
            LevelManager.instance.AddCurrency(10);
        }

        // 2. AICI ERA PROBLEMA: Distrugem obiectul inamic
        Destroy(gameObject);

        // Opțional: Instanțiem un efect de explozie înainte de distrugere
        // Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

    // --- ADĂUGAT EXTRA: DETECȚIE COLIZIUNE ---
    // Dacă glonțul nu apelează direct TakeDamage(), o putem face aici.
    // Asigură-te că glonțul are un Collider2D setat pe "Is Trigger" și Tag-ul "Bullet"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // CAZUL 1: Inamicul este lovit de un GLONȚ
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }

        // CAZUL 2: Inamicul a atins TURETA (Baza)
        if (collision.CompareTag("Turret"))
        {
            // --- MODIFICAREA ESTE AICI ---
            // Apelăm GameManager să scădem o viață
            if (GameManager.instance != null)
            {
                GameManager.instance.ReduceLives();
            }
            else
            {
                Debug.LogError("Nu am găsit GameManager în scenă!");
            }

            // Inamicul moare
            Die();
        }
    }

    void OnDrawGizmos()
    {
        if (turretTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, turretTarget.position);
        }
    }
}