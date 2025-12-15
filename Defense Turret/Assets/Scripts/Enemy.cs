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
        if (turretTarget != null)
        {
            // Move directly toward the Turret/Target's current position
            MoveToTarget(turretTarget);
        }
        else
        {
            // If the turret is destroyed, stop or move to the nearest target
            // For now, we will stop the enemy
        }
        if (anim != null)
        {
            // For simplicity, we just use the fixed speed value for now since we are always moving:
            anim.SetFloat("Speed", speed);

            // A more complex check would be:
            // float currentMovementMagnitude = (transform.position - lastPosition).magnitude / Time.deltaTime;
            // anim.SetFloat("Speed", currentMovementMagnitude);
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