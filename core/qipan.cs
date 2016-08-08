using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public delegate void HSetQiZi(IControl con,int r,int c);
    public delegate byte[][] HGetQiPan();
    /// <summary>
    /// 
    /// 控制者数量小于255
    /// </summary>
    public class qipan
    {
        public byte[][] q_data = null;//棋盘数据,0代表空，1代表第一控制者的子，2代表第二控制者的子,依此类推
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dper">显示者</param>
        /// <param name="cons">控制者列表,按列表顺序编号,第一个控制者编号为1</param>
        public qipan(IDisplay dper,IEnumerable<IControl> cons)
        {
            if (dper == null || cons == null) throw new Exception("初始化失败……");
            dp = dper;
            //初始化棋盘目前懒得加参数就定为 12*12了
            q_data = new byte[12][];
            for(int i=0;i<q_data.Length;i++)
            {
                q_data[i] = new byte[12];
            }
            foreach(var v in q_data)
            {
                for(int i=0;i<v.Count();++i)
                {
                    v[i] = 0;
                }
            }
            //控制者优先初始化
            foreach(var v in cons)
            {
                conts.Add(v);
                dict.Add(v, (byte)conts.Count);
                //使用反向查找表获取id
                v.init((IControl con,int r,int c)=> {
                    byte b = dict[con];
                    SetQiZi(b, r, c);
                },()=> (byte[][])q_data.Clone(),(byte)conts.Count); 
            }
            //初始化对象
            dper.init(this);
        }
        public void init()
        {
            //初始化棋盘
            foreach (var v in q_data)
            {
                for (int t = 0; t < v.Count(); ++t)
                {
                    v[t] = 0;
                }
            }

            //初始化控制者优先原则
            int i = 1;
            foreach (var v in conts)
            {
                //使用反向查找表获取id
                v.init((IControl con, int r, int c) => {
                    byte b = dict[con];
                    SetQiZi(b, r, c);
                }, () => (byte[][])q_data.Clone(),(byte)i);
                i++;
            }
            dp.init(this);
        }
        IDisplay dp = null;
        public List<IControl> conts = new List<IControl>();
        //方查找表
        public Dictionary<IControl, byte> dict = new Dictionary<IControl, byte>();

        //此函数专用委托
        delegate bool HSFun(Func<bool> a);
        /// <summary>
        /// 放棋子
        /// </summary>
        /// <param name="id">控制者iD</param>
        /// <param name="r">位置行</param>
        /// <param name="c">位置列</param>
        public void SetQiZi(byte id,int r,int c)
        {
            //防止错误和防止重复放置
            if (r >= 12 || c >= 12||r<0||c<0||q_data[r][c]!=0) return;
            q_data[r][c] = id;
            //告诉所有对象已经放子,控制者优先
            foreach (var v in conts)
            {
                v.seted(id, r, c);
            }
            dp.seted(id, r, c);
            //判断赢家
            for (int i=0;i<q_data.Length;++i)
            {
                for(int j=0;j<q_data[i].Length;++j)
                {
                    if (q_data[i][j] != 0)
                    {
                        int temp = q_data[i][j];
                        int ti = i, tj = j;//临时变量
                                           //以下为判断函数

                        HSFun fun = (Func<bool> a) =>
                           {
                               //循环4次
                               for (int k = 1; k < 5; ++k)
                               {
                                   if (!a()) return false;
                                   if (q_data[ti][tj] == temp) continue;
                                   else return false;
                               }
                               return true;
                           };
                        //横向判断
                        Func<bool> a1 = () =>
                         {
                             tj--;
                             if (tj < 0) return false;
                             return true;
                         };
                        //横向右判断
                        Func<bool> a2 = () =>
                        {
                            tj++;
                            if (tj >= q_data[i].Length) return false;
                            return true;
                        };
                        //纵向判断
                        Func<bool> a3 = () =>
                          {
                              ti--;
                              if (ti < 0) return false;
                              return true;
                          };
                        //纵向下判断
                        Func<bool> a4 = () =>
                        {
                            ti++;
                            if (ti >= q_data.Length) return false;
                            return true;
                        };
                        //斜向左上判断
                        Func<bool> a5 = () =>
                        {
                            if (a1() && a3()) return true;
                            return false;
                        };
                        //右上
                        Func<bool> a6 = () =>
                        {
                            if (a2() && a3()) return true;
                            return false;
                        };
                        //左下
                        Func<bool> a7 = () =>
                        {
                            if (a1() && a4()) return true;
                            return false;
                        };
                        //右下
                        Func<bool> a8 = () =>
                        {
                            if (a2() && a4()) return true;
                            return false;
                        };


                        //如果任何一次返回真则temp代表的控制者赢
                        Func<bool>[] ass = { a1,a2,a3,a4,a5,a6,a7,a8};
                        foreach(var v in ass)
                        {
                            if(fun(v))
                            {
                                
                                //赢了

                                //赢棋通知遵循控制者优先原则
                                conts[id-1].win();

                                dp.win(id);
                                //注意最后初始化棋盘,初始化棋盘会自动调用显示器和控制者的init函数
                                init();
                                return;
                                //
                            }
                            ti = i;tj = j;

                        }
                    }
                
                }
            }

        }
        /// <summary>
        /// 添加控制者
        /// </summary>
        /// <param name="c">控制者对象</param>
        /// <returns>控制者ID</returns>
        public byte AddControl(IControl c)
        {
            if (c == null) return 0;
            conts.Add(c);
            dict.Add(c, (byte)conts.Count);
            return (byte)conts.Count;
        }
    }
}
