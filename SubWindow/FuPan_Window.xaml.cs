using System.Linq;
using System.Windows;
using System.Windows.Input;
using static Chess.SuanFa.Qipu;
using Chess.SuanFa;

namespace Chess.SubWindow
{
    /// <summary>
    /// FuPan_Window.xaml 的交互逻辑
    /// </summary>
    public partial class FuPan_Window : Window
    {

        private static ContractQPClass[] qPSteps;
        private static int qpIndex = -1;
        public FuPan_Window()
        {
            InitializeComponent();
            FuPanDataGrid.ItemsSource = GlobalValue.fuPanDataList;
            GlobalValue.Reset();
            qPSteps = GlobalValue.fuPanDataList.ToArray();
            qpIndex = -1;


        }

        private void PreStep(object sender, RoutedEventArgs e)
        {
            if (qpIndex > -1)
            {
                GlobalValue.HuiQi();
                qpIndex--;

            }
        }

        private void NextStep(object sender, RoutedEventArgs e)
        {
            if (qpIndex < qPSteps.Length - 1)
            {
                qpIndex++;
                StepCode step = qPSteps[qpIndex].StepData;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);

            }
        }

        private void ReStart(object sender, RoutedEventArgs e)
        {
            GlobalValue.Reset();
            qpIndex = -1;

        }

        private void NextChange(object sender, RoutedEventArgs e)
        {

        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            menu.IsOpen = true;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GlobalValue.Reset();
            qpIndex = -1;

            int index = FuPanDataGrid.SelectedIndex;
            while (qpIndex < index)
            {
                qpIndex++;
                StepCode step = qPSteps[qpIndex].StepData;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);
            }
            for (int i = 0; i < Qipu.QiPuList.Count; i++)
            {
                QiPuList[i].Remarks = GlobalValue.fuPanDataList[i].Remarks;
            }
        }
    }
}
