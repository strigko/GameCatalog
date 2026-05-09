using GameShared;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GameServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            var listener = new TcpListener(IPAddress.Any, Protocol.DEFAULT_PORT);
            listener.Start();
            Console.WriteLine($"[Server] Server started. Port: {Protocol.DEFAULT_PORT}");
            Console.WriteLine("[Server] Waiting for connection...\n");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        static async Task HandleClientAsync(TcpClient tcpClient)
        {
            var endpoint = tcpClient.Client.RemoteEndPoint;
            Console.WriteLine($"[+] Connecting client: {endpoint}");

            try
            {
                using var stream = tcpClient.GetStream();
                var buffer = new byte[4096];

                while (tcpClient.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string command = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    Console.WriteLine($"[CMD] {endpoint} -> \"{command}\"");

                    if (command == Protocol.GET_GAMES)
                    {
                        var games = await GetGamesFromDb();
                        string json = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = false });
                        string response = $"{Protocol.GAMES_JSON}:{json}";
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        Console.WriteLine($"[->] Send {games.Count} games to the client {endpoint}");
                    }
                    else if (command.StartsWith(Protocol.ADD_PRODUCT + ":"))
                    {
                        string json = command.Substring(Protocol.ADD_PRODUCT.Length + 1);
                        var game = JsonSerializer.Deserialize<Game>(json);
                        if (game != null)
                        {
                            using var db = new GameDbContext();
                            db.Games.Add(game);
                            await db.SaveChangesAsync();
                            Console.WriteLine($"[+] Added game: {game.Name}");
                            byte[] ok = Encoding.UTF8.GetBytes($"{Protocol.SUCCESS}:Game added.");
                            await stream.WriteAsync(ok, 0, ok.Length);
                        }
                    }
                    else if (command.StartsWith(Protocol.DELETE_PRODUCT + ":"))
                    {
                        string idStr = command.Substring(Protocol.DELETE_PRODUCT.Length + 1);
                        if (int.TryParse(idStr, out int id))
                        {
                            using var db = new GameDbContext();
                            var game = await db.Games.FindAsync(id);
                            if (game != null)
                            {
                                db.Games.Remove(game);
                                await db.SaveChangesAsync();
                                Console.WriteLine($"[-] Deleted game id={id}");
                                byte[] ok = Encoding.UTF8.GetBytes($"{Protocol.SUCCESS}:Game deleted.");
                                await stream.WriteAsync(ok, 0, ok.Length);
                            }
                            else
                            {
                                byte[] err = Encoding.UTF8.GetBytes($"{Protocol.ERROR}:Game not found.");
                                await stream.WriteAsync(err, 0, err.Length);
                            }
                        }
                    }
                    else if (command.StartsWith(Protocol.EDIT_PRODUCT + ":"))
                    {
                        string json = command.Substring(Protocol.EDIT_PRODUCT.Length + 1);
                        var updated = JsonSerializer.Deserialize<Game>(json);
                        if (updated != null)
                        {
                            using var db = new GameDbContext();
                            var game = await db.Games.FindAsync(updated.Id);
                            if (game != null)
                            {
                                game.Name = updated.Name;
                                game.Description = updated.Description;
                                game.Price = updated.Price;
                                game.YearOfRelease = updated.YearOfRelease;
                                game.PictureFilePath = updated.PictureFilePath;
                                await db.SaveChangesAsync();
                                Console.WriteLine($"[~] Edited game id={updated.Id}");
                                byte[] ok = Encoding.UTF8.GetBytes($"{Protocol.SUCCESS}:Game updated.");
                                await stream.WriteAsync(ok, 0, ok.Length);
                            }
                            else
                            {
                                byte[] err = Encoding.UTF8.GetBytes($"{Protocol.ERROR}:Game not found.");
                                await stream.WriteAsync(err, 0, err.Length);
                            }
                        }
                    }
                    else
                    {
                        string errMsg = $"{Protocol.ERROR}:Unknown command '{command}'";
                        byte[] errBytes = Encoding.UTF8.GetBytes(errMsg);
                        await stream.WriteAsync(errBytes, 0, errBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Client Error {endpoint}: {ex.Message}");
            }
            finally
            {
                tcpClient.Close();
                Console.WriteLine($"[-] Disconnected client: {endpoint}");
            }
        }

        static async Task<System.Collections.Generic.List<Game>> GetGamesFromDb()
        {
            using var db = new GameDbContext();
            return await db.Games.ToListAsync();
        }
    }
}
