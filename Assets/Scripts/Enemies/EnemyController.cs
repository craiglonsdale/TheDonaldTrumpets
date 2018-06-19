using UnityEngine;

public abstract class EnemyController : MonoBehaviour {

    public LevelController levelController;
    public PathCreator IdlePath;
    public float SpeedMultiplier = 1;
    Vector2[] pathPoints;
    int currentPointIndex = 0;
    protected EnemyHealth enemyHealth;

	// Use this for initialization
	void Start () {
        SetUpEnemy();
        pathPoints = IdlePath.path.CalculateSpacedPoints(.1f, 1f);
        transform.position = pathPoints[0];
	}

    protected abstract void SetUpEnemy();

	// Update is called once per frame
	void Update () {

        if (enemyHealth.currentHealth <= 0)
        {
            levelController.startNextScene("Win");
            return;
        }
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 currentTarget = pathPoints[LoopIndex(currentPointIndex)];
        if (currentPos != currentTarget)
        {
            transform.position = Vector2.MoveTowards(currentPos, currentTarget, SpeedMultiplier * Time.deltaTime);
        }
        else
        {
            currentPointIndex++;
        }
	}

    int LoopIndex(int i)
    {
        return (i + pathPoints.Length) % pathPoints.Length;
    }
}
