using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resDropdown;
    public Toggle       fullscreenToggle;
    public bool         initialising = true;

    private void Start()
    {
        initialising = true;
        SetResToSaved();
        SetResolutionDropdowns();
        initialising = false;
    }

    public void SetResToSaved()
    {
        Screen.SetResolution(GameGlobals.gameInfo.videoSettings.width, GameGlobals.gameInfo.videoSettings.height,
                             GameGlobals.gameInfo.videoSettings.fullscreen, GameGlobals.gameInfo.videoSettings.refreshRate);
    }

    public void SetResolutionDropdowns()
    {
        resDropdown.ClearOptions();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resDropdown.options.Add(new TMP_Dropdown.OptionData($"{Screen.resolutions[i].width}x{Screen.resolutions[i].height}/{Screen.resolutions[i].refreshRate}hz"));
            if (GameGlobals.gameInfo.videoSettings.width == Screen.resolutions[i].width && GameGlobals.gameInfo.videoSettings.height == Screen.resolutions[i].height &&
                GameGlobals.gameInfo.videoSettings.refreshRate == Screen.resolutions[i].refreshRate)
            {
                resDropdown.value = i;
            }
        }
        fullscreenToggle.isOn = GameGlobals.gameInfo.videoSettings.fullscreen;
    }

    public void UpdateResolution()
    {
        if (initialising) return;
        Resolution newRes = Screen.resolutions[resDropdown.value];
        Screen.SetResolution(newRes.width, newRes.height, fullscreenToggle.isOn, newRes.refreshRate);
        GameGlobals.gameInfo.videoSettings = new VideoSettings
        {
            width = newRes.width,
            height = newRes.height,
            refreshRate = newRes.refreshRate,
            fullscreen = fullscreenToggle.isOn
        };
        GameInfoSerialiser.SaveGameInfo(GameGlobals.gameInfo);
    }

    public void UpdateFullscreen()
    {
        if (initialising) return;
        Screen.fullScreen = fullscreenToggle.isOn;
        GameGlobals.gameInfo.videoSettings.fullscreen = fullscreenToggle.isOn;
        GameInfoSerialiser.SaveGameInfo(GameGlobals.gameInfo);
    }
}
