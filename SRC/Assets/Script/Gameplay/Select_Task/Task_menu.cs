using UnityEngine;
using UnityEngine.UI;

public class Task_menu : MonoBehaviour
{
    public GameObject menucanvas;
    [SerializeField] AudioSource Clicking;
    void Start()
    {
        if (menucanvas != null)
        {
            menucanvas.SetActive(false);
        }
    }

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }

    }

    public  void Backtomenu()
    {
        if (menucanvas != null)
        {
            menucanvas.SetActive(false); 
            Time.timeScale = 1f; 
        }
        Clicking.Play();
    }

    void ToggleMenu()
    {
        if (menucanvas != null)
        {
            bool isActive = !menucanvas.activeSelf;
            menucanvas.SetActive(isActive);

           
            if (isActive) 
                Time.timeScale = 0f; // หยุดเวลา
            else 
                Time.timeScale = 1f; // เวลาเดินต่อ
          
        }
        Clicking.Play();
    }
}
