using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_practice_gosniias
{
        public class MainWindowViewModel
        {
            public List<ISeries> Series { get; set; }
            public List<Axis> XAxes { get; set; }
            public List<Axis> YAxes { get; set; }

            public ObservableCollection<ObservablePoint> Points { get; set; }

            public MainWindowViewModel(Dictionary<DateTime, double> dataList)
            {
                Points = new ObservableCollection<ObservablePoint>();
                foreach (var item in dataList)
                {
                    Points.Add(new ObservablePoint(item.Key.ToOADate(), item.Value));
                }

                Series = new List<ISeries>
            {
                new LineSeries<ObservablePoint>
                {
                    Values = Points,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = 3 },
                    GeometrySize = 1
                }
            };

                XAxes = new List<Axis>
            {
                new Axis
                {
                    Name = "time",
                    Labeler = (key) => DateTime.FromOADate(key).ToString("yyyy-MM-dd HH:mm:ss")
                }
            };

                YAxes = new List<Axis>
            {

                new Axis{Name = "value"}
            };
            }
        }

        
}
