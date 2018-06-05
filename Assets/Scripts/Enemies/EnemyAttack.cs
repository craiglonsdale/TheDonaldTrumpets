using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour {
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage;

    float timer;
    GameObject trump;
    TrumpHealth trumpHealth;
    EnemyHealth enemyHealth;

    // Use this for initialization
    void Start () {
        trump = GameObject.FindGameObjectWithTag("Trump");
        trumpHealth = trump.GetComponent<TrumpHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks)
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        ResetTimer();
    }

    protected void ResetTimer()
    {
        timer = 0;
    }
}
