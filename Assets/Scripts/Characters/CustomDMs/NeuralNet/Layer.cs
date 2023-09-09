using NaughtyAttributes;
using UnityEngine;

namespace Game.NeuralNet
{
    [System.Serializable]
    public struct Layer
    {
        [SerializeField, ReadOnly] private Neuron[] _neurons;
        public int NeuronsCount { get { return _neurons.Length; } }

        public Layer(float[][] initialNeuronWeights, ActivationFunctionDelegate activationFunction)
        {
            _neurons = new Neuron[initialNeuronWeights.GetLength(0)];

            for (int i = 0; i < initialNeuronWeights.GetLength(0); i++)
            {
                _neurons[i] = new Neuron(initialNeuronWeights[i], activationFunction, string.Format("Neuron {0}",i));
            }
        }

        public float[] Run(float[] inputSignals)
        {
            Debug.Log("Layer inputs: " + ArrayToString(inputSignals));

            float[] layerOutput = new float[_neurons.Length];

            for (int i = 0; i < _neurons.Length; i++)
            {
                layerOutput[i] = _neurons[i].Run(inputSignals);
            }
            Debug.Log("Layer outputs: " + ArrayToString(layerOutput));
            return layerOutput;
        }

        private string ArrayToString(float[] array)
        {
            string output = string.Empty;
            foreach (var item in array) output += string.Format("{0},", item);

            return output;
        }
    }
}
