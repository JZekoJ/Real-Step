using System.Collections;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Location;


public class MapboxMap : MonoBehaviour
{
    public AbstractMap map; // R�f�rence � la carte Mapbox dans la sc�ne
    public float updateInterval = 2f; // Toutes les X secondes, on met � jour la position

    private bool gpsReady = false;
    private float timer = 0f;

    void Start()
    {
        StartCoroutine(StartLocationService());
    }

    void Update()
    {
        if (gpsReady)
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                UpdateUserLocation();
                timer = 0f;
            }
        }
    }

    IEnumerator StartLocationService()
    {
#if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
            yield return new WaitForSeconds(2);
        }
#endif

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("GPS d�sactiv�. Activez-le dans les param�tres.");
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

        gpsReady = true;
        UpdateUserLocation(); // Mise � jour imm�diate
    }

    void UpdateUserLocation()
    {
        float lat = Input.location.lastData.latitude;
        float lon = Input.location.lastData.longitude;

        Debug.Log("Position GPS : " + lat + ", " + lon);
        Vector2d gpsPos = new Vector2d(lat, lon);
        map.UpdateMap(gpsPos, map.Zoom);
    }
}
