using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SoundDetectionISensor : ISensor
{
    private string sensorName;
    private int maxObjects;
    private float detectionRadius;
    private LayerMask detectableLayer;
    private Transform sensorTransform;
    private const int maxParametersPerObjectObservation = 3;

    // Reuse the list to avoid frequent allocations
    private List<float> observationList = new List<float>();

    public SoundDetectionISensor(string sensorName, int maxObjects, float detectionRadius, LayerMask layer, Transform sensorTransform)
    {
        this.sensorName = sensorName;
        this.maxObjects = maxObjects;
        this.detectionRadius = detectionRadius;
        this.detectableLayer = layer;
        this.sensorTransform = sensorTransform;
    }

    public ObservationSpec GetObservationSpec() => ObservationSpec.Vector(maxObjects * maxParametersPerObjectObservation);

    public CompressionSpec GetCompressionSpec() => CompressionSpec.Default();

    public void Update() { }

    public int[] GetObservationShape() => new int[] { maxObjects * maxParametersPerObjectObservation };

    public string GetName() => sensorName;

    public void Reset() { }

    public byte[] GetCompressedObservation() => null;

    public int Write(ObservationWriter writer)
    {
        // Clear the list at the start
        observationList.Clear();

        // Detect objects and sort by type, excluding the sensor itself
        Collider[] detectedObjects = Physics.OverlapSphere(sensorTransform.position, detectionRadius, detectableLayer)
                                            .Where(obj => obj.transform != sensorTransform)
                                            .OrderBy(obj => GetObjectType(obj))
                                            .ToArray();

        int objectsToProcess = Mathf.Min(detectedObjects.Length, maxObjects);

        for (int i = 0; i < objectsToProcess; i++)
        {
            var obj = detectedObjects[i];
            Vector3 direction = (obj.transform.position - sensorTransform.position).normalized;
            float distance = Vector3.Distance(sensorTransform.position, obj.transform.position);
            float objectType = GetObjectType(obj);

            // Add observation data to the list
            observationList.Add(direction.x);
            // Uncomment if you decide to include the Y component
            // observationList.Add(direction.y);
            observationList.Add(direction.z);
            observationList.Add(distance);
            // Uncomment if you decide to include object type
            // observationList.Add(objectType);
            // observationList.Add(0f); // Padding or additional parameters if needed
        }

        // Fill remaining slots with zeros if fewer objects are detected than maxObjects
        for (int i = objectsToProcess; i < maxObjects; i++)
        {
            observationList.Add(0f);
            observationList.Add(0f);
            observationList.Add(0f);

            // Optional: Collect padding data for debugging
            // observationList.Add(0f);
            // observationList.Add(0f);
            // observationList.Add(0f);
        }

        // Write all observations at once
        writer.AddList(observationList);

        // Debug Log: Print the final observation content
        // To prevent performance issues during training, consider enabling this only in development builds
#if UNITY_EDITOR
        string observationContent = string.Join(", ", observationList.Select(val => val.ToString("F2")));
        Debug.Log($"[SoundDetectionISensor] Observation Content: [{observationContent}]");
#endif

        return maxObjects * maxParametersPerObjectObservation;
    }

    private float GetObjectType(Collider obj)
    {
        if (obj.CompareTag("ball")) return 1f;
        if (obj.CompareTag("blueAgent")) return 2f;
        if (obj.CompareTag("purpleAgent")) return 3f;

        return 0f;
    }
}
