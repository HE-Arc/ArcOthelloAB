using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ArcOthelloAB
{
    class ScoreHandler : DispatcherTimer, INotifyPropertyChanged
    {
        // Properties
        private int scoreBlack;    // Seconds
        public int ScoreBlack
        {
            get { return scoreBlack; }
            set { scoreBlack = value; RaisePropertyChanged("ScoreBlack"); }
        }

        private int scoreWhite;    // Seconds
        public int ScoreWhite
        {
            get { return scoreWhite; }
            set { scoreWhite = value; RaisePropertyChanged("ScoreWhite"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ScoreHandler(int timePlayedWhite = 2, int timePlayedBlack = 2)
        {
            ScoreBlack = timePlayedWhite;
            ScoreWhite = timePlayedBlack;
        }

        public void SetScores(int scoreBlack, int scoreWhite)
        {
            ScoreBlack = scoreBlack;
            ScoreWhite = scoreWhite;
        }
    }
}
