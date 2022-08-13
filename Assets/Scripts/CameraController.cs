using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float m_PositionStep = 10.0f;

    [SerializeField]
    private float m_MouseSensitive = 90.0f;
    
    private bool m_isCameraMoveEnabled = true;

    private Vector3 m_StartMousePosition    = Vector3.zero;
    private Vector3 m_PresentCameraRotation = Vector3.zero;
    private Vector3 m_PresentCamPosition    = Vector3.zero;
    
    private void Update()
    {
        if (m_isCameraMoveEnabled)
        {
            ResetCameraRotation();
            RotateCamera();
            MoveCamera();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private void ResetCameraRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
            transform.position = new Vector3(0, 3, -15);
        }
    }

    private void RotateCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_StartMousePosition = Input.mousePosition;
            m_PresentCamPosition.x = transform.eulerAngles.x;
            m_PresentCameraRotation.y = transform.eulerAngles.y;
        }

        if (Input.GetMouseButton(1))
        {
            float x = (m_StartMousePosition.x - Input.mousePosition.x) / Screen.width;
            float y = (m_StartMousePosition.y - Input.mousePosition.y) / Screen.height;

            float eulerX = m_PresentCameraRotation.x + y * m_MouseSensitive;
            float eulerY = m_PresentCameraRotation.y - x * m_MouseSensitive;

            transform.rotation = Quaternion.Euler(eulerX, eulerY, 0);
        }
    }

    private void MoveCamera()
    {
        Vector3 cameraPosition = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            cameraPosition += transform.forward * Time.deltaTime * m_PositionStep;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            cameraPosition -= transform.forward * Time.deltaTime * m_PositionStep;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cameraPosition -= transform.right * Time.deltaTime * m_PositionStep;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            cameraPosition += transform.right * Time.deltaTime * m_PositionStep;
        }

        if (Input.GetKey(KeyCode.E))
        {
            cameraPosition += transform.up * Time.deltaTime * m_PositionStep;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            cameraPosition -= transform.up * Time.deltaTime * m_PositionStep;
        }

        transform.position = cameraPosition;
    }
}
