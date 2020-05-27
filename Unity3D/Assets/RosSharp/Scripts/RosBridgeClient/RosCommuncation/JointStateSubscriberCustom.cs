using System.Collections.Generic;
using UnityEngine;
using RosSharp.Urdf;

namespace RosSharp.RosBridgeClient
{
    public class JointStateSubscriberCustom : UnitySubscriber<MessageTypes.Sensor.JointState>
    {
        public UrdfRobot _urdfRobot;
        public List<string> JointNames;
        public bool MessageReceived = false;

        [HideInInspector] public Dictionary<string, float> Velocities;
        [HideInInspector] public Dictionary<string, float> Positions;
        [HideInInspector] public Dictionary<string, float> Efforts;

        private void Awake()
        {
            JointNames = new List<string>();
            Velocities = new Dictionary<string, float>();
            Positions = new Dictionary<string, float>();
            Efforts = new Dictionary<string, float>();

            foreach (UrdfJoint urdfJoint in _urdfRobot.GetComponentsInChildren<UrdfJoint>())
            {
                if (urdfJoint.JointType != UrdfJoint.JointTypes.Fixed)
                {
                    JointNames.Add(urdfJoint.JointName);
                    Velocities[urdfJoint.JointName] = 0f;
                    Positions[urdfJoint.JointName] = 0f;
                    Efforts[urdfJoint.JointName] = 0f;
                }
            }
        }

        protected override void ReceiveMessage(MessageTypes.Sensor.JointState message)
        {
            if (!MessageReceived)
                MessageReceived = true;

            int index;
            for (int i = 0; i < message.name.Length; i++)
            {
                index = JointNames.IndexOf(message.name[i]);
                if (index != -1)
                {
                    Velocities[JointNames[i]] = (float) message.velocity[i];
                    Positions[JointNames[i]] = (float) message.position[i];
                    Efforts[JointNames[i]] = (float) message.effort[i];
                }
            }
        }
    }
}