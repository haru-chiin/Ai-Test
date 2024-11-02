using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using OpenAI;

public class OpenAITextToSpeech : MonoBehaviour
{
    public AudioSource audioSource;
    private OpenAIApi openAI = new OpenAIApi();
    private string apiKey = "OPEN AI API"; // Ganti dengan API key OpenAI Anda
    private string openAiUrl = "https://api.openai.com/v1/audio/speech"; // Ganti dengan URL endpoint text-to-speech dari OpenAI

    public void GenerateSpeech(string text)
    {
        StartCoroutine(SendTextToOpenAI(text));
    }

    private IEnumerator SendTextToOpenAI(string inputText)
    {
        var requestData = new RequestData
        {
            model = "tts-1",
            voice = "alloy",
            input = inputText
        };

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log("JSON Data: " + jsonData);
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        // Buat request HTTP POST
        UnityWebRequest request = new UnityWebRequest(openAiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("JSON Data: " + jsonData);
        Debug.Log(apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Ambil hasil audio sebagai data byte
            byte[] audioData = request.downloadHandler.data;

            // Simpan file audio sementara untuk di-load oleh Unity
            string path = Path.Combine(Application.persistentDataPath, "speech.mp3");
            File.WriteAllBytes(path, audioData);

            // Load dan mainkan audio file
            StartCoroutine(PlayAudioFromFile(path));
        }
        else
        {
            Debug.LogError("Failed to generate audio: " + request.error);
        }
    }

    private IEnumerator PlayAudioFromFile(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Failed to load audio: " + www.error);
            }
        }
    }
}

[System.Serializable]
public class RequestData
{
    public string model;
    public string voice;
    public string input;
}