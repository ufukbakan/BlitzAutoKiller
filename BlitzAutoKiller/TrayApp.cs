using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace BlitzAutoKiller
{
    public class TrayApp : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Timer timer;
        private string blitzpath;
        private bool isLeagueRunning;
        private bool isBlitzRunning;

        public TrayApp()
        {
            // Initialize Components

            trayIcon = new NotifyIcon()
            {
                Text = "BlitzAutoKiller",
                Icon = Properties.Resources.autoblitz,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem() {
                        Text = "Auto Blitz Killer",
                        Enabled = false
                    },
                    new MenuItem() {
                        Text = "Ufuk Bakan 2021",
                        Enabled = false
                    },
                    new MenuItem("Exit", Exit)
                })
            ,
                Visible = true,
            };

            trayIcon.MouseClick += TrayIcon_MouseClick;

            blitzpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Programs\\Blitz\\Blitz.exe";
            timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
            GC.Collect(0, GCCollectionMode.Forced);

        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            GC.Collect(0, GCCollectionMode.Forced);
        }

        private void checkLeague()
        {
            if (Process.GetProcessesByName("League of Legends").Length < 1)
                isLeagueRunning = false;
            else
                isLeagueRunning = true;
        }

        private void checkBlitz()
        {
            if (Process.GetProcessesByName("Blitz").Length < 1)
                isBlitzRunning = false;
            else
                isBlitzRunning = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            checkBlitz();
            checkLeague();

            if (isLeagueRunning && isBlitzRunning)
                foreach (Process p in Process.GetProcessesByName("Blitz"))
                    try
                    {
                        p.Kill();
                    }
                    catch { }

            else if (!isLeagueRunning && !isBlitzRunning)
                Process.Start(blitzpath);

            GC.Collect(0, GCCollectionMode.Forced);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Exit(sender,e);
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}
