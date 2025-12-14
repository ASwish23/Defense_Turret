using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;     // How fast the bullet flies
    public int damage = 1;        // How much damage it deals

    // This is the function the Turret was looking for! 
    // It tells the bullet who to chase.
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        // If the target disappears (dies) before the bullet hits, destroy the bullet
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate direction to the target
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // If the bullet is close enough to hit this frame...
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Otherwise, keep moving towards the target
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        // Optional: Rotate bullet to face the target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void HitTarget()
    {
        // Try to find the "Enemy" script on the object we hit
        Enemy enemyScript = target.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }

        // Destroy the bullet immediately after hitting
        Destroy(gameObject);
    }
}