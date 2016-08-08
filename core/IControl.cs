using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    /// <summary>
    /// 每次赢棋，win先被调用，init后被调用
    /// </summary>
    public interface IControl
    {

        /// <summary>
        /// 初始化函数，第一次加载和每次赢棋后调用
        /// </summary>
        /// <param name="fun">放子函数</param>
        /// <param name="gqipan">取得棋盘数据的函数</param>
        /// <param name="id">自己的id</param>
        void init(HSetQiZi fun,HGetQiPan gqipan,byte id);
        /// <summary>
        /// 赢棋通知
        /// </summary>
        void win();
        /// <summary>
        /// 名字可以重复，但最好不重复
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 控制者放子，包括自己
        /// </summary>
        void seted(byte id,int r,int c);
        /// <summary>
        /// 说明文字
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 通知控制者开始放子
        /// </summary>
        void startset();
    }
}
