using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.NeuralNet
{
    [Serializable]
    public struct Neuron
    {
        [SerializeField, ReadOnly] private string _name;
        [SerializeField, ReadOnly] private float[] _weights;

        [SerializeField, ReadOnly] private float[] _inputs;
        [SerializeField, ReadOnly] private float _weightedInputs;
        [SerializeField, ReadOnly] private float _calculatedValue;
        [SerializeField, ReadOnly] private string _activationFunctionDescription;
        private ActivationFunctionBase _activationFunction;
        private string _customNeuronName;

        public float CalculatedValue 
        { 
            get => _calculatedValue;
            set => _calculatedValue = value;
        }

        public Neuron(float[] initialWeights, ActivationFunctionBase activationFunction, string customNeuronName = "")
        {
            _customNeuronName = customNeuronName;
            _weights = initialWeights;
            _inputs = new float[initialWeights.Length];
            _weightedInputs = 0;
            _calculatedValue = 0;
            if(activationFunction != null)
            {
                _activationFunction = activationFunction;
                _activationFunctionDescription = activationFunction.ToString();
            }
            else
            {
                _activationFunction = null;
                _activationFunctionDescription = "NONE";
            }

            _name = CreateName(customNeuronName, initialWeights, _calculatedValue);
        }

        private static string CreateName(string customNeuronName, float[] initialWeights, float calculatedValue)
        {
            string name = customNeuronName + ": [";
            foreach (var item in initialWeights) name += string.Format("{0},", item);
            name = name.Remove(name.Length - 1);
            name += "] = " + calculatedValue.ToString();
            return name;
        }

        public float Run(float[] inputSignals)
        {
            if (inputSignals.Length != _weights.Length)
            {
                Debug.LogError(string.Format("Signals are not equal to weights!"));
                _inputs = new float[_weights.Length];
                _weightedInputs = 0;
                _calculatedValue = 0;
                UpdateName();
                return 0;
            }
            int minLength = Math.Min(_weights.Length, inputSignals.Length);
            _inputs = inputSignals;
            float output = 0;
            for (int i = 0; i < minLength; i++)
            {
                output += inputSignals[i] * _weights[i];
            }

            if (_activationFunction != null)
            {
                _weightedInputs = output;
                _calculatedValue = _activationFunction.Function(output);
                UpdateName();
                return _calculatedValue;
            }
            else
            {
                Debug.LogWarning(string.Format("{0} - has no activation function!", this._name));
                _weightedInputs = output;
                _calculatedValue = output;
                UpdateName();
                return output;
            }
        }
        public float[] Learn(float error, float learningRate = 1, bool debug = true)
        {
            if (_activationFunction == null)
            {
                Debug.LogWarning("Can't learn without activation function!");
                return new float[0];
            }

            float[] errors = new float[_weights.Length];
            float derivative = _activationFunction.DerivativeOfFunction(_weightedInputs);
            float weightsDelta = error * derivative;
            if (debug) Debug.Log(string.Format("{0}: error: {1}; derivative: {2}; weights_delta = {3};", _customNeuronName, error, derivative, weightsDelta));
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = _weights[i] - _inputs[i] * weightsDelta * learningRate;
                errors[i] = _weights[i] * weightsDelta;
            }
            UpdateName();
            return errors;
        }
        private void UpdateName()
        {
            _name = CreateName(_customNeuronName, _weights, _calculatedValue);
        }
        public void Refresh()
        {
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = (float)Math.Round(UnityEngine.Random.value, 2);
            }
            _calculatedValue = 0;
            UpdateName();
        }
        public override string ToString()
        {
            return _name;
        }
    }
}