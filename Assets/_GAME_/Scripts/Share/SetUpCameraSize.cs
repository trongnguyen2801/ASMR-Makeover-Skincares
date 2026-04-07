using UnityEngine;

public class SetUpCameraSize : MonoBehaviour
{
    public static SetUpCameraSize Instance;
    [SerializeField] private float desiredCameraWidth; // Kích thước mặc định của camera theo chiều rộng
    [SerializeField] private float targetAspectRatio = 9f / 16f; // Tỉ lệ màn hình mục tiêu (vd: 16:9)
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
       // SetCameraOrthoSizeMapHard();
        SetCameraOrthoSizeMapNormal();
    }

    public void SetDesiredCamWidth(float value)
    {
        desiredCameraWidth = value;
    }

    public void SetCameraOrthoSizeMapNormal()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        if (currentAspectRatio < targetAspectRatio)            // resize theo chiều rộng giảm thì orthoSize tăng 
        {
            var orthoSize = desiredCameraWidth / currentAspectRatio / 2;
            mainCamera.orthographicSize = orthoSize;
        }
    }

    public void SetCameraOrthoSizeMapHard()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        if (currentAspectRatio > targetAspectRatio)         // resize theo chiều rộng tăng thì orthoSize giảm 
        {
            var orthoSize = desiredCameraWidth / currentAspectRatio / 2;
            mainCamera.orthographicSize = orthoSize;
            Debug.LogError("Width");
        }
    }

    public void SetCameraSize(float sizeXLevel, float sizeYLevel)
    {
        if (sizeXLevel > sizeYLevel - 1)
        {
            float currentAspectRatio = (float)Screen.width / Screen.height;
            var orthoSize = (sizeXLevel + 1) * 2 / currentAspectRatio / 2;
            mainCamera.orthographicSize = orthoSize;
            Debug.LogError("Width");
        }
        else
        {
            var orthoSize = sizeYLevel + 4.5f;
            mainCamera.orthographicSize = orthoSize;
            Debug.LogError("Height");
        }
    }
}
