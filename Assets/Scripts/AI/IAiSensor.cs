using UnityEngine;

public interface IAiSensor
{
    public delegate void AlertSensor(GameObject obj);
    public event AlertSensor onSensor;
}