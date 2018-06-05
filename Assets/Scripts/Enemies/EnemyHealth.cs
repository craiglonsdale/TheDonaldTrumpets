using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyHealth : MonoBehaviour {
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image fullHealth;
    public Image threeQuaterHealth;
    public Image halfHealth;
    public Image quarterHealth;

    public EnemyHealth()
    {

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;
    }
}
