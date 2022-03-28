using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B10956029_0328
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UdpClient U;
        Thread Th; 

        //監聽副程序
        private void Listen()
        {
            //設定監聽用通訊Port
            int Port = int.Parse(textBox_listenPort.Text);
            //監聽UDP監聽器實體
            U = new UdpClient(Port);
            //建立本機端點資訊
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            while(true)
            {
                byte[] B = U.Receive(ref EP);//接收到的訊息放入B陣列
                textBox_receiveMsg.Text = Encoding.Default.GetString(B);//翻譯B陣列為字串
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//忽略跨執行續錯誤
            Th = new Thread(Listen);//建立監聽執行續，目標副程序-->Listen
            Th.Start();//啟動監聽執行續
            button_startListen.Enabled = false;//使buttleg失效(不能重複開啟監聽):按一次按鍵變灰色，不能在按第二下
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            try 
            {
                Th.Abort();//關閉監聽執行續
                U.Close();//關閉監聽器
            }
            catch
            {
                //忽略錯誤
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string IP = textBox_targetIP.Text;//設定發送目標IP
            int Port = int.Parse(textBox_targetPort.Text);//設定發送目標Port
            byte[] B = Encoding.Default.GetBytes(textBox_sendMsg.Text);//字串翻譯成位元
            UdpClient S = new UdpClient();//建立UDP Client
            S.Send(B, B.Length, IP, Port);//發送資料到指定位址
            S.Close();//關閉UDP Client
        }
    }
}
