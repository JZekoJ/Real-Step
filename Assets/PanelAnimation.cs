using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{


    [SerializeField] AnimationCurve _animationCourbe;
    [SerializeField] float _duration;

    private CanvasGroup _canvasGroup;

    private float _timer;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void TogglePanel(bool open)
    {
        if (open)
        {
            StartCoroutine(OpenCor());
        }
        else
        {
            StartCoroutine(CloseCor());
        }
    }

    private IEnumerator OpenCor()
    {
        _timer = 0f;
        while (_timer < _duration)
        {
            _timer += Time.deltaTime;
            this.transform.localScale = Vector3.one * _animationCourbe.Evaluate(_timer / _duration);
            _canvasGroup.alpha = _animationCourbe.Evaluate(_timer / _duration);
            yield return null;
        }
    }

    private IEnumerator CloseCor()
    {
        _timer = _duration;
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            this.transform.localScale = Vector3.one * _animationCourbe.Evaluate(_timer / _duration);
            _canvasGroup.alpha = _animationCourbe.Evaluate(_timer / _duration);
            yield return null;
        }
        if (_timer < 0)
        {
            this.transform.localScale = Vector3.one * _animationCourbe.Evaluate(0);
            _canvasGroup.alpha = _animationCourbe.Evaluate(0);
        }

    }

}
