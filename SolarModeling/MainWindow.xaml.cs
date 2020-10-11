using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SolarModeling
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Modeling model = new Modeling();
        private Thread thread;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            model.Start(Convert.ToInt32(TbPointNumber.Text),Convert.ToDouble(TbMass.Text), Convert.ToDouble(TbModelRadius.Text), Canv);
            thread = new Thread(model.Work);
            thread.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            model.Terminate();            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.Terminate();
            thread?.Abort();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //model.ModelX = e.NewSize.Width;
            //model.ModelY = e.NewSize.Height;
        }

        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.SetTimeScale(e.NewValue);
        }
    }
}
