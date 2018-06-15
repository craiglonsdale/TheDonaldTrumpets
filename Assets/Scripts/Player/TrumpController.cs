using UnityEngine;

public class TrumpController : MonoBehaviour {

    public float moveSpeed;
    public float pushBack;

    TrumpHealth trumpHealth;

    // Use this for initialization
    void Start()
    {
        var trump = GameObject.FindGameObjectWithTag("Trump");
        trumpHealth = trump.GetComponent<TrumpHealth>();
    }

    // Update is called once per frame
    void Update () {

        if (trumpHealth.currentHealth <= 0)
        {
            return;
        }

        var rawHorizontalInput = Input.GetAxisRaw("Horizontal");
        var rawVerticalInput = Input.GetAxisRaw("Vertical");

        if (rawHorizontalInput > .5f || rawHorizontalInput < .5f)
        {
            transform.Translate(new Vector3(rawHorizontalInput * (moveSpeed) * Time.deltaTime, 0f, 0f));
        }

        if (rawVerticalInput > .5f || rawVerticalInput < .5f)
        {
            transform.Translate(new Vector3(0f, rawVerticalInput  * (moveSpeed ) * Time.deltaTime, 0f));
        }

        transform.Translate(new Vector3(pushBack * Time.deltaTime, 0f, 0f));
    }
}
