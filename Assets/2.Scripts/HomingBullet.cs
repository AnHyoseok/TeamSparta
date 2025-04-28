using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float speed = 10f;
    private Transform target;
    private Vector2 currentDirection;
    private bool lockedDirection = false;
    public bool isHoming = true; 

    public void SetTarget(Transform enemy)
    {
        target = enemy;
        isHoming = true;
    }

    public void SetDirection(Vector2 direction)
    {
        currentDirection = direction.normalized;
        isHoming = false;
        lockedDirection = true;

        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        if (isHoming && target != null)
        {
            currentDirection = ((Vector2)target.position - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            if (currentDirection == Vector2.zero)
                currentDirection = Vector2.right; 
        }

        transform.position += (Vector3)(currentDirection * speed * Time.deltaTime);

        if (target == null && isHoming)
        {
            lockedDirection = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null && !zombie.IsDead)
            {
                GameObject hiteffect = Instantiate(hitEffect, zombie.transform);
                Destroy(hiteffect, 1f);
                zombie.TakeDamage(1);

                Destroy(gameObject);
            }
        }
    }
}
