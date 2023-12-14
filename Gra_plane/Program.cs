//From Here
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Okno
{
    class Program
    {
        static void Main()
        {
            int restart = 0;
            do
            {
                Rendering Game = new Rendering(restart);
                restart = Game.Wybor();
            } while (restart == 1);

            Console.WriteLine("Koniec gry");
            //Console.ReadLine();
        }
    } //End Program
    class Rendering
    {
        public RenderWindow okno; 
        public int selected;
        public int restart { get; set; }
        public Rendering(int restart)
        {
            this.restart = restart;
            Console.WriteLine("Render okna");

            VideoMode mode = new VideoMode(1280, 1024);
            okno = new RenderWindow(mode, "Chmurowe Starcie: Super Pilot");
            okno.Closed += new EventHandler(OnClose);

            Color windowColor = new Color(0, 192, 255);
            okno.SetFramerateLimit(60);
        }
        void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
        public int Wybor()
        {
            Menu select = new Menu(okno);
            selected = select.Wybor();
            Console.WriteLine(selected);

            if(selected == 1)
            {
                Gra Game = new Gra(okno,restart);
                restart = Game.Show();
            }
            else if(selected == 2)
            {
                okno.Close();
                restart = 0;
            }
            return restart;
        }
    }
    class Menu
    {
        public RenderWindow mywindow { get; set; }
        public Menu(RenderWindow mywindow)
        {
            this.mywindow = mywindow;
        }
        public int Wybor()
        {
            Console.WriteLine("render menu");
            int selected = 0;
            while (mywindow.IsOpen && selected == 0)
            {
                mywindow.DispatchEvents();
                if (Keyboard.IsKeyPressed(Keyboard.Key.G) )
                {
                    Console.WriteLine("G");
                    selected = 1;
                };
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    Console.WriteLine("D");
                    selected = 2;
                };

                Texture drewno = new Texture(@"resources\drewno.png", new IntRect(0, 0, (int)mywindow.Size.X, 200));
                Sprite dolne = new Sprite(drewno);
                //Shape dolne = new RectangleShape(new Vector2f(app.Size.X, 200));
                dolne.Position = new Vector2f(0, mywindow.Size.Y - 200);
                dolne.Color = new Color(139, 69, 19);   
                mywindow.Draw(dolne);
                
                // Update the window
                mywindow.Display();
            }
            return selected;
        }
    }
    class Gra
    {
        public RenderWindow okno { get; set; }
        public int restart { get; set; }
        public Gra(RenderWindow mywindow, int restart)
        {
            this.okno = mywindow;
            this.restart = restart;
        }
        public int Show()
        {
            // Create the main window
            
            Color windowColor = new Color(0, 192, 255);



            //Texture obrazek = new Texture(@"resources\samlot_kradziony2.png");
            //Sprite sprite = new Sprite(obrazek);
            //sprite.Position = new Vector2f(((1280 / 2) - (obrazek.Size.X) / 2), ((1024 / 2) - (obrazek.Size.Y) / 2));

            //Sprite menu = new Sprite(int);
            Texture drewno = new Texture(@"resources\drewno.png", new IntRect(0, 0, (int)okno.Size.X, 200));
            Sprite dolne = new Sprite(drewno);
            //Shape dolne = new RectangleShape(new Vector2f(app.Size.X, 200));
            dolne.Position = new Vector2f(0, okno.Size.Y - 200);
            dolne.Color = new Color(139, 69, 19);
            //dolne.FillColor = new Color(139, 69, 19);
            //sprite.Position = new Vector2f(900, 600);
            Texture plane = new Texture(@"resources\plane.png", new IntRect(0, 0, 0, 0));
            //Sprite samolot = new RectangleShape(new Vector2f(100, 100)); 
            Sprite samolot = new Sprite(plane);
            samolot.Position = new Vector2f(1280 - plane.Size.X, 512);
            int tick = 0, nrpocisku=0;
            // Start the game loop
            List<enemy> enemies = new List<enemy>();
            for (int kkk = 0; kkk < 10; kkk++)
            {
                enemies.Add(new enemy(okno));
            }
            List<pocisk> pociski = new List<pocisk>();
            for (int kkk = 0; kkk < 30; kkk++)
            {
                pociski.Add(new pocisk(okno,plane));
            }
            foreach (pocisk pocisk in pociski)
            {
                pocisk.SpawnPocisk();
                pocisk.Draw();
            }
            while (okno.IsOpen)
            {
                // Process events
                okno.DispatchEvents();
                if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                {
                    Console.WriteLine("R");
                    okno.Close();
                    return 1;
                };
                // Clear screen
                float Y = Mouse.GetPosition(okno).Y;
                //samolot.GetGlobalBounds().Intersects(new FloatRect(new Vector2f(1200, 0), new Vector2f(1280, 600)))
                if (Y>(1 + (int)(plane.Size.Y / 2)) && Y<(824 - (plane.Size.Y / 2)))
                {
                    samolot.Position = new Vector2f(1280 - plane.Size.X, Y - (plane.Size.Y / 2));
                }
                else
                {
                    samolot.Position = new Vector2f(samolot.Position.X, samolot.Position.Y);
                }
                    



                if (tick == 60) tick = 0;
                okno.Clear(windowColor);
                /*
                switch (ii)
                {
                    case 0:
                        sprite.Position = sprite.Position + new Vector2f(0, -obrazek.Size.Y / 100);
                        sprite.Color = Color.Green;
                        break;
                    case 1:
                        sprite.Position = sprite.Position + new Vector2f(0, obrazek.Size.Y / 100);
                        break;
                    case 2:
                        sprite.Position = sprite.Position + new Vector2f(obrazek.Size.X / 100, 0);
                        sprite.Color = Color.Blue;
                        break;
                    case 3:
                        sprite.Position = sprite.Position + new Vector2f(-obrazek.Size.X / 100, 0);
                        break;
                    case 4:
                        sprite.Position = sprite.Position + new Vector2f(0, obrazek.Size.Y / 100);
                        sprite.Color = Color.Yellow;
                        break;
                    case 5:
                        sprite.Position = sprite.Position + new Vector2f(0, -obrazek.Size.Y / 100);
                        break;
                    case 6:
                        sprite.Position = sprite.Position + new Vector2f(-obrazek.Size.X / 100, 0);
                        sprite.Color = Color.Red;
                        break;
                    case 7:
                        sprite.Position = sprite.Position + new Vector2f(obrazek.Size.X / 100, 0);
                        break;
                }
                okno.Draw(sprite);
                */

                Random random1 = new Random();
                foreach (enemy enemy in enemies)
                {
                    int d = random1.Next(50);
                    //Console.WriteLine("losowa liczba1: " + d);
                    if (d == 0)
                    {
                        //Console.WriteLine("losowa liczba1: " + d);
                        if (enemy.CheckIfSpawned() == 0)
                        {
                            int c = random1.Next(3);
                            //Console.WriteLine("losowa liczba2: " + c);
                            if (c == 0)
                            {
                                enemy.SpawnEnemy(c);
                            }
                        }
                    }
                    enemy.Move();
                    enemy.Draw();
                }
                if (tick % 5 == 0)
                {
                    //Console.WriteLine("SpawnPocisk: "+nrpocisku);
                    pociski[nrpocisku].SpawnPocisk();
                    nrpocisku++;
                }
                foreach (pocisk pocisk in pociski)
                {
                    pocisk.Move();
                    pocisk.Draw();
                }

                if (nrpocisku==30) nrpocisku = 0;

                foreach (enemy enemy in enemies)
                {
                    foreach (pocisk pocisk in pociski)
                    {
                        kolizje kolizja= new kolizje(okno, enemy.samolot, pocisk.bullet);
                        if(enemy.CheckIfSpawned() == 1)
                        {
                            if (pocisk.CheckIfSpawned() ==1)
                            {
                                if (kolizja.check())
                                {
                                    enemy.Remove();
                                    pocisk.Remove();
                                }
                                
                            }
                        }
                    }
                }
                //okno.Draw(pocisk1);
                okno.Draw(samolot);
                okno.Draw(dolne);
                // Update the window
                okno.Display();
                tick++;
            } //End game loop
            return 0;
        }
    }
    class pocisk
    {
        public RenderWindow okno { get; set; }
        private Texture plane;
        public Shape bullet;
        public pocisk(RenderWindow mywindow, Texture plane)
        {
            this.okno = mywindow;
            this.plane = plane;
        }
        public void SpawnPocisk()
        {
            bullet = new RectangleShape(new Vector2f(10, 10));
            bullet.FillColor = new Color(139, 69, 19);
            bullet.Position = new Vector2f(900, 600);
            float Y = Mouse.GetPosition(okno).Y;
            bullet.Position = new Vector2f(1280 - plane.Size.X, Y-5);
        }
        public int Draw()
        {
            if (CheckIfSpawned() == 1)
                okno.Draw(bullet);
            return 0;
        }
        public int CheckIfSpawned()
        {
            if (bullet != null)
            {
                return 1;
            }
            else return 0;

        }
        public void Move()
        {
            if (CheckIfSpawned() == 1)
            {
                bullet.Position += new Vector2f(-10, 0);
            }
        }
        public void Remove()
        {
            bullet = null;
        }
    }
    class enemy
    {
        public RenderWindow okno { get; set; }
        public Sprite samolot;
        public enemy(RenderWindow mywindow)
        {
            this.okno = mywindow;
        }
        public int Draw()
        {
            if (CheckIfSpawned() == 1) 
                okno.Draw(samolot);
            return 0;
        }
        public int SpawnEnemy(int c)
        {
            Console.WriteLine(c);
            Random random = new Random();
            Texture plane = new Texture(@"resources\enemy2.png", new IntRect(0, 0, 0, 0));
            samolot = new Sprite(plane);
            int d = random.Next(824 - (int)plane.Size.Y);
            Console.WriteLine("wysokosc spawnu: " + d);
            samolot.Position = new Vector2f(0, d);
            return 0;
        }
        public int CheckIfSpawned()
        {
            if (samolot != null)
            {
                return 1;
            }else return 0;

        }
        public void Move()
        {
            if (CheckIfSpawned() == 1)
            {
                samolot.Position += new Vector2f(2, 0);
            }
        }
        public void Remove()
        {
            samolot = null;
        }


    }
    class kolizje
    {
        public RenderWindow okno { get; set; }
        private Sprite samolot;
        private Shape bullet;
        public kolizje(RenderWindow mywindow, Sprite samolot, Shape bullet) {
            this.okno = mywindow;
            this.samolot = samolot;
            this.bullet = bullet;
            
        }
        public bool check(){
            if (samolot.GetGlobalBounds().Intersects(bullet.GetGlobalBounds()))
            {
                Console.WriteLine("przeciecie");
                samolot = null;
                bullet = null;
                return true;
            }
            return false;
        }
    }
}
//To Here