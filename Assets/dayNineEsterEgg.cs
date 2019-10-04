using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dayNineEsterEgg : MonoBehaviour
{
    int counter = 1;
    byte[] whiteBackup;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        Texture2D texture = Texture2D.whiteTexture;
        whiteBackup = texture.EncodeToPNG();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            counter++;
            if(counter % 2 == 0)
            {
                if(!isActive)
                {
                    ActivateEasteregg();
                }
            }
            else if(isActive)
            {
                DeactivateEasteregg();
            }
        }
    }

    void ActivateEasteregg()
    {
        isActive = true;

        Texture2D texture = Texture2D.whiteTexture;
        whiteBackup = texture.EncodeToPNG();
        try
        {
            string path = Application.dataPath + "/LevelPreviews/" + "day9" + ".png";
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
        /*
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
        */
    }

    void DeactivateEasteregg()
    {
        isActive = false;

        Texture2D texture = Texture2D.whiteTexture;
        texture.LoadImage(whiteBackup);
    }
}
