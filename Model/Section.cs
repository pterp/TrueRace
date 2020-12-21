using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum SectionTypes
    {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish,
        Empty
    }
    public class Section
    {
        //constructor
        public Section(SectionTypes type)
        {
            SectionType = type;
        }

        public SectionTypes SectionType
        {
            get;
            set;
        }
    }
}
