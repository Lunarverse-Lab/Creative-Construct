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
using TMPro;

public class SentenceGeneration : MonoBehaviour
{
    public string inputText;
    public GameObject prefebText;
    //public Texture2D texture;

    private const string YourApiKey = "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";

    public class ImageGeneratedParameter
    {
        public string model;
        public string prompt;
        //public int n;
        //public float temperature;

        public int max_tokens;

    }

    public ImageGeneratedParameter imageGeneration = new ImageGeneratedParameter();
    //public RawImage textureImage;
    //public TMP_InputField input;


    void Start()
    {
        //imageGeneration.prompt = "Using Hololens to enter Hyper-Connected Metaverse in Toronto";
        imageGeneration.model = "text-davinci-003";
        //imageGeneration.temperature = (float)0.9;
        //imageGeneration.n = 5;
        //imageGeneration.prompt = "Generate five keywords for \"" +  inputText + "\" in json format";
        imageGeneration.max_tokens = 50;



    }

    public void generateSentenceFunction()
    {
        inputText = GetComponent<TMP_Text>().text;
        imageGeneration.prompt = "Generate a sentence from \"" + inputText + "\"";
        StartCoroutine(FillAndSend(JsonMapper.ToJson(imageGeneration)));

    }

    public IEnumerator FillAndSend(string json)
    {
        using (var request = new UnityWebRequest("https://api.openai.com/v1/completions", "POST"))
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
            //Debug.Log(request.downloadHandler.text);
            //var imageData = JsonUtility.FromJson<OpenAIResponImage>(request.downloadHandler.text).data;
            JsonData jsondata = JsonMapper.ToObject(request.downloadHandler.text);


            //
            //var arrayKeywords = jsondata;
            //Debug.Log(request.downloadHandler.text);
            //Debug.Log(arrayKeywords[0]);
            Debug.Log(jsondata["choices"][0]["text"]);
            string arrayKeywords = (string)jsondata["choices"][0]["text"];

            
           
                Debug.Log(arrayKeywords);
                GameObject textObject = Instantiate(prefebText, transform);
                textObject.GetComponent<TMP_Text>().text = arrayKeywords;
            
            /*
            var responededKeywords = jsondata["choices"][0]["text"]["Keywords"];
            for (int i =0;i< responededKeywords.Count; i++)
            {
                Debug.Log(responededKeywords[i]);
            }
            */






        }
    }
    void OnMouseDown()
    {
        // Destroy the gameObject after clicking on it
        generateSentenceFunction();
    }
}
