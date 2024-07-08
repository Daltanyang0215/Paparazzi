using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActor : MonoBehaviour
{
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _hightLightSprite;
    
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = _idleSprite;
    }

    private void OnMouseEnter()
    {
        _renderer.sprite = _hightLightSprite;
    }

    private void OnMouseDown()
    {
        Debug.Log($"{gameObject.name} Poto");
        Camera.main.transform.position = transform.position + Vector3.back * 5;
        MainGameManager.Instance.CameraCapture();
    }

    private void OnMouseExit()
    {
        _renderer.sprite = _idleSprite;
    }

}
