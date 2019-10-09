using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreviewScreenshotter : MonoBehaviour
{

    Camera screenshotCamera;
    bool takeScreenshotOnNextFrame;

    // Start is called before the first frame update
    void Start()
    {
        screenshotCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Attempting to take screenshot");
            TakeScreenshot(Screen.width, Screen.height);
        }
    }

    void OnPostRender()
    {
        Debug.Log("onPostRender");
        if(takeScreenshotOnNextFrame)
        {
            
        }
    }

    void TakeScreenshot(int width, int height)
    {
        screenshotCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        screenshotCamera.Render();

        RenderTexture renderTexture = screenshotCamera.targetTexture;

        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderResult.width, renderResult.height);

        RenderTexture.active = renderTexture;
        renderResult.ReadPixels(rect, 0, 0);
        RenderTexture.active = null;

        byte[] byteArray = renderResult.EncodeToPNG();
        string path = Application.dataPath + "/LevelPreviews/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + ".png";

        System.IO.File.WriteAllBytes(path, byteArray);

        Debug.Log("Screenshot saved to " + path);

        RenderTexture.ReleaseTemporary(renderTexture);
        screenshotCamera.targetTexture = null;
    }
}
