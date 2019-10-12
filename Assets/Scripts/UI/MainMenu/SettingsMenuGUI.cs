using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuGUI : MonoBehaviour
{
    MainMenuManager manager;
    [SerializeField] AudioMixer mainMixer;
    AudioMixer musicMixer;
    AudioMixer effectsMixer;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;

    [SerializeField] Dropdown resolutionDropdown;

    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        musicMixer = mainMixer.FindMatchingGroups("Music")[0].audioMixer;
        effectsMixer = mainMixer.FindMatchingGroups("Sound Effects")[0].audioMixer;

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuManager>();

        SetupVolumeSliders();
        SetupResolutionDropdown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupVolumeSliders()
    {
        if(manager.fileManager.data != null)
        {
            mainMixer.SetFloat("masterVolume", manager.fileManager.data.masterVolume);
            masterSlider.value = manager.fileManager.data.masterVolume;

            musicMixer.SetFloat("musicVolume", manager.fileManager.data.musicVolume);
            musicSlider.value = manager.fileManager.data.musicVolume;

            effectsMixer.SetFloat("effectsVolume", manager.fileManager.data.effectsVolume);
            effectsSlider.value = manager.fileManager.data.effectsVolume;
        }
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
        manager.fileManager.SaveData();
        manager.state = MenuState.main;
    }

    public void SetMasterVolume(Slider s)
    {
        mainMixer.SetFloat("masterVolume", s.value);
        manager.fileManager.data.masterVolume = s.value;
    }

    public void SetMusicVolume(Slider s)
    {
        mainMixer.FindMatchingGroups("Music")[0].audioMixer.SetFloat("musicVolume", s.value);
        manager.fileManager.data.musicVolume = s.value;
    }

    public void SetEffectsVolume(Slider s)
    {
        mainMixer.FindMatchingGroups("Sound Effects")[0].audioMixer.SetFloat("effectsVolume", s.value);
        manager.fileManager.data.effectsVolume = s.value;
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
