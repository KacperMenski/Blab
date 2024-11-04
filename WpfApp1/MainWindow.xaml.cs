using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        // Struktura danych, które przychodzą z API
        public class CityWeather
        {
            public int Id { get; set; }
            public string City { get; set; }
            public int Temp { get; set; }
            public string Weather { get; set; }
        }

        public class ApiResponse
        {
            public List<CityWeather> Cities { get; set; }
            public bool Success { get; set; }
        }

        private List<CityWeather> cities;

        public MainWindow()
        {
            InitializeComponent();
            FetchWeatherData();
        }

        private async void FetchWeatherData()
        {
            // Pobieranie danych z API
            string apiUrl = "https://fakeapi.multiquestion.eu/temperature.php?amount=3"; // Tutaj należy podać prawidłowy adres API
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(apiUrl);
                    ApiResponse data = JsonConvert.DeserializeObject<ApiResponse>(response);

                    if (data.Success)
                    {
                        cities = data.Cities;
                        CityComboBox.ItemsSource = cities;
                        CityComboBox.DisplayMemberPath = "City";
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się pobrać danych.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd przy pobieraniu danych: " + ex.Message);
                }
            }
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityComboBox.SelectedItem is CityWeather selectedCity)
            {
                CityNameTextBlock.Text = selectedCity.City;
                TemperatureTextBlock.Text = $"Temperatura: {selectedCity.Temp}°C";
                SetWeatherImage(selectedCity.Weather);
            }
        }

        private void SetWeatherImage(string weatherCondition)
        {
            string imagePath = "";

            switch (weatherCondition.ToLower())
            {
                case "sunny":
                    imagePath = "Images/sunny.png";
                    break;
                case "cloudy":
                    imagePath = "Images/cloudy.png";
                    break; 
                case "stormy":
                    imagePath = "Images/stormy.png";
                    break; 
                case "windy":
                    imagePath = "Images/windy.png";
                    break;
                case "foggy":
                    imagePath = "Images/foggy.png";
                    break;
                case "rainy":
                    imagePath = "Images/rainy.png";
                    break;
                default:
                    imagePath = "Images/default.png";
                    break;
            }

            WeatherImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            FetchWeatherData();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
