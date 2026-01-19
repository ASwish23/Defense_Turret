using UnityEngine;

public class TurretV2 : MonoBehaviour
{
    [Header("Attributes")]
    public int turretDamage = 2;
    public float range = 12f;
    public float fireRate = 1.5f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public Transform[] firePoints; // Aici pui cele 2 tevi

    [Space(10)]
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;

    [Header("Weapon Settings")]
    public bool hasRockets = true;
    private Transform target;
    private int currentWeaponMode = 0; // 0 = Glont, 1 = Racheta

    void Start()
    {
        // --- LINIA NOUA ---
        if (hasRockets)
        {
            currentWeaponMode = 1; // Activeaza rachetele automat la start
        }
        // ------------------

        // Cauta tinta de 2 ori pe secunda
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // --- DEBUG START ---
        if (enemies.Length == 0)
        {
            // Daca apare asta in consola, inseamna ca inamicii NU au Tag-ul "Enemy"
            Debug.LogWarning("Tureta nu gaseste niciun obiect cu tag-ul: " + enemyTag);
        }
        // -------------------

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // --- DEBUG PENTRU DISTANTA ---
            // Daca distanta e mereu mare (ex: 50+), avem o problema cu axa Z
            // Debug.Log("Distanta pana la inamic: " + distanceToEnemy); 

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            Debug.Log("TINTA GASITA: " + target.name); // Confirmare ca a gasit pe cineva
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        // Schimbare arma: tasta 1 sau 2
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeaponMode = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasRockets) currentWeaponMode = 1;

        if (target == null) return;

        // Rotire cap tureta spre inamic
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (partToRotate != null)
        {
            partToRotate.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }

        // Logica de tragere
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / (currentWeaponMode == 1 ? fireRate * 0.5f : fireRate);
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject prefabToUse = (currentWeaponMode == 1) ? rocketPrefab : bulletPrefab;
        int damageToApply = (currentWeaponMode == 1) ? turretDamage * 4 : turretDamage;

        foreach (Transform fp in firePoints)
        {
            if (fp != null)
            {
                GameObject projectileGO = Instantiate(prefabToUse, fp.position, fp.rotation);
                BulletMovement projectile = projectileGO.GetComponent<BulletMovement>();
                if (projectile != null)
                {
                    projectile.damage = damageToApply;
                    projectile.Seek(target);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}