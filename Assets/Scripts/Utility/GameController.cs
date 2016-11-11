using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public enum GameMode { Normal = 0, BulletHell = 1 };

    public Text scoreTextObject; 

    public int maxNumberOfEnemyFormations;
    public int numberOfEnemyFormations;

    public float bossAppearanceInterval;
	public AudioClip bossEntranceAudio;

    public GameObject[] bulletHellActiveObjects;
    public GameObject[] objectsToDeactivate;
    public GameObject[] objectsToActivate;

    private static GameMode gameMode = GameMode.Normal;
	private AudioSource audioSource;

    private int score = 0;
    private float bossAppearanceTimer;
    private bool bossAppeared = false;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);

        // Setup variables
        bossAppearanceTimer = bossAppearanceInterval;

		audioSource = GameObject.FindGameObjectWithTag("MainAudioSource").GetComponent<AudioSource>();

        // Turn on bullet hell mode objects
        if (gameMode == GameMode.BulletHell)
        {
            // Bullets hit the player in bullet hell mode
            Bullet.hitsPlayer = true;

            foreach (GameObject go in bulletHellActiveObjects)
                go.SetActive(true);
        }
    }

    void Update()
    {
        if (!bossAppeared)
        {
            bossAppearanceTimer -= Time.deltaTime;

            if (bossAppearanceTimer <= 0)
            {

				StartCoroutine(BossEntrance ());
            }
        }
    }

	private IEnumerator BossEntrance()
	{
		audioSource.PlayOneShot (bossEntranceAudio);

		bossAppeared = true;

		yield return new WaitForSeconds (5);

		foreach (GameObject go in objectsToDeactivate)
			go.SetActive(false);

		foreach (GameObject go in objectsToActivate)
			go.SetActive(true);
	}

    public void IncreaseScore(int add)
    {
        score += add;

        // Set the displayed text to be the word "Score" followed by the score value.
        scoreTextObject.text = "Score: " + score;
    }

	public void BonusScore(int bonus)
	{
		score *= bonus;

		// Set the displayed text to be the word "Score" followed by the score value.
		scoreTextObject.text = "Score: " + score;
	}

    public void SetGameMode(int mode)
    {
        gameMode = (GameMode) mode;
    }

    public void WaitAndReload(float delayInterval)
    {
        StartCoroutine(WaitAndReloadCoroutine(delayInterval));
    }

    private static IEnumerator WaitAndReloadCoroutine(float delayInterval)
    {
        yield return new WaitForSeconds(delayInterval);
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}
