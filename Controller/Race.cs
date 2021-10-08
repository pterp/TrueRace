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
        const int TotalLaps = 1;
        const int SectionLength = 100;
        private static System.Timers.Timer RaceTimer;
        public static event EventHandler<RaceStartedEventArgs> RaceStarted;
        public Track track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public List<IParticipant> StaticParticipants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random { get; set; }
        private Queue<int> Scores { get; set; }


        public Race(List<IParticipant> Participantlist, Track tracki)
        {
            StaticParticipants = Participantlist;
            //create duplicate participantlist so only copy gets deleted
            Participants = new List<IParticipant>();
            foreach (IParticipant part in Participantlist)
            {
                Participants.Add(part);
            }
            track = tracki;
            _random = new Random(DateTime.Now.Millisecond);
            StartTime = DateTime.Now;
            _positions = new Dictionary<Section, SectionData>();
            _lapsCompleted = new Dictionary<IParticipant, int>();
            RemoveParticipantList = new List<IParticipant>();
            Scores = new Queue<int>();

            SetScores(Participantlist.Count);
            GiveStartingPositions(Participantlist, track);
            RandomizeEquipment();
            SetTimer();
            Start();
        }

        public void StartVisualize()
        {
            RaceStarted?.Invoke(this, new RaceStartedEventArgs() { Race = this });
        }
        private void Start()
        {
            StartVisualize();
            RaceTimer.Enabled = true;
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            RaceTimer = new System.Timers.Timer(500);
            // Hook up the Elapsed event for the timer. 
            RaceTimer.Elapsed += OnTimedEvent;
            RaceTimer.AutoReset = true;
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            FailEquipment();
            SimulateRace(Participants, track);
            FixEquipment();
        }

        public void SimulateRace(List<IParticipant> part, Track t)
        {
            bool drivermoved = false;
            foreach (IParticipant participant in part)
            {
                LinkedListNode<Section> current = t.Sections.First;
                bool notFound = true;
                //find participant on track
                while (notFound)
                {
                    //check if broken
                    if (!participant.Equipment.IsBroken)
                    {
                        SectionData CurrentSection = getSectionData(current.Value);
                        //check if participant is on currentsection
                        if (CurrentSection.Left == participant)
                        {
                            // if found on left move the participant
                            notFound = false;
                            CurrentSection.DistanceLeft += (participant.Equipment.Speed + participant.Equipment.Performance);
                            //if participant finishes current section move onto the next
                            if (CurrentSection.DistanceLeft >= SectionLength)
                            {
                                SectionData next = getSectionData(getNext(current, t).Value);
                                if (next.Right == null)
                                {
                                    next.Right = participant;
                                    next.DistanceRight = CurrentSection.DistanceLeft - SectionLength;
                                    CurrentSection.Left = null;
                                    CurrentSection.DistanceLeft = 0;
                                    drivermoved = true;
                                    IsFinish(participant, getNext(current, t).Value);
                                }
                                else if (next.Left == null)
                                {
                                    next.Left = participant;
                                    next.DistanceLeft = CurrentSection.DistanceLeft - SectionLength;
                                    CurrentSection.Left = null;
                                    CurrentSection.DistanceLeft = 0;
                                    drivermoved = true;
                                    IsFinish(participant, getNext(current, t).Value);
                                }
                                else // if next section is full stay on current section
                                {
                                    CurrentSection.DistanceLeft = SectionLength;
                                }
                            }
                        }
                        else if (CurrentSection.Right == participant)
                        {
                            // if found on left move the participant
                            notFound = false;
                            CurrentSection.DistanceRight += (participant.Equipment.Speed + participant.Equipment.Performance);
                            //if participant finishes current section move onto the next
                            if (CurrentSection.DistanceRight >= SectionLength)
                            {
                                SectionData next = getSectionData(getNext(current, t).Value);
                                if (next.Right == null)
                                {
                                    next.Right = participant;
                                    next.DistanceRight = CurrentSection.DistanceRight - SectionLength;
                                    CurrentSection.Right = null;
                                    CurrentSection.DistanceRight = 0;
                                    drivermoved = true;
                                    IsFinish(participant, getNext(current, t).Value);
                                }
                                else if (next.Left == null)
                                {
                                    next.Left = participant;
                                    next.DistanceLeft = CurrentSection.DistanceRight - SectionLength;
                                    CurrentSection.Right = null;
                                    CurrentSection.DistanceRight = 0;
                                    drivermoved = true;
                                    IsFinish(participant, getNext(current, t).Value);
                                }
                                else // if next section is full stay on current section
                                {
                                    CurrentSection.DistanceRight = SectionLength;
                                }
                            }
                        }
                        else //if not on current section continue to next section
                        {
                            current = current.Next;
                        }
                    }
                    else// if broken
                    {
                        notFound = false;
                        drivermoved = true;
                    }
                }
            }
            if (drivermoved)
            {
                OnDriversChanged();
            }
            RemoveParticipants();
            if (Participants.Count == 0)
            {
                EndRace();
            }
        }

        public void FixEquipment()
        {
            foreach (IParticipant part in Participants)
            {
                if (part.Equipment.IsBroken) 
                { 
                    if (part.Equipment.Quality >= _random.Next(0, 100))
                    {
                        part.Equipment.IsBroken = false;
                    }
                }
            }

        }
        public void FailEquipment()
        {
            foreach (IParticipant part in Participants)
            {
                if (part.Equipment.Quality < _random.Next(0,100))
                {
                    part.Equipment.IsBroken = true;
                    if (part.Equipment.Performance < 0)
                    {
                        part.Equipment.Performance--;
                    }                    
                }                
            }
        }
        // get the next section of the track
        public LinkedListNode<Section> getNext(LinkedListNode<Section> current, Track tk)
        {
            if (current.Next == null)
            {
                current = tk.Sections.First;
            }
            else
            {
                current = current.Next;
            }
            return current;
        }
        public void EndRace()
        {
            Clean();
            RaceTimer.Stop();
            Data.NextRace();
        }

        private List<IParticipant> RemoveParticipantList { get; set; }
        private Dictionary<IParticipant, int> _lapsCompleted;
        public int GetLaps(IParticipant part)
        {
            if (_lapsCompleted.ContainsKey(part))
            {
                return _lapsCompleted[part];
            }
            else
            {
                _lapsCompleted[part] = -1;
                return _lapsCompleted[part];
            }
        }
        public void SetLaps(IParticipant part, int laps)
        {
            _lapsCompleted[part] = laps;
        }

        public void IsFinish(IParticipant part, Section section)
        {
            if (section.SectionType == SectionTypes.Finish)
            {
                int laps = GetLaps(part);
                laps++;
                SetLaps(part, laps);
                //check if race finished
                if (laps >= TotalLaps)
                {
                    RemoveParticipantList.Add(part);
                    SectionData currentSection = getSectionData(section);
                    if (currentSection.Left == part)
                    {
                        currentSection.Left = null;
                    }
                    else
                    {
                        currentSection.Right = null;
                    }
                    IParticipant p = StaticParticipants.First(x => x == part);
                    p.Points += Scores.Dequeue();
                }
            }
            return;
        }

        public void RemoveParticipants()
        {
            foreach (IParticipant part in RemoveParticipantList)
            {
                Participants.Remove(part);
            }
            RemoveParticipantList.Clear();
            return;
        }


        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public void OnDriversChanged()
        {
            DriversChangedEventArgs driverArgs = new DriversChangedEventArgs();
            driverArgs.track = track;
            DriversChanged?.Invoke(this, driverArgs);
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
                            partcount++;
                        }
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

        public void RandomizeEquipment()
        {
            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].Equipment.Performance = _random.Next(1, 11);
                Participants[i].Equipment.Quality = _random.Next(85, 95);
            }
        }
        private Dictionary<Section, SectionData> _positions;
        public SectionData getSectionData(Section section)
        {
            if (_positions.ContainsKey(section))
            {
                return _positions[section];

            }
            else
            {
                SectionData nieuw = new SectionData();
                _positions[section] = nieuw;
                return _positions[section];

            }
        }
        // create points based on contestant numbers with 1st place always one point more
        public void SetScores(int length)
        {
            Scores.Enqueue(length + 1);
            length--;
            while (length > 0)
            {
                Scores.Enqueue(length);
                length--;
            }
        }

        // Cleanup events
        public void Clean()
        {
            DriversChanged = null;
        }
    }
}
