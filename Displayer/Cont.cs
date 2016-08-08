using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core;
namespace Displayer
{
    class Cont : IControl
    {
        protected HSetQiZi f;
        protected HGetQiPan qip;
        byte myid;

        //名字相关
        string name=null;
        string dess = null;
        public Cont(string pname,string des)
        {
            name = pname;
            dess = des;
        }
        string IControl.Name
        {
            get
            {
                return name;
            }
        }

        string IControl.Description
        {
            get
            {
                return dess;
            }
        }

        void IControl.init(HSetQiZi fun, HGetQiPan gqipan,byte id)
        {
            
            f = fun;
            qip = gqipan;
            myid = id;
        }

        void IControl.seted(byte id, int r, int c)
        {
            
        }

        void IControl.win()
        {
            
        }

        public void set(int r,int c)
        {
            f(this, r, c);
        }
        
        /// <summary>
        /// 这个函数在这里什么都不做
        /// </summary>
        void IControl.startset()
        {

        }
    }
}
