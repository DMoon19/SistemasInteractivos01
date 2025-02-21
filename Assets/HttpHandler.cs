using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpHandler : MonoBehaviour
{
    [SerializeField]
    private string url = "https://rickandmortyapi.com/api/character/";
    
    IEnumerator GetCharacter(int id)
    {
        UnityWebRequest www = UnityWebRequest.Get(url+"/"+id);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.responseCode == 200)
            {
              
                Personaje personaje = JsonUtility.FromJson<Personaje>(www.downloadHandler.text);

              //  StartCoroutine(GetImage(personaje.image));
             
             
            }
            else
            {
                string mensaje = "status:" + www.responseCode;
                mensaje += "\nErro: " + www.error;
                Debug.Log(mensaje);
            }
        }
    }
    // IEnumerator GetImage(string imageUrl)
    // {
    //     UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
    //     yield return request.SendWebRequest();
    //     if (request.result == UnityWebRequest.Result.ConnectionError)
    //     {
    //         Debug.Log(request.error);
    //     }
    //     else
    //     {
    //         var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    //         picture.texture = texture;
    //     }
    // }
}
