using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GhostCollector : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        StartCoroutine(GetRequest("http://danielsworkshop.ddns.net:9443/ghosts"));
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
            Debug.Log("Received: " + s);

            Ghosts newGhosts = JsonUtility.FromJson<Ghosts>(s);
            foreach (Ghost g in newGhosts.ghosts)
            {
                GameObject newGhost = Instantiate(ghostPrefab);
                newGhost.transform.position = g.ghostData.position;
                newGhost.transform.rotation = g.ghostData.rotation;

                newGhost.name = g.inserterID;
            }
        }
    }
}
