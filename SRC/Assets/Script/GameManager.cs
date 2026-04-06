using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        else if(level == Level.witch)
        {
            floor2 = (gridPosition.x >= 2 && gridPosition.x <= 5 &&
               gridPosition.y >= -10 && gridPosition.y <= -7) | (gridPosition.x >= 4 && gridPosition.x <= 7 &&
               gridPosition.y >= -2 && gridPosition.y <= 1) | (gridPosition.x >= -2 && gridPosition.x <= 7 &&
               gridPosition.y >= 2 && gridPosition.y <= 5 ) ;
        }

            bool mainArea = gridPosition.x >= minX && gridPosition.x <= maxX && gridPosition.y >= minY && gridPosition.y <= maxY;

        return mainArea | floor2;


    }

 


}
