using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;

namespace Game.NeuralNet
{
    [Serializable]
    public struct Neuron
    {
        [SerializeField, ReadOnly] private string _name;
        [SerializeField, ReadOnly] private float[] _weights;
        private ActivationFunctionDelegate _activationFunction;

        public Neuron(float[] initialWeights, ActivationFunctionDelegate activationFunction, string customNeuronName = "")
        {
            _name = CreateName(customNeuronName, initialWeights);
            _weights = initialWeights;
            _activationFunction = activationFunction;
        }

        private static string CreateName(string customNeuronName, float[] initialWeights)
        {
            string name = customNeuronName + ": [";
            foreach (var item in initialWeights) name += string.Format("{0},", item);
            name = name.Remove(name.Length - 1);
            name += "]";
            return name;
        }

        public float Run(float[] inputSignals)
        {
            if (inputSignals.Length < _weights.Length)
            {
                Debug.LogError(string.Format("Not enough weights! More signals then neuron can handle!"));
                return 0;
            }
            int minLength = Math.Min(_weights.Length, inputSignals.Length);
            float output = 0;
            for (int i = 0; i < minLength; i++)
            {
                output += inputSignals[i] * _weights[i];
            }

            if (_activationFunction != null)
                return _activationFunction(output);
            else
                return output;
        }
    }
}