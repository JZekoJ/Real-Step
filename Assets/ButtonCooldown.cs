using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownButton : MonoBehaviour
{
    private Button button;
    private Image image;

    [Range(0, 999)]
    [SerializeField] private int cooldownDuration = 30;

    [Range(0f, 1f)]
    [SerializeField] private float disabledAlpha;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void OnButtonClick()
    {
        StartCoroutine(ButtonCooldown());
    }

    private IEnumerator ButtonCooldown()
    {
        button.interactable = false;

        Color tempColor = image.color;
        tempColor.a = disabledAlpha;
        image.color = tempColor;

        yield return new WaitForSeconds(cooldownDuration);

        button.interactable = true;

        tempColor.a = 1f;
        image.color = tempColor;
    }
}