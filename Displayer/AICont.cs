using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core;

namespace Displayer
{
    /// <summary>
    /// 目前还没有实现
    /// </summary>
    class AICont:Cont,IControl
    {
        public AICont(string pname,string pd):base(pname,pd)
        {
            
        }

        void IControl.startset()
        {
            byte[][] tqipan = qip();
            for(int i=0;i<tqipan.Length;++i)
            {
                for(int j=0;j<tqipan[i].Length;++j)
                {
                    if(tqipan[i][j] == 0)
                    {
                        f(this, i, j);
                        return;
                    }

                }
            }
            throw new Exception("未知错误@！！！！！");
        }
    }
}
