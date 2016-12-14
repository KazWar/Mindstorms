using System;
using System.Collections.Generic;
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
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System.Diagnostics;

namespace MindstormsProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brick _brick;
        int _forward = 50;
        int _backward = -50;
        uint _time = 500;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _brick = new Brick(new BluetoothCommunication("COM4"));
            await _brick.ConnectAsync();
            await _brick.DirectCommand.PlayToneAsync(100, 1000, 300);
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            TurnLeft();
        }

        private void ButtonBackwards_Click(object sender, RoutedEventArgs e)
        {
            MoveForward();
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            TurnRight();
        }

        private void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            MoveBackward();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    MoveBackward();
                    break;

                case Key.D:
                    TurnRight();
                    break;

                case Key.A:
                    TurnLeft();
                    break;

                case Key.S:
                    MoveForward();
                    break;

                case Key.Q:
                    PickUp();
                    break;

                case Key.E:
                    DropDown();
                    break;
            }
        }


        private void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            Debug.WriteLine("Brick Changed!");
        }

        public async void TurnLeft()
        {
            Debug.Write("Left");
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.D, _forward, _time, false);
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, _backward, _time, false);
            await _brick.BatchCommand.SendCommandAsync();
        }

        public async void MoveBackward()
        {
            Debug.Write("Backward");
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.D | OutputPort.A, _backward, _time, false);
        }
        public async void TurnRight()
        {
            Debug.Write("Right");
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.D, _backward, _time, false);
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, _forward, _time, false);
            await _brick.BatchCommand.SendCommandAsync();
        }

        public async void MoveForward()
        {
            Debug.Write("Forward");
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.D | OutputPort.A, _forward, _time, false);
        }

        public async void PickUp()
        {
            Debug.Write("Open");
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.C, 100,100, false);
        }

        public async void DropDown()
        {
            Debug.Write("Close");
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.C, -100, 100, false);
        }

    }
}
