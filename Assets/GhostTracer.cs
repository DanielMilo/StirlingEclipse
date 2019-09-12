using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GhostTracer : MonoBehaviour
{
    public string id = "Witty";
    public string app = "StirlingEclipse";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Create the json
            GhostData data = new GhostData();
            data.position = transform.position;
            data.rotation = transform.rotation;

            string json = JsonUtility.ToJson(data);

            Debug.Log(json);

            StartCoroutine(PostRequest("http://localhost:9443/ghosts?app=" + app + "&id=" + id, json));
        }
    }

    private void Start()
    {
        
    }

    IEnumerator PostRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
