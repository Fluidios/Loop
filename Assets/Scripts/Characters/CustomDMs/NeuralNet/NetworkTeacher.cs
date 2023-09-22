using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using static Game.NeuralNet.Network;

namespace Game.NeuralNet
{
    [RequireComponent(typeof(Network))]
    public class NetworkTeacher : MonoBehaviour
    {
        [SerializeField] private LearningInputData[] _inputs;
        [SerializeField] private float _learnRate = 0.1f;
        [SerializeField] private bool _learnInOneFrame;
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

        [Button("Learn")]
        public void RunLearningCycle(bool debug = true)
        {
            //if expected answer is true, what would be expected calculated value from that layer neuron? It could be any from 0.51 to 1 
            StartCoroutine(RunLearningProcedure(debug));
        }
        [Button("Set Layers for Task1")]
        private void Task1()
        {
            var inputs = new Network.InputData[3]
            {
                new Network.InputData("Vodka", 0),
                new Network.InputData("Rain", 0),
                new Network.InputData("Friend", 1)
            };

            ActivationFunctionBase activationFunction = new Step(0.5f);

            var netLayers = new Layer[2];

            netLayers[0] = new Layer
                (
                    new float[2][]
                    {
                        new float[3]{0.25f, 0.25f, 0},
                        new float[3]{0.5f, -0.4f, 0.9f}
                    },
                    activationFunction
                );
            netLayers[1] = new Layer
                (
                    new float[1][]
                    {
                        new float[2]{-1,1}
                    },
                    activationFunction
                );

            Network.NetLayers = netLayers;
            Network.RunNetwork(ref inputs, true);
        }

        [Button("Set Layers for Task2")]
        private void Task2()
        {
            ActivationFunctionBase activationFunction = new Sigmoid();

            var netLayers = new Layer[2];

            netLayers[0] = new Layer
                (
                    new float[2][]
                    {
                        new float[3]{ 0.79f, 0.44f, 0.43f },
                        new float[3]{ 0.85f, 0.43f, 0.29f }
                    },
                    activationFunction
                );
            netLayers[1] = new Layer
                (
                    new float[1][]
                    {
                        new float[2]{0.5f, 0.52f }
                    },
                    activationFunction
                );
            Network.NetLayers = netLayers;
        }

        IEnumerator RunLearningProcedure(bool debug = true)
        {
            float answerFromNetwork;
            float expectedAnswer;
            int learningIteration;
            var startTime = DateTime.Now;
            for (int i = 0; i < _inputs.Length; i++)
            {
                answerFromNetwork = Network.RunNetwork(ref _inputs[i].InputData, false);
                expectedAnswer = _inputs[i].ExpectedAnswer;
                if (answerFromNetwork != expectedAnswer)
                {
                    learningIteration = 0;
                    while (!Network.RunLearningCycle(_inputs[i].ExpectedAnswer, _learnRate, false))
                    {
                        Debug.Log("Run learning iteration: " + learningIteration);
                        learningIteration++;
                        if(!_learnInOneFrame) yield return null;
                    }
                    Debug.Log(string.Format("Solution for input {0} - found. Moving to start...", i));
                    i = -1;
                }
                else
                {
                    Debug.Log(string.Format("Input {0} - passed", i));
                }
            }
            Debug.Log("Learning - ended. Time spend: " + (DateTime.Now.Subtract(startTime)).TotalSeconds);
        }

        [System.Serializable]
        class LearningInputData
        {
            [SerializeField] private bool _expectedAnswer;
            public float ExpectedAnswer => _expectedAnswer ? 1 : 0;
            public InputData[] InputData;
        }
    }
}
