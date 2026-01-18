using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Attributes")]
    public int turretDamage = 1;
    public float range = 10f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate; // ASIGURĂ-TE CĂ AI TRAS OBIECTUL AICI ÎN INSPECTOR!
    public Transform firePoint;

    [Space(10)]
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;

    [Header("Settings")]
    public bool hasRockets = false;

    // --- MODIFICARE 1: Offset reglabil din Inspector ---
    [Range(-360f, 360f)]
    public float rotationOffset = -90f; // Modifică asta dacă turela stă strâmb
    public float rotationSpeed = 10f;   // Cât de repede se rotește

    // Variabile interne
    private Transform target;
    private int currentWeaponMode = 0; // 0 = Mitraliera, 1 = Rachete

    void Start()
    {
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
        // 1. Schimbare armă
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponMode = 0;
            Debug.Log("Arma: MITRALIERA");
        }

        // AICI ERA EROAREA - Am corectat paranteza
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasRockets)
        {
            currentWeaponMode = 1;
            Debug.Log("Arma: RACHETE");
        }

        if (target == null) return;

        // 2. --- ROTIREA TURETEI ---
        LockOnTarget();

        // 3. Tragerea
        if (fireCountdown <= 0f)
        {
            Shoot();
            if (currentWeaponMode == 1)
                fireCountdown = 1f / (fireRate * 0.5f);
            else
                fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void LockOnTarget()
    {
        // Calculăm direcția
        Vector3 dir = target.position - transform.position;

        // Calculăm unghiul
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Creăm rotația finală folosind Offset-ul din Inspector
        Quaternion rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);

        // Aplicăm rotația fluid (Lerp) în loc de instantaneu
        // Dacă vrei instant, folosește linia comentată mai jos
        partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, rotation, Time.deltaTime * rotationSpeed);

        // partToRotate.rotation = rotation; // Varianta instantanee (veche)
    }

    void Shoot()
    {
        GameObject prefabToUse = (currentWeaponMode == 1) ? rocketPrefab : bulletPrefab;
        int currentDamage = (currentWeaponMode == 1) ? turretDamage * 4 : turretDamage;

        GameObject bulletGO = (GameObject)Instantiate(prefabToUse, firePoint.position, firePoint.rotation);
        BulletMovement bullet = bulletGO.GetComponent<BulletMovement>();

        if (bullet != null)
        {
            bullet.damage = currentDamage;
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}