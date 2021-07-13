using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int bubbleSpawnCount = 5;
    public GameObject main;
    public GameObject options;
    public GameObject optionsButton;

    private void Start()
    {
        GameGlobals.gameInfo = GameInfoSerialiser.LoadGameInfo();
        options.SetActive(false);
        main.SetActive(true);
        GameGlobals.inMainMenu = true;
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
