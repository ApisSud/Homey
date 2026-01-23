using UnityEngine;
using TMPro;

public class Trashmanager : MonoBehaviour
{
    public static Trashmanager instance; // ทำเป็น Singleton ให้เรียกใช้ง่ายๆ
    public TextMeshProUGUI scoreText; // ลาก Text มาใส่ตรงนี้
    public int totalTrash = 0; // จำนวนขยะทั้งหมดที่เก็บได้

    void Awake()
    {
        instance = this; // ตั้งค่าตัวมันเองเป็นตัวหลัก
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void CollectTrash()
    {
        totalTrash++; 
        UpdateScoreUI(); 
     
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Total Trash :  " + totalTrash + " / 3";
    }
}
