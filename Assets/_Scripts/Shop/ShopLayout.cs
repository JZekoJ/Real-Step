using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLayout : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject CasePrefab;
    void Start()
    {
        for(int i = 0; i < 9; i++)
        {
            GameObject uiObj = Instantiate(CasePrefab, this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
