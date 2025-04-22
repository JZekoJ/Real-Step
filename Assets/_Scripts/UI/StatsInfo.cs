using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsInfo : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenClose);
    }

    private void OpenClose()
    {
        if (GetComponent<Animator>().GetBool("OnOff"))
        {
            GetComponent<Animator>().SetBool("OnOff", false);
        }
        else if(!GetComponent<Animator>().GetBool("OnOff"))
        {
            GetComponent<Animator>().SetBool("OnOff", true);
        }
    }


}
