using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Object> list = new List<Object>();
    public GameObject prefabUI;
    public GameObject content;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory(int type)//pas récupérable sur un bouton
    {
        foreach (ScriptableWeapon obj in list) {
            if (obj.type == (Weapon_Type) type) { 
                Instantiate(prefabUI,content.transform);
            }
        }
    }


}
