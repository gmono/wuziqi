using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
namespace core
{
    class Program
    {

        //加载时以第一个加载到的displayer为准，暂不支持多displayer
        static int Main(string[] args)
        {
            if(args.Length==0)
            {
                Console.WriteLine("参数过少……");
                Console.ReadKey();
                return 1;
            }
            List<IControl> clist = new List<IControl>();
            IDisplay dp = null;
            foreach (var v in args)
            {
                FileInfo info = new FileInfo(v);
                Assembly asm = Assembly.LoadFile(info.FullName);
                Type[] ts = asm.GetTypes();
                foreach(var t in ts)
                {
                    Type type=t.GetInterface("core.IBase");
                    if(type!= null)
                    {
                        IBase b = (IBase)Activator.CreateInstance(t);
                        if (dp == null && b.Displayer != null) dp = b.Displayer;
                        if(b.Conts!=null)
                        {
                            clist.AddRange(b.Conts);
                        }
                    }
                }
                
            }
            if(clist.Count<2||dp==null)
            {
                Console.WriteLine("参数不正确！");
                Console.ReadKey();
                return 2;
            }
            //初始化在这里调用，构造函数里
            qipan qp = new qipan(dp, clist);
            //启动
            dp.start();
            return 0;
        }
    }
}
