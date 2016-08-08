using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    /// <summary>
    /// 实现IBase接口以便被core.exe直接加载
    /// </summary>
    public interface IBase
    {
        //没有就为null
        List<IControl> Conts { get; }
        IDisplay Displayer { get; }

    }
}
