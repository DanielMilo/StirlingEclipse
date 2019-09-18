using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GhostTracer : MonoBehaviour
{
    public string id = "Anonymous";
    public string app = "StirlingEclipse"; // This should be used as a version indicator
    public string stageID = "testing"; // This should be different in every stage
    GameObject player;

    public string ghostServerHost = "ghost-server-ghostserver.apps.ca-central-1.starter.openshift-online.com";

    private void Start()
    {
        stageID = SceneManager.GetActiveScene().name;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SubmitNewGhost(id, player.transform.position, player.transform.rotation);
        }
    }

    public void SubmitNewGhost(string name, Vector3 position, Quaternion rotation)
    {
        // Create the json
        GhostData data = new GhostData();
        data.position = position;
        data.rotation = rotation;

        string json = JsonUtility.ToJson(data);

        Debug.Log(json);

        StartCoroutine(PostRequest("http://" + ghostServerHost + "/stages/" + stageID + "/ghosts?app=" + app + "&id=" + name, json));
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
