using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceHealthBar : MonoBehaviour
{
    #region Variables
    public Zombie zombie;
    public Image healthBarImage;
    public Transform healthBarPivot;

    [SerializeField] private bool hideFullHealthBar = true;
    #endregion

    private void Update()
    {
        if (zombie == null || healthBarImage == null || Camera.main == null) return;

        healthBarImage.fillAmount = zombie.GetHpRatio();

        healthBarPivot.LookAt(Camera.main.transform);
        healthBarPivot.Rotate(0, 180, 0);

        if (hideFullHealthBar)
        {
            healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1f);
        }
    }
}
