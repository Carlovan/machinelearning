﻿namespace MachineLearning
{
    class DataPoint : UIPoint
    {
        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
