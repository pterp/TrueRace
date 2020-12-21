using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Track
    {
        //constructor
        public Track(string n, SectionTypes[] s)
        {
            Name = n;
            Sections = new LinkedList<Section>();
            Sections = arrayToList(s);
            
        }

        public String Name
        {
            get;
            set;
        }

        public LinkedList<Section> Sections
        {
            get;
            set;
        }
        private LinkedList<Section> arrayToList(SectionTypes[] sect)
        {
            LinkedList<Section> temp = new LinkedList<Section>();
            for (int i = 0; i < sect.Length; i++)
            {
                Section tempsection = new Section(sect[i]);
                temp.AddLast(tempsection);
            }
            return temp;
        }
    }
}
