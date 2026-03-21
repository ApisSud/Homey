using UnityEngine;

public class ScrollBackground : MonoBehaviour
{

    public float speed;
    [SerializeField] private Renderer bgRederer;

    // Update is called once per frame
    void Update()
    {
        bgRederer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
