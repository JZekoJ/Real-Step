using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float duration = 1f;

    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        Destroy(gameObject, duration);
    }

    public void SetDamage(float amount)
    {
        if (textMesh != null)
            textMesh.text = "-" + amount.ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.LookAt(Camera.main.transform); // Toujours vers la caméra
    }
}
