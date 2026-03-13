using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    [Header("Transition Prefabs")]
    [SerializeField] private GameObject startTransitionPrefab; 
    [SerializeField] private GameObject endTransitionPrefab;  
    [SerializeField] private Transform mainCanvas;            

    private void Awake()
    {
       
        if (mainCanvas == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                mainCanvas = canvas.transform;
            }
        }

        
        PlayStartTransition();
    }

    private void PlayStartTransition()
    {
        if (startTransitionPrefab != null && mainCanvas != null)
        {
            Instantiate(startTransitionPrefab, mainCanvas);
        }
    }

   
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(TransitionAndLoad(sceneName));
    }

    private IEnumerator TransitionAndLoad(string sceneName)
    {
        float waitTime = 0f;

       
        if (endTransitionPrefab != null && mainCanvas != null)
        {
            GameObject transitionObj = Instantiate(endTransitionPrefab, mainCanvas);
            Animator anim = transitionObj.GetComponent<Animator>();

            if (anim != null)
            {
                yield return null; 
                waitTime = anim.GetCurrentAnimatorStateInfo(0).length; 
            }
        }

        
        yield return new WaitForSeconds(waitTime);

        
        SceneManager.LoadScene(sceneName);
    }
}
