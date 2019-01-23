// ===============================
// TimeHandler
//
// PROJECT: ArcOthelloAB
// AUTHORS: Arzul Paul & Biloni Kim Aurore
// DATE: 23.01.19
// ORGANISATION: HE-Arc Neuchâtel
// ===============================

using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace ArcOthelloAB
{
    class TimeHandler : DispatcherTimer, INotifyPropertyChanged
    {
        // Properties
        private int timePlayedBlack;    // Seconds
        public int TimePlayedBlack
        {
            get { return timePlayedBlack; }
            set { timePlayedBlack = value; RaisePropertyChanged("TimePlayedBlack"); }
        }

        private int timePlayedWhite;    // Seconds
        public int TimePlayedWhite
        {
            get { return timePlayedWhite; }
            set { timePlayedWhite = value; RaisePropertyChanged("TimePlayedWhite"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool whiteTurn;
        private readonly EventHandler WhiteEventHandler;
        private readonly EventHandler BlackEventHandler;

        public TimeHandler(int timePlayedWhite = 0, int timePlayedBlack = 0) : base()
        {
            TimePlayedWhite = timePlayedWhite;
            TimePlayedBlack = timePlayedBlack;
            Interval = new TimeSpan(0, 0, 1);
            WhiteEventHandler = new EventHandler(dispatcherTimer_TickWhite);
            BlackEventHandler = new EventHandler(dispatcherTimer_TickBlack);

            whiteTurn = true;
            Tick += WhiteEventHandler;
        }

        public new void Start()
        {
            base.Start();
        }

        public void Switch()
        {
            whiteTurn = !whiteTurn;
            if (whiteTurn)
            {
                Tick += WhiteEventHandler;
                Tick -= BlackEventHandler;
            }
            else
            {
                Tick += BlackEventHandler;
                Tick -= WhiteEventHandler;
            }
        }

        public bool IsWhitePlaying()
        {
            return whiteTurn;
        }

        public bool IsBlackPlaying()
        {
            return !whiteTurn;
        }

        private void dispatcherTimer_TickWhite(object sender, EventArgs e)
        {
            TimePlayedWhite++;
        }

        private void dispatcherTimer_TickBlack(object sender, EventArgs e)
        {
            TimePlayedBlack++;
        }
    }
}
