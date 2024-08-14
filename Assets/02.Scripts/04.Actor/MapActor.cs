using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActor : MonoBehaviour
{
    // 사진 중복 확인용 액터 아이디. 고유값의 의미는 없음
    [SerializeField] public int ActorID { get; private set; }
    [field: SerializeField] public ActorData ActorData { get; private set; }
    private SpriteRenderer _renderer;

    public void SetData(ActorData data, int id)
    {
        ActorData = data;
        ActorID = id;
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

    private void Update()
    {
        if (ReferenceEquals(ActorData, null) || ActorData.MoveVec == ActorMoveVec.None) return;

        transform.Translate(ActorData.MoveVec switch
        {
            ActorMoveVec.LB2RT => new Vector3(1, 1, 0),
            ActorMoveVec.RT2LB => new Vector3(-1, -1, 0),
            ActorMoveVec.LT2RB => new Vector3(1, -1, 0),
            ActorMoveVec.RB2LT => new Vector3(-1, 1, 0),
            _ => Vector3.zero
        } * Time.deltaTime 
        * ActorData.ActorElement.ActorType switch
        {
            ActorType.Actor => 1,
            ActorType.Car => 6,
            _ => 0
        });

        if ((transform.position.y > 0 ? transform.position.y : -transform.position.y) > 55)
        {
            transform.position -= ActorData.MoveVec switch
            {
                ActorMoveVec.LB2RT => new Vector3(1, 1, 0),
                ActorMoveVec.RT2LB => new Vector3(-1, -1, 0),
                ActorMoveVec.LT2RB => new Vector3(1, -1, 0),
                ActorMoveVec.RB2LT => new Vector3(-1, 1, 0),
                _ => Vector3.zero
            } * 110;
        }
    }

    //private void OnMouseEnter()
    //{
    //    if (MainGameManager.Instance.IsCameraMove)
    //        _renderer.enabled = true;
    //}

    //private void OnMouseDown()
    //{
    //    if (!MainGameManager.Instance.IsCameraMove) return;
    //    //Debug.Log($"{gameObject.name} Poto");
    //    // 촬영중에 하이라이트가 나오지 않도록 잠시 껏다 킴
    //    _renderer.enabled = false;

    //    Camera.main.transform.position = transform.position + Vector3.back * 5;
    //    MainGameManager.Instance.CameraCapture(ActorData.ActorElement, ActorID);

    //    _renderer.enabled = true;
    //}

    //private void OnMouseExit()
    //{
    //    _renderer.enabled = false;
    //}

}
