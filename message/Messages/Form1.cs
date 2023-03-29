using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Messages
{
    public partial class Form1 : Form
    {
        private string BasePath { get; }
        private string ConfigFilePath { get; }
        public string loveCode { get; }
        public string clientId { get; }
        public string topicr { get; }
        public string topicw { get; }

        Font nfont;
        Color colorn;
        int leftMargines;
        int topMargines;
        readonly object __lock = new object();
        int distance;
        Font tfont;
        DataTable ListData;
        MqttClient messageMQTT;

        public Form1()
        {

            InitializeComponent();
            BasePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            ConfigFilePath = BasePath + "conf.ini";
            IniFile MyIni = new IniFile(ConfigFilePath);

            messageMQTT = new MqttClient("27.102.129.115", 1883, false, MqttSslProtocols.None, null, null);
            ListData = new DataTable("List");
             ListData.Columns.Add("Logs", System.Type.GetType("System.String"));
            nfont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            tfont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            colorn = Color.Black;
            leftMargines = 8;
            topMargines = 10;
            distance = 12;
           
            loveCode = MyIni.Read("lovecode", "config");
            clientId = MyIni.Read("clientId", "config");
            topicr = MyIni.Read("topicr", "config");
            topicw = MyIni.Read("topicw", "config");

            messageMQTT.Connect(clientId, "tnandzjx", "sagklashjg18fi");

            Task rejectMessage = new Task(rejectMsg);
            rejectMessage.Start();
        }




        public void rejectMsg() {
            messageMQTT.MqttMsgPublishReceived += MqttPostProperty_MqttMsgPublishReceived;
            messageMQTT.Subscribe(new string[] { topicr }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }
        private void MqttPostProperty_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            string originalRtf = System.Text.Encoding.UTF8.GetString(e.Message);
            if (originalRtf == "shisangeyi")
            {
                string path = "sound.wav";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(path);
                player.PlaySync();//另起线程播放
            }
            else {
                addLogToFrom(DateTime.Now.ToLocalTime().ToString() + "她/他：" + " \n" + originalRtf);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            addLogToFrom(DateTime.Now.ToLocalTime().ToString() + "我：" + richTextBox1.Text);

            if (!messageMQTT.IsConnected)
            {
                var result = messageMQTT.Connect(clientId, "tnandzjx", "sagklashjg18fi");
            }
            byte[] netWorkBytesSender = Encoding.UTF8.GetBytes(richTextBox1.Text);
            ushort reslt = messageMQTT.Publish(topicw, netWorkBytesSender, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

            richTextBox1.Text = "";
        }


        public void addLogToFrom(string logs)
        {
            lock (__lock)
            {
                ListData.Rows.Add(
                    new object[]
                    {
                   logs
                    });
                Action<string> action = (logs) =>
                {
                    this.listBox1.Enabled = false;
                    this.listBox1.Items.Clear();
                    for (int i = 0; i <= ListData.Rows.Count - 1; i++)
                    {
                        this.listBox1.Items.Add(i);
                    }
                    this.listBox1.Enabled = true;
                    listBox1.SetSelected(listBox1.Items.Count - 1, true);
                    if (ListData.Rows.Count > 100)
                    {
                        ListData.Rows.Clear();
                        this.listBox1.Items.Clear();
                    }
                };
                Invoke(action, logs);
            }
        }


        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出吗?", "温馨提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                System.Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {


            Graphics gfx = e.Graphics;
            int number = e.Index + 1;

            //string xcaption = ListData.Rows[e.Index]["Caption"].ToString();
            string dtext = ListData.Rows[e.Index]["Logs"].ToString();

            SizeF f1 = gfx.MeasureString(number.ToString() + ".", nfont);
            // SizeF f11 = gfx.MeasureString(xcaption, nfont);

            Rectangle rect = new Rectangle(leftMargines + (int)f1.Width + 8, topMargines + (int)f1.Height + distance, this.listBox1.ClientSize.Width - leftMargines - (int)f1.Width - 8, e.ItemHeight);
            StringFormat stf = new StringFormat();
            stf.FormatFlags = StringFormatFlags.FitBlackBox;
            SizeF f2 = gfx.MeasureString(dtext, tfont, rect.Width, stf);
            int Temp = e.ItemWidth;

            //if (f2.Width < (leftMargines + f1.Width + 8 + f11.Width)) 
            //    Temp = leftMargines + (int)f1.Width + 8 + (int)f11.Width + 4;
            if (f2.Width < (leftMargines + f1.Width + 8))
                Temp = leftMargines + (int)f1.Width + 8 + 4;//行间距


            e.ItemHeight = topMargines + (int)f1.Height + distance + (int)f2.Height + 8 + 8;//上下高度

        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

            int number = e.Index + 1;

            //string xcaption = ListData.Rows[e.Index]["Caption"].ToString();
            string dtext = ListData.Rows[e.Index]["Logs"].ToString();

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                Graphics gfx = e.Graphics;

                Pen pen = new Pen(Brushes.SteelBlue, 0.4f);

                gfx.DrawRectangle(pen, e.Bounds.X + 4, e.Bounds.Y + 4, e.Bounds.Width - 8, e.Bounds.Height - 8);
                gfx.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 9, e.Bounds.Height - 9);


                gfx.DrawString(number.ToString() + ".", nfont, Brushes.Black, e.Bounds.X + leftMargines, e.Bounds.Y + topMargines);
                SizeF f1 = gfx.MeasureString(number.ToString() + ".", nfont);

                //gfx.DrawString(xcaption, nfont, Brushes.SteelBlue, e.Bounds.X + leftMargines + f1.Width + 8, e.Bounds.Y + topMargines);
                //SizeF f11 = gfx.MeasureString(xcaption, nfont);

                Rectangle rect = new Rectangle(e.Bounds.X + leftMargines + (int)f1.Width + 8, e.Bounds.Y + topMargines + (int)f1.Height + distance, this.listBox1.ClientSize.Width - leftMargines - (int)f1.Width - 8, this.ClientSize.Height);
                StringFormat stf = new StringFormat();
                stf.FormatFlags = StringFormatFlags.FitBlackBox;

                gfx.DrawString(dtext, tfont, Brushes.Black, rect, stf);


            }
            else
            {
                Graphics gfx = e.Graphics;
                gfx.FillRectangle(Brushes.White, e.Bounds.X + 4, e.Bounds.Y + 4, e.Bounds.Width - 7, e.Bounds.Height - 7);
                gfx.FillRectangle(Brushes.SteelBlue, e.Bounds.X + 4, e.Bounds.Y + e.Bounds.Height - 8, e.Bounds.Width - 8, 1);
                Pen pen = new Pen(Brushes.SteelBlue, 0.4f);

                gfx.DrawString(number.ToString() + ".", nfont, Brushes.Black, e.Bounds.X + leftMargines, e.Bounds.Y + topMargines);
                SizeF f1 = gfx.MeasureString(number.ToString() + ".", nfont);

                //gfx.DrawString(xcaption, nfont, Brushes.SteelBlue, e.Bounds.X + leftMargines + f1.Width + 8, e.Bounds.Y + topMargines);
                //SizeF f11 = gfx.MeasureString(xcaption, nfont);

                Rectangle rect = new Rectangle(e.Bounds.X + leftMargines + (int)f1.Width + 8, e.Bounds.Y + topMargines + (int)f1.Height + distance, this.listBox1.ClientSize.Width - leftMargines - (int)f1.Width - 8, this.ClientSize.Height);
                StringFormat stf = new StringFormat();
                stf.FormatFlags = StringFormatFlags.FitBlackBox;
                gfx.DrawString(dtext, tfont, Brushes.Black, rect, stf);
            }
            e.DrawFocusRectangle();

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!messageMQTT.IsConnected)
            {
                var result = messageMQTT.Connect(clientId, "tnandzjx", "sagklashjg18fi");
            }
            byte[] netWorkBytesSender = Encoding.UTF8.GetBytes("shisangeyi");
            ushort reslt = messageMQTT.Publish(topicw, netWorkBytesSender, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                addLogToFrom(DateTime.Now.ToLocalTime().ToString() + "我：" + richTextBox1.Text);

                if (!messageMQTT.IsConnected)
                {
                    var result = messageMQTT.Connect(clientId, "tnandzjx", "sagklashjg18fi");
                }
                byte[] netWorkBytesSender = Encoding.UTF8.GetBytes(richTextBox1.Text);
                ushort reslt = messageMQTT.Publish(topicw, netWorkBytesSender, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

                richTextBox1.Text = "";
            }
        }
    }
}
