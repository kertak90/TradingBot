using Models.CalculateModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.SettingsModels
{
    public class MovingAverageSettings
    {
        public int SamplingWidth { get; set; }
        public Centering CenteringRule { get; set; }
        public double SmoothingConstant { get; set; }
    }
}
