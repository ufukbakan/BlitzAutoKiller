using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;

namespace BlitzAutoKiller
{
    public class TrayApp : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private string blitzpath;
        private bool isBlitzRunning, isLeagueRunning, isClientRunning;
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
                        Text = "Ufuk Bakan 2022",
                        Enabled = false
                    },
                    new MenuItem("Exit", Exit)
                })
            ,
                Visible = true,
            };

            trayIcon.MouseClick += TrayIcon_MouseClick;
            blitzpath =
                (new System.Text.StringBuilder()).Append(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                .Append("\\Programs\\Blitz\\Blitz.exe").ToString();

            ManagementEventWatcher startWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(ProcessTrigger);
            startWatch.Start();
            ManagementEventWatcher stopWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(ProcessTrigger);
            stopWatch.Start();

            GC.Collect(0, GCCollectionMode.Forced);
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            GC.Collect(0, GCCollectionMode.Forced);
        }

        private bool checkLeague()
        {
            return Process.GetProcessesByName("League of Legends").Length > 0;
        }

        private bool checkBlitz()
        {
            return Process.GetProcessesByName("Blitz").Length > 0;
        }

        private bool checkClient()
        {
            return Process.GetProcessesByName("LeagueClient").Length > 0;
        }

        private void killBlitz()
        {
            foreach (Process p in Process.GetProcessesByName("Blitz"))
                try
                {
                    p.Kill();
                }
                catch { }
        }

        private void ProcessTrigger(object sender, EventArgs e)
        {
            isBlitzRunning = checkBlitz();
            isLeagueRunning = checkLeague();

            if (isLeagueRunning && isBlitzRunning)
            {
                killBlitz();
            }
            else if (!isLeagueRunning)
            {
                isClientRunning = checkClient();
                if (isClientRunning && !isBlitzRunning)
                {
                    Process.Start(blitzpath);
                }
                else if(!isClientRunning && isBlitzRunning)
                {
                    killBlitz();
                }
            }

            GC.Collect(0, GCCollectionMode.Optimized);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Exit(sender,e);
        }

        void Exit(object sender, EventArgs e)
        {
            try
            {
                //startWatch.Stop();
                //stopWatch.Stop();
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.Visible = false;
            }
            catch {}
            finally
            {
                Application.Exit();
            }
        }
    }
}
