using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace Game.NeuralNet
{
    public class Network : MonoBehaviour
    {
        [SerializeField, ReadOnly] private InputData[] _inputs;

        [SerializeField, ReadOnly] private Layer[] _netLayers;

        internal Layer[] NetLayers
        {
            get => _netLayers;
            set => _netLayers = value;
        }

        public float RunNetwork(ref InputData[] inputs, bool debug = true)
        {
            if(inputs != null)
                _inputs = inputs;
            return RunNetwork(debug);
        }
        public float RunNetwork(bool debug = true)
        {
            if (_inputs == null || _inputs.Length == 0)
            {
                if (debug)
                    Debug.LogWarning("Could not run network without input data!");
            }
            else if (_netLayers.Length == 0)
            {
                if (debug)
                    Debug.LogWarning("Could not run network without at least one layer!");
            }
            else
            {
                var inputSygnals = Enumerable.Range(0, _inputs.Length).Select(x => _inputs[x].Value).ToArray();
                float output = RunLayer(0, inputSygnals, debug)[0];
                if (debug)
                    Debug.Log("Answer = " + (output > 0.5f).ToString());

                return output > 0.5f ? 1 : 0;
            }
            return -1;
        }
        public bool RunLearningCycle(float expectedAnswer, float learningRate = 1, bool debug = true)
        {
            if(_inputs == null || _inputs.Length == 0)
            {
                Debug.LogError("Could not run network without input data!");
            }
            float existingAnswer = _netLayers[_netLayers.Length - 1][0] > 0.5 ? 1 : 0;
            if (expectedAnswer != existingAnswer)
            {
                float answerError = _netLayers[_netLayers.Length - 1][0] - expectedAnswer;
                float[] errorsInput = new float[1] { answerError };
                RunLearningCycleOnLayer(_netLayers.Length - 1, errorsInput, learningRate, debug);

                var inputSygnals = Enumerable.Range(0, _inputs.Length).Select(x => _inputs[x].Value).ToArray();
                float newOutput = RunLayer(0, inputSygnals, false)[0];
                if (debug)
                {
                    existingAnswer = newOutput > 0.5f ? 1 : 0;
                    if(existingAnswer != expectedAnswer)
                        Debug.Log(string.Format("New answer from network = {0}, but we expecting {1}", existingAnswer, expectedAnswer));
                    else
                        Debug.Log(string.Format("New answer from network = {0}, that is what we are expected :)", existingAnswer, expectedAnswer));
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        [Button("Refresh Network")]
        public void RefreshNetwork()
        {
            _inputs = new InputData[0];
            for (int i = 0; i < _netLayers.Length; i++)
            {
                _netLayers[i].Refresh();
            }
        }

        private float[] RunLayer(int layerIndex, float[] inputs, bool debug)
        {
            if(layerIndex < _netLayers.Length)
            {
                if(debug) Debug.Log("Run layer " + layerIndex + ":");
                float[] inputsForNextLayer = _netLayers[layerIndex].Run(inputs, debug);
                
                return RunLayer(layerIndex + 1, inputsForNextLayer, debug);
            }
            else
            {
                return inputs;
            }
        }
        private void RunLearningCycleOnLayer(int layerIndex, float[] errorsFromPreviousLayer, float learningRate, bool debug)
        {
            if (layerIndex > -1)
            {
                var errors = _netLayers[layerIndex].Learn(errorsFromPreviousLayer, learningRate, debug);
                RunLearningCycleOnLayer(layerIndex -1, errors, learningRate, debug);
            }
            else
            {
                Debug.Log("Learning reached first layer.");
            }
        }
        [System.Serializable]
        public struct InputData
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
