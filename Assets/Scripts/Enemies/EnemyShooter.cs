using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShooter : MonoBehaviour {
    public float timeBetweenAttacks = 0.5f;
    public float attackTime = 0.5f;
    public int attackDamage;
    public ParticleSystem bulletEmitter;
    public ParticleSystem contactEmitter;
    public Gradient particleColorGradient;
    public ParticleDecalPool splatDecalPool;

    bool attackFinished = true;
    float timer;
    float attackTimer;
    GameObject trump;
    TrumpHealth trumpHealth;
    EnemyHealth enemyHealth;
    List<ParticleCollisionEvent> collisionEvents;


    // Use this for initialization
    void Start () {
        trump = GameObject.FindGameObjectWithTag("Trump");
        trumpHealth = trump.GetComponent<TrumpHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackFinished)
        {
            timer += Time.deltaTime;
            Debug.Log("Timer " + timer);
        }

        if (timer >= timeBetweenAttacks)
        {
            Debug.Log("Shooting Start");
            //if (!bulletEmitter.isPlaying)
            //{
                bulletEmitter.Play(true);
            //}
            attackFinished = false;
            ResetTimer();
        }
        
        if (bulletEmitter.isPlaying)
        {
            attackTimer += Time.deltaTime;
            Debug.Log("Attack Timer " + attackTimer);
        }

        if (attackTimer >= attackTime)
        {
            Debug.Log("SHooting stop");
            bulletEmitter.Stop(true);
            attackFinished = true;
            ResetAttackTimer();
        }
    }

    public abstract void Attack();

    private void ResetAttackTimer()
    {
        attackTimer = 0;
    }

    private void ResetTimer()
    {
        timer = 0;
    }

    void OnParticleCollision(GameObject other)
    {
        trumpHealth.TakeDamage(attackDamage);
    }
}
