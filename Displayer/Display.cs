using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core;
using System.Windows.Forms;
using System.Drawing;
namespace Displayer
{
    class Display : Form, IDisplay
    {
        //是否启用的列表
        List<bool> enlist = new List<bool>(new bool[] { true,true,false,false});

        void setqizi(byte i,int r,int c)
        {
            //下面是标准代码段1（懒得弄成函数了）
            int c1 = -1, c2 = -1;
            int t = 0;
            foreach (var v in enlist)
            {
                if (v)
                {
                    if (c1 == -1) c1 = t+1;
                    else c2 = t+1;
                    
                }
                t++;
            }


            SolidBrush p = new SolidBrush(i == 0 ? Color.Yellow : (i == c1 ? Color.Black : Color.White));
            int y = r * 50;
            int x = c * 50;
            
            CreateGraphics().FillEllipse(p, x, y, 50, 50);
            
        }

        private Button button1;
        private Label label1;
        private Button button2;
        private Button button3;
        qipan qp = null;
        void IDisplay.init(qipan qip)
        {
            //注意，棋盘的初始化函数会调用这个函数，所以这里不能调用棋盘的init函数以防止无限递归
            qp = qip;
            initqipan();
        }

        void IDisplay.seted(byte id, int r, int c)
        {
            setqizi(id, r, c);
        }

        void IDisplay.start()
        {

            if (Visible == false)
                Application.Run(this);
        }
        void initqipan()
        {
            for (int i = 0; i < qp.q_data.Length; i++)
            {
                for (int j = 0; j < qp.q_data[i].Length; j++)
                {
                    setqizi(qp.q_data[i][j], i, j);
                }
            }
        }
        void IDisplay.win(byte id)
        {
            int c1 = -1, c2 = -1;
            int t = 0;
            foreach (var v in enlist)
            {
                if (v)
                {
                    if (c1 == -1) c1 = t + 1;
                    else c2 = t + 1;

                }
                t++;
            }
            MessageBox.Show(id == c1 ? "黑方赢" : "白方赢");
            //initqipan(); //在init中,每次赢后init自动被调用
            winsign = true;//设置赢棋标志
            //fix();  //这里重定位没意义
        }

        Displayer dp;
        public Display(Displayer dper)
        {
            InitializeComponent();
            
            dp = dper;
        }
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(697, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "初始化棋盘";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 740);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(671, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "目前用按钮代替，请在启动后点一下这个初始化按钮，如果要重新开始也可以点上面的按钮,控制者最好在启动时修改…………";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(697, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 2;
            this.button2.Text = "显示控制者列表";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(697, 109);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 75);
            this.button3.TabIndex = 3;
            this.button3.Text = "开始(用于双AI对战，使下一个用户放子)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Display
            // 
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Display";
            this.Text = "五子棋";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Display_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        int curindex=0;

        private void next()
        {
            if (curindex < enlist.Count-1)
            {
                //如果不是在最末尾，才执行先后搜索
                //搜索从当前项后面开始
                for (int i = curindex+1; i < enlist.Count; ++i)
                {
                    if (enlist[i])
                    {
                        curindex = i;
                        return;
                        //可用的话
                    }
                }
            }
            for(int i=0;i<curindex;++i)
            {
                if (enlist[i])
                {
                    curindex = i;
                    return;
                    //可用的话
                }
            }
            //这里就出错了
            throw new Exception("发生未知错误（搜索超界)");
        }
        private void Display_MouseDown(object sender, MouseEventArgs e)
        {

            int r = e.Y / 50;
            int c = e.X / 50;

            //防止错误和防止重复放置,注意，棋盘本身判断防止错误会直接返回，并且不返回结果
            if (r >= 12 || c >= 12 || r < 0 || c < 0 || qp.q_data[r][c] != 0) return;

            //
            if (enlist[curindex] == false) return;//如果这个控制者已经禁用则返回
            if (curindex == 0 || curindex == 1) {
                ((Cont)dp.cts[curindex]).set(r, c);
                next();
                if (curindex == 0 || curindex == 1) return;//如果下一个是人类控制者就直接返回
                qp.conts[curindex].startset();//否则调用开始函数
                next();//调用完了必须next

                if (winsign) { fix(); winsign = false; }//如果赢棋就重定位
                //因为是单线程，所以不用担心重复调用
            }//这是UI控制者的情况
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //不知道为什么构造函数里执行这个语句无用
            //所以用按钮代替，直接执行棋盘的init，然后由棋盘重新初始化所有控制者以及显示器
            qp.init();
        }

        /// <summary>
        /// 定为到第一个控制者
        /// </summary>
        private void fix()
        {
            for (int i = 0; i < enlist.Count; ++i)
            {
                if (enlist[i])
                {
                    curindex = i;
                    return;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CList f = new CList(qp, enlist);
            f.ShowDialog();
            //定位到第一个控制者
            fix();
        }

        bool winsign = false;
        private void button3_Click(object sender, EventArgs e)
        {
            //调用当前控制者的开始函数,人类控制者无效
            if (!(curindex == 0 || curindex == 1))
            {
                if (!enlist[curindex]) next();
                qp.conts[curindex].startset();
                next();
            }
            while(!winsign)
            {
                
                if (curindex == 0 || curindex == 1) return;
                qp.conts[curindex].startset();
                next();
            }

            winsign = false;
            fix();
            //不用担心人机对战中按这个按钮，因为AI放完子后，才人放，期间这个按钮不可用
        }
    }
}
