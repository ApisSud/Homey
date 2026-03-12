using UnityEngine;

public class DragWateringCan : MonoBehaviour
{
    private Vector3 offset;

    void OnMouseDown()
    {
        
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void OnMouseDrag()
    {
       
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
    }
}
