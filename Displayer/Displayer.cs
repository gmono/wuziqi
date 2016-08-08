using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core;
namespace Displayer
{
    public class Displayer : IBase
    {
        Display dp;
        public IControl[] cts = new IControl[4];
        public Displayer()
        {
            cts[0] = new Cont("人类用户1","人类用户");
            cts[1] = new Cont("人类用户2", "人类用户");
            cts[2] = new AICont("AI用户1", "自带AI用户");
            cts[3] = new AICont("AI用户2", "自带AI用户");
            dp = new Display(this);
        }
        List<IControl> IBase.Conts
        {
            get
            {
                var v = new List<IControl>();
                v.AddRange(cts);
                return v;
            }
        }

        IDisplay IBase.Displayer
        {
            get
            {
                return dp;
            }
        }
    }
}
