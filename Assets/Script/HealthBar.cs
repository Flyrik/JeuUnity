using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public CharacterStats playerStats;

    private void Update()
    {
        if (playerStats != null)
        {
            fillImage.fillAmount = (float)playerStats.currentHealth / playerStats.maxHealth;
        }
    }
}