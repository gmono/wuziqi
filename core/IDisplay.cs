using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    /// <summary>
    /// 每次赢棋，win先被调用，init后被调用
    /// 注意一切遵循控制者优先原则
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// 初始化函数,会在被加载和每次赢棋后调用
        /// </summary>
        /// <param name="qip">棋盘对象，显示者拥有完全控制权</param>
         void init(qipan qip);
        /// <summary>
        /// 赢棋通知函数，每次赢棋调用
        /// </summary>
        /// <param name="id">赢棋的空着者id</param>
        void win(byte id);
        /// <summary>
        /// 
        /// 放子通知函数，每次放子时通知
        /// </summary>
        /// <param name="id"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        void seted(byte id, int r, int c);

        /// <summary>
        /// 第一次加载完毕后调用
        /// </summary>
        void start();
    }
}
