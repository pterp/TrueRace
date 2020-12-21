﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Car : IEquipment
    {
        public Car(int qual, int perf, int speed)
        {
            Quality = qual;
            Performance = perf;
            Speed = speed;
            IsBroken = false;
        }

        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
    }
}
