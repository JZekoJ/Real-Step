using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPortal : MonoBehaviour
{
    public List<GameObject> Portals;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t>1)
        {
            GameObject portal = Portals[Random.Range(0, Portals.Count)];
            Vector3 loc = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(portal, loc, portal.transform.rotation);
            t = 0;
        }
    }
}
