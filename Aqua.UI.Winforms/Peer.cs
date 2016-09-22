namespace Aqua.UI.Winforms
{
    using System;
    using System.Drawing;
    using System.Net;
    using System.Windows.Forms;

    public partial class Peer : Form
    {
        private const string PlaceholderText = "e.g. 192.168.1.1:1000";

        public IPAddress Address
        {
            get
            {
                IPAddress address = IPAddress.None;
                IPAddress.TryParse(ipString.Text.Split(':')[0], out address);
                return address;
            }
        }

        public int Port
        {
            get
            {
                int port = -1;
                int.TryParse(ipString.Text.Split(':')[1], out port);
                return port;
            }
        }

        public Peer()
        {
            InitializeComponent();

            ipString.GotFocus += (sender, e) =>
                                 {
                                     if (ipString.Text == PlaceholderText)
                                     {
                                         ipString.Text = string.Empty;
                                         ipString.ForeColor = Color.Black;
                                     }
                                 };

            ipString.LostFocus += (sender, e) =>
                                  {
                                      if (string.IsNullOrWhiteSpace(ipString.Text))
                                      {
                                          ipString.Text = PlaceholderText;
                                          ipString.ForeColor = Color.Gray;
                                      }
                                  };
        }

        private void ValidateInputs()
        {
            if (this.Address == IPAddress.None)
            {
                MessageBox.Show("IP Address was invalid.");
            }

            if (this.Port == -1)
            {
                MessageBox.Show("Port number was invalid");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ValidateInputs();
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
