    using UnityEngine;

public class Trashobject : MonoBehaviour
{
    public GameObject Trashparticle;
    void OnMouseDown()
    {
   
        if (Trashmanager.instance != null)
        {
            Trashmanager.instance.CollectTrash();
        }
        if (Trashparticle != null)
        {
            
            Instantiate(Trashparticle,transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

