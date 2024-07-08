using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    private static MainGameManager _instance;
    public static MainGameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("GameManager").GetComponent<MainGameManager>();
            return _instance;
        }
    }

    [field: SerializeField] public List<Sprite> Captures { get; private set; } = new List<Sprite>();
    private Camera _camera;
    [field: SerializeField] public byte MaxCaptureCount { get; private set; } = 5;
    public byte CurCaptureCount { get; private set; }

    public Action CaptureAction;

    private void Start()
    {
        _camera = Camera.main;
        CaptureAction += CheckDailyFinish;
    }

    public void CameraCapture()
    {
        if (CurCaptureCount >= MaxCaptureCount) return;
        CurCaptureCount++;

        RenderTexture render = new RenderTexture(512, 512, 24);
        Rect rect = new Rect(0, 0, 512, 512);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, false);

        _camera.targetTexture = render;
        _camera.Render();

        RenderTexture.active = render;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        _camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(render);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
        Captures.Add(sprite);

        CaptureAction?.Invoke();
    }

    private void CheckDailyFinish()
    {
        if (CurCaptureCount != MaxCaptureCount) return;
        Debug.Log("Finish");
    }

}
