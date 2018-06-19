using UnityEngine;

public class TrumpController : MonoBehaviour {

    public float moveSpeed;
    public float pushBack;
    public LevelController levelController;

    TrumpHealth trumpHealth;

    // Use this for initialization
    void Start()
    {
        var trump = GameObject.FindGameObjectWithTag("Trump");
        trumpHealth = trump.GetComponent<TrumpHealth>();
    }

    // Update is called once per frame
    void Update () {
        Camera cam = Camera.main;
        float height = cam.orthographicSize;
        float width = height * cam.aspect;
        float trumpWidth = GetComponent<PolygonCollider2D>().bounds.size.x * 0.5f;
        float trumpHeight = GetComponent<PolygonCollider2D>().bounds.size.y * 0.5f;
        Debug.Log((transform.position.y + trumpHeight));
        if (trumpHealth.currentHealth <= 0)
        {
            levelController.startNextScene("Death");
            return;
        }

        var rawHorizontalInput = Input.GetAxisRaw("Horizontal");
        var rawVerticalInput = Input.GetAxisRaw("Vertical");
        var leftBoundsXCheck = rawHorizontalInput < .5f && (transform.position.x - trumpWidth) >= -width;
        var rightBoundsXCheck = rawHorizontalInput > .5f && (transform.position.x + trumpWidth) <= width;
        var bottomBoundsYCheck = rawVerticalInput < .5f && (transform.position.y - trumpHeight) >= -height;
        var topBoundsYCheck = rawVerticalInput > .5f && (transform.position.y + trumpHeight) <= height;

        if (leftBoundsXCheck || rightBoundsXCheck)
        {
            transform.Translate(new Vector3(rawHorizontalInput * (moveSpeed) * Time.deltaTime, 0f, 0f));
        }

        if (topBoundsYCheck || bottomBoundsYCheck)
        {
            transform.Translate(new Vector3(0f, rawVerticalInput  * (moveSpeed ) * Time.deltaTime, 0f));
        }

        //transform.Translate(new Vector3(pushBack * Time.deltaTime, 0f, 0f));
    }
}
