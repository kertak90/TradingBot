using Models.Calculate;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovingAverage.Models
{
    public class SimpleMovingAverageSettings
    {
        public int SamplingWidth { get; set; }
        public Centering CenteringRule { get; set; }
    }
}
