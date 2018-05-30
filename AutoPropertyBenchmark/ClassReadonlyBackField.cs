namespace AutoPropertyBenchmark
{
    internal sealed class ClassReadonlyBackField : IClass
    {
        private readonly int _x;
        private readonly int _y;

        public ClassReadonlyBackField(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X => _x;
        public int Y => _y;

        public int Sum() => _x + _y;
    }
}