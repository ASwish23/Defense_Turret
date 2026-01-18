using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    // Health variables
    public int maxHealth = 3;
    private int currentHealth;

    private Transform turretTarget = null; // The Turret object is always the target
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        FindTarget();
        anim = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // 1. Verificăm dacă nu avem țintă SAU dacă ținta a fost distrusă
        if (turretTarget == null)
        {
            FindTarget();
        }

        // 2. Mișcăm inamicul DOAR dacă am găsit o țintă nouă
        if (turretTarget != null)
        {
            MoveToTarget(turretTarget);
        }
        else
        {
            // Dacă e null aici, înseamnă că FindTarget nu a găsit nimic tagged "Turret"
            // Inamicul stă pe loc, nu mai dă erori.
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", speed);
        }
    }

    // This function finds the Turret object by its tag
    void FindTarget()
    {
        // Find the FIRST object in the scene tagged "Turret"
        GameObject turretGO = GameObject.FindGameObjectWithTag("Turret");

        if (turretGO != null)
        {
            turretTarget = turretGO.transform;
        }
        else
        {
            Debug.LogError("Enemy failed to find object with tag 'Turret'.");
        }
    }

    void MoveToTarget(Transform target)
    {
        // Move directly to the target position
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Optional: Make the enemy sprite rotate to face the target
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        // Destroy the enemy object
        LevelManager.instance.AddCurrency(10);
    }

    // Optional: Draw a line to the target in the editor for easy debugging
    void OnDrawGizmos()
    {
        if (turretTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, turretTarget.position);
        }
    }
}