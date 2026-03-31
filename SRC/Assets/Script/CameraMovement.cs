using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam; 

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize; 

    [SerializeField]
    private float scrollSensitivity = 10f;

    [SerializeField]
    private float panSpeed = 15f;

    [SerializeField]
    private SpriteRenderer mapRenderer;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private Vector3 dragOrigin;


    private void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    void Start()
    {

    }

    void Update()
    {
        PanCamera();
        HandleScrollZoom(); 
    }


    private void PanCamera()
    {
        float moveX = Input.GetAxis("Horizontal");

        // รับค่าจากปุ่ม W/S หรือ บน/ล่าง (-1 ถึง 1)
        float moveY = Input.GetAxis("Vertical");

        // ถ้ามีการกดปุ่มขยับ
        if (moveX != 0f || moveY != 0f)
        {
           
            Vector3 moveDirection = new Vector3(moveX, moveY, 0f);

           
            Vector3 targetPosition = cam.transform.position + moveDirection * panSpeed * Time.deltaTime;

            
            cam.transform.position = ClampCamera(targetPosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        
        if (Input.GetMouseButton(1))
        {
           
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

          
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }
   
    private void HandleScrollZoom()
    {
      
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (scrollData != 0f)
        {
            
            float newSize = cam.orthographicSize - (scrollData * scrollSensitivity);

            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

          
            cam.transform.position = ClampCamera(cam.transform.position);
        }
    }


    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        cam.transform.position = ClampCamera(cam.transform.position);   //ograniczenie obszaru
    }


    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        cam.transform.position = ClampCamera(cam.transform.position); 
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
