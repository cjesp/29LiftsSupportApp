using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace _29LiftsSupportApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _client;
        private readonly string LYFT_STATUS_URL = "https://api.lyft.com/v1/sandbox/rides/";
        private readonly string LYFT_RIDETYPES_URL = "https://api.lyft.com/v1/sandbox/ridetypes";

        public MainWindow()
        {
            InitializeComponent();
            _client = new HttpClient();

        }

        private bool SetupHttpClient()
        {
            if (string.IsNullOrEmpty(Token.Text) || string.IsNullOrEmpty(RideId.Text))
            {
                var box = MessageBox.Show("You need to provide a token");
                return false;
            }
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Text);
            return true;
        }

        private async void Accepted_Click(object sender, RoutedEventArgs e)
        {
            await ChangeStatus("accepted");
        }

        private async void Arrived_Click(object sender, RoutedEventArgs e)
        {
            await ChangeStatus("arrived");
        }

        private async void Pickup_Click(object sender, RoutedEventArgs e)
        {
            await ChangeStatus("pickedUp");
        }

        private async void DroppedOff_Click(object sender, RoutedEventArgs e)
        {
            await ChangeStatus("droppedOff");
        }

        private async void Canceled_Click(object sender, RoutedEventArgs e)
        {
            await ChangeStatus("canceled");
        }

        private async Task ChangeStatus(string status)
        {
            if (SetupHttpClient())
            {
                var scopeJson = $"{{\"status\": \"{status}\"}}";
                var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync(LYFT_STATUS_URL + RideId.Text, content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    StatusText.Text = status;
                }
                else
                {
                    StatusText.Text = "Error!";
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddCar_Click(object sender, RoutedEventArgs e)
        {
            if (SetupHttpClient())
            {
                var scopeJson = $"{{ \"lat\": 47.639722, \"lng\": -122.128333, \"ride_types\": [\"lyft\", \"lyft_line\"]}}";
                var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync(LYFT_RIDETYPES_URL, content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    StatusText.Text = "Car added!";
                }
                else
                {
                    StatusText.Text = "Error adding car!";
                }
            }
        }
    }
}
