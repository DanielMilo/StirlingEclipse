﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GhostCollector : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    public string app = "StirlingEclipse"; // This should be used as a version indicator
    public string stageID = "testing"; // This should be different in every stage

    public string ghostServerHost = "ghost-server-ghostserver.apps.ca-central-1.starter.openshift-online.com";
    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        stageID = SceneManager.GetActiveScene().name;
        StartCoroutine(GetRequest("http://" + ghostServerHost + "/stages/" + stageID + "/ghosts?app=" + app));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            string s = uwr.downloadHandler.text;
            //Debug.Log("Received: " + s);

            Ghosts newGhosts = JsonUtility.FromJson<Ghosts>(s);
            foreach (Ghost g in newGhosts.ghosts)
            {
                GameObject newGhost = Instantiate(ghostPrefab, transform);
                newGhost.transform.position = g.ghostData.position;
                newGhost.transform.rotation = g.ghostData.rotation;

                newGhost.name = g.inserterID;
            }
        }
    }
}
