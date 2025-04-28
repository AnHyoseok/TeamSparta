using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public int maxHp = 30;
    private int currentHp;
    public Image healthBarImage;
    public Transform healthBarPivot;

    void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = (float)currentHp / maxHp;
            healthBarPivot.LookAt(Camera.main.transform);
            healthBarPivot.Rotate(0, 180, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
