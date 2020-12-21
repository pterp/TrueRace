using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Model
{
    public class Driver : IParticipant
    {
         public Driver(string a, int b, IEquipment c, Teamcolors d)
        {
            Name = a;
            Points = b;
            Equipment = c;
            Teamcolor = d;
        }
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public Teamcolors Teamcolor { get; set; }
    }
}
