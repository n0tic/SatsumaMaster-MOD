using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MSCLoader;

namespace SatsumaMaster
{
    public class SixGears
    {
        SatsumaMaster modParent;

        //Six Gears
        public Drivetrain.Transmissions lastDrivetype;
        public int driveType = 1; // Default forward
        public float[] oldRatio;
        public float[] newRatio = new float[]
        {
            /**
             * Default:
             * -4.093f
             * 0f
             * 3673f
             * 2217f
             * 1456f
             * 1f
             */
            -4.093f,// reverse 
            0f,     // neutral
            3.424f, // 1st
            2.141f, // 2nd...
            1.318f, // 3nd...
            1f,     // 4nd... 
            0.90f,  // 5nd... 
            0.85f,  // 6nd... 
        };

        //SixGears
        public int rememberGear = 0; // A variable to remember the last gear used.
        public bool autoTransEnabled;

        public SixGears(SatsumaMaster _modParent) { modParent = _modParent; }
    }
}
