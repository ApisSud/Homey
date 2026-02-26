    using UnityEngine;
using static Trashobject;

public class Trashobject : MonoBehaviour
{
    public enum TrashType
    {
        water,
        glass,
        bags,

    }

    public GameObject Trashparticle;
    [SerializeField]
    private TrashType Type;

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
        if (Type == TrashType.water)
        {
            SoundManager.instance.playSFX(SoundManager.instance.Dirtywater);
        }
        Destroy(gameObject);
    }

   

}

