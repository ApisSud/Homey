using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Taskcontroller : MonoBehaviour
{
    public Image[] Taskimage;
    public GameObject[] pages;
    
    
 
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

    public void SelectLevel(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
