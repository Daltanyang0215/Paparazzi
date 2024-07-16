using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _memoTransfrom;
    [SerializeField] private TMP_Text _memoText;
    public void ShowPanel(bool show)
    {
        _memoText.transform.parent.gameObject.SetActive(show);
        StartCoroutine(MemoAnimation(show));
    }
    public void SetMemoList(ActorElement element)
    {
        _memoText.text = element.ActorPart.ToString();
    }

    private IEnumerator MemoAnimation(bool show)
    {

        //float startpos = show ? 0f : _memoTransfrom.rect.size.x * _memoTransfrom.localScale.x;
        //float endPos = show ? _memoTransfrom.rect.size.x * _memoTransfrom.localScale.x : 0;
        Vector2 startSize = show ? new Vector2(450, 120) : new Vector2(450, 200);
        Vector2 endSize = show ? new Vector2(450, 200) : new Vector2(450, 120);

        float animationTimer = 0;
        float animationMaxTime = .5f;

        float x = 0;
        float t = 0;
        while (animationTimer < animationMaxTime)
        {
            animationTimer += Time.deltaTime;
            x = animationTimer / animationMaxTime;
            t = 1 - Mathf.Pow(1 - x, 5);
            //_memoTransfrom.anchoredPosition = Vector2.up * 100 + Vector2.left * Mathf.Lerp(startpos, endPos, t);
            _memoTransfrom.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            yield return null;
        }
        //_memoTransfrom.anchoredPosition = Vector2.up * 100 + Vector2.left * endPos;
        _memoTransfrom.sizeDelta = endSize;
    }
}
