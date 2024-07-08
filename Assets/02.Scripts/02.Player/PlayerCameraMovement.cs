using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private bool _isCanMove;

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

        float cemarasize = Camera.main.orthographicSize;
        float aspectratio = Screen.width / (float)Screen.height;
        _cameraHalfSize = new Vector2(cemarasize * aspectratio, cemarasize);
        _cameraCanMoveSize = _cameraClampHalfSize - _cameraHalfSize;
    }

    private void Update()
    {
        // ī�޶� ��ü�� ����
        if (Input.GetMouseButtonDown(2))
        {
            _isCanMove = !_isCanMove;
            _camera.Priority = _isCanMove ? 100 : 1;
        }
        if (!_isCanMove) return;

        // ���콺 ����
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                _inputMousebefo = Input.mousePosition;
            }
            _inputVec = (_inputMousebefo - Input.mousePosition) * .68f;
            _inputMousebefo = Input.mousePosition;
        }
        // Ű���� ����
        else
        {
            _inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        // Ŭ����
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

        // ����
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
