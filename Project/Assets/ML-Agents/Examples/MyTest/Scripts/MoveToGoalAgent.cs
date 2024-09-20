using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(8.5f, -4f), 2.913424f, Random.Range(-7.5f, 4.66f));
        targetTransform.localPosition = new(Random.Range(8.5f, -4f), 3.031968f, Random.Range(-7.5f, 4.66f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 5f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider collided)
    {
        if(collided.TryGetComponent<Wall>(out Wall wall))
        {
            Debug.Log("Here is Wall");
            SetReward(-1);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
        if (collided.TryGetComponent<Goal>(out Goal goal))
        {
            Debug.Log("Here is Goal");
            SetReward(1);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
    }
}
