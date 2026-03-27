using UnityEngine;

public enum Level
{
    fairy,
    witch
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private Level level;
    private bool floor2;
    [SerializeField] int minX = -6, maxX = 2;
    [SerializeField] int minY = -6, maxY = 2;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsWithinBounds(Vector3Int gridPosition)
    {
        if(level == Level.fairy)
        {
                floor2 = (gridPosition.x >= 0 && gridPosition.x <= 15 &&
                gridPosition.y >= 8 && gridPosition.y <= 13) | (gridPosition.x >= 10 && gridPosition.x <= 15 &&
                gridPosition.y >= -4 && gridPosition.y <= 7);
        }

        bool mainArea = gridPosition.x >= minX && gridPosition.x <= maxX && gridPosition.y >= minY && gridPosition.y <= maxY;

        return mainArea | floor2;


    }



}
