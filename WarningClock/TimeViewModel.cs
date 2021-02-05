using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WarningClock
{
    public class TimeViewModel : INotifyPropertyChanged
    {
        private DateTime time;
        private Color color;

        public TimeViewModel()
        {
            time = DateTime.Now;
            color = Color.FromRgb(0, 0, 0);
        }

        public DateTime Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }

        public Color Color
        {

            get
            {
                return color;
            }
            set
            {
                color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
