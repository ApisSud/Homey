using UnityEngine;

public class Hoversprites : MonoBehaviour
{

    public GameObject highlightObject;

    void Start()
    {
        
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
       
        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
       
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }
}
