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

namespace WordListApp
{
    /// <summary>
    /// Interaction logic for BoderExPhrase.xaml
    /// </summary>
    public partial class BoderExPhrase : UserControl
    {
        string? pname;  
        public BoderExPhrase()
        {
            InitializeComponent();
        }

        public string? Pname { get => pname; set => pname = value; }
    }
}
