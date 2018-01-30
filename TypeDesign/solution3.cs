using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeDesign
{
    public interface ICar
    {
        IEngineType Engine { get; }
    }

    public interface IEngineType
    {
        int Price { get; }
        int HorsePower { get; }
    }

    public class V8Engine : IEngineType
    {
        public int HorsePower { get { return 200; } }
        public int Price { get { return 20000; } }
    }

    public class Hyundai : ICar
    {
        public Hyundai()
        {
            Engine = new V8Engine();
        }
        public IEngineType Engine { get; set; }
    }
}
