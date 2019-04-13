using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace FlashFrancais
{
    /// <summary>
    /// Interaction logic for CreateCards.xaml
    /// </summary>
    public partial class CreateCardsControl : UserControl
    {
        public CreateCardsControl()
        {
            InitializeComponent();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                string result = File.ReadAllText(openFileDialog.FileName);
                Debug.WriteLine(result);
            }
        }

        private void BtnSubmitNewCard_Click(object sender, RoutedEventArgs e)
        {
            string cardFront = FrontTextbox.Text;
            string cardBack = BackTextbox.Text;
            FrontTextbox.Clear();
            BackTextbox.Clear();
            Debug.WriteLine("Front: {0}, Back: {1}", cardFront, cardBack);
        }
    }
}
