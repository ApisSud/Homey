using UnityEngine;

public class CauldronManager : MonoBehaviour
{
    public static CauldronManager Instance;

    [Header("Highlight Settings")]
   
    public GameObject cauldronHighlight;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
       
        if (cauldronHighlight != null)
        {
            cauldronHighlight.SetActive(false);
        }
    }

    
    public void SetHighlight(bool isShow)
    {
       
        if (cauldronHighlight != null)
        {
            cauldronHighlight.SetActive(isShow);
        }
    }
}
