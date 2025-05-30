using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public enum PanelType { Tires, Engine, Color } 
    public PanelType panelToOpen;
    public CarManager carManager;

    private void OnMouseDown()
    {
        if (carManager == null) return;

        switch (panelToOpen)
        {
            case PanelType.Tires:
                carManager.OpenTirePanel();
                break;
            case PanelType.Engine:
                carManager.OpenEnginePanel();
                break;
            case PanelType.Color:
                carManager.OpenColorPanel();
                break;
               
            
        }
    }
}
