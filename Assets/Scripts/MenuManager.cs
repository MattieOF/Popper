using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int bubbleSpawnCount = 5;
    public TextMeshProUGUI highscoreText;
    public string highscoreTextFormat = "HIGHSCORE: {s}";
    public GameObject main;
    public GameObject options;
    public GameObject optionsButton;

    private void Start()
    {
        // Load gameinfo if needed
        if (GameGlobals.gameInfo is null)
            GameGlobals.gameInfo = GameInfoSerialiser.LoadGameInfo();
        
        // Set highscore text
        highscoreText.text = highscoreTextFormat.Replace("{s}", 
            GameGlobals.gameInfo.highscore.ToString());
        
        // Configure menu objects
        options.SetActive(false);
        main.SetActive(true);
        
        // Update state
        GameGlobals.inMainMenu = true;
        
        // Spawn 10 bubbles
        ObjectSpawner spawner = GetComponent<ObjectSpawner>();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            optionsButton.SetActive(false);
        for (int i = 0; i < bubbleSpawnCount; i++)
            spawner.Spawn();
    }

    public void OpenOptions()
    {
        options.SetActive(true);
        main.SetActive(false);
    }

    public void CloseOptions()
    {
        options.SetActive(false);
        main.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quitting game from menu.");
        Application.Quit();
    }

    public void LoadScene(string scene)
    {
        GameGlobals.inMainMenu = false;
        SceneManager.LoadScene(scene);
    }
}
