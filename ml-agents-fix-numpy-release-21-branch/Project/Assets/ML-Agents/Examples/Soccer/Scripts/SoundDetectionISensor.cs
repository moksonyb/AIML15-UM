using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;

public class SoundDetectionISensor : ISensor
{
    private string sensorName;
    private int maxObjects;
    private float detectionRadius;
    private LayerMask detectableLayer;
    private Transform sensorTransform;

    public SoundDetectionISensor(string sensorName, int maxObjects, float detectionRadius, LayerMask layer, Transform sensorTransform)
    {
        this.sensorName = sensorName;
        this.maxObjects = maxObjects;
        this.detectionRadius = detectionRadius;
        this.detectableLayer = layer;
        this.sensorTransform = sensorTransform;
    }

    public ObservationSpec GetObservationSpec() => ObservationSpec.Vector(maxObjects * 6);

    public CompressionSpec GetCompressionSpec() => CompressionSpec.Default();

    public void Update() { }

    public int[] GetObservationShape() => new int[] { maxObjects * 6 };

    public string GetName() => sensorName;

    public void Reset() { }

    public byte[] GetCompressedObservation() => null;

    public int Write(ObservationWriter writer)
    {
        Collider[] detectedObjects = Physics.OverlapSphere(sensorTransform.position, detectionRadius, detectableLayer);
        int index = 0;

        foreach (var obj in detectedObjects)
        {
            if (obj.transform == sensorTransform) continue;
            if (index >= maxObjects * 6) break;

            Vector3 direction = (obj.transform.position - sensorTransform.position).normalized;
            float distance = Vector3.Distance(sensorTransform.position, obj.transform.position);
            float objectType = GetObjectType(obj);

            writer[index++] = direction.x;
            writer[index++] = direction.y;
            writer[index++] = direction.z;
            writer[index++] = distance;
            writer[index++] = objectType;
            writer[index++] = 0;

            if (index >= maxObjects * 6) break;
        }

        // Fill remaining slots with zero if fewer objects are detected than maxObjects
        while (index < maxObjects * 6)
        {
            writer[index++] = 0f;
        }

        return maxObjects * 6;
    }

    private float GetObjectType(Collider obj)
    {
        if (obj.CompareTag("ball")) return 1f;
        if (obj.CompareTag("blueAgent")) return 2f;
        if (obj.CompareTag("purpleAgent")) return 2f;

        return 0f;
    }
}
