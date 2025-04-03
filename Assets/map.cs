using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Android;
using System;

public class map : MonoBehaviour
{
    public string apiKey;
    public float lat = 45.74038f; // Par défaut : Lyon
    public float lon = 4.87844f;
    public int zoom = 18;
    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;
    public enum type { roadmap, satellite, hybrid, terrain };
    public type mapType = type.roadmap;

    private string url = "";
    private int mapWidth = 1920;
    private int mapHeight = 1080;
    private Rect rect;
    private bool updateMap = true;
    private bool locationServiceStarted = false;

    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);

        // Démarrer la localisation
        StartCoroutine(StartLocationService());
    }

    void Update()
    {
        if (updateMap)
        {
            StartCoroutine(GetGoogleMap());
            updateMap = false;
        }
    }

    IEnumerator GetGoogleMap()
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon +
              "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight +
              "&scale=" + (int)mapResolution + "&maptype=" + mapType.ToString().ToLower() +
              "&key=" + apiKey;

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Map error: " + www.error);
        }
        else
        {
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("GPS désactivé. Activez-le dans les paramètres.");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0 || Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Impossible d'obtenir la localisation.");
            yield break;
        }

        lat = Input.location.lastData.latitude;
        lon = Input.location.lastData.longitude;
        Debug.Log("Localisation détectée : " + lat + ", " + lon);

        locationServiceStarted = true;
        updateMap = true;
    }
}
