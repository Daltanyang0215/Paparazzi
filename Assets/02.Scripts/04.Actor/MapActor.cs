using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActor : MonoBehaviour
{
    [field : SerializeField] public ActorData ActorData {  get; private set; }
    [SerializeField] private bool _isFlip;
    private SpriteRenderer _renderer;

    public void SetData(ActorData data)
    {
        ActorData = data;
        OnValidate();
    }


    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = ActorData.IdleSprite;
        GetComponent<SpriteRenderer>().flipX = _isFlip;
        _renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _renderer.sprite = ActorData.HightLightSprite;
        _renderer.flipX = _isFlip;
        _renderer.enabled = false;
    }

    private void Start()
    {
        _renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    private void OnMouseEnter()
    {
        _renderer.enabled = true;
    }

    private void OnMouseDown()
    {
        Debug.Log($"{gameObject.name} Poto");
        // 촬영중에 하이라이트가 나오지 않도록 잠시 껏다 킴
        _renderer.enabled = false;

        Camera.main.transform.position = transform.position + Vector3.back * 5;
        MainGameManager.Instance.CameraCapture();

        _renderer.enabled = true;
    }

    private void OnMouseExit()
    {
        _renderer.enabled = false;
    }

}
