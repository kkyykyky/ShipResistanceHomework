using static Integration.Integration;
using static System.Math;
namespace ShipAnalysis
{
    public class ResistanceAnalysis
    {
        /// <summary>
        /// 根据船形参数设置，计算兴波阻力数据
        /// </summary>
        /// <param name="configuration">船形设置</param>
        /// <returns>兴波阻力计算结果，第0项为傅汝德数数组，第1项为兴波阻力数组，第2项为兴波阻力系数数组</returns>
        public static List<List<double>> AnalyzeShipResistance(Configuration configuration)
        {
            List<List<double>> result = new();
            List<double> froudeNumbers = new();
            List<double> resistances = new();
            List<double> resistanceCoefficients = new();
            result.Add(froudeNumbers);
            result.Add(resistances);
            result.Add(resistanceCoefficients);

            Func<double, double, double> f = ShipShape.WigleyWithParallelMiddleBody(configuration);
            Func<double, double, double> fx = ShipShape.WigleyWithParallelMiddleBodyFx(configuration);
            double L = configuration.LengthOverall;
            double g = configuration.GravitationalAcceleration;
            double T = configuration.Drought;
            double interval = configuration.IntegrationInterval;

            for (double froudeNumber = configuration.MinFroudeNumber; froudeNumber <= configuration.MaxFroudeNumber; froudeNumber += configuration.FroudeNumberInterval)
            {
                froudeNumbers.Add(froudeNumber);
                configuration.CurrentProcess = string.Format("{0:F3}", froudeNumber);
                double u = froudeNumber * Pow(g * L, 0.5);
                double K0 = g / Pow(u, 2);
                double resistance = 4 * configuration.WaterDensity * K0 / PI
                    * Trapezoidal(
                        (theta) =>
                        {
                            double J;//I, J;
                            //I = Trapezoidal((z) =>
                            //{
                            //    return Trapezoidal((x) =>
                            //    {
                            //        Func<double,double> fx = (x) => f(x, z);
                            //        return Diff(fx, x) * Exp(K0 * z / Pow(Cos(theta), 3)) * Cos(K0 * x / Cos(theta));
                            //    }
                            //    , -L / 2, L / 2);
                            //}
                            //, -T, 0); 奇函数积分为0
                            J = Trapezoidal((z) =>
                            {
                                return Trapezoidal((x) =>
                                {
                                    //Func<double, double> fx = (x) => f(x, z);
                                    return fx(x, z) * Exp(K0 * z / Pow(Cos(theta), 3)) * Sin(K0 * x / Cos(theta));
                                }
                                , -L / 2, L / 2,interval);
                            }
                            , -T, 0,interval);

                            //return (I * I + J * J) / Pow(Cos(theta), 3);
                            return J * J / Pow(Cos(theta), 3);
                        }
                        , 0, 3.14 / 2,interval);
                resistances.Add(resistance);
                resistanceCoefficients.Add(resistance * 2 / configuration.WaterDensity / Pow(u, 2) / configuration.WettedSurfaceArea);
            }
            configuration.CurrentProcess = "计算完成";
            return result;
        }
        /// <summary>
        /// 根据船形参数设置，计算船舶湿表面积
        /// </summary>
        /// <param name="configuration">船形设置</param>
        /// <returns>湿表面积</returns>
        public static double CalculateWettedSurfaceArea(Configuration configuration)
        {
            double L = configuration.LengthOverall;
            double D = configuration.Drought;
            var fx = ShipShape.WigleyWithParallelMiddleBodyFx(configuration);
            return 2 * Trapezoidal((z) => { return Trapezoidal((x) => { return Sqrt(1 + fx(x, z) * fx(x, z)); }, -L / 2, L / 2); }, -D, 0);
        }
    }
}
