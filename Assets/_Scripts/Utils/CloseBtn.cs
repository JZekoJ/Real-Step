using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseBtn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().Select();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
