using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public WheelCollider WheelCollider;
    public Transform WheelMesh;
    public bool steerable;
    public bool motorized;

    void Update()
    {
        if (WheelCollider && WheelMesh)
        {
            Vector3 pos;
            Quaternion rot;
            WheelCollider.GetWorldPose(out pos, out rot);
            WheelMesh.position = pos;
            WheelMesh.rotation = rot;
        }
    }
}
