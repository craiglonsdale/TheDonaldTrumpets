using UnityEngine;

public class ObamaDummyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;

	// Use this for initialization
	void Start () {
        currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TakeDamage(int amount, Vector3 hitPoint)
    {
        currentHealth -= amount;
    }
}
