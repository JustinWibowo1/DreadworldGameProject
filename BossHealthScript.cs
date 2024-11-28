using UnityEngine;
using UnityEngine.UI;

public class BossHealthScript : MonoBehaviour
{
    private Image healthBar;

    void Awake()
    {
        healthBar = GetComponent<Image>();
    }

    public void SetHealth(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }
}