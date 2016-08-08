using core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
namespace Displayer
{
    public partial class CList : Form
    {
        qipan qp;
        List<bool> eb;
        public CList(qipan pqp,List<bool> b)
        {
            InitializeComponent();
            qp = pqp;

            eb = b;
        }

        //这是映射表,注意目前控制者只能添加不能删除
        Dictionary<int, string> ndmap = new Dictionary<int, string>();
        
        private void CList_Load(object sender, EventArgs e)
        {
            int t = 0;
            foreach(var v in qp.conts)
            {
                listBox1.Items.Add(v.Name);
                ndmap.Add(t, v.Description);
                t++;
            }
            if (t != 0) listBox1.SelectedIndex = 0; //如果列表里有则选中第一个
            for(int i=0;i<eb.Count;++i)
            {
                if (eb[i]) listBox1.Items[i] = listBox1.Items[i] + "[已激活]";
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            if (listBox1.Items.Count == 0) return;//保险
            string ds = ndmap[listBox1.SelectedIndex];
            label1.Text = ds;
            label1.Text += "\n\n ID：";
            label1.Text += (listBox1.SelectedIndex+1).ToString();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //目前还没有实现
        }
        int curjihuo = 2;
        private void 激活ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(curjihuo<2)
            {
                listBox1.Items[listBox1.SelectedIndex] += "[已激活]";
                eb[listBox1.SelectedIndex] = true;
                curjihuo++;
            }
        }

        private void 取消激活ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(eb[listBox1.SelectedIndex])
            {
                eb[listBox1.SelectedIndex] = false;
                listBox1.Items[listBox1.SelectedIndex] = ((string)listBox1.Items[listBox1.SelectedIndex]).Substring(0, ((string)listBox1.Items[listBox1.SelectedIndex]).Length - 5);
                curjihuo--;
            }
        }

        private void CList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (curjihuo == 2) return;
            MessageBox.Show("激活的项不为2，自动选定没有激活的第一项", "警告！！");
            for(int i=0;i<eb.Count;++i)
            {
                if(eb[i]==false)
                {
                    eb[i] = true;
                    return;
                }
            }
            //到这里说明发生未知错误
            throw new Exception("发生未知错误！");
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = openFileDialog1.ShowDialog();
            if(r==DialogResult.OK)
            {
                Assembly asm = Assembly.LoadFile(openFileDialog1.FileName);
                if (asm == null) return;
                Type[] ts = asm.GetTypes();
                foreach(var v in ts)
                {
                    Type t=v.GetInterface("core.IControl");
                    if(t!=null)
                    {
                        IControl c = (IControl)Activator.CreateInstance(t);
                        qp.AddControl(c);
                        eb.Add(false);//由于不会删除，所以顺序添加不用管返回的ID号
                    }
                }
            }
        }
    }
}
