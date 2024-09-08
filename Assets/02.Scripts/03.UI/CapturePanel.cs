using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CapturePanel : UICanvasBase
{
    [Header("CameraAngle")]
    [SerializeField] private Transform _cameraAngle;
    [SerializeField] private float _angleFadeTime;
    [SerializeField] private float _angleFadeStartScale;
    private Coroutine _angleFadeCoroutine;
    private Vector3 _angleOriginScale;
    private List<Color> _angleOriginColorList;

    [Header("CaptureCount")]
    [SerializeField] private TMP_Text _captureCountText;
    [Header("NextDay")]
    [SerializeField] private Button _nextDay;
    [Header("Effect")]
    [SerializeField] private Image _effectImage;
    [SerializeField] private AnimationCurve _effectColorCurve;
    [SerializeField] private float _fadeTime = 0.5f;


    protected override void Awake()
    {
        base.Awake();
        _effectImage.enabled = false;
        _nextDay.gameObject.SetActive(false);
        MainGameManager.Instance.CaptureAction += UpdateCaptureCountUI;
        //MainGameManager.Instance.CameraChangeAction += CameraAngleEnable;
        MainGameManager.Instance.DayEndAction += ShowNextDayButton;

        _angleOriginScale = _cameraAngle.localScale;
        _cameraAngle.localScale = Vector3.one * _angleFadeStartScale;
        _angleOriginColorList = new List<Color>();
        foreach (Image image in _cameraAngle.GetComponentsInChildren<Image>())
        {
            _angleOriginColorList.Add(image.color);
            Color color = image.color;
            color.a = 0;
            image.color = color;
        }

    }

    private void Update()
    {
        if (!canvas.enabled) return;

        Vector2 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, 360, Screen.width - 360);
        mousePos.y = Mathf.Clamp(mousePos.y, 240, Screen.height - 240);
        _cameraAngle.position = mousePos;




        if (Input.GetMouseButtonDown(1))
        {
            CameraAngleEnable(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            CameraAngleEnable(false);
        }
        else if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))
        {
            MainGameManager.Instance.CameraCapture(mousePos);
        }

    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        _nextDay.gameObject.SetActive(false);

        _captureCountText.text = $"{MainGameManager.Instance.MaxCaptureCount} /  {MainGameManager.Instance.MaxCaptureCount}";
        CameraAngleEnable(false);
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    private void UpdateCaptureCountUI()
    {
        _captureCountText.text = $"{MainGameManager.Instance.MaxCaptureCount - MainGameManager.Instance.CurCaptureCount} / {MainGameManager.Instance.MaxCaptureCount}";
        StopAllCoroutines();
        StartCoroutine(CpatureEffect());
    }

    private void CameraAngleEnable(bool isEnable)
    {
        if (_angleFadeCoroutine != null)
        {
            StopCoroutine(_angleFadeCoroutine);
        }
        _angleFadeCoroutine = StartCoroutine(CameraFadeAnimation(isEnable));
    }

    private IEnumerator CameraFadeAnimation(bool isEnable)
    {
        float startScale = _cameraAngle.transform.localScale.x;
        List<Image> angleImages = new List<Image>();
        List<Color> imageColors = new List<Color>();
        foreach (Image image in _cameraAngle.GetComponentsInChildren<Image>(true))
        {
            angleImages.Add(image);
            imageColors.Add(image.color);
        }

        float t = 0;
        if (isEnable)
        {
            _cameraAngle.gameObject.SetActive(true);
            while (t < _angleFadeTime)
            {
                _cameraAngle.transform.localScale = Vector3.Lerp(Vector3.one * startScale, _angleOriginScale, t / _angleFadeTime);
                for (int i = 0; i < angleImages.Count; i++)
                {
                    angleImages[i].color = Color.Lerp(imageColors[i], _angleOriginColorList[i], t / _angleFadeTime);
                }
                yield return null;
                t += Time.deltaTime;
            }
            _cameraAngle.transform.localScale = Vector3.one;
            for (int i = 0; i < angleImages.Count; i++)
                angleImages[i].color = _angleOriginColorList[i];
        }
        else
        {
            List<Color> clearColors = new List<Color>();
            foreach (Color item in imageColors)
            {
                Color addColor = item;
                addColor.a = 0;
                clearColors.Add(addColor);
            }
            while (t < _angleFadeTime)
            {
                _cameraAngle.transform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * _angleFadeStartScale, t / _angleFadeTime);
                for (int i = 0; i < angleImages.Count; i++)
                {
                    angleImages[i].color = Color.Lerp(imageColors[i], clearColors[i], t / _angleFadeTime);
                }
                yield return null;
                t += Time.deltaTime;
            }
            _cameraAngle.transform.localScale = Vector3.one * _angleFadeStartScale;
            for (int i = 0; i < angleImages.Count; i++)
                angleImages[i].color = clearColors[i];
            _cameraAngle.gameObject.SetActive(false);
        }
        _angleFadeCoroutine = null;
    }

    private void ShowNextDayButton()
    {
        _nextDay.gameObject.SetActive(true);
    }

    private IEnumerator CpatureEffect()
    {
        _effectImage.enabled = true;
        float t = 0;
        while (t < _fadeTime)
        {
            t += Time.deltaTime;
            _effectImage.color = new Color(1, 1, 1, _effectColorCurve.Evaluate(t / _fadeTime));
            yield return null;
        }
        _effectImage.enabled = false;
    }
}
