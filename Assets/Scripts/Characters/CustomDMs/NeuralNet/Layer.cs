using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NeuralNet
{
    [System.Serializable]
    public struct Layer
    {
        [SerializeField, ReadOnly] private Neuron[] _neurons;

        public float this[int index]
        {
            get { return _neurons[index].CalculatedValue; }
        }

        public Layer(float[][] initialNeuronWeights, ActivationFunctionBase activationFunction)
        {
            _neurons = new Neuron[initialNeuronWeights.GetLength(0)];

            for (int i = 0; i < initialNeuronWeights.GetLength(0); i++)
            {
                _neurons[i] = new Neuron(initialNeuronWeights[i], activationFunction, string.Format("Neuron {0}",i));
            }
        }

        public float[] Run(float[] inputSignals, bool debug = true)
        {
            if(debug) Debug.Log("Layer inputs: " + ArrayToString(inputSignals));

            float[] layerOutput = new float[_neurons.Length];

            for (int i = 0; i < _neurons.Length; i++)
            {
                layerOutput[i] = _neurons[i].Run(inputSignals);
            }
            if(debug) Debug.Log("Layer outputs: " + ArrayToString(layerOutput));
            return layerOutput;
        }
        public float[] Learn(float[] errors, float learningRate = 1, bool debug = true)
        {
            //we expecting that every neron in layer will produce the same errors array since they would get same outputs from previous layer?
            //or we should get average from each neuron?
            return _neurons[0].Learn(errors[0], learningRate, debug);
        }

        private string ArrayToString(float[] array)
        {
            string output = string.Empty;
            foreach (var item in array) output += string.Format("{0};", item);

            return output;
        }
        public void Refresh()
        {
            for (int i = 0; i < _neurons.Length; i++)
            {
                _neurons[i].Refresh();
            }
        }
    }
}
