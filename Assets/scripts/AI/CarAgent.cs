using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    public Transform target;
    public CarControl carControl;
    public Transform head;
    public Transform tail;

    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Если агент упал — обнуляем всё
        if (this.transform.localPosition.y < -0.1f)
        {
            rBody.angularVelocity = Vector3.zero;
            rBody.linearVelocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0); // лучше немного приподнять
        }

        // Случайное положение цели
        target.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Положение цели
        sensor.AddObservation(target.localPosition);

        // Положение головы и хвоста
        var headPos = transform.parent.InverseTransformPoint(head.position);
        var tailPos = transform.parent.InverseTransformPoint(tail.position);
        sensor.AddObservation(headPos);
        sensor.AddObservation(tailPos);

        // Скорость агента
        sensor.AddObservation(rBody.linearVelocity.x);
        sensor.AddObservation(rBody.linearVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        carControl.UpdateCar(controlSignal.x, controlSignal.z);

        // Проверка расстояния
        var headPos = transform.parent.InverseTransformPoint(head.position);
        var tailPos = transform.parent.InverseTransformPoint(tail.position);

        float distanceToHead = Vector3.Distance(headPos, target.localPosition);
        float distanceToTail = Vector3.Distance(tailPos, target.localPosition);

        if (distanceToHead < 2f)
        {
            SetReward(1f);
            EndEpisode();
        }
        else if (distanceToTail < 2f || this.transform.localPosition.y <= -0.1f)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}
