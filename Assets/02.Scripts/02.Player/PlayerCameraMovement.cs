using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{

    [Header("Proparty")]
    [SerializeField] private Vector2 _cameraClampHalfSize;
    [SerializeField] private Vector2 _cameraHalfSize;
    private Vector2 _cameraCanMoveSize;

    [SerializeField] private float _moveSpeed;
    private Vector3 _inputVec;
    private Vector3 _inputMousebefo;
    private Vector3 _nextTemp;

    private CinemachineVirtualCamera _camera;

    private void Start()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();

        //float cemarasize = Camera.main.orthographicSize;
        float cemarasize = _camera.m_Lens.OrthographicSize;
        float aspectratio = Screen.width / (float)Screen.height;
        _cameraHalfSize = new Vector2(cemarasize * aspectratio, cemarasize);
        _cameraCanMoveSize = _cameraClampHalfSize - _cameraHalfSize;
    }

    private void ChangeMainCamera()
    {
        MainGameManager.Instance.IsCameraMove = !MainGameManager.Instance.IsCameraMove;
        _camera.Priority = MainGameManager.Instance.IsCameraMove ? 100 : 1;
        MainGameManager.Instance.CameraChangeAction?.Invoke();
    }
    public void ResetMainCamera()
    {
        MainGameManager.Instance.IsCameraMove = false;
        _camera.Priority = 1;
        transform.position = Vector3.zero;
        MainGameManager.Instance.CameraChangeAction?.Invoke();
    }

    private void Update()
    {
        // 카메라 전체로 변경
        if (Input.GetMouseButtonDown(2))
        {
            if (!MainGameManager.Instance.IsCameraMove)
            {
                Vector3 zoomPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                zoomPos.z = -5;
                transform.position = zoomPos;
            }
            ChangeMainCamera();
        }
        if (!MainGameManager.Instance.IsCameraMove) return;

  
        // 키보드 조작
        else
        {
            _inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // 키보드등 입력이 없으면
            if (_inputVec == Vector3.zero)
            {

                Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                _inputVec.x = pos.x switch
                {
                    < 0.1f => -1,
                    > 0.9f => 1,
                    _ => 0f,
                };
                _inputVec.y = pos.y switch
                {
                    < 0.1f => -1,
                    > 0.9f => 1,
                    _ => 0f,
                };
            }
        }

        // 클램핑
        _nextTemp = transform.position + (_moveSpeed * Time.deltaTime) * _inputVec;
        if (_nextTemp.x * _nextTemp.x > _cameraCanMoveSize.x * _cameraCanMoveSize.x)
        {
            _nextTemp = new Vector2(_cameraCanMoveSize.x * (_nextTemp.x < 0 ? -1 : 1), _nextTemp.y);
        }
        if (_nextTemp.y * _nextTemp.y > _cameraCanMoveSize.y * _cameraCanMoveSize.y)
        {
            _nextTemp = new Vector2(_nextTemp.x, _cameraCanMoveSize.y * (_nextTemp.y < 0 ? -1 : 1));
        }
        _nextTemp.z = -5f;

        // 적용
        transform.position = _nextTemp;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, _cameraClampHalfSize * 2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, (_cameraClampHalfSize - _cameraHalfSize) * 2);
    }
#endif
}
