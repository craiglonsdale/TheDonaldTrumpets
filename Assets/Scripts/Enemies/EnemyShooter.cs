using System.Collections;
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

    protected bool isWaiting;
    GameObject trump;
    TrumpHealth trumpHealth;
    protected EnemyHealth enemyHealth;
    List<ParticleCollisionEvent> collisionEvents;


    // Use this for initialization
    void Start () {
        trump = GameObject.FindGameObjectWithTag("Trump");
        trumpHealth = trump.GetComponent<TrumpHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        collisionEvents = new List<ParticleCollisionEvent>();
        isWaiting = false;
        var main = bulletEmitter.main;
        main.duration = attackTime;
        bulletEmitter.Play(true);
    }

    // Update is called once per frame
    void Update()
    {        
        if (isWaiting)
        {
            StartCoroutine(WaitForIt());
        }
        if (!isWaiting)
        {
            StartCoroutine(Shooting());
        }
        Attack();
    }

    public abstract void Attack();

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isWaiting = false;
        Debug.Log("Starting SHooting");
        bulletEmitter.Play(true);
    }

    IEnumerator Shooting()
    {
        yield return new WaitUntil(() => bulletEmitter.isStopped);
        isWaiting = true;
    }

    void OnParticleCollision(GameObject other)
    {
        var damage = other.transform.parent.gameObject.GetComponent<TrumpShooter>().attackDamage;
        enemyHealth.TakeDamage(damage);
    }
}
