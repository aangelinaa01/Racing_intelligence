using UnityEngine;

public class FinishLine : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Пересечена финишная линия игроком");
            RaceTimer timer = FindObjectOfType<RaceTimer>();
            if (timer != null)
            {
                timer.OnFinishLinePassed();
            }
        }
        else
        {
            Debug.Log("Пересечена финишная линия, но не игроком: " + other.name);
        }
    }
}
