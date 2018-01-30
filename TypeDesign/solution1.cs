using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeDesign
{
    public abstract class DieselEngineCar : ICar
    {
        public int GetHorsePower() { return 80; }
        public int GetPrice() { return 5000; }
    }
    public abstract class V8EngineCar : ICar
    {
        public int GetHorsePower() { return 200; }
        public int GetPrice() { return 20000; }
    }
    public class Toyota : V8EngineCar
    {
    }
}
