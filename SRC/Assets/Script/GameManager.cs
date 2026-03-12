using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int minX = -6, maxX = 2;
    [SerializeField] int minY = -6, maxY = 2;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsWithinBounds(Vector3Int gridPosition)
    {

        return (gridPosition.x >= minX && gridPosition.x <= maxX &&
                gridPosition.y >= minY && gridPosition.y <= maxY);
    }
}
