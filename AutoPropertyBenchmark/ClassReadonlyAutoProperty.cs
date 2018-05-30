namespace AutoPropertyBenchmark
{
    internal sealed class ClassReadonlyAutoProperty : IClass
    {
        public ClassReadonlyAutoProperty(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public int Sum() => X + Y;
    }
}