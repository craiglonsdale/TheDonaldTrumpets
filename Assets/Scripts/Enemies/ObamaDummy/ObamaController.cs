using UnityEngine;

public class ObamaController : EnemyController
{
    protected override void SetUpEnemy()
    {
        var obama = GameObject.FindGameObjectWithTag("Obama");
        enemyHealth = obama.GetComponent<EnemyHealth>();
    }
}
