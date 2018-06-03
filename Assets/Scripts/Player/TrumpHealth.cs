using UnityEngine;
using UnityEngine.UI;

public class TrumpHealth : MonoBehaviour {
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image fullHealth;
    public Image threeQuaterHealth;
    public Image halfHealth;
    public Image quarterHealth;

    AudioSource playerAudio;
    TrumpController trumpController;
    bool damaged;

	// Use this for initialization
	void Start () {
        currentHealth = startingHealth;
        playerAudio = GetComponent<AudioSource>();
        trumpController = GetComponent<TrumpController>();
        fullHealth.enabled = true;
        threeQuaterHealth.enabled = false;
        halfHealth.enabled = false;
        quarterHealth.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (damaged)
        {
            // Do damaged shit
            damaged = false;
        }
        damaged = false;
	}

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if (!playerAudio.isPlaying)
        {
            playerAudio.Play();
        }


        if (currentHealth > 75)
        {
            fullHealth.enabled = true;
            quarterHealth.enabled = false;
            halfHealth.enabled = false;
            quarterHealth.enabled = false;
        }
        else if (currentHealth <= 75 && currentHealth > 50)
        {
            fullHealth.enabled = false;
            quarterHealth.enabled = true;
            halfHealth.enabled = false;
            quarterHealth.enabled = false;
        }
        else if (currentHealth <= 50 && currentHealth > 25)
        {
            fullHealth.enabled = false;
            quarterHealth.enabled = false;
            halfHealth.enabled = true;
            quarterHealth.enabled = false;
        }
        else
        {
            fullHealth.enabled = false;
            quarterHealth.enabled = false;
            halfHealth.enabled = false;
            quarterHealth.enabled = true;
        }
    }
}
