using UnityEngine;
using UnityEngine.UI;

public class HighlightButton : MonoBehaviour
{
    public GameObject HighlightBG;
    public Button button;

    public BookContent bookscript;
    public void OnEnterbutton()
    {
        HighlightBG.SetActive(true);

        if (bookscript != null  && bookscript.isBookopen)
        {
            HighlightBG.SetActive(false);
        }
    }
}
