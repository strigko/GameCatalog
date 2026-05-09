using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using GameShared;

namespace GameClient.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.UTF8;
            PrintHeader();

            while (true)
            {
                System.Console.WriteLine("\n--- MENU ---");
                System.Console.WriteLine("1. Show all games");
                System.Console.WriteLine("2. Add game");
                System.Console.WriteLine("3. Delete game");
                System.Console.WriteLine("4. Edit game");
                System.Console.WriteLine("0. Exit");
                System.Console.Write("\nChoose: ");

                string? choice = System.Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        var games = await FetchGamesAsync();
                        if (games != null) ShowGames(games);
                        else System.Console.WriteLine("[!] Could not get data from server.");
                        break;

                    case "2":
                        await AddGameAsync();
                        break;

                    case "3":
                        await DeleteGameAsync();
                        break;

                    case "4":
                        await EditGameAsync();
                        break;

                    case "0":
                        System.Console.WriteLine("\nSee ya!");
                        return;

                    default:
                        System.Console.WriteLine("[!] Invalid option.");
                        break;
                }
            }
        }

        static async Task<List<Game>?> FetchGamesAsync()
        {
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(Protocol.DEFAULT_HOST, Protocol.DEFAULT_PORT);
                using var stream = client.GetStream();

                byte[] cmd = Encoding.UTF8.GetBytes(Protocol.GET_GAMES);
                await stream.WriteAsync(cmd, 0, cmd.Length);

                string response = await ReadResponseAsync(stream, client);

                if (response.StartsWith(Protocol.GAMES_JSON + ":"))
                {
                    string json = response.Substring(Protocol.GAMES_JSON.Length + 1);
                    return JsonSerializer.Deserialize<List<Game>>(json);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[!] Error: {ex.Message}");
                return null;
            }
        }

        static async Task AddGameAsync()
        {
            System.Console.WriteLine("\n-- Add Game --");
            var game = PromptGame(null);

            string json = JsonSerializer.Serialize(game);
            string message = $"{Protocol.ADD_PRODUCT}:{json}";
            string response = await SendCommandAsync(message);
            System.Console.WriteLine(response.StartsWith(Protocol.SUCCESS)
                ? "[OK] Game added successfully."
                : $"[!] {response}");
        }

        static async Task DeleteGameAsync()
        {
            System.Console.Write("\nEnter game ID to delete: ");
            string? idStr = System.Console.ReadLine()?.Trim();

            string response = await SendCommandAsync($"{Protocol.DELETE_PRODUCT}:{idStr}");
            System.Console.WriteLine(response.StartsWith(Protocol.SUCCESS)
                ? "[OK] Game deleted."
                : $"[!] {response}");
        }

        static async Task EditGameAsync()
        {
            System.Console.Write("\nEnter game ID to edit: ");
            string? idStr = System.Console.ReadLine()?.Trim();
            if (!int.TryParse(idStr, out int id)) { System.Console.WriteLine("[!] Invalid ID."); return; }

            System.Console.WriteLine("Enter new values (leave blank to keep current):");
            var game = PromptGame(id);

            string json = JsonSerializer.Serialize(game);
            string response = await SendCommandAsync($"{Protocol.EDIT_PRODUCT}:{json}");
            System.Console.WriteLine(response.StartsWith(Protocol.SUCCESS)
                ? "[OK] Game updated."
                : $"[!] {response}");
        }

        static Game PromptGame(int? id)
        {
            System.Console.Write("Name           : "); string name = System.Console.ReadLine() ?? "";
            System.Console.Write("Description    : "); string desc = System.Console.ReadLine() ?? "";
            System.Console.Write("Price          : "); decimal.TryParse(System.Console.ReadLine(), out decimal price);
            System.Console.Write("Year of release: "); int.TryParse(System.Console.ReadLine(), out int year);
            System.Console.Write("Picture file   : "); string pic = System.Console.ReadLine() ?? "";

            return new Game
            {
                Id = id ?? 0,
                Name = name,
                Description = desc,
                Price = price,
                YearOfRelease = year,
                PictureFilePath = pic
            };
        }

        static async Task<string> SendCommandAsync(string command)
        {
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(Protocol.DEFAULT_HOST, Protocol.DEFAULT_PORT);
                using var stream = client.GetStream();

                byte[] cmd = Encoding.UTF8.GetBytes(command);
                await stream.WriteAsync(cmd, 0, cmd.Length);

                return await ReadResponseAsync(stream, client);
            }
            catch (Exception ex)
            {
                return $"error:{ex.Message}";
            }
        }

        static async Task<string> ReadResponseAsync(NetworkStream stream, TcpClient client)
        {
            var sb = new StringBuilder();
            var buffer = new byte[65536];
            stream.ReadTimeout = 5000;
            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    if (client.Available == 0) break;
                }
            }
            catch (System.IO.IOException) { }
            return sb.ToString();
        }

        static void ShowGames(List<Game> games)
        {
            System.Console.WriteLine($"\n{'='.ToString().PadRight(60, '=')}");
            System.Console.WriteLine($"  GAMES CATALOG ({games.Count} games)");
            System.Console.WriteLine($"{'='.ToString().PadRight(60, '=')}\n");

            foreach (var g in games)
            {
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.Write($"  [{g.Id:D2}] ");
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine(g.Name);
                System.Console.ResetColor();
                System.Console.WriteLine($"       >> {g.Description}");
                System.Console.WriteLine($"       >> Release Year : {g.YearOfRelease}");
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"       >> Price        : {g.Price:F2}");
                System.Console.ResetColor();
                System.Console.WriteLine($"       >> Picture      : {g.PictureFilePath}");
                System.Console.WriteLine($"       {new string('-', 50)}");
            }
        }

        static void PrintHeader()
        {
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine(@"
|||||||||||||||||||||||||||||||||||||||||||||||
|||           GAME CATALOG CLIENT           |||
|||                **Console**              |||
|||||||||||||||||||||||||||||||||||||||||||||||");
            System.Console.ResetColor();
        }
    }
}