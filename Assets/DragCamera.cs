using UnityEngine;

public class DragCamera : MonoBehaviour
{
    private Vector3 _dragOrigin;
    private Camera _camera;
    public float dragSpeed = 1.5f;
    float STARTING_CAMERA_Z = -10f;

    private void Start()
    {
        _camera = Camera.main;
        STARTING_CAMERA_Z = _camera.transform.position.z;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = (_dragOrigin - Input.mousePosition) * dragSpeed;
            _camera.transform.Translate(difference.x * Time.deltaTime, difference.y * Time.deltaTime, 0);
            
            Vector3 clampedPosition = _camera.transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -10f, 2f);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -10f, 2f);
            clampedPosition.z = STARTING_CAMERA_Z;
            _camera.transform.position = clampedPosition;

            _dragOrigin = Input.mousePosition;
        }
    }
}