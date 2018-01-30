using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeDesign
{
    public class EngineType
    {
        public static EngineType V8 = new EngineType(20000, 200);
        public static EngineType Diesel = new EngineType(5000, 80);

        private EngineType(int price, int horsePower)
        {
            Price = price;
            HorsePower = horsePower;

        }
        public int Price { get; private set; }
        public int HorsePower { get; private set; }
    }
}
