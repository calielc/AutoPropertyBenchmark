namespace AutoPropertyBenchmark
{
    internal sealed class ClassSettableAutoProperty : IClass
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Sum() => X + Y;
    }
}