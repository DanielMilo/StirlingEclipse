using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuGUI : MonoBehaviour
{
    MainMenuManager manager;
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] Dropdown resolutionDropdown;

    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuManager>();

        SetupResolutionDropdown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionNames = new List<string>();

        int currentResIndex = 0;
        for(int index = 0; index < resolutions.Length; index++)
        {
            string s = resolutions[index].width + " x " + resolutions[index].height;
            resolutionNames.Add(s);

            if(Screen.width == resolutions[index].width && Screen.height == resolutions[index].height)
            {
                currentResIndex = index;
            }
        }

        resolutionDropdown.AddOptions(resolutionNames);
        if(Screen.fullScreen)
        {
            resolutionDropdown.value = resolutions.Length - 1;
        }
        else
        {
            resolutionDropdown.value = currentResIndex;
        }
        
        resolutionDropdown.RefreshShownValue();

        SetResolution();
    }

    public void OnBackButton()
    {
        manager.state = MenuState.main;
    }

    public void SetVolume(Slider s)
    {
        mainMixer.SetFloat("masterVolume", s.value);
    }

    public void SetFullscreen(bool b)
    {
        Screen.fullScreen = b;
    }

    public void SetResolution()
    {
        int index = resolutionDropdown.value;
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }
}
