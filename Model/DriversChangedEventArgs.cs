using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DriversChangedEventArgs : EventArgs
    {
        public Track track
        {
            get;
            set;
        }


    }

}
