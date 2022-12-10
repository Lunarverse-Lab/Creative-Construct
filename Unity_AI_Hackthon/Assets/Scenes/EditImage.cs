using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using TreeEditor;
using UnityEditor.PackageManager.Requests;
using LitJson;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;
using System.IO;


//https://stackoverflow.com/questions/74326253/curl-request-c-sharp-unity3d
public class EditImage : MonoBehaviour
{

    public string inputText;
    public Texture2D texture;

    public Texture2D imageAsset;
    public Texture2D maskAsset;

    private const string YourApiKey = "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";

    public class ImageGeneratedParameter
    {
        public Texture2D image;
        public Texture2D mask;
        public string prompt;
        public int n;
        public string size;
    }

    public ImageGeneratedParameter imageGeneration = new ImageGeneratedParameter();
    public RawImage textureImage;

    void Start()
    {
        //imageGeneration.prompt = "Using Hololens to enter Hyper-Connected Metaverse in Toronto";
        imageGeneration.image = imageAsset;
        imageGeneration.mask = maskAsset;
        imageGeneration.prompt = inputText;
        imageGeneration.n = 1;
        imageGeneration.size = "1024x1024";
        //var json = "{\"prompt\": \"A cute baby sea otter in metaverse\",\"n\": 2,\"size\": \"1024x1024\"}";
        StartCoroutine(FillAndSend(JsonMapper.ToJson(imageGeneration)));
        texture = new Texture2D(1024, 1024);
    }

    public IEnumerator FillAndSend(string json)
    {
        using (var request = new UnityWebRequest("https://api.openai.com/v1/images/edits", "POST"))
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
            Debug.Log(request.downloadHandler.text);
            //var imageData = JsonUtility.FromJson<OpenAIResponImage>(request.downloadHandler.text).data;
            JsonData jsondata = JsonMapper.ToObject(request.downloadHandler.text);
            Debug.Log(jsondata["data"][0]["url"]);

            //URL Image


            WWW www = new WWW((string)jsondata["data"][0]["url"]);
            yield return www;
            www.LoadImageIntoTexture(texture);
            www.Dispose();
            www = null;

            textureImage.texture = texture;



        }
    }

}
