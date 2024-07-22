using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsPanel : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private RectTransform _newsTransfrom;

    [SerializeField] private TMP_Text _newsText;

    [SerializeField] private GameObject _nextButton;
    private void Start()
    {
        _canvas = GetComponent<Canvas>();
    }


    public void SnowPanel(bool show)
    {
        _canvas.enabled = show;

        if (show)
        {
            StartCoroutine(NewsAnimation());
        }
    }

    public void DialyStart()
    {
        MainUIManager.Instance.FadeEffect(true, () => MainGameManager.Instance.ChangeNextState());
    }

    private IEnumerator NewsAnimation()
    {
        _newsText.text = MainGameManager.Instance.CurMapData.TargetNewComant;
        _nextButton.SetActive(false);

        float startPos = -1100;
        float endPos = -50;
        float maxt = .5f;
        float t = 0;
        while (t < maxt)
        {
            t += Time.deltaTime;
            _newsTransfrom.anchoredPosition = Vector3.up * Mathf.Lerp(startPos, endPos, 1 - (1 - t / maxt) * (1 - t / maxt));
            yield return null;
        }
        _newsTransfrom.anchoredPosition = Vector3.up * endPos;
        _nextButton.SetActive(true);
        MainUIManager.Instance.MemoPanel.gameObject.SetActive(true);
    }
}
