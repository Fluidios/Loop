using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NeuralNet
{
    [Serializable]
    public class Sigmoid : ActivationFunctionBase
    {
        protected override float Run(float input)
        {
            return 1f / (1+(float)Math.Exp(-input));
        }
        protected override float Derivative(float input)
        {
            float run = Run(input);
            //Debug.Log(string.Format("Run({0}) = {1}", input, run));
            return run * (1f - run);
        }

        public override string ToString()
        {
            return "Sigmoid";
        }
    }
}