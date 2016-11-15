using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using log4net;
using NRobot.Server.Imp;
using NRobot.Server.Imp.Config;

namespace NRobot.Server
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		
		//log4net
		private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
		
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Log.Debug("Starting NRobot Server");
			var trayapp = new TrayApplication();
			if (trayapp.IsRunning)
			{
				Application.Run(trayapp);
			}
			Application.Exit();
		}
		
	}
	
	public class TrayApplication : ApplicationContext
	{
		
		//fields
		private NotifyIcon _trayicon;
		private ContextMenuStrip _contextmenu;
		private ToolStripMenuItem _exitoption;
		private ToolStripMenuItem _aboutoption;
		private ToolStripMenuItem _keywordsoption;
        private NRobotService _service;
	    private NRobotServerConfig _serviceConfig;
		public bool IsRunning;
		
		//constructor
		public TrayApplication()
		{
			//setup controls
			_contextmenu = new ContextMenuStrip();
			_exitoption = new ToolStripMenuItem("Exit");
			_aboutoption = new ToolStripMenuItem("About");
			_keywordsoption = new ToolStripMenuItem("Keywords");
			_trayicon = new NotifyIcon();
			//setup tray icon and tooltip
			System.IO.Stream st;
            System.Reflection.Assembly a = Assembly.GetExecutingAssembly();
            st = a.GetManifestResourceStream("LogoIcon");
            _trayicon.Icon = new System.Drawing.Icon(st); 
            _trayicon.Text = String.Format("NRobot.Server version {0}",Assembly.GetExecutingAssembly().GetName().Version);
            //setup context menu
            _contextmenu.Items.Add(_keywordsoption);
            _contextmenu.Items.Add(_aboutoption);
            _contextmenu.Items.Add(_exitoption);
            //setup events
            _exitoption.Click += ExitOptionClick;
            _aboutoption.Click += AboutOptionClick;
            _keywordsoption.Click += KeywordsOptionClick;
            //display
            _trayicon.ContextMenuStrip = _contextmenu;
            _trayicon.Visible = true;
            
            //setup nrobot server
            try
			{
	        	//start service
			    _serviceConfig = NRobotServerConfig.LoadXmlConfiguration();
				_service = new NRobotService(_serviceConfig);
				_service.StartAsync();
				IsRunning = true;
				
			}
			catch (Exception e)
			{
				MessageBox.Show(String.Format("Unable to start NRobot.Server: \n\n{0}",e.ToString()),"Error",MessageBoxButtons.OK);
				IsRunning = false;
			}
		
		}
		
		/// <summary>
		/// Handles exit context menu click
		/// </summary>
		public void ExitOptionClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		/// <summary>
		/// Handles about context menu click
		/// </summary>
		public void AboutOptionClick(object sender, EventArgs e)
		{
			var frm = new AboutForm();
			frm.ShowDialog();
		}
		
		/// <summary>
		/// Handles keywords context menu click
		/// </summary>
		public void KeywordsOptionClick(object sender, EventArgs e)
		{
			Process.Start(String.Format("http://localhost:{0}",_serviceConfig.Port));
		}
		
	}
}
