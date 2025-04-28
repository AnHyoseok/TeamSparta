using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int maxHp = 3;
    private int currentHp;
    public int attackDamage = 1;
    public GameObject damageTextPrefab;
    private SpriteRenderer[] spriteRenderers;
    public bool IsDead { get; private set; } = false;

    public int CurrentHp => currentHp; 
    public float GetHpRatio() => (float)currentHp / maxHp; 

    void Start()
    {
        currentHp = maxHp;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHp -= damage;
        StartCoroutine(FlashColor());
        ShowDamageText(damage);
        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Box>(out Box box))
        {
            box.TakeDamage(attackDamage); 
        }
    }
    IEnumerator FlashColor()
    {
        foreach (var sr in spriteRenderers)
            sr.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        foreach (var sr in spriteRenderers)
            sr.color = Color.white;
    }

    void Die()
    {
        if (IsDead) return;

        IsDead = true;

     

        CoinManager.instance?.AddCoin(1);
        Destroy(gameObject, 0.1f);
    }

    void ShowDamageText(int dmg)
    {
        if (damageTextPrefab == null) return;

        Vector3 spawnPos = transform.position + new Vector3(0, 1.5f, -1f);
        GameObject go = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

        if (go.TryGetComponent<DamageText>(out var text))
        {
            text.SetText(dmg.ToString());
        }
    }
}
