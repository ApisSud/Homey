using UnityEngine;

public class Dragitem : MonoBehaviour
{
    private bool isDragging = true; 

  
    void OnMouseDown()
    {
        isDragging = true; 
    }
   

    void Update()
    {
       
        if (isDragging)
        {
           
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 

            
            transform.position = mousePos;

           
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false; 
                
            }
        }
    }
}
