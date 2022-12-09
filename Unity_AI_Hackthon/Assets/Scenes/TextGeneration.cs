using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.Text;

//https://stackoverflow.com/questions/74326253/curl-request-c-sharp-unity3d
public class TextGeneration : MonoBehaviour
{



    //OpenAIClient api = new OpenAIClient(new OpenAIAuthentication("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"));
    public class OpenAIResponImage
    {
        public int created;
        
        public List<string> data;
    }


    private const string YourApiKey = "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";

    void Start()
    {
        var json = "{\"prompt\": \"A cute baby sea otter in metaverse\",\"n\": 1,\"size\": \"1024x1024\"}";
        StartCoroutine(FillAndSend(json));
    }

    public IEnumerator FillAndSend(string json)
    {
        using (var request = new UnityWebRequest("https://api.openai.com/v1/images/generations", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {YourApiKey}");
            request.SetRequestHeader("Accept", " text/plain");

            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer(); // You can also download directly PNG or other stuff easily. Check here for more information: https://docs.unity3d.com/Manual/UnityWebRequest-CreatingDownloadHandlers.html
            //DownloadHandlerTexture
            //request.downloadHandler = new DownloadHandlerTexture();
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) // Depending on your Unity version, it will tell that these are obsolote. You can try this: request.result != UnityWebRequest.Result.Success
            {
                Debug.LogError(request.error);
                yield break;
            }

            //Debug.Log(request.downloadHandler.text);
            Debug.Log(request.downloadHandler);
            var response = request.downloadHandler.data; // Or you can directly get the raw binary data, if you need.
            //Debug.Log(response);
        }
    }

}
