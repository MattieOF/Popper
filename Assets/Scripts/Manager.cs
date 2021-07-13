using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour
{
    public ObjectSpawner   bubbleSpawner;
    public Player          player;
    public TextMeshProUGUI bubbleCountText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gameOverBubbleCountText;
    public GameObject      gameOverHighscoreText;
    public TextMeshProUGUI gameOverTimeText;
    public GameObject      gameOverUI;
    public GameObject      pauseMenu;

    public float           bubbleSpawnRate   = 5f;
    public int             bubbleCount       = 0;

    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    private void Start()
    {
        GameGlobals.menuOpen = false;
        StartGame();
        RefreshBubbleCountText();
        gameOverUI.SetActive(false);
        gameOverHighscoreText.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void StartGame()
    {
        GameGlobals.alive = true;
        stopwatch.Start();
        StartSpawning();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameGlobals.menuOpen) Quit();
            else ShowPauseMenu();
        }

        timeText.text = stopwatch.Elapsed.ToString("g").Split('.')[0];
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        GameGlobals.menuOpen = true;
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        GameGlobals.menuOpen = false;
    }

    public void Quit()
    {
        Debug.Log("Quitting game!");
        Application.Quit();
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }

    public void StartSpawning()
    {
        InvokeRepeating("SpawnBubble", 0, bubbleSpawnRate);
    }

    public void SpawnBubble()
    {
        if (GameGlobals.menuOpen) return;
        bubbleSpawner.Spawn();
    }

    public void RefreshBubbleCountText()
    {
        bubbleCountText.text = bubbleCount.ToString();
    }

    public void AddBubble()
    {
        bubbleCount++;
        RefreshBubbleCountText();
    }

    public void ShowGameOverUI()
    {
        HidePauseMenu();
        gameOverUI.SetActive(true);
        GameGlobals.menuOpen = true;
    }

    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
        GameGlobals.menuOpen = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void DestroyAllBubbles()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Bubble"))
            Destroy(go);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Particle"))
            Destroy(go);
    }

    public void Restart()
    {
        GameGlobals.alive = true;
        bubbleCount = 0;
        DestroyAllBubbles();
        RefreshBubbleCountText();
        player.Restart();
        StartSpawning();
        gameOverUI.SetActive(false);
        HideGameOverUI();
        stopwatch.Restart();
    }

    public void GameOver()
    {
        GameGlobals.alive = false;
        stopwatch.Stop();
        gameOverTimeText.text = stopwatch.Elapsed.ToString("g").Split('.')[0];
        if (GameGlobals.gameInfo.bestTime < stopwatch.Elapsed.TotalSeconds)
            GameGlobals.gameInfo.bestTime = stopwatch.Elapsed.TotalSeconds;
        gameOverHighscoreText.SetActive(false);
        if (GameGlobals.gameInfo.highscore < bubbleCount)
        {
            GameGlobals.gameInfo.highscore = bubbleCount;
            gameOverHighscoreText.SetActive(true);
        }
        gameOverBubbleCountText.text = bubbleCount.ToString();
        StopSpawning();
        bubbleCount = 0;
        GameInfoSerialiser.SaveGameInfo(GameGlobals.gameInfo);
        ShowGameOverUI();
    }
}
