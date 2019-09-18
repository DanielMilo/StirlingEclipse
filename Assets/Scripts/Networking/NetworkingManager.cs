using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkingManager : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;

    string sceneName;
    GameObject player;

    public string ghostServerHost = "ghost-server-ghostserver.apps.ca-central-1.starter.openshift-online.com";
    public string app = "StirlingEclipse"; // This should be used as a version indicator

    [HideInInspector] public List<Score> scoreList;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        sceneName = "testing";
        player = GameObject.FindGameObjectWithTag("Player");
        LoadGhosts();
        LoadScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGhosts()
    {
        StartCoroutine(GetGhosts());
    }

    public void LoadScores()
    {
        StartCoroutine(GetScores());
    }

    public void SubmitNewGhost(string name, Vector3 position, Quaternion rotation)
    {
        // Create the json
        GhostData data = new GhostData();
        data.position = position;
        data.rotation = rotation;

        string json = JsonUtility.ToJson(data);

        Debug.Log(json);

        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/ghosts?app=" + app + "&id=" + name;
        StartCoroutine(PostRequest(uri, json));
    }

    public void SubmitNewScore(string name, float time)
    {
        // Create the json
        ScoreData data = new ScoreData();
        data.time = time;

        string json = JsonUtility.ToJson(data);

        Debug.Log(json);

        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/scores?app=" + app + "&id=" + name;
        StartCoroutine(PostRequest(uri, json));
    }

    private IEnumerator GetScores()
    {
        string result = "";
        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/scores?app=" + app;

        yield return StartCoroutine(GetRequest(uri, value => result = value));

        if(!string.IsNullOrEmpty(result))
        {
            Debug.Log("Received Scores: " + result);
            Scores newGhosts = JsonUtility.FromJson<Scores>(result);
            scoreList = new List<Score>();
            foreach(Score s in newGhosts.scores)
            {
                scoreList.Add(s);
            }
            scoreList.Sort((x, y) => x.scoreData.time.CompareTo(y.scoreData.time));
        }
    }

    private IEnumerator GetGhosts()
    {
        string result = "";
        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/ghosts?app=" + app;

        yield return StartCoroutine(GetRequest(uri, value => result = value));

        if(!string.IsNullOrEmpty(result))
        {
            Debug.Log("Received Ghosts: " + result);
            Ghosts newGhosts = JsonUtility.FromJson<Ghosts>(result);
            foreach(Ghost g in newGhosts.ghosts)
            {
                SpawnGhost(g);
            }
        }
    }

    private void SpawnGhost(Ghost g)
    {
        GameObject newGhost = Instantiate(ghostPrefab, transform);
        newGhost.transform.position = g.ghostData.position;
        newGhost.transform.rotation = g.ghostData.rotation;

        newGhost.name = g.inserterID;
    }

    private IEnumerator GetRequest(string uri, System.Action<string> result)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if(uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            result(null);
        }
        else
        {
            result(uwr.downloadHandler.text);
        }
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

        if(uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
