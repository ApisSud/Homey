using UnityEngine;

public class Task_menu : MonoBehaviour
{
    public GameObject menucanvas;
    void Start()
    {
        menucanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menucanvas.SetActive(!menucanvas.activeSelf);
        }

    }
}
