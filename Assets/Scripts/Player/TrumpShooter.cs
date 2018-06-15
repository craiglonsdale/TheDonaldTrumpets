using System.Collections.Generic;
using UnityEngine;

public class TrumpShooter : MonoBehaviour {

    public ParticleSystem bulletEmitter;
    public ParticleSystem contactEmitter;
    public Gradient particleColorGradient;
    public ParticleDecalPool splatDecalPool;

    List<ParticleCollisionEvent> collisionEvents;
    // Use this for initialization
    void Start () {
        collisionEvents = new List<ParticleCollisionEvent>();
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

    void OnParticleCollision(GameObject other)
    {
        //int numCollisionEvents = bulletEmitter.GetCollisionEvents(other, collisionEvents);
        other.GetComponent<EnemyHealth>().TakeDamage(1);
        //Destroy(gameObject);
        //ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

        //for (int i = 0; i < collisionEvents.Count; i++)
        //{
        //    splatDecalPool.ParticleHit(collisionEvents[i], particleColorGradient);
        //    EmitAtLocation(collisionEvents[i]);
        //}

    }
}
