using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Input
{
    public partial class @GameControls
    {
        public static GameControls Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameControls();
                    _instance.Enable();
                }
                return _instance;
            }
        }

        private static @GameControls _instance;
    }
}
