using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public Image fadeImage;
    public Animator fadeAnimator;

    public TrumpHealth playerHealth;
    public EnemyHealth enemyHealth;

    private string nextScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth.currentHealth <= 0)
        {
            nextScene = "Death";
            StartCoroutine(Fading());
        }
        if (enemyHealth.currentHealth <= 0)
        {
            nextScene = "Win";
            StartCoroutine(Fading());
        }
	}

    IEnumerator Fading()
    {
        fadeAnimator.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        SceneManager.LoadScene(nextScene);
    }
}
