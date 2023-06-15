using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    [SerializeField] private float cameraScaleMin = 10, cameraScaleMax = 80;

    private Camera node_camera;
    private float originSize;
    void Awake()
    {
        node_camera = GameObject.FindGameObjectWithTag("NodeCamera").GetComponent<Camera>();
        originSize = node_camera.orthographicSize;
    }
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        //줌
        if (node_camera.orthographic)
        {
            // ... change the orthographic size based on the change in distance between the touches.
            node_camera.orthographicSize -= Input.mouseScrollDelta.y * orthoZoomSpeed;

            // 카메라크기최소제한
            node_camera.orthographicSize = Mathf.Max(node_camera.orthographicSize, cameraScaleMin);
            // 카메라크기최대제한
            node_camera.orthographicSize = Mathf.Min(node_camera.orthographicSize, cameraScaleMax);

            float scale = node_camera.orthographicSize / originSize;
            transform.localScale = new Vector3(scale, scale, 1);

            //float scaleY = node_camera.orthographicSize / originSize;
            //float scaleX = node_camera.aspect * scaleY;
            //transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
#else
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            Camera mainCamera = Camera.main;
            if (mainCamera.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                mainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed * 0.1f;

                // Make sure the orthographic size never drops below zero.
                mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                mainCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 0.1f, 179.9f);
            }
        }

        if (Input.touchCount >= 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                oldClickPosition = Input.GetTouch(0).position;
            else
            {
                Vector2 delta = (Vector2)Input.GetTouch(0).position - oldClickPosition;
                if (GameObject.Find("Controller").GetComponent<Controller>().selectedObject == null)
                    transform.position -= new Vector3(delta.x, delta.y) * cameraSpeed * Time.deltaTime * camera.orthographicSize;
                oldClickPosition = Input.GetTouch(0).position;
            }
        }
#endif
    }
}