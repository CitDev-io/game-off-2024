using UnityEngine;

public class DragCamera : MonoBehaviour
{
    private Vector3 _dragOrigin;
    private Camera _camera;
    public float dragSpeed = 1.5f;
    float STARTING_CAMERA_Z = -10f;
    GameBoardManager gbm;

    private void Start()
    {
        _camera = Camera.main;
        STARTING_CAMERA_Z = _camera.transform.position.z;
        gbm = GameObject.Find("GameBoard").GetComponent<GameBoardManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gbm.OnUserCameraMovement();
            _dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = (_dragOrigin - Input.mousePosition) * dragSpeed;
            gbm.OnUserCameraMovement();
            _camera.transform.Translate(difference.x * Time.deltaTime, difference.y * Time.deltaTime, 0);
            
            Vector3 clampedPosition = _camera.transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -7f, 7f);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -7f, 7f);
            clampedPosition.z = STARTING_CAMERA_Z;
            gbm.OnUserCameraMovement();
            _camera.transform.position = clampedPosition;

            _dragOrigin = Input.mousePosition;
        }

        //scroll or pinch to zoom
        if (Input.touchCount == 2)
        {
            gbm.OnUserCameraMovement();
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            gbm.OnUserCameraMovement();
            Zoom(difference * 0.01f);
        }
        else
         if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;
                _camera.transform.Translate(-touchDeltaPosition.x * dragSpeed * Time.deltaTime, -touchDeltaPosition.y * dragSpeed * Time.deltaTime, 0);
                
                Vector3 clampedPosition = _camera.transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, -7f, 7f);
                clampedPosition.y = Mathf.Clamp(clampedPosition.y, -7f, 7f);
                clampedPosition.z = STARTING_CAMERA_Z;
                gbm.OnUserCameraMovement();
                _camera.transform.position = clampedPosition;
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            Zoom(Input.mouseScrollDelta.y);
        }
    }

    void Zoom(float increment)
    {
        gbm.OnUserCameraMovement();
        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - increment, 20f, 70f);
    }
}