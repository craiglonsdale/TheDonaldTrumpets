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
    private bool transitioning;
	// Use this for initialization
	void Start () {
        transitioning = false;
	}
	
    public void startNextScene(string sceneName)
    {
        if (!transitioning)
        {
            nextScene = sceneName;
            transitioning = true;
        }
    }

	// Update is called once per frame
	void Update () {
        if (transitioning)
        {
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
