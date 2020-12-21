using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SectionData
    {
        //constructor
        public SectionData()
        {

        }
        /*
        public SectionData(IParticipant L, IParticipant R, int DL, int DR)
        {
            Left = L;
            DistanceLeft = DL;
            Right = R;
            DistanceRight = DR;

        }
        */
        public IParticipant Left
        {
            get;
            set;
        }
        public int DistanceLeft
        {
            get;
            set;
        }
        public IParticipant Right
        {
            get;
            set;
        }
        public int DistanceRight
        {
            get;
            set;
        }
    }
}
