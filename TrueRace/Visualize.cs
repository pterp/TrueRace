using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;

namespace TrueRace
{
    public static class Visualize
    {
        public static void Initialize()
        {
           Race.RaceStarted += OnNewRace;
            //Data.currentRace.DriversChanged += OnDriversChanged;
        }

        public static void OnNewRace(object o, RaceStartedEventArgs e)
        {
            e.Race.DriversChanged += OnDriversChanged;
        }
        
        public static void OnDriversChanged(object o, DriversChangedEventArgs args)
        {
            DrawTrack(args.track);
        }


        private const int V = 5;
        #region graphics
        //N = North 0
        //E = East 1
        //S = South 2
        //W = West 3
        private static string[] _finishE = { 
            "oooo", 
            " 2# ", 
            " 1# ", 
            "oooo" };
        private static string[] _StartgridE = { 
            "oooo", 
            "2|  ", 
            "  1|", 
            "oooo" };
        private static string[] _bendNW = { 
            "o  o", 
            " 2 o", 
            "  1o", 
            "oooo" };
        private static string[] _bendNE = { 
            "o  o", 
            "o 2 ", 
            "o1  ", 
            "oooo" };
        private static string[] _bendSW = { 
            "oooo", 
            "  1o", 
            " 2 o", 
            "o  o" };
        private static string[] _bendSE = { 
            "oooo", 
            "o1  ", 
            "o 2 ", 
            "o  o" };
        private static string[] _straightEW = { 
            "oooo", 
            " 2  ", 
            "  1 ", 
            "oooo" };
        private static string[] _straightNS = { 
            "o  o", 
            "o2 o", 
            "o 1o", 
            "o  o" };
        private static string[] _empty = { "    ", "    ", "    ", "    " };
        //private static string[] _straightNS = { "o  o", "o  o", "o  o", "o  o", };
        #endregion
        public static void DrawTrack(Track t)
        {            
            int Direction = 1;
            int[] parameters = FindDrawParameters(t, Direction);
            string[,] stringarray = CreateDrawTrack(t, Direction, parameters);
            Draw(parameters, stringarray);

        }
        private static int[] FindDrawParameters(Track t, int Direction)
        {
            int x = 0, y = 0, xmax = 0, xmin = 0, ymax = 0, ymin = 0;
            LinkedList<Section> drawList = t.Sections;
            //find array bounds
            foreach (Section sect in drawList)
            {
                if (sect.SectionType == SectionTypes.Straight || sect.SectionType == SectionTypes.Finish || sect.SectionType == SectionTypes.StartGrid)
                {
                    if (Direction == 0)
                    {
                        y--;
                        if (y < ymin)
                        {
                            ymin = y;
                        }
                    }
                    else if (Direction == 1)
                    {
                        x++;
                        if (x > xmax)
                        {
                            xmax = x;
                        }
                    }
                    else if (Direction == 2)
                    {
                        y++;
                        if (y > ymax)
                        {
                            ymax = y;
                        }
                    }
                    else if (Direction == 3)
                    {
                        x--;
                        if (x < xmin)
                        {
                            xmin = x;
                        }
                    }


                }
                else if (sect.SectionType == SectionTypes.LeftCorner)
                {
                    if (Direction == 0)
                    {
                        x--;
                        if (x < xmin)
                        {
                            xmin = x;
                        }
                    }
                    else if (Direction == 1)
                    {
                        y--;
                        if (y < ymin)
                        {
                            ymin = y;
                        }
                    }
                    else if (Direction == 2)
                    {
                        x++;
                        if (x > xmax)
                        {
                            xmax = x;
                        }
                    }
                    else if (Direction == 3)
                    {
                        y++;
                        if (y > ymax)
                        {
                            ymax = y;
                        }
                    }
                    Direction--;
                    if (Direction < 0) { Direction = 3; }
                }
                else if (sect.SectionType == SectionTypes.RightCorner)
                {
                    if (Direction == 0)
                    {
                        x++;
                        if (x > xmax)
                        {
                            xmax = x;
                        }
                    }
                    else if (Direction == 1)
                    {
                        y++;
                        if (y > ymax)
                        {
                            ymax = y;
                        }
                    }
                    else if (Direction == 2)
                    {
                        x--;
                        if (x < xmin)
                        {
                            xmin = x;
                        }
                    }
                    else if (Direction == 3)
                    {
                        y--;
                        if (y < ymin)
                        {
                            ymin = y;
                        }
                    }
                    Direction++;
                    if (Direction > 3) { Direction = 0; }
                }
            }

            int startx = getStartcoordinate(xmin);
            int starty = getStartcoordinate(ymin);

            xmax = AdjustMax(xmax, startx);
            ymax = AdjustMax(ymax, starty);

            int[] parameters = { startx, starty, xmax, ymax};
            return parameters;
        }

        private static string[,] CreateDrawTrack(Track t, int Direction, int[] parameters)
        {            
            int startx = parameters[0];
            int starty = parameters[1];
            int xmax = parameters[2];
            int ymax = parameters[3];
            
            LinkedList<Section> drawList = t.Sections;

            string[,] stringarray = new string[(ymax) * 4, (xmax)];

            //fill array with empty spaces
            for (int i = 0; i < (ymax)*4; i++)
            {
                for (int a = 0; a < xmax; a++)
                {
                    stringarray[i, a] = "    ";
                }
            }
            int x, y;
            x = startx; y = starty;
            //create array of strings
            foreach (Section sect in drawList)
            {
                SectionData tempSectionData = Data.currentRace.getSectionData(sect);
                IParticipant part1 = tempSectionData.Right;
                IParticipant part2 = tempSectionData.Left;
                if (sect.SectionType == SectionTypes.Straight)
                {
                    
                    if (Direction == 0)
                    {
                        stringarray[y * 4, x] = AddParticipants(_straightNS[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_straightNS[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_straightNS[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_straightNS[3], part1, part2);
                        y--;
                    }
                    else if (Direction == 1)
                    {
                        stringarray[y * 4, x] = AddParticipants(_straightEW[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_straightEW[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_straightEW[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_straightEW[3], part1, part2);
                        x++;
                    }
                    else if (Direction == 2)
                    {
                        stringarray[y * 4, x] = AddParticipants(_straightNS[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_straightNS[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_straightNS[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_straightNS[3], part1, part2);
                        y++;
                    }
                    else if (Direction == 3)
                    {
                        stringarray[y * 4, x] = AddParticipants(_straightEW[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_straightEW[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_straightEW[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_straightEW[3], part1, part2);
                        x--;
                    }

                }
                else if (sect.SectionType == SectionTypes.Finish)
                {
                    if (Direction == 0)
                    {
                        stringarray[y * 4, x] = AddParticipants(_finishE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_finishE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_finishE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_finishE[3], part1, part2);
                        y--;
                    }
                    else if (Direction == 1)
                    {
                        stringarray[y * 4, x] = AddParticipants(_finishE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_finishE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_finishE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_finishE[3], part1, part2);
                        x++;
                    }
                    else if (Direction == 2)
                    {
                        stringarray[y * 4, x] = AddParticipants(_finishE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_finishE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_finishE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_finishE[3], part1, part2);
                        y++;
                    }
                    else if (Direction == 3)
                    {
                        stringarray[y * 4, x] = AddParticipants(_finishE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_finishE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_finishE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_finishE[3], part1, part2);
                        x--;
                    }
                }
                else if (sect.SectionType == SectionTypes.StartGrid)
                {
                    if (Direction == 0)
                    {
                        
                        stringarray[y * 4, x] = AddParticipants(_StartgridE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_StartgridE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_StartgridE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_StartgridE[3], part1, part2);
                        y--;
                    }
                    else if (Direction == 1)
                    {
                        stringarray[y * 4, x] = AddParticipants(_StartgridE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_StartgridE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_StartgridE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_StartgridE[3], part1, part2);
                        x++;
                    }
                    else if (Direction == 2)
                    {
                        stringarray[y * 4, x] = AddParticipants(_StartgridE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_StartgridE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_StartgridE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_StartgridE[3], part1, part2);
                        y++;
                    }
                    else if (Direction == 3)
                    {
                        stringarray[y * 4, x] = AddParticipants(_StartgridE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_StartgridE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_StartgridE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_StartgridE[3], part1, part2);
                        x--;
                    }
                }
                else if (sect.SectionType == SectionTypes.LeftCorner)
                {
                    if (Direction == 0)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendSW[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendSW[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendSW[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendSW[3], part1, part2);
                        x--;
                    }
                    else if (Direction == 1)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendNW[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendNW[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendNW[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendNW[3], part1, part2);
                        y--;
                    }
                    else if (Direction == 2)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendNE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendNE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendNE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendNE[3], part1, part2);
                        x++;
                    }
                    else if (Direction == 3)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendSE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendSE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendSE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendSE[3], part1, part2);
                        y++;
                    }
                    Direction--;
                    if (Direction < 0) { Direction = 3; }

                }
                else if (sect.SectionType == SectionTypes.RightCorner)
                {
                    if (Direction == 0)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendSE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendSE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendSE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendSE[3], part1, part2);
                        x++;
                    }
                    else if (Direction == 1)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendSW[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendSW[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendSW[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendSW[3], part1, part2);
                        y++;
                    }
                    else if (Direction == 2)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendNW[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendNW[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendNW[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendNW[3], part1, part2);
                        x--;
                    }
                    else if (Direction == 3)
                    {
                        stringarray[y * 4, x] = AddParticipants(_bendNE[0], part1, part2);
                        stringarray[y * 4 + 1, x] = AddParticipants(_bendNE[1], part1, part2);
                        stringarray[y * 4 + 2, x] = AddParticipants(_bendNE[2], part1, part2);
                        stringarray[y * 4 + 3, x] = AddParticipants(_bendNE[3], part1, part2);
                        y--;
                    }
                    Direction++;
                    if (Direction > 3) { Direction = 0; }

                }
            }
            /*
            //print array of strings
            for (y = 0; y < (ymax)*4; y++)
            {                
                for (x = 0; x < xmax; x++)
                {
                    Console.Write(stringarray[y,x]);
                }
                Console.WriteLine();
            }
            */
            return stringarray;
        }

        private static string AddParticipants (string s, IParticipant part1, IParticipant part2)
        {
            if (s.Contains('1') || s.Contains('2'))
            {
                if (s.Contains('1') && part1 != null)
                {
                    char[] temp = part1.Name.ToCharArray();
                    s = s.Replace('1', temp[0]);
                }
                else
                {
                    s = s.Replace('1', ' ');
                }
                if (s.Contains('2') && part2 != null)
                {
                    char[] temp = part2.Name.ToCharArray();
                    s = s.Replace('2', temp[0]);
                }
                else
                {
                    s = s.Replace('2', ' ');
                }
            }
            return s;
        }
        private static void Draw(int[] parameters, string[,] stringarray)
        {
            int xmax = parameters[2];
            int ymax = parameters[3];
            //clear console
            Console.Clear();
            //write the track to console
            for (int y = 0; y < (ymax) * 4; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    Console.Write(stringarray[y, x]);
                }
                Console.WriteLine();
            }
        }

        private static int getStartcoordinate(int min)
        {
            int start = 0;
            while (min < 0)
            {
                start++;
                min++;
            }
            return start;
        }

        private static int AdjustMax(int max, int adjust)
        {
            for (int i = 0; i < adjust; i++)
            {
                max++;
            }
            return max + 1;
        }

    }

}














/*garbage
 *             foreach (Section sect in drawList)
            {
                if(sect.SectionType == SectionTypes.Straight || sect.SectionType == SectionTypes.Finish || sect.SectionType == SectionTypes.StartGrid)
                {
                    Console.WriteLine("s");
                }else if (sect.SectionType == SectionTypes.RightCorner || sect.SectionType == SectionTypes.LeftCorner)
                {
                    Console.WriteLine("c");
                }
            }
            //old
            foreach (Section sect in drawList)
            {
                if (sect.SectionType == SectionTypes.Straight || sect.SectionType == SectionTypes.Finish || sect.SectionType == SectionTypes.StartGrid)
                {                    
                    PrintArray[y, x] = sect.SectionType;
                    if (Direction == 0)
                    {
                        y--;                         
                    }
                    else if (Direction == 1)
                    { 
                        x++;                         
                    }
                    else if (Direction == 2)
                    { 
                        y++;                         
                    }
                    else if (Direction == 3)
                    { 
                        x--;                         
                    }
                }
                else if (sect.SectionType == SectionTypes.LeftCorner)
                {
                    PrintArray[y, x] = sect.SectionType;

                    if (Direction == 0)
                    {
                        x--;                         
                    }
                    else if (Direction == 1)
                    { 
                        y--;                         
                    }
                    else if (Direction == 2)
                    { 
                        x++;                         
                    }
                    else if (Direction == 3)
                    { 
                        y++;                         
                    }
                    Direction--;
                    if (Direction < 0) { Direction = 3; }

                }
                else if (sect.SectionType == SectionTypes.RightCorner)
                {
                    PrintArray[y, x] = sect.SectionType;

                    if (Direction == 0)
                    { 
                        x++;                         
                    }
                    else if (Direction == 1)
                    { 
                        y++;                        
                    }
                    else if (Direction == 2)
                    { 
                        x--;                       
                    }
                    else if (Direction == 3)
                    {
                        y--;                        
                    }
                    Direction++;
                    if (Direction > 3) { Direction = 0; }

                }
                Console.WriteLine(sect.SectionType);
                Console.WriteLine(x +" "+ y);
            }
           
            
            for (int i = 0; i < 2; i++)
            {
                for (int d = 0; d < 5; d++)
                {
                    Console.WriteLine(PrintArray[i, d] + " there");
                }
                Console.WriteLine("enter");
            }
            
Direction = 1;
            for ( y = 0; y < ymax+1; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (x = 0; x < xmax+1; x++)
                    {
                        {
                            if (PrintArray[y, x] == SectionTypes.Straight)
                            {
                                //Console.WriteLine("straight");
                                if (Direction == 0)
                                {
                                    Console.Write(_straightNS[i]);
                                }
                                else if (Direction == 1)
                                {
                                    Console.Write(_straightEW[i]);
                                }
                                else if (Direction == 2)
                                {
                                    Console.Write(_straightNS[i]);
                                }
                                else if (Direction == 3)
                                {
                                    Console.Write(_straightEW[i]);
                                }
                            }
                            else if (PrintArray[y, x] == SectionTypes.Finish)
                            {
                                //Console.WriteLine("fin");
                                if (Direction == 0)
                                {
                                    Console.Write(_finishE[i]);
                                }
                                else if (Direction == 1)
                                {
                                    Console.Write(_finishE[i]);
                                }
                                else if (Direction == 2)
                                {
                                    Console.Write(_finishE[i]);
                                }
                                else if (Direction == 3)
                                {
                                    Console.Write(_finishE[i]);
                                }
                            }
                            else if (PrintArray[y, x] == SectionTypes.StartGrid)
                            {
                                //Console.WriteLine("start");
                                if (Direction == 0)
                                {
                                    Console.Write(_StartgridE[i]);
                                }
                                else if (Direction == 1)
                                {
                                    Console.Write(_StartgridE[i]);
                                }
                                else if (Direction == 2)
                                {
                                    Console.Write(_StartgridE[i]);
                                }
                                else if (Direction == 3)
                                {
                                    Console.Write(_StartgridE[i]);
                                }
                            }
                            else if (PrintArray[y, x] == SectionTypes.LeftCorner)
                            {
                                //Console.WriteLine("left");
                                if (Direction == 0)
                                {
                                    Console.Write(_bendSW[i]);
                                }
                                else if (Direction == 1)
                                {
                                    Console.Write(_bendNW[i]);
                                }
                                else if (Direction == 2)
                                {
                                    Console.Write(_bendSE[i]);
                                }
                                else if (Direction == 3)
                                {
                                    Console.Write(_bendNE[i]);
                                }
                                if (i == 3)
                                {
                                    Direction--;
                                    if (Direction < 0) { Direction = 3; }
                                }
                            }
                            else if (PrintArray[y, x] == SectionTypes.RightCorner)
                            {
                                //Console.WriteLine("right");
                                if (Direction == 0)
                                {
                                    Console.Write(_bendSE[i]);
                                }
                                else if (Direction == 1)
                                {
                                    Console.Write(_bendNW[i]);
                                }
                                else if (Direction == 2)
                                {
                                    Console.Write(_bendNE[i]);
                                }
                                else if (Direction == 3)
                                {
                                    Console.Write(_bendSW[i]);
                                }
                                if (i == 3)
                                {
                                    Direction++;
                                    if (Direction > 3) { Direction = 0; }
                                }

                            }
                            //Console.Write("aaa");
                        }
                    }

                    Console.WriteLine();
                    
                    //Console.WriteLine(i);
                }
            }





*/






