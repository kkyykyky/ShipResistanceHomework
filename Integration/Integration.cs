namespace Integration
{
    internal static class ListExtension
    {
        /// <summary>
        /// 数组的前n项和
        /// </summary>
        /// <param name="list">数组</param>
        /// <param name="end">要求和的个数，若end大于数组长度，则不计算超出部分的和</param>
        /// <returns></returns>
        internal static double Sum(this List<double> list, int end)
        {
            double result = 0;
            for (int i = 0; i < end && i < list.Count; i++)
            {
                result += list[i];
            }
            return result;
        }
    }
    public static class Integration
    {
        /// <summary>
        /// 基本复化数值积分，给定起点、终点、步长，指定权重
        /// </summary>
        /// <param name="function">被积实函数</param>
        /// <param name="intervalBegin">积分起点</param>
        /// <param name="intervalEnd">积分终点</param>
        /// <param name="interval">步长</param>
        /// <param name="weights">权重，改变权重的数量和大小可以实现不同的积分方法</param>
        /// <returns></returns>
        public static double BasicIntegration(Func<double, double> function, double intervalBegin, double intervalEnd, double interval, List<double> weights,double max=1E80)
        {
            if (intervalBegin == intervalEnd)
            {
                return 0;
            }
            else if (intervalBegin > intervalEnd)
            {
                return -BasicIntegration(function, intervalEnd, intervalBegin, interval, weights);
            }
            else
            {
                weights = weights.ConvertAll((x) => { return x / weights.Sum(); });
                double x, y;
                for (x = intervalBegin, y = 0; x < intervalEnd; x += interval)
                {
                    double temp = 0;
                    for (int i = 0; i < weights.Count; i++)
                    {
                        temp += weights[i] * function(x + weights.Sum(i + 1) * interval);


                    }
                    if (double.IsNaN(temp) ||double.IsInfinity(temp) ||Math.Abs(temp)>max)
                    {
                        continue;
                    }
                    else
                    {
                        y += interval * temp;
                    }
                }
                return y;
            }
        }
        /// <summary>
        /// 复化矩形法
        /// </summary>
        /// <param name="function"></param>
        /// <param name="intervalBegin"></param>
        /// <param name="intervalEnd"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static double Square(Func<double, double> function, double intervalBegin, double intervalEnd, double interval = 1E-2)
        {
            return BasicIntegration(function, intervalBegin, intervalEnd, interval, new List<double>() { 1 });
        }
        /// <summary>
        /// 复化梯形法
        /// </summary>
        /// <param name="function"></param>
        /// <param name="intervalBegin"></param>
        /// <param name="intervalEnd"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static double Trapezoidal(Func<double, double> function, double intervalBegin, double intervalEnd, double interval = 1E-2)
        {
            return BasicIntegration(function, intervalBegin, intervalEnd, interval, new List<double>() { 1, 1 });
        }
        /// <summary>
        /// 复化辛普森法
        /// </summary>
        /// <param name="function"></param>
        /// <param name="intervalBegin"></param>
        /// <param name="intervalEnd"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static double Simpson(Func<double, double> function, double intervalBegin, double intervalEnd, double interval = 1E-2)
        {
            return BasicIntegration(function, intervalBegin, intervalEnd, interval, new List<double>() { 1, 4, 1 });
        }
        /// <summary>
        /// 复化科特斯
        /// </summary>
        /// <param name="function"></param>
        /// <param name="intervalBegin"></param>
        /// <param name="intervalEnd"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static double Cotes(Func<double, double> function, double intervalBegin, double intervalEnd, double interval = 1E-2)
        {
            return BasicIntegration(function, intervalBegin, intervalEnd, interval, new List<double>() { 7, 32, 12, 32, 7 });
        }
    }
}