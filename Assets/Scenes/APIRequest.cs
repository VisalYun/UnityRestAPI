using System;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIRequest : MonoBehaviour
{
    string url = "https://jsonplaceholder.typicode.com/posts";

    public bool useThread;

    // Start is called before the first frame update
    void Start()
    {
        if(useThread){
            Thread requestThread = new Thread(new ThreadStart(ThreadRequest));
            requestThread.Start();
        }
        else{
            StartCoroutine(Request());
        }
        Debug.Log("I'm Running first!");
    }

    IEnumerator Request()
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.Send();
        while (!req.isDone)
            yield return null;
        byte[] result = req.downloadHandler.data;
        string jsonResponse = System.Text.Encoding.Default.GetString(result);
            
        List<PostInfo> postList = JsonHelper.FromJson<PostInfo>(jsonResponse);
        foreach(var post in postList){
            string temp = "userId: " + post.userId + ", id: " + post.id + ", title: " + post.title + ", body: " + post.body;
            Debug.Log(temp);
        }
    }

    void ThreadRequest()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();

        List<PostInfo> postList = JsonHelper.FromJson<PostInfo>(jsonResponse);
        foreach(var post in postList){
            string temp = "userId: " + post.userId + ", id: " + post.id + ", title: " + post.title + ", body: " + post.body;
            Debug.Log(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[Serializable]
public class PostInfo
{
    public int userId;
    public int id;
    public string title;
    public string body;
}


