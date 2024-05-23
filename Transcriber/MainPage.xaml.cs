namespace Transcriber
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                var temp = await SpeechToText.FromStream();
            }
            catch (Exception ex)
            {

            }
        }
    }

}
