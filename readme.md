# 记一次类型设计的求索历程 #
多年以前，在我的刚接触编程语言时，我遇到了一个超出能力范围的类型设计问题。这个问题困扰我多年，让我寝食难安。原因并不是因为这个问题有多复杂，恰恰相反，让我纠结的是，这个问题看起来很简单，而我却找不到一个优秀的解决方法。

俗话说踏破铁鞋无觅处，得来全不费工夫。苦苦求索而不得的多年以后后，我从一次系统设计过程中得到了启发，我终于想出了一个伟大的解决方案。本文除了吹吹牛逼告诉你们我从几年前开始考虑代码的细节外，我还想和你们分享一下我的激动人心的优秀解决方案的产生过程。

## 问题所在 ##

现在已有一个汽车类，我们需要获取关于发动机的信息。发动机的种类有很多种。我们可以有一个柴油发动机，一个V型发动机，一个直列式发动机，诸如此类。

事实上，作为程序猿的我对汽车发动机一无所知。

问题是：如何在代码中表示我们的发动机类型？

我希望我表示的发动机类型具有很多功能。发动机类型用`EngineType`表示，这个类应该包含以下功能：
- 获取马力值：GetHorsePower()
- 获取价格：GetPrice()
- 获取二手价格：GetPriceForUsed(int yearsOld)
- 根据车身重量计算加速度：CalculateAccelerationWithCarWeight(int carWeight)
- ......

## 1#方案：继承 ##
我们可以为发动机建立基类，然后汽车将继承这些基类。
```
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
```
这确实可行，但感觉不是那么回事。

如果我们想添加GearType和WheelsType呢？我们总不能在C＃中使用多重继承吧？即使我能采用这种做法，这种设计也是有缺陷的。发动机是汽车的一部分，但汽车并不是发动机的一个子类。

那么我们来看看2#方案。
## 2#方案：枚举 ##

我们将构造一个枚举，并列举出我们所有的发动机类型。
ICar将有一个EngineType属性
```
public enum EngineType { Diesel, V8, Straight, Boxer }
public interface ICar
{
    EngineType Engine { get; }
}
```
这样就好多了。但是，Car.Engine.GetPrice()或Car.Engine.GetHorsePower()该要如何实现呢？
首先想到的是我们枚举的扩展方法：
```
public static int GetPrice(this EngineType engineType)
{
    switch(engineType)
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
```
好吧，这样确实可以达到我要的效果，但有些鸡肋，因为我们在代码中将会包含有很多`switch-case`判断语句。每当我们添加一个新的发动机类型，我们将不得不去`switch`下的`case`中添加新的发动机。如果我们忘了呢？此外，我们是面向对象的编程，这种通过`switch`控制并不是很“面向对象”......

让我们尝试解决方法，看看解决方案3#......
## 3#解决方案：类 ##

园友们太熟悉不过了，实现起来应该也是相当简单的吧？
```
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
```
如果满分100分的话，这样的实现可以打80分了。但是，还是有一些可以改进的地方。
- 为什么我需要创建一个V8Engine的新实例？ 它总是一样的...看起来很浪费
- 我如何比较两个引擎？ 我需要类似的东西
if（engine1.GetType（）== engine2.GetType（））

确实是个问题！如果我把我所有的class换成Singletons会怎样呢？
```
public class V8Engine : IEngineType
{
    private static readonly Lazy<V8Engine> _instance =
        new Lazy<V8Engine>(() => new V8Engine());
    public static V8Engine Instance => _instance.Value;

    private V8Engine()
    {
    }
    public int HorsePower { get { return 200; } }
    public int Price { get { return 20000; } }
}
```
我更倾向于这种解决方案。比较两个引擎时，我可以使用简单的`=`运算符，因为这儿只有一个实例。

要是说还有缺陷的话，那就是任何人都可以从`IEngine`继承并创建自己的自定义`EngineType`。

如果你了解Java，你可能会想“在Java中，我只需扩展我的枚举。“。但对不起，C#中并不支持这么做。

事实上，在Java中这是很容易解决的:
```
public enum Level {
    HIGH  (3),  
    MEDIUM(2),  
    LOW   (1) ; 
    private final int levelCode;

    Level(int levelCode) {
        this.levelCode = levelCode;
    }    
    public int getLevelCode() {
        return this.levelCode;
    }
}
```
哈哈很酷是不是？

## 4#方案：类Java枚举 ##

用一个小小的魔法嵌套类，你可以达到这个目的。
```
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
```
或者是这样：
```
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
```

构造函数应该是私有的，这样这个类中的任何人都不能创建一个新的`EngineType`。

如此一来，我的类型设计问题得到了有效的解决。

其实还有一个技巧，就是我们可以使用带有属性的常规枚举，例如：
```
public enum EngineType
{
    [EngineAttr(80, 5000)]
    Boxer,
    [EngineAttr(100, 10000)]
    Straight,
    [EngineAttr(200, 20000)]
    V8
}
```
通过扩展方法，我们也可以访问这个枚举类的属性值。

**程序代码**：<a href="https://github.com/daivven/TypeDesign">https://github.com/daivven/TypeDesign</a>



<div style="background: #f0f8ff; padding: 10px; border: 2px dashed #990d0d; font-family: 微软雅黑;">
&nbsp;作者：<a href="http://www.cnblogs.com/yayazi/">阿子</a>
<br>
&nbsp;博客地址：<a href="http://www.cnblogs.com/yayazi/">http://www.cnblogs.com/yayazi/</a>
<br>
&nbsp;本文地址：<a href="http://www.cnblogs.com/yayazi/p/8383485.html">http://www.cnblogs.com/yayazi/p/8383485.html</a>
<br>
&nbsp;声明：本博客原创文字允许转载，转载时必须保留此段声明，且在文章页面明显位置给出原文链接。

</div>