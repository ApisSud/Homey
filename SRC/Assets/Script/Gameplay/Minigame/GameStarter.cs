using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject miniGamePanel; // ลากตัว WindowMiniGame มาใส่ในช่องนี้

    void Start()
    {
        miniGamePanel.SetActive(false); // ซ่อนเกมไว้ก่อน
    }

    void OnMouseDown() // เมื่อคลิกที่วัตถุนี้
    {
        miniGamePanel.SetActive(true); // เปิดมินิเกม
        // อาจจะ disable การควบคุมอื่นๆ ของตัวละครหลักที่นี่
    }
}
