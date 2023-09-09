using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace Game.NeuralNet
{
    public class Network : MonoBehaviour
    {
        [SerializeField] private InputData[] _inputs;

        [SerializeField, ReadOnly] private Layer[] _netLayers;

        [Button("Run Network")]
        public void RunNetwork()
        {
            if (_inputs.Length == 0)
                Debug.LogWarning("Could not run network without input data!");
            else if(_netLayers.Length == 0)
                Debug.LogWarning("Could not run network without at least one layer!");
            else
            {
                var inputs = Enumerable.Range(0, _inputs.Length).Select(x => _inputs[x].Value).ToArray();
                Debug.Log("Answer = " + RunLayer(0, inputs)[0].ToString());
            }
        }

        private float[] RunLayer(int layerIndex, float[] inputs)
        {
            if(layerIndex < _netLayers.Length)
            {
                Debug.Log("Run layer " + layerIndex + ":");
                float[] inputsForNextLayer = _netLayers[layerIndex].Run(inputs);
                
                return RunLayer(layerIndex + 1, inputsForNextLayer);
            }
            else
            {
                return inputs;
            }
        }

        [Button("Set Layers for Task1")]
        private void Task1()
        {
            _inputs = new InputData[3]
            {
                new InputData("Vodka", 0),
                new InputData("Rain", 0),
                new InputData("Friend", 1)
            };

            ActivationFunctionBase activationFunction = new Step(0.5f);

            _netLayers = new Layer[2];

            _netLayers[0] = new Layer
                (
                    new float[2][]
                    {
                        new float[3]{0.25f, 0.25f, 0},
                        new float[3]{0.5f, -0.4f, 0.9f}
                    },
                    activationFunction.Function
                );
            _netLayers[1] = new Layer
                (
                    new float[1][]
                    {
                        new float[2]{-1,1}
                    },
                    activationFunction.Function
                );
        }

        [System.Serializable]
        struct InputData
        {
            public string Name;
            public float Value;

            public InputData(string name, float value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}
