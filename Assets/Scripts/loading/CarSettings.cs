// CarSettings.cs
using UnityEngine;

[CreateAssetMenu(fileName = "CarSettings", menuName = "Car/CarSettings")]
public class CarSettings : ScriptableObject
{
    public int carIndex;
    public int colorIndex;
    public int tireIndex;
    public int engineIndex;
}