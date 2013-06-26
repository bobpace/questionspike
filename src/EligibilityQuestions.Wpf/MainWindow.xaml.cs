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

namespace EligibilityQuestions.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var firstQuestion = new YesNoQuestion();
            firstQuestion.ForAnswer<TestModel>(x => x.LikesBlue);
            firstQuestion.QuestionText = "Do you like blue?";

            var secondQuestion = new YesNoQuestion();
            secondQuestion.ForAnswer<TestModel>(x => x.LikesGreen);
            secondQuestion.QuestionText = "Do you like green?";

            var thirdQuestion = new YesNoQuestion();
            thirdQuestion.ForAnswer<TestModel>(x => x.LikesRed);
            thirdQuestion.QuestionText = "Do you like red?";

            firstQuestion.OnNo(x => secondQuestion);

            secondQuestion.OnYes(x => thirdQuestion);

            DataContext = firstQuestion;
        }
    }
}
