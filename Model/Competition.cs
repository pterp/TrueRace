using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        //constructor
        public Competition()
        {
            Participants = new List<IParticipant>();

            Tracks = new Queue<Track>();

        }
        public List<IParticipant> Participants
        {
            get;
            set;
        }
        public Queue<Track> Tracks
        {
            get;
            set;
        }

        public Track NextTrack()
        {
            if (Tracks.Count == 0)
            {
                return null;
            }
            else
            {
                Track temp = Tracks.Dequeue();
                return temp;
            }
        }
    }
}
