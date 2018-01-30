using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeDesign
{
    public enum EngineType { Diesel, V8, Straight, Boxer }
    public interface ICar
    {
        EngineType Engine { get; }
    }
}
