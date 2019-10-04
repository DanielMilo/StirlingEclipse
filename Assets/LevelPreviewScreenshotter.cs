using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreviewScreenshotter : MonoBehaviour
{

    Camera camera;
    bool takeScreenshotOnNextFrame;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot(Screen.width, Screen.height);
        }
    }

    void OnPostRender()
    {
        if(takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = camera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            string path = Application.dataPath + "/LevelPreviews/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + ".png";
            System.IO.File.WriteAllBytes(path, byteArray);

            Debug.Log("Screenshot saved to " + path);

            RenderTexture.ReleaseTemporary(renderTexture);
            camera.targetTexture = null;
        }
    }

    void TakeScreenshot(int width, int height)
    {
        camera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }
}
