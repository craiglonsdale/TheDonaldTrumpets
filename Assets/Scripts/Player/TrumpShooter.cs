using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TrumpShooter : MonoBehaviour {
    public ParticleSystem bulletEmitter;
    public ParticleSystem contactEmitter;
    public Gradient particleColorGradient;
    public ParticleDecalPool splatDecalPool;
    public int attackDamage;
    List<ParticleCollisionEvent> collisionEvents;
    TrumpHealth health;

    // Use this for initialization
    void Start () {
        collisionEvents = new List<ParticleCollisionEvent>();
        health = GetComponent<TrumpHealth>();
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            // ParticleSystem.MainModule psMain = bulletEmitter.main;
            // psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));
            if (!bulletEmitter.isPlaying)
            {
               bulletEmitter.Play(true);
            }
        }
        else if (bulletEmitter.isPlaying)
        {
            bulletEmitter.Stop(true);
        }
    }

    IEnumerator Shooting()
    {
        // bulletEmitter.Stop(true);
        yield return new WaitUntil(() => !Input.GetButton("Fire1"));
        bulletEmitter.Stop(true);
    }

    IEnumerator Wait()
    {
        // bulletEmitter.Stop(true);
        yield return new WaitUntil(() => Input.GetButton("Fire1"));
        bulletEmitter.Play(true);
    }

    void OnParticleCollision(GameObject other)
    {
        var damage = other.transform.parent.gameObject.GetComponent<EnemyShooter>().attackDamage;
        health.TakeDamage(damage);
    }
}
