using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectGUI : MonoBehaviour
{

    [SerializeField] Image levelImage;
    [SerializeField] Text levelName;

    MainMenuManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.sceneList.Count > 0)
        {
            levelName.text = manager.sceneList[manager.sceneIndex];
            levelImage.sprite = LoadScenePreview(manager.sceneList[manager.sceneIndex], (int)levelImage.preferredWidth, (int)levelImage.preferredHeight);
        }
    }

    Sprite LoadScenePreview(string name, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        try
        {
            string path = Application.dataPath + "/LevelPreviews/" + name + ".png";
            if(!System.IO.File.Exists(path))
            {
                path = Application.dataPath + "/LevelPreviews/" + "default" + ".png";
            }
            byte[] byteArray = System.IO.File.ReadAllBytes(path);

            texture.LoadImage(byteArray);
            Debug.Log("Loaded " + path);
        }
        catch(System.IO.IOException e)
        {
            Debug.LogError(e.Message);
        }

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
        return sprite;
    }

    public void OnForwardButton()
    {
        if(manager.sceneList.Count > 0)
        {
            manager.sceneIndex = Mathf.Clamp(manager.sceneIndex + 1, 0, manager.sceneList.Count - 1);
        }
    }

    public void OnBackwardButton()
    {
        if(manager.sceneList.Count > 0)
        {
            manager.sceneIndex = Mathf.Clamp(manager.sceneIndex - 1, 0, manager.sceneList.Count - 1);
        }
    }

    public void OnBackButton()
    {
        manager.state = MenuState.main;
    }

    public void OnPlayButton()
    {
        manager.state = MenuState.loadingInitiate;
    }
}
