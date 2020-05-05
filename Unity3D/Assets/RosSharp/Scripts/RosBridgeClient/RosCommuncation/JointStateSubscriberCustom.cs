using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using MessageTypes = RosSharp.RosBridgeClient.MessageTypes;
using UnityEngine;

public class JointStateSubscriberCustom : UnitySubscriber<MessageTypes.Sensor.JointState>
{

    [HideInInspector] public string[] JointNames;
    [HideInInspector] public float[] Velocities;
    [HideInInspector] public float[] Positions;
    [HideInInspector] public float[] Efforts;
    [HideInInspector] public bool MessageReceived = false;

    protected override void ReceiveMessage(MessageTypes.Sensor.JointState message)
    {
        if (!MessageReceived)
            MessageReceived = true;

        JointNames = new string[message.name.Length];
        Velocities = new float[message.name.Length];
        Positions = new float[message.name.Length];
        Efforts = new float[message.name.Length];

        for (int i = 0; i < message.name.Length; i++)
        {
            JointNames[i] = message.name[i];
            Velocities[i] = (float) message.velocity[i];
            Positions[i] = (float) message.position[i];
            Efforts[i] = (float) message.effort[i];
        }
    }
}