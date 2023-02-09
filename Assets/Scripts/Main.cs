using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class Main : MonoBehaviour {

    private const string API_URI = "https://opentdb.com/api.php?amount=";
    private int amount = 10;

    public QuestionList requestResult;
    
    void Start() {
        StartCoroutine(GetRequest(API_URI + amount));
    }

    IEnumerator GetRequest(string uri) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {

            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    requestResult = JsonUtility.FromJson<QuestionList>(webRequest.downloadHandler.text);
                    Debug.Log(ComposeMessage(requestResult));
                    break;
            }
        }
    }

    private string ComposeMessage(QuestionList questionList) {
        
        // Compoñemos toda a mensaxe nun mesmo string para non colapsar a vista de consola
        
        string composedMessage = $"Preguntas solicitadas: {amount}. (Preguntas recibidas: {questionList.results.Count}\n"; 

        if (questionList.results.Count > 0) {
            Question question = questionList.results[0];
            composedMessage += $"Pregunta: {question.question}\n" 
                + $"Resposta correcta: {question.correct_answer}\n"
                + $"Respostas incorrectas: {String.Join(", ", question.incorrect_answers)}\n"
                + $"Categoría: {question.category}";
        }

        return composedMessage;
    }

}
