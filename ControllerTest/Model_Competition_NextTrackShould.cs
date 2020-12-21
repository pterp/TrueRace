using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
//dsing TrueRace;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;
        [SetUp]
        public void Setup()
        {
            _competition = new Competition();
        }
        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track result = _competition.NextTrack();
            Assert.IsNull(result);
        }
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Section Straight1 = new Section(SectionTypes.Straight);
            Section finish1 = new Section(SectionTypes.Finish);
            Section startgrid = new Section(SectionTypes.StartGrid);
            Section right = new Section(SectionTypes.RightCorner);
            Section left = new Section(SectionTypes.LeftCorner);
            Section[] sectarr2 = { startgrid, startgrid, finish1, left, Straight1, left, left, right, Straight1, right, left, Straight1, left };

            Track testTrack = new Track("test",sectarr2);
            Queue<Track> q = new Queue<Track>();
            q.Enqueue(testTrack);
            _competition.Tracks = q;
            Track result = _competition.NextTrack();
            Assert.AreEqual(testTrack, result);
        }
        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Section Straight1 = new Section(SectionTypes.Straight);
            Section finish1 = new Section(SectionTypes.Finish);
            Section startgrid = new Section(SectionTypes.StartGrid);
            Section right = new Section(SectionTypes.RightCorner);
            Section left = new Section(SectionTypes.LeftCorner);
            Section[] sectarr2 = { startgrid, startgrid, finish1, left, Straight1, left, left, right, Straight1, right, left, Straight1, left };

            Track testTrack = new Track("test", sectarr2);
            Queue<Track> q = new Queue<Track>();
            q.Enqueue(testTrack);
            _competition.Tracks = q;
            Track result = _competition.NextTrack();
            Assert.AreEqual(testTrack, result);
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }
        
         [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Section Straight1 = new Section(SectionTypes.Straight);
            Section finish1 = new Section(SectionTypes.Finish);
            Section startgrid = new Section(SectionTypes.StartGrid);
            Section right = new Section(SectionTypes.RightCorner);
            Section left = new Section(SectionTypes.LeftCorner);
            Section[] sectarr2 = { startgrid, startgrid, finish1, left, Straight1, left, left, right, Straight1, right, left, Straight1, left };

            Track testTrack1 = new Track("test", sectarr2);
            Track testTrack2 = new Track("test2", sectarr2);
            Queue<Track> q = new Queue<Track>();
            q.Enqueue(testTrack1);
            q.Enqueue(testTrack2);
            _competition.Tracks = q;

            Track result = _competition.NextTrack();
            Assert.AreEqual(testTrack1, result);
            result = _competition.NextTrack();
            Assert.AreEqual(testTrack2, result);
        }
        [Test]
        public void AddParticipantstest()
        {
            AddParticipants
        }
    }


}
