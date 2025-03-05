using System.Xml;

namespace ScalaParser
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnParseButtonClicked(object sender, EventArgs e)
        {
            string? code = TextBox.Text;
            TableLabel.Text = TextBox.Text;
        }
        void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //
        }
    }

}
