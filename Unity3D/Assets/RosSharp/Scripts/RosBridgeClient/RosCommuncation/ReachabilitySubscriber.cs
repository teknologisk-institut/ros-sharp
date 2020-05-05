using System;
using System.IO;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.MapCreator;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class ReachabilitySubscriber : MonoBehaviour
    {
        [SerializeField] private string topic;
        [SerializeField] private bool receiveOnce = true;
        [SerializeField] private bool serialize = true;
        [SerializeField] private ReachabilityMapData map;

        private RosConnector _rosConnector;
        private string _subscriptionId;

        protected void Start()
        {
            _rosConnector = GetComponent<RosConnector>();
            _subscriptionId = _rosConnector.RosSocket.Subscribe<WorkSpace>(topic, MessageHandler);
        }

        private void MessageHandler(WorkSpace message)
        {
            map = new ReachabilityMapData();
            map.points = new Vector3[message.WsSpheres.Length];
            map.ris = new float[message.WsSpheres.Length];

            for (int i = 0; i < message.WsSpheres.Length; i++)
            {
                map.points[i] = GetPoint(message.WsSpheres[i]).Ros2Unity();
                map.ris[i] = message.WsSpheres[i].ri;
            }

            if (serialize)
                Serialize();

            if (receiveOnce)
                _rosConnector.RosSocket.Unsubscribe(_subscriptionId);
        }

        private Vector3 GetPoint(WsSphere message)
        {
            return new Vector3(message.point.x, message.point.y, message.point.z);
        }

        private void Serialize()
        {
            string path = Application.dataPath + "/Resources";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string mapJson = JsonUtility.ToJson(map, true);
            File.WriteAllText(path + "/map.json", mapJson);

            Debug.Log("Map stored at " + path);
        }
    }
}