using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
   [SerializeField] 
   private RawImage[] _images;
   [SerializeField]
   private string dB = "https://my-json-server.typicode.com/dmoon19/sistemasinteractivos01";
   [SerializeField]
   private string apiUrl = "https://rickandmortyapi.com/api/character/";
   [SerializeField]
   private TMP_Text[] textObjects; // Arreglo para los objetos TMP_Text.
   [SerializeField]
   private TMP_Dropdown dropdown;
    public void SendRequest()
    {
        int id = dropdown.value;
        Debug.Log(id);
     StartCoroutine("GetUser" , id+1);
    }
    
    IEnumerator GetUser(int id)
    {
     UnityWebRequest request = UnityWebRequest.Get(dB +"/users/"+id);
     yield return request.SendWebRequest();
    
        if (request.responseCode == 200)
        {
            FakeUser user = JsonUtility.FromJson<FakeUser>(request.downloadHandler.text);
            
            GameObject.Find("username").GetComponent<TMP_Text>().text = user.username;
            Console.WriteLine(user.username);
            for (int i = 0; i < user.deck.Length; i++)
            {
                var cardId = user.deck[i];
                StartCoroutine(GetCharacter(cardId, i));
            }
            
        }
        else
        {
            string mensaje = "status:" + request.responseCode;
            mensaje += "\nError: " + request.error;
            Debug.Log(mensaje);
        }
    }
    IEnumerator GetCharacter(int id, int index)
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl + "/"+ id);
        yield return www.SendWebRequest();
    
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.responseCode == 200)
            {
                Character character = JsonUtility.FromJson<Character>(www.downloadHandler.text);
                Debug.Log(character.name +" is a "+character.species);
                textObjects[index].text = character.name; 

                StartCoroutine(GetImage(character.image, index));
            }
            else
            {
                string mensaje = "status:" + www.responseCode;
                mensaje += "\nError: " + www.error;
                Debug.Log(mensaje);
            }
        }
    }

    IEnumerator GetImage(string imageUrl, int index)
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
            _images[index].texture = texture;
        }
    }
}

class Character
{
    public string id;
    public string name;
    public string species;
    public string image;
}
class FakeUser
{
    public int id;
    public string username;
    public bool state;
    public int[] deck;
}
