using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using GameServer.Models;
using System.Net;
using GameServer.Models.Strategy;
using GameServer.Models.Monsters;
using GameServer.Models.Factory;

namespace GameClient
{
    public partial class Form1 : Form
    {
       static HttpClient client = new HttpClient();

        static string requestUri = "api/player/";
        static string mediaType = "application/json";

        public Rectangle test = new Rectangle(10,10,20,30);
        public Rectangle tower1 = new Rectangle(460, 10, 60, 30);
        public Rectangle tower2 = new Rectangle(10, 430, 60, 30);
        public List<Monster> monsters = new List<Monster>();
        public ICollection<Player> players;
        private static Factory factory;
        public int UpdateInterval = 0;

        public int i = 1;
        //Observer test
        private Player playerObserver = new Player
        {
            Name = "Pavyzdys",
            Score = 111,
            PosX = 20,
            PosY = 30
        };

        private Observer observer = new Observer();

        Random rnd = new Random();

        //-------

        public Form1()
        {
            InitializeComponent();            
            KeyPreview = true;
            KeyDown += new KeyEventHandler(Form1_KeyDown);

            Player playerNew = new Player
            {
                Id = 99,
                Name = "zaidejas",
                Score = 100,
                PosX = 20,
                PosY = 30
            };

            GameServer.Settings nustatymai = GameServer.Settings.getInstance();


            gameTimer.Interval = 700 / nustatymai.GameSpeed;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            label1.Text = "labas " + playerNew.Name;

            //DoStuff();
            RunAsync();

            factory = new MonstersFactory();
            
            Monster monsterFast = new Skeleton(1, 1, 10, 10, 240);
            monsterFast.setStrategy(new MoveFast());
            Monster monsterSlow = new Zombie(2, 1, 10, 10, 260);
            monsterSlow.setStrategy(new MoveSlow());
            Monster monsterFlying = new Slime(3, 1, 10, 10, 260);
            monsterFlying.setStrategy(new Fly());

            Monster testingFactory = factory.factoryMethod("zombie", 100, 280);
            testingFactory.setStrategy(new MoveSlow());

            monsters.Add(monsterFast);
            monsters.Add(monsterSlow);
            monsters.Add(monsterFlying);
            monsters.Add(testingFactory);

        }

        private async void GameTimer_Tick(object sender, EventArgs e)
        {
            GenerateRect();
            GenerateScore();
            foreach (Monster mon in monsters)
            {
                mon.move();
            }
            if (UpdateInterval >= 100)
            {
                players = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
                UpdateInterval = 0;
            }
            else UpdateInterval++;
            
            pictureBox1.Invalidate();
        }

        private void GenerateRect()
        {
            int maxXPos = pictureBox1.Size.Width;
            int maxYPos = pictureBox1.Size.Height;
           
            test.X = test.X + 10 * i;
            test.Y = test.Y + 10 * i;
            if (test.X > 490) i *= -1;
            if (test.X < 10) i *= -1;
            label1.Text = test.X.ToString();
            label2.Text = test.Y.ToString();
        }
        private void GenerateScore()
        {
            int e = rnd.Next(-4,5);
            observer.Update(playerObserver, e);

            label3.Text = playerObserver.Score.ToString();
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            canvas.FillEllipse(Brushes.Red, test.X, test.Y, test.Height, test.Width);
            canvas.FillRectangle(Brushes.Green, tower1.X, tower1.Y, tower1.Height, tower1.Width);
            canvas.FillRectangle(Brushes.Orange, tower2.X, tower2.Y, tower2.Height, tower2.Width);
            foreach(Monster mon in monsters)
            {
                canvas.FillRectangle(Brushes.Azure, mon.posX, mon.posY, 20, 20);
            }

            //players = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            if (players != null)
            {
                foreach (Player pla in players)
                {
                    canvas.FillRectangle(Brushes.Black, pla.PosX, pla.PosY, 30, 30);
                }
            }
        }

        void ConsoleWrite(Player player)
        {
            richTextBox1.AppendText($"Id: {player.Id}\tName: {player.Name}\tScore: " +
                              $"{player.Score}\tposX: {player.PosX}\tposY: {player.PosY}\n");
            
        }

        async Task<Uri> CreatePlayerAsync(Player player)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                requestUri, player);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            Player player2 = await response.Content.ReadAsAsync<Player>();
            if (player2 != null)
            {
                ConsoleWrite(player2);
            }

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<ICollection<Player>> GetAllPlayerAsync(string path)
        {
            ICollection<Player> players = null;
            HttpResponseMessage response = await client.GetAsync(path + "api/player");
            if (response.IsSuccessStatusCode)
            {
                players = await response.Content.ReadAsAsync<ICollection<Player>>();
            }
            return players;
        }

        static async Task<Player> GetPlayerAsync(string path)
        {
            Player player = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                player = await response.Content.ReadAsAsync<Player>();
            }
            return player;
        }

        static async Task<HttpStatusCode> UpdatePlayerAsync(Player player)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                requestUri + $"{player.Id}", player);
            response.EnsureSuccessStatusCode();

            return response.StatusCode;
        }

        static async Task<HttpStatusCode> PatchPlayerAsync(Coordinates coordinates)
        {
            string jsonString = JsonConvert.SerializeObject(coordinates);    //"{\"id\":1, \"posX\":777,\"posY\":777}";

            HttpContent httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, mediaType);
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri(client.BaseAddress + requestUri),
                Content = httpContent,
            };

            HttpResponseMessage response = await client.SendAsync(request);
            return response.StatusCode;

        }

        static async Task<HttpStatusCode> DeletePlayerAsync(long id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                requestUri + $"{id}");
            return response.StatusCode;
        }

        static async Task<HttpStatusCode> DeleteAllPlayers()
        {
            ICollection<Player> playersList = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            foreach(Player player in playersList)
            {
                long id = player.Id;
                HttpResponseMessage response = await client.DeleteAsync(
                requestUri + $"{id}");
            }
            HttpResponseMessage response2 = new HttpResponseMessage();
            return response2.StatusCode; //response.StatusCode;
        }

        //static void Main()
        //{
        //    richTextBox1.AppendText("Web API Client says: \"Hello World!\"");
        //    RunAsync().GetAwaiter().GetResult();
        //}

        async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://gameserverobjpr.azurewebsites.net/"); //api /player/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(mediaType));

            try
            {
                // Get all players
                richTextBox1.AppendText("0)\tGet all player\n");
                players = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);               
                foreach (Player p in players)
                {
                    ConsoleWrite(p);
                }


                // Create a new player
                richTextBox1.AppendText("1.1)\tCreate the player\n");
                Player player = new Player
                {
                    Name = "Studentas-" + players.Count.ToString(),
                    Score = 100,
                    PosX = 20,
                    PosY = 30
                };

                var url = await CreatePlayerAsync(player);
                richTextBox1.AppendText($"Created at {url}\n");

                // Get the created player
                richTextBox1.AppendText("1.2)\tGet created player\n");
                player = await GetPlayerAsync(url.PathAndQuery);
                ConsoleWrite(player);

                richTextBox1.AppendText("2.1)\tFull Update the player's score\n");
                player.Score = 80;
                var updateStatusCode = await UpdatePlayerAsync(player);
                richTextBox1.AppendText($"Updated (HTTP Status = {(int)updateStatusCode})\n");

                // Get the full updated player
                richTextBox1.AppendText("2.2)\tGet updated the player\n");
                player = await GetPlayerAsync(url.PathAndQuery);
                ConsoleWrite(player);

                //Partial update - patch - of the player
                richTextBox1.AppendText("3.1)\tPatch Update the player's score\n");
                Coordinates coordinates = new Coordinates
                {
                    Id = player.Id,
                    PosX = player.PosX + 10,
                    PosY = player.PosY + 15
                };
                var patchStatusCode = await PatchPlayerAsync(coordinates);
                richTextBox1.AppendText($"Patched (HTTP Status = {(int)patchStatusCode})\n");

                // Get the patched  player
                richTextBox1.AppendText("3.2)\tGet patched player\n");
                player = await GetPlayerAsync(url.PathAndQuery);
                ConsoleWrite(player);

                //Create player for deletion
                richTextBox1.AppendText("4.1)\tCreate the player for deletion\n");
                Player delPlayer = new Player
                {
                    Name = "StudentasDel-" + (players.Count + 1).ToString(),
                    Score = 444,
                    PosX = 444,
                    PosY = 444
                };
                var url4Del = await CreatePlayerAsync(delPlayer);
                richTextBox1.AppendText($"Created at {url4Del}");

                //Show created player for deletion
                richTextBox1.AppendText("4.2)\tGet the player created for deletion \n");
                delPlayer = await GetPlayerAsync(url4Del.PathAndQuery);
                ConsoleWrite(delPlayer);

                // Delete the player
                richTextBox1.AppendText("4.3)\tDelete the player");
                var statusCode = await DeletePlayerAsync(delPlayer.Id);
                richTextBox1.AppendText($"Deleted (HTTP Status = {(int)statusCode})\n");

                //check if deletion was successful
                richTextBox1.AppendText("4.4)\tCheck if deletion was successful\n");
                delPlayer = await GetPlayerAsync(url4Del.PathAndQuery);
                ConsoleWrite(delPlayer);

                richTextBox1.AppendText("Web API Client says: \"GoodBy!\"\n");

            }
            catch (Exception e)
            {
                richTextBox1.AppendText(e.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ActiveControl = null;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            // {
            if (e.KeyCode == Keys.Down)
            {
                richTextBox1.AppendText($"Žemyn\n");
            }
            if (e.KeyCode == Keys.F1)
            {
                richTextBox1.AppendText($"F1\n");
            }
            if (e.KeyCode == Keys.F2)
            {
                richTextBox1.AppendText("1.1)\tCreate the player\n");
                Player player = new Player
                {
                    Name = "Naujas-" + players.Count.ToString(),
                    Score = 100,
                    PosX = rnd.Next(150, 300),
                    PosY = rnd.Next(150, 300)
                };

                var url = await CreatePlayerAsync(player);
                richTextBox1.AppendText($"Created at {url}\n");

                // Get the created player
                richTextBox1.AppendText("1.2)\tGet created player\n");
                player = await GetPlayerAsync(url.PathAndQuery);
                ConsoleWrite(player);
                players = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            }
            if (e.KeyCode == Keys.F4)
            {
                var statusCode = await DeleteAllPlayers();
                richTextBox1.AppendText($"Deleting ALL players)\n");
            }
            //  }

            //case Keys.Down:                  
            //  break;

            /*
            case Keys.Down:

                ICollection<Player> playersList = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
                richTextBox1.AppendText("1.1)\tCreate the player\n");
                Player player = new Player
                {
                    Name = "Naujas-" + playersList.Count.ToString(),
                    Score = 100,
                    PosX = rnd.Next(150, 300),
                    PosY = rnd.Next(150, 300)
                };

                var url = await CreatePlayerAsync(player);
                richTextBox1.AppendText($"Created at {url}\n");

                // Get the created player
                richTextBox1.AppendText("1.2)\tGet created player\n");
                player = await GetPlayerAsync(url.PathAndQuery);
                ConsoleWrite(player);

                break;


            case Keys.Left:
                var statusCode = await DeleteAllPlayers();
                richTextBox1.AppendText($"Deleting ALL players)\n");
                break;
/*
            case Keys.Right:                   
                break;

            case Keys.Up:                   
                break;
                */

        }
    }
}
