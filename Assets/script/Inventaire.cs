using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Object> list = new List<Object>();
    public List<Object> listSelected = new List<Object>();
    public GameObject prefabUI;
    public GameObject content;

    [SerializeField] private PlayerStat Player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory(int type)//pas récupérable sur un bouton
    {
        for (int i = 0; i < listSelected.Count; i++)
        {
            Destroy(listSelected[i]);
        }
        listSelected.Clear();

        foreach (ScriptableWeapon obj in list) {
            if (obj.type == (Weapon_Type) type) {
                GameObject go = Instantiate(prefabUI,content.transform);
                go.GetComponent<Weapon>().m_weapon = obj;
                go.GetComponent<Weapon>().Init();
                go.GetComponent<Button>().onClick.AddListener(() => Player.Equip(go.GetComponent<Weapon>().m_weapon));
                listSelected.Add(go);
            }
        }
    }


}
