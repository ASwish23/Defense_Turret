using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Try to find the "EnemyHealth" script on the object we hit
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();

        // 2. If the object actually HAS that script...
        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damage);

            // Destroy the bullet so it doesn't pass through
            Destroy(gameObject);
        }
    }
}