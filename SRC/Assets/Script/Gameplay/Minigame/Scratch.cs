using UnityEngine;
using UnityEngine.UI;


public class Scratch : MonoBehaviour
{
    public RectTransform recttransform;
    public int width;
    public int height;
    public Texture2D masktexture;
    public int scratchradius;
    
    void initializevalue()
    {
        recttransform = GetComponent<RectTransform>();
        width = (int)recttransform.sizeDelta.x;
        height = (int)recttransform.sizeDelta.y;
        masktexture = new Texture2D(width, height);
        GetComponent<Image>().material.mainTexture = masktexture;
        masktexture.Apply(false);
    }
}
