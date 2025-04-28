using UnityEngine;
using UnityEngine.EventSystems;

public class AutoShoot : MonoBehaviour
{
    public Transform gun;             
    public Transform firePoint;     
    public GameObject bulletPrefab;    
    public string enemyTag = "Zombie";
    public float fireRate = 0.5f;

    private float fireTimer;
    private bool isManualAiming = false;
    private LineRenderer aimLine;

    private void Start()
    {
        aimLine = gameObject.AddComponent<LineRenderer>();
        aimLine.positionCount = 2;
        aimLine.startWidth = 0.05f;
        aimLine.endWidth = 0.05f;
        aimLine.enabled = false;
    }

    private void Update()
    {
        if (IsMouseOverUI())
        {
            isManualAiming = false;
            aimLine.enabled = false;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                isManualAiming = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isManualAiming = false;
                aimLine.enabled = false;
            }
        }

        fireTimer += Time.deltaTime;

        if (isManualAiming)
        {
            ManualAim();

            if (fireTimer >= fireRate)
            {
                FireManual(); 
                fireTimer = 0f;
            }
        }
        else
        {
            AutoAim();
        }
    }

    private void AutoAim()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length == 0) return;

        GameObject nearest = FindNearestEnemy(enemies);
        if (nearest == null) return;

        // ����
        Vector2 dir = nearest.transform.position - gun.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0, 0, angle);

        if (fireTimer >= fireRate)
        {
            Fire(nearest.transform);
            fireTimer = 0f;
        }
    }

    private void ManualAim()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorld - (Vector2)gun.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0, 0, angle);

        aimLine.enabled = true;
        aimLine.SetPosition(0, firePoint.position);
        aimLine.SetPosition(1, mouseWorld);
    }

    private void Fire(Transform target)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        HomingBullet homing = bullet.GetComponent<HomingBullet>();
        if (homing != null)
        {
            homing.SetTarget(target); 
        }

        Destroy(bullet, 3f);
    }

    private void FireManual()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mouseWorld - (Vector2)firePoint.position).normalized;

        HomingBullet homing = bullet.GetComponent<HomingBullet>();
        if (homing != null)
        {
            homing.SetDirection(shootDir); 
        }

        Destroy(bullet, 3f);
    }



    private GameObject FindNearestEnemy(GameObject[] enemies)
    {
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            if (!IsInCameraView(enemy.transform)) continue;

            Zombie zombie = enemy.GetComponent<Zombie>();
            if (zombie != null && zombie.IsDead) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    private bool IsInCameraView(Transform target)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(target.position);
        return viewPos.x >= 0f && viewPos.x <= 1f &&
               viewPos.y >= 0f && viewPos.y <= 1f &&
               viewPos.z > 0f;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
