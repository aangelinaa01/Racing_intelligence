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
    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null) outline.enabled = false;
    }
    //void Update()
    //{
    //    if (outline != null)
    //        Debug.Log(gameObject.name + ": Outline enabled = " + outline.enabled);
    //}


}