namespace ShipAnalysis
{
    public class ShipShape
    {
        /// <summary>
        /// 根据船形参数设置，返回Wigley船的由x和z确定y的船形函数
        /// </summary>
        /// <param name="configuration">船形设置</param>
        /// <returns>船形函数，由x和z确定y，超出范围则y为0</returns>
        public static Func<double, double, double> WigleyWithParallelMiddleBody(Configuration configuration)
        {
            double PMBL = configuration.ParallelMiddleBodyLength;
            double Lpp = configuration.LengthOverall - PMBL;
            double D = configuration.Drought;
            double B = configuration.Breadth;

            return (x, z) =>
            {
                if (x < -Lpp / 2 || x > Lpp / 2)
                {
                    return 0;
                }
                else if (z > 0 || z < -D)
                {
                    return 0;
                }
                else
                {
                    if (x > PMBL / 2)
                    {
                        return 2 * B
                        * ((1 / 4 - ((x - PMBL / 2) / ((Lpp - PMBL) / 2)) * ((x - PMBL / 2) / ((Lpp - PMBL) / 2)))
                        * (1 - (z / D) * (z / D)));
                    }
                    else if (-PMBL / 2 <= x && x <= PMBL / 2)
                    {
                        return B / 2 * (1 - (z / D) * (z / D));
                    }
                    else
                    {
                        return 2 * B
                        * ((1 / 4 - ((x + PMBL / 2) / ((Lpp - PMBL) / 2)) * ((x + PMBL / 2) / ((Lpp - PMBL) / 2)))
                        * (1 - (z / D) * (z / D)));
                    }
                }
            };
        }
        /// <summary>
        /// 根据船形参数设置，返回Wigley船的由x和z确定y的船形函数对x的偏导
        /// </summary>
        /// <param name="configuration">船形设置</param>
        /// <returns>船形函数，由x和z确定y对x的偏导，超出范围则偏导为0</returns>
        public static Func<double, double, double> WigleyWithParallelMiddleBodyFx(Configuration configuration)
        {
            double PMBL = configuration.ParallelMiddleBodyLength;
            double Lpp = configuration.LengthOverall - PMBL;
            double D = configuration.Drought;
            double B = configuration.Breadth;
            return (x, z) =>
            {
                if (x < -Lpp / 2 || x > Lpp / 2)
                {
                    return 0;
                }
                else if (z > 0 || z < -D)
                {
                    return 0;
                }
                else
                {
                    if (x > PMBL / 2)
                    {
                        return 2 * B
                        * (-(x - PMBL / 2) / ((Lpp - PMBL) / 2) / ((Lpp - PMBL) / 2))
                        * (1 - (z / D) * (z / D));
                    }
                    else if (-PMBL / 2 <= x && x <= PMBL / 2)
                    {
                        return 0;
                    }
                    else
                    {
                        return 2 * B
                        * (-(x + PMBL / 2) / ((Lpp + PMBL) / 2) / ((Lpp + PMBL) / 2))
                        * (1 - (z / D) * (z / D));
                    }
                }
            };

        }
    }
}