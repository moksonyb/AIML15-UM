using Unity.MLAgents.Sensors;
using UnityEngine;

[AddComponentMenu("ML Agents/Sensors/Sound Detection Sensor")]
public class SoundDetectionSensorComponent : SensorComponent
{
    [Tooltip("Name of the sensor")]
    public string sensorName = "SoundSensor";

    [Tooltip("Maximum number of objects the sensor can detect.")]
    public int maxObjects = 4;

    [Tooltip("Detection radius for sound.")]
    public float detectionRadius = 10f;

    [Tooltip("Layer mask for detectable objects.")]
    public LayerMask detectableLayer;

    public override ISensor[] CreateSensors()
    {
        return new ISensor[] { new SoundDetectionISensor(sensorName, maxObjects, detectionRadius, detectableLayer, transform) };
    }
}
