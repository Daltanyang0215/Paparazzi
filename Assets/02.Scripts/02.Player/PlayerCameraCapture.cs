using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraCapture : MonoBehaviour
{
    // 현재 미사용

    [SerializeField] private byte _maxCaptureCount;
    private byte _curCaptureCount;

    [field: SerializeField] public List<Sprite> Captures { get; private set; } = new List<Sprite>();



    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CameraCapture();
        }
    }

    private void CameraCapture()
    {
        if (_curCaptureCount >= _maxCaptureCount) return;
        _curCaptureCount++;

        RenderTexture render = new RenderTexture(512, 512, 24);
        Rect rect = new Rect(0, 0, 512, 512);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, false);

        Camera.main.targetTexture = render;
        Camera.main.Render();

        RenderTexture.active = render;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(render);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
        Captures.Add(sprite);
    }
}
