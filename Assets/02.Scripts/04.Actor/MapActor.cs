using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActor : MonoBehaviour
{
    [field: SerializeField] public ActorData ActorData { get; private set; }
    private SpriteRenderer _renderer;

    public void SetData(ActorData data)
    {
        ActorData = data;
        OnValidate();
    }


    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = ActorData.IdleSprite;
        GetComponent<SpriteRenderer>().flipX = ActorData.IsFlip;
        _renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _renderer.sprite = ActorData.HightLightSprite;
        _renderer.flipX = ActorData.IsFlip;
        _renderer.enabled = false;

        GetComponent<BoxCollider2D>().size = ActorData.ActorElement.ActorType switch
        {
            ActorType.Car => Vector2.one * 8,
            _ => Vector2.one * 4
        };
    }

    private void Start()
    {
        _renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    private void OnMouseEnter()
    {
        if (MainGameManager.Instance.IsCameraMove)
            _renderer.enabled = true;
    }

    private void OnMouseDown()
    {
        if (!MainGameManager.Instance.IsCameraMove) return;
        //Debug.Log($"{gameObject.name} Poto");
        // 촬영중에 하이라이트가 나오지 않도록 잠시 껏다 킴
        _renderer.enabled = false;

        Camera.main.transform.position = transform.position + Vector3.back * 5;
        MainGameManager.Instance.CameraCapture(ActorData.ActorElement);

        _renderer.enabled = true;
    }

    private void OnMouseExit()
    {
        _renderer.enabled = false;
    }

}
