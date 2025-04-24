using UnityEngine;

public class ObjectPreviewM : MonoBehaviour
{
    public void BackButtonPressed()
    {
        ObjectPrev active = ObjectPrev.GetActivePreview();
        if (active != null)
        {
            active.ReturnFromButton();
        }
    }
}
