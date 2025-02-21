using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendWebRequest : MonoBehaviour
{   
    [SerializeField]
    private RawImage picture;
    [SerializeField]
    private string url = "https://rickandmortyapi.com/api/character/";
    public void SendRequest()
    {
        StartCoroutine(GetCharacter(56));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

                StartCoroutine(GetImage(personaje.image));
             
             
            }
            else
            {
                string mensaje = "status:" + www.responseCode;
                mensaje += "\nErro: " + www.error;
                Debug.Log(mensaje);
            }
        }
    }

    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
 
        if(www.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log(www.error);
        }
        else {
            if (www.responseCode == 200)
            {
                Debug.Log(www.error);
                ListaDePersonajes personajes = JsonUtility.FromJson<ListaDePersonajes>(www.downloadHandler.text);
                foreach (var personaje in personajes.results)
                {
                    Debug.Log($"{personaje.id} : {personaje.name} es un {personaje.species}");
                }
                
            }
            else
            {
                string mensaje = "status:" + www.responseCode;
                mensaje += "\nError: " + www.error;
                Debug.Log(mensaje);
            }
        }
        
    }
    IEnumerator GetImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            picture.texture = texture;
        }
    }

}


[System.Serializable]
public class Personaje
{
    public string id;
    public string name;
    public string species;
    public string image;
}
[System.Serializable]
public class ListaDePersonajes
{
    public Personaje[] results;
}
