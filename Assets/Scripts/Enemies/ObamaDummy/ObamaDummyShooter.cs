using UnityEngine;

public class ObamaDummyShooter : EnemyShooter {

    private bool isPhaseTwo = false;
    private bool isPhaseThree = false;
    public ParticleSystem phaseTwo;
    public ParticleSystem phaseThree;
    public override void Attack()
    {
        if (enemyHealth.currentHealth <= enemyHealth.startingHealth * 0.66 &&
            enemyHealth.currentHealth >= enemyHealth.startingHealth * 0.33
            && !bulletEmitter.isPlaying && !isPhaseTwo)
        {
            Debug.Log("PHASE TWOOOOOOOOO");
            isPhaseTwo = true;
            bulletEmitter = phaseTwo;
            isWaiting = true;
            
        }
        if (enemyHealth.currentHealth <= enemyHealth.startingHealth * 0.33 && !bulletEmitter.isPlaying && !isPhaseThree)
        {
            Debug.Log("PHASE THREE");
            isPhaseTwo = false;
            isPhaseThree = true;
            bulletEmitter = phaseThree;
        }
    }
}
