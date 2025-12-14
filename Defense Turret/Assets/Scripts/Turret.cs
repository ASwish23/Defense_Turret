using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 10f;
    public float fireRate = 1f;
    public float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy"; // The turret searches for objects with this tags
    public Transform partToRotate;    // Drag the Turret object itself here
    public GameObject bulletPrefab;   // Drag your Bullet Prefab here
    public Transform firePoint;       // Where the bullet comes out

    private Transform target;

    void Start()
    {
        // Check for nearest enemy 2 times a second (saves performance)
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null) return;

        // 1. Rotate to look at the enemy
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Subtract 90 degrees if your sprite faces Up, or 0 if it faces Right
        partToRotate.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        // 2. Shoot
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletMovement bullet = bulletGO.GetComponent<BulletMovement>();

        if (bullet != null)
            bullet.Seek(target);
    }

    // Draw the range circle in the editor so you can see it
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}