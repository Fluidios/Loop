using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Game.NeuralNet
{
    public class NetworkTester : MonoBehaviour
    {
        [SerializeField] private Network.InputData[] _inputs;
        private Network _network;
        private Network Network
        {
            get
            {
                if (_network == null)
                    _network = GetComponent<Network>();
                return _network;
            }
        }

        [Button("Test network")]
        public void RunNetworkTest()
        {
            Network.RunNetwork(ref _inputs, true);
        }
    }
}
