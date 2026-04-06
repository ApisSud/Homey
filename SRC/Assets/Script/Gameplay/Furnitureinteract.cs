using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]

public class Furnitureinteract : MonoBehaviour
{
    private Animator animator;

   

    void Start()
    {
      
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
           
            Debug.Log("rmb!");

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("play anim");
                animator.SetTrigger("Playanim");
            }
        }
    }
}
