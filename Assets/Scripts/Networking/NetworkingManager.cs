using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkingManager : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] int maxNumberOfGhosts;

    string sceneName;
    GameObject player;

    public string ghostServerHost = "ghost-server-ghostserver.apps.ca-central-1.starter.openshift-online.com";
    public string app = "StirlingEclipse"; // This should be used as a version indicator

    [HideInInspector] public List<Score> scoreList;
    [HideInInspector] public Score playerScore;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
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

    public void AddPlayerScore(float timer, bool submitOnline)
    {
        if(submitOnline)
        {
            SubmitNewScore(player.name, timer);
        }

        Score s = new Score();
        s.inserterID = player.name;
        s.scoreData.time = timer;
        playerScore = s;
        scoreList.Add(s);
        scoreList.Sort((x, y) => x.scoreData.time.CompareTo(y.scoreData.time));
    }

    public void SubmitNewGhost(string name, Vector3 position, Quaternion rotation)
    {
        // Create the json
        GhostData data = new GhostData();
        data.position = position;
        data.rotation = rotation;

        string json = JsonUtility.ToJson(data);

        //Debug.Log(json);

        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/ghosts?app=" + app + "&id=" + name;
        StartCoroutine(PostRequest(uri, json));
    }

    public void SubmitNewScore(string name, float time)
    {
        // Create the json
        ScoreData data = new ScoreData();
        data.time = time;

        string json = JsonUtility.ToJson(data);

        //Debug.Log(json);

        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/scores?app=" + app + "&id=" + name;
        StartCoroutine(PostRequest(uri, json));
    }

    List<Score> CutDownScores(List<Score> input)
    {
        input.Sort((x, y) => x.scoreData.time.CompareTo(y.scoreData.time));
        List<string> keysSeen = new List<string>();

        List<Score> output = new List<Score>();
        foreach(Score score in input)
        {
            if(!keysSeen.Contains(score.inserterID))
            {
                keysSeen.Add(score.inserterID);
                output.Add(score);
            }
        }

        return output;
    }

    List<Ghost> CutDownGhosts(List<Ghost> input, int maxNumber)
    {
        if(input.Count <= maxNumber)
        {
            return input;
        }

        Dictionary<string, List<Ghost>> sortedByName = new Dictionary<string, List<Ghost>>();

        foreach(Ghost g in input)
        {
            if(sortedByName.ContainsKey(g.inserterID))
            {
                sortedByName[g.inserterID].Add(g);
            }
            else
            {
                List<Ghost> newList = new List<Ghost>();
                newList.Add(g);
                sortedByName.Add(g.inserterID, newList);
            }
        }

        string[] keys = new string[sortedByName.Keys.Count];
        sortedByName.Keys.CopyTo(keys, 0);

        List<Ghost> output = new List<Ghost>();

        int[] randomIndexes = GetRandomIndexes(maxNumber, keys.Length);
        //int ghostListIndex = 0;
        while(output.Count < maxNumber)
        {
            foreach(int i in randomIndexes)
            {
                List<Ghost> ghostListForName = sortedByName[keys[i]];

                if(ghostListForName.Count > 0)
                {
                    Ghost g = ghostListForName[Random.Range(0, ghostListForName.Count)];
                    output.Add(g);
                    ghostListForName.Remove(g);

                    if(output.Count >= maxNumber)
                    {
                        break;
                    }
                }

                /*
                if(ghostListIndex < ghostListForName.Count)
                {
                    output.Add(ghostListForName[ghostListIndex]);
                    if(output.Count >= maxNumber)
                    {
                        break;
                    }
                }
                */
            }
            //ghostListIndex++;
        }

        return output;
    }

    int[] GetRandomIndexes(int count, int totalElements)
    {
        if(totalElements < count)
        {
            count = totalElements;
        }
        List<int> allIndexes = new List<int>();
        int[] output = new int[count];

        for(int x = 0; x < totalElements; x++)
        {
            allIndexes.Add(x);
        }

        for(int x = 0; x < count; x++)
        {
            int randomIndex = (int)Random.Range(0, allIndexes.Count);
            output[x] = allIndexes[randomIndex];
            allIndexes.RemoveAt(randomIndex);
        }

        return output;
    }

    private void SpawnGhost(Ghost g)
    {
        GameObject newGhost = Instantiate(ghostPrefab, transform);
        newGhost.transform.position = g.ghostData.position;
        newGhost.transform.rotation = g.ghostData.rotation;

        newGhost.name = g.inserterID;
    }

    private IEnumerator GetScores()
    {
        string result = "";
        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/scores?app=" + app;

        yield return StartCoroutine(GetRequest(uri, value => result = value));

        if(!string.IsNullOrEmpty(result))
        {
            Debug.Log(result);
            Scores newGhosts = JsonUtility.FromJson<Scores>(result);
            scoreList = new List<Score>();
            foreach(Score s in newGhosts.scores)
            {
                scoreList.Add(s);
            }
            scoreList = CutDownScores(scoreList);
        }
    }

    private IEnumerator GetGhosts()
    {
        string result = "";
        string uri = "http://" + ghostServerHost + "/stages/" + sceneName + "/ghosts?app=" + app;

        List <Ghost> ghostList = new List<Ghost>();

        yield return StartCoroutine(GetRequest(uri, value => result = value));

        if(!string.IsNullOrEmpty(result))
        {
            Ghosts newGhosts = JsonUtility.FromJson<Ghosts>(result);
            foreach(Ghost g in newGhosts.ghosts)
            {
                ghostList.Add(g);
            }

            ghostList = CutDownGhosts(ghostList, maxNumberOfGhosts);
            foreach(Ghost g in ghostList)
            {
                SpawnGhost(g);
            }
        }
    }

    private IEnumerator GetRequest(string uri, System.Action<string> result)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        Debug.Log(uri);
        yield return uwr.SendWebRequest();

        if(uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            result(null);
        }
        else
        {
            if(uwr.isHttpError)
            {
                Debug.Log("isHTTPError");
            }
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
            //Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
