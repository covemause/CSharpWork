using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sample001
{
    public partial class Form1 : Form
    {

        static int cnt = 0;
        BlockingCollection<string> bc;
        CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();

            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.button1.Text = "Start";
            this.button2.Text = "Stop";
            this.button3.Text = "Add";

            button3.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = true;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
            bc.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            bc = new BlockingCollection<string>();
            WorkData wkdt = new WorkData();
            cmdBase cmd = new cmdBase();

            Task.Run(() =>
            {

                while (cts.IsCancellationRequested == false)
                {
                    string s;
                    try
                    {
                        s = bc.Take(cts.Token);
                        QueueRefresh();
                    }
                    catch (OperationCanceledException)
                    {
                        s = "[canceled]";
                    }

                    // 処理開始msg
                    LabelRefresh(s);

                    string wk = s.Substring(s.Length - 1, 1);

                    if (wk.Length != 0)
                    {
                        int tmp = 0;
                        int.TryParse(wk, out tmp);
                        if (tmp != 0 )
                        {
                            // 末尾の数値が偶数ならA、奇数ならBを実行
                            if (tmp % 2 == 0)
                            {
                                cmd = new cmdPatternA();
                            }
                            else
                            {
                                cmd = new cmdPatternB();
                            }

                            cmd.SetWork(wkdt);
                            cmd.AddCount();
                            
                        }
                    }

                    // 重い処理
                    for (int i= 0; i<1000000000;i++)
                    {
                        int x =0;
                        x += 1;
                    }



                    // 処理完了msg
                    AddMessage(DateTime.Now.ToString() + " " + s);
                }
            }).ContinueWith(t =>
            {
                cts.Dispose();
                bc.Dispose();
                button1.Invoke(new Action(() =>
                {
                    button3.Enabled = false;
                    button2.Enabled = false;
                    button1.Enabled = true;
                }));
            });
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bc.Add("[Add]" + cnt++);
            QueueRefresh();
        }

        // 処理完了msgをGUIスレッドの移譲
        void AddMessage(string msg)
        {
            textBox1.Invoke(new Action(() => {
                textBox1.AppendText(msg + Environment.NewLine);
            }));
        }

        // Queueが処理されている情報表示をGUIスレッドの移譲
        void QueueRefresh()
        {
            textBox2.Invoke(new Action(() => {
                string s = "";
                foreach (string b in bc)
                {
                    s += b;
                }
                textBox2.AppendText(s + Environment.NewLine);
            }));
        }

        // 処理開始msgの表示をGUIスレッドの移譲
        void LabelRefresh(string s)
        {
            label.Invoke(new Action(() => {
                label.Text = s +  " を処理しています。";
            }));
        }

    }
}
