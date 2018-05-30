namespace AutoPropertyBenchmark
{
    internal sealed class ClassSettableBackField : IClass
    {
        private int _x;
        private int _y;

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Sum() => _x + _y;
    }
}