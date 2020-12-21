using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Timers;
using Model;

namespace Controller
{
    public class Race
    {
        public Race(List<IParticipant> Participantlist, Track tracki)
        {
            Participants = Participantlist;
            track = tracki;
            _random = new Random(DateTime.Now.Millisecond);
            StartTime = DateTime.Now;
            _positions = new Dictionary<Section, SectionData>();

            GiveStartingPositions(Participants, track);
            SetTimer();
            Start();
        }

        public Track track
        {
            get;
            set;
        }

        public List<IParticipant> Participants
        {
            get;
            set;
        }

        DateTime StartTime
        {
            get;
            set;
        }

        private Random _random
        {
            get;
            set;
        }

        private static System.Timers.Timer RaceTimer;

        private static void Start()
        {
            RaceTimer.Enabled = true;
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            RaceTimer = new System.Timers.Timer(500);
            // Hook up the Elapsed event for the timer. 
            RaceTimer.Elapsed += OnTimedEvent;
            RaceTimer.AutoReset = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }
        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public void OnDriversChanged()
        {
            DriversChangedEventArgs driverArgs = new DriversChangedEventArgs();
            driverArgs.track = track;
            //EventHandler handler = DriversChanged;
            Console.WriteLine("ondriverschanged");
            DriversChanged.Invoke(this, driverArgs);
        }


        public void GiveStartingPositions(List<IParticipant> part, Track t)
        {

            LinkedList<Section> Sectiontemplist = t.Sections;
            LinkedListNode<Section> current = Sectiontemplist.Last;
            IParticipant[] partArray = new IParticipant[part.Count];
            int i = 0;
            foreach (IParticipant p in part)
            {
                partArray[i] = p;
                i++;
            }
            int count = 0;
            int partcount = 0;
            while (count < Sectiontemplist.Count)
            {
                while (partcount < part.Count)
                {                
                    if (current.Value.SectionType == SectionTypes.StartGrid)
                    {
                        SectionData tempData = getSectionData(current.Value);
                        if (tempData.Right == null)
                        {
                            tempData.Right = partArray[partcount];
                            tempData.DistanceRight = 1;
                            partcount++;                        }
                        else
                        {
                            tempData.Left = partArray[partcount];
                            tempData.DistanceLeft = 0;                                                        
                            count++;
                            partcount++;
                            current = current.Previous;
                        }

                    }
                    else
                    {
                        current = current.Previous;
                        count++;
                    }
                }
                count++;                
            }
        }

        public void RandomizeEquipment(){
            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].Equipment.Performance = _random.Next(1, 11);
                Participants[i].Equipment.Quality = _random.Next(1, 11);
            }
        }
        private Dictionary<Section, SectionData> _positions;
        public SectionData getSectionData(Section section)
        {
            if (_positions.ContainsKey(section))
            {
                return _positions[section];
                
            }
            else {
                SectionData nieuw = new SectionData();
                _positions[section] = nieuw;
                return _positions[section];
                
            }
        }
    }
}
