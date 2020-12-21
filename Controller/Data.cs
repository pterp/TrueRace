using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition race1;
        public static Race currentRace;

        public static void initialize()
        {            
            race1 = new Competition();
            addparticipants();
            addTracks();
        }

        public static void addparticipants()
        {
            IEquipment car1 = new Car(100, 100, 100);
            IEquipment car2 = new Car(100, 100, 100);
            IEquipment car3 = new Car(100, 100, 100);
            IParticipant part1 = new Driver("Bert",0,car1, Teamcolors.Green);
            IParticipant part2 = new Driver("Ernie", 0, car2, Teamcolors.Yellow);
            IParticipant part3 = new Driver("Pino", 0, car3, Teamcolors.Yellow);
            List<IParticipant> list = new List<IParticipant>
            {
                part1,
                part2,
                part3
            };
            race1.Participants = list;
        }
        static void addTracks()
        {
            /*
            Section Straight1 = new Section(SectionTypes.Straight);
            Section finish1 = new Section(SectionTypes.Finish);
            Section startgrid = new Section(SectionTypes.StartGrid);
            Section right = new Section(SectionTypes.RightCorner);
            Section left = new Section(SectionTypes.LeftCorner);
            
            Section[] sectarr = { startgrid, startgrid, finish1, left, left, Straight1, Straight1, Straight1, left, left };
            Track track1 = new Track("simpel",sectarr);

            Section[] sectarr2 = { startgrid, startgrid, finish1, left, Straight1, left, left, right, Straight1, right, left, Straight1, left };
            Track track2 = new Track("bochtjes", sectarr2);
            */

            SectionTypes[] sectarr1 = { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, 
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.LeftCorner} ;
            Track track1 = new Track("simpel", sectarr1);

            SectionTypes[] sectarr2 = { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.LeftCorner,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.LeftCorner};
            Track track2 = new Track("bochtjes", sectarr2);
            Queue<Track> q = new Queue<Track>();
            q.Enqueue(track2);
            q.Enqueue(track1);

            race1.Tracks = q;


        }

        public static void NextRace()
        {
            Track temp = race1.NextTrack();
            if (temp != null) { 
                currentRace = new Race(race1.Participants, temp);
            }
        }
    }
}
