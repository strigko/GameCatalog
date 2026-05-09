using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using GameShared;

namespace GameClient.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FooterText.Text = $"Game Catalog Client v1.0  |  TCP://{Protocol.DEFAULT_HOST}:{Protocol.DEFAULT_PORT}";
            Loaded += async (_, _) => await LoadGamesAsync();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadGamesAsync();
        }

        private async Task LoadGamesAsync()
        {
            SetStatus("loading");
            RefreshButton.IsEnabled = false;
            LoadingOverlay.Visibility = Visibility.Visible;
            GamesItemsControl.ItemsSource = null;
            SubtitleText.Text = "Connecting to the server...";

            try
            {
                var games = await Task.Run(FetchGamesFromServer);

                if (games != null)
                {
                    GamesItemsControl.ItemsSource = games;
                    CountText.Text = $"{games.Count} games in the catalog";
                    SubtitleText.Text = $"Updated: {DateTime.Now:HH:mm:ss}  •  Got {games.Count} things";
                    SetStatus("ok");
                }
                else
                {
                    SubtitleText.Text = "Error of getting data";
                    CountText.Text = "";
                    SetStatus("error");
                    MessageBox.Show("Could got data from the server.\nEnsure, that server is working.",
                                    "Connection error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                SubtitleText.Text = "Connection error";
                SetStatus("error");
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
                RefreshButton.IsEnabled = true;
            }
        }

        private List<Game>? FetchGamesFromServer()
        {
            using var client = new TcpClient();
            client.Connect(Protocol.DEFAULT_HOST, Protocol.DEFAULT_PORT);

            using var stream = client.GetStream();
            stream.ReadTimeout = 8000;

            byte[] cmdBytes = Encoding.UTF8.GetBytes(Protocol.GET_GAMES);
            stream.Write(cmdBytes, 0, cmdBytes.Length);

            var sb = new StringBuilder();
            var buffer = new byte[65536];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                if (client.Available == 0) break;
            }

            string response = sb.ToString();
            if (response.StartsWith(Protocol.GAMES_JSON + ":"))
            {
                string json = response.Substring(Protocol.GAMES_JSON.Length + 1);
                return JsonSerializer.Deserialize<List<Game>>(json);
            }
            return null;
        }

        private void SetStatus(string state)
        {
            Dispatcher.Invoke(() =>
            {
                switch (state)
                {
                    case "ok":
                        StatusDotBrush.Color = Colors.LimeGreen;
                        StatusText.Text = "Connected";
                        StatusText.Foreground = new SolidColorBrush(Colors.LimeGreen);
                        break;
                    case "error":
                        StatusDotBrush.Color = Colors.OrangeRed;
                        StatusText.Text = "Error";
                        StatusText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                        break;
                    case "loading":
                        StatusDotBrush.Color = Color.FromRgb(0xFF, 0xCC, 0x00);
                        StatusText.Text = "Loading...";
                        StatusText.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xCC, 0x00));
                        break;
                }
            });
        }
    }
}
