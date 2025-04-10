using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPortal : MonoBehaviour
{
    public GameObject m_gPortal;
    float time;
    [SerializeField] float Range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > 1)
        {
            
            Vector3 loc = new Vector3(Random.Range(-Range, Range), 0.5f, Random.Range(-Range, Range));
            Vector3 rot = new Vector3(m_gPortal.transform.rotation.x, m_gPortal.transform.rotation.y, Random.Range(0.0f, 360.0f));
            m_gPortal.transform.Rotate(rot);
            Instantiate(m_gPortal, loc, m_gPortal.transform.rotation);
            time = 0;
        }
    }
}
