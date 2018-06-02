using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpController : MonoBehaviour {

    public float moveSpeed;
    public float pushBack;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
