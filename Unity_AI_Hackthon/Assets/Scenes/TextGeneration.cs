using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using System.Threading.Tasks;
using System;
public class TextGeneration : MonoBehaviour
{

    // Start is called before the first frame update
    async void Start()
    {
        OpenAIClient api = new OpenAIClient(new OpenAIAuthentication("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"));
        var result = await api.CompletionEndpoint.CreateCompletionAsync("I want to be a", temperature: 0.1, engine: Engine.Davinci);
        print(result);
        
    }

    
}
