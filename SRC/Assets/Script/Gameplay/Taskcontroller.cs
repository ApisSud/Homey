using UnityEngine;
using UnityEngine.UI;

public class Taskcontroller : MonoBehaviour
{
    public Image[] Taskimage;
    public GameObject[] pages;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateTask(0);
    }

  public void ActivateTask(int taskNo)
    {
        for (int i = 0 ; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            Taskimage[i].color = Color.grey;
        }
        pages[taskNo].SetActive(true);
        Taskimage[taskNo].color = Color.white;
    }
}
