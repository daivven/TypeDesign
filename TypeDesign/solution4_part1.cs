using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeDesign
{
    public abstract class EngineType
    {
        public static EngineType V8 = new V8EngineType();
        public static EngineType Diesel = new DieselEngineType();

        private EngineType()
        {
        }
        public abstract int Price { get; }
        public abstract int HorsePower { get; }

        public class V8EngineType : EngineType
        {
            public override int HorsePower { get { return 200; } }
            public override int Price { get { return 20000; } }
        }
        public class DieselEngineType : EngineType
        {
            public override int HorsePower { get { return 80; } }
            public override int Price { get { return 5000; } }
        }
    }
}
