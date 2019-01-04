using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcOthelloAB
{
    class TimeHandler : INotifyPropertyChanged
    {
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

    }
}
