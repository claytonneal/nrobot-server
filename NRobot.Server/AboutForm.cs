using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace NRobot.Server
{

	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void OKButtonClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void AboutFormShown(object sender, EventArgs e)
		{
			VersionLabel.Text = String.Format("Version {0}",Assembly.GetExecutingAssembly().GetName().Version);
		}
		
		void ProjectHomeLinkLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            Process.Start("https://github.com/claytonneal/nrobot-server");
		}
	}
}
