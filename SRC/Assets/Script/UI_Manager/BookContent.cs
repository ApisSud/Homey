using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BookContent : MonoBehaviour
{
    [SerializeField] float pageSpeed = 0.1f;
    [SerializeField] List<Transform> pages;
    int index = -1;
    bool rotate = false;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject forwardButton;

    [Header("Book Appearance Settings")]
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float showDuration = 1.2f;

    private void Start()
    {
        InitialState();
        transform.localPosition = startPosition;
    }

    public void InitialState()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].transform.rotation = Quaternion.identity;
        }
        pages[0].SetAsLastSibling();
        backButton.SetActive(false);

    }


    public void ShowBookAndOpen()
    {

        transform.DOLocalMove(targetPosition, showDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {

                RotateForward();
            });
    }
    public void RotateForward()
    {
        if (rotate == true) { return; }
        index++;
        float angle = 180; //in order to rotate the page forward, you need to set the rotation by 180 degrees around the y axis
        ForwardButtonActions();
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(angle, true));

        if (forwardButton != null) forwardButton.transform.SetAsLastSibling();
        if (backButton != null) backButton.transform.SetAsLastSibling();
    }

    public void ForwardButtonActions()
    {
        if (backButton.activeInHierarchy == false)
        {
            backButton.SetActive(true); //every time we turn the page forward, the back button should be activated
        }
        if (index == pages.Count - 1)
        {
            forwardButton.SetActive(false); //if the page is last then we turn off the forward button
        }
    }

    public void RotateBack()
    {
        if (rotate == true) { return; }
        float angle = 0; //in order to rotate the page back, you need to set the rotation to 0 degrees around the y axis
        pages[index].SetAsLastSibling();
        BackButtonActions();
        StartCoroutine(Rotate(angle, false));

        if (forwardButton != null) forwardButton.transform.SetAsLastSibling();
        if (backButton != null) backButton.transform.SetAsLastSibling();
    }

    public void BackButtonActions()
    {
        if (forwardButton.activeInHierarchy == false)
        {
            forwardButton.SetActive(true); //every time we turn the page back, the forward button should be activated
        }
        if (index - 1 == -1)
        {
            backButton.SetActive(false); //if the page is first then we turn off the back button
        }
    }

    IEnumerator Rotate(float angle, bool forward)
    {
        float value = 0f;
        while (true)
        {
            rotate = true;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            value += Time.deltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value); //smoothly turn the page
            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation); //calculate the angle between the given angle of rotation and the current angle of rotation
            if (angle1 < 0.1f)
            {
                if (forward == false)
                {
                    index--;
                }
                rotate = false;
                break;

            }
            yield return null;

        }
    }

    public void HideBook()
    {
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        
        if (rotate)
        {
            yield return new WaitUntil(() => rotate == false);
        }

        
        while (index > 0)
        {
            RotateBack(); 
            yield return null; 

            
            yield return new WaitUntil(() => rotate == false);
        }

      
        transform.DOLocalMove(startPosition, showDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                
                index = -1;
                InitialState();
            });
    }
}
