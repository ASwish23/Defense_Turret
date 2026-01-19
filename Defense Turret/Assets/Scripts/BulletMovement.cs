using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public int damage = 1;

    // AM SCOS variabila impactEffect. Nu mai e treaba glontului!

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void HitTarget()
    {
        // AICI ERA EXPLOZIA INAINTE. AM SCOS-O.
        // Glontul acum doar da o "palma" (damage) si dispare.

        Enemy normalEnemy = target.GetComponent<Enemy>();
        if (normalEnemy != null)
        {
            normalEnemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        EnemyBig bigEnemy = target.GetComponent<EnemyBig>();
        if (bigEnemy != null)
        {
            bigEnemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        EnemySmall smallEnemy = target.GetComponent<EnemySmall>();
        if (smallEnemy != null)
        {
            smallEnemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}