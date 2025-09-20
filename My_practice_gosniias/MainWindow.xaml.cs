using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Drawing;
using LiveChartsCore.Defaults;

namespace My_practice_gosniias
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<DateTime,double> FromPSQLtoC()    // метод, выделяющий из базы данных столбцы данных и возвращает их 
        {
            Dictionary<DateTime, double> dataList = new Dictionary<DateTime, double>();
            //List<int> idsList = new List<int>();
            List<DateTime> timesList = new List<DateTime>();
            List<double> valuesList = new List<double>();

            string CommandText = "Select * from function_and_derivative ORDER BY time ";
            using (NpgsqlConnection sqlConn = new NpgsqlConnection("server=localhost;port=5432;User Id=postgres;password=123;database=practice"))
            {

                try
                {
                    sqlConn.Open();
                    NpgsqlCommand sqlCmd = new NpgsqlCommand(CommandText, sqlConn);

                    using (NpgsqlDataReader reader = sqlCmd.ExecuteReader()) // Используем DataReader для более эффективного чтения
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                DateTime time = reader.GetDateTime(1); // Получаем DateTime из первого столбца (индекс 1)
                                double value = reader.GetDouble(2);    // Получаем double из второго столбца (индекс 2)

                                dataList.Add(time, value); // Добавляем пару в список
                            }
                            catch (InvalidCastException ex)
                            {
                                Console.WriteLine($"Ошибка при чтении данных: {ex.Message}");
                            }
                        }
                    }

                    // Вывод для отладки (можно убрать после проверки)
                    Console.WriteLine("Прочитанные данные:");
                    foreach (var item in dataList)
                    {
                        Console.WriteLine($"Time: {item.Key}, Value: {item.Value}");
                    }
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine($"Ошибка подключения или выполнения запроса: {ex.Message}");
                }
            }

            return dataList; // Возвращаем список пар (DateTime, double)
        }

        public double Derivative(Dictionary<DateTime, double> listof2)
        {
            double derivative = 0;
            double interval = 0;
            int count = 0;
            int count_pos = 0;
            var dates = listof2.Keys.ToArray();
            var values = listof2.Values.ToArray();
            for (int i = 0; i < listof2.Count()-1; i++)
            {
                interval = (values[i + 1] - values[i]) / (dates[i + 1] - dates[i]).TotalSeconds;
                derivative += interval;
                count++;
                if (interval > 0) { count_pos++; }
                Console.WriteLine(interval);
            }
            double average_derivative = derivative / count;
            Console.WriteLine($"average_derivative = {average_derivative}");
            Console.WriteLine($"positive_derivative = {count_pos}");
            Console.WriteLine($"negative_derivative = {count-count_pos}");
            return derivative / count;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<DateTime, double> dat = FromPSQLtoC();
            double derivative = Derivative(dat); // Получаем значение derivative
            derivative = Math.Round(derivative, 2);
            string derivative_string;
            string imagePath; // Объявляем переменную для пути к изображению


            if (derivative>0)
            {

                derivative_string = $"derivative is positive {derivative}";
            }
            else
            {
                
                derivative_string = $"derivative is not positive: {derivative}";
            }



            DataContext = new MainWindowViewModel(dat);

            myChart_derivative.Text = derivative_string;

            myChart_derivative.Visibility = Visibility.Visible;
            myChart_text.Visibility = Visibility.Visible;
            myChart.Visibility = Visibility.Visible;
        }

        private void escButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
