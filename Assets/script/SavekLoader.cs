using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SavekLoader : MonoBehaviour
{

    public UnityEvent OnLoadSave;
    // Start is called before the first frame update
    private void Awake()
    {
        ReferenceManager.SaveLoader = this;
    }
    private void Start()
    {
        SaveLoadSystem.Load(ReferenceManager.Player);
        OnLoadSave.Invoke();
    }

    
}
