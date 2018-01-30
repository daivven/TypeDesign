using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeDesign
{
    public class EngineCar:ICar
    {
        public static int GetPrice(this EngineType engineType)
        {
            switch (engineType)
            {
                case EngineType.Diesel: return 5000;
                case EngineType.Boxer: return 10000;
                default:
                case EngineType.V8:
                    return 20000;
            }
        }
        public static int GetHorsePower(this EngineType engineType)
        {
            switch (engineType)
            {
                case EngineType.Diesel: return 80;
                case EngineType.Boxer: return 100;
                default:
                case EngineType.V8:
                    return 200;
            }
        }
    }
}
