using UnityEngine;

public class ObamaDummyAttack : MonoBehaviour {
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage;

    float timer;
    GameObject trump;
    TrumpHealth trumpHealth;
    ObamaDummyHealth obamaDummyHealth;

	// Use this for initialization
	void Start () {
        trump = GameObject.FindGameObjectWithTag("Trump");
        trumpHealth = trump.GetComponent<TrumpHealth>();
        obamaDummyHealth = GetComponent<ObamaDummyHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks)
        {
            Attack();
        }
	}

    void Attack()
    {
        timer = 0;
    }
}
