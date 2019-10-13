using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Sbn.Products.SVN.SVNClient
{
    public class SVNNotify
    {


        public static Thread _Thread = null;
        public static TimeSpan _Delay = new TimeSpan(0, 15, 0);
        public static ManualResetEvent _Reset = null;

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.ComponentModel.IContainer components;
        private Window _mainWindow;

        public void ShowBaloonTip(string message)
        {

            notifyIcon1.BalloonTipText = message;

            notifyIcon1.ShowBalloonTip(1000);

        }
        public void CreateNotifyicon(Window mainWindow)
        {
            

            _mainWindow = mainWindow;

            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = global::Sbn.Products.SVN.SVNClient.Properties.Resources.Graphicloads_Colorful_Long_Shadow_Html_tags;// new Icon("Icon1.ico");

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "Console App (Console example)";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);

        }
        private void notifyIcon1_Click(object Sender, EventArgs e)
        {

            _mainWindow.Show();

        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            //MessageBox.Show("Double clicked");
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            _Reset.Set();
            _Thread.Join(100);

            Application.Current.Shutdown();
            //System.Windows.Forms.Application.Exit();

        }
        //
    }
}
