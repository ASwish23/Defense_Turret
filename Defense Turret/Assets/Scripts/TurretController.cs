using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("General Settings")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate; // Assign the moving part here!

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint1; // Left
    public Transform firePoint2; // Right

    [Header("Upgrade Settings")]
    public SpriteRenderer turretRenderer;
    public Sprite level2Sprite;

    // Internal Variables
    private Transform target;
    private bool isDoubleShotActive = false;

    public bool hasRockets; 

    void Start()
    {
        // Check for enemies 2 times a second (saves performance)
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
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
        if (target == null)
            return;

        // --- 1. ROTATION LOGIC (Aims at the enemy) ---
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // -90 adjusts for sprite facing up
        partToRotate.rotation = Quaternion.Euler(0f, 0f, angle);

        // --- 2. SHOOTING LOGIC ---
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    [Header("Combat Settings")]
    public int damage = 1;  // <--- Add this variable!

    void Shoot()
    {
        // Logic for Left Bullet
        if (firePoint1 != null)
        {
            SpawnBullet(firePoint1);
        }

        // Logic for Right Bullet
        if (isDoubleShotActive && firePoint2 != null)
        {
            SpawnBullet(firePoint2);
        }
    }

    void SpawnBullet(Transform point)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, point.position, point.rotation);

        // Find the bullet script and apply the turret's current damage
        BulletCollision bulletScript = bulletObj.GetComponent<BulletCollision>();
        if (bulletScript != null)
        {
            bulletScript.damage = damage;
        }
    }

    // Call this from your Button
    public void ActivateDoubleShot()
    {
        isDoubleShotActive = true;
        if (turretRenderer != null && level2Sprite != null)
        {
            turretRenderer.sprite = level2Sprite;
        }
    }

    // Visualize the range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}