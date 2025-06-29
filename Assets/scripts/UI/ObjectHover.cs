using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    public Outline outline; 
    
    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }
   

}