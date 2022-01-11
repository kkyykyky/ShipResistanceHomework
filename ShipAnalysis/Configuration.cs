using System.ComponentModel;

namespace ShipAnalysis
{
    public class Configuration : INotifyPropertyChanged
    {
        #region 字段
        private double maxFroudeNumber = 1;
        private double minFroudeNumber = 0.01;
        private double froudeNumberStepLength = 0.05;

        private double lengthOverall = 3;
        private double parallelMiddleBodyLength = 0;
        private double breadth = 0.3;
        private double wettedSurfaceArea = 1.13;

        private double waterDensity = 1025;
        private double gravitationalAcceleration = 9.8;
        private double drought = 0.1875;

        private double integrationInterval = 1E-2;
        private string currentProcess = string.Empty;
        #endregion

        #region 属性
        /// <summary>
        /// 傅汝德数最大值
        /// </summary>
        public double MaxFroudeNumber
        {
            get
            {
                return maxFroudeNumber;
            }
            set
            {
                maxFroudeNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxFroudeNumber)));
            }
        }
        /// <summary>
        /// 傅汝德数最小值
        /// </summary>
        public double MinFroudeNumber
        {
            get
            {
                return minFroudeNumber;
            }
            set
            {
                minFroudeNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinFroudeNumber)));
            }
        }
        /// <summary>
        /// 傅汝德数间隔
        /// </summary>
        public double FroudeNumberInterval
        {
            get
            {
                return froudeNumberStepLength;
            }
            set
            {
                froudeNumberStepLength = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FroudeNumberInterval)));
            }
        }
        /// <summary>
        /// 船舶总长
        /// </summary>
        public double LengthOverall
        {
            get
            {
                return lengthOverall;
            }
            set
            {
                lengthOverall = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LengthOverall)));
                UpdateWettedSurfaceArea();
            }
        }
        /// <summary>
        /// 船舶型宽
        /// </summary>
        public double Breadth
        {
            get
            {
                return breadth;
            }
            set
            {
                breadth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Breadth)));
                UpdateWettedSurfaceArea();
            }
        }
        /// <summary>
        /// 船舶吃水
        /// </summary>
        public double Drought
        {
            get
            {
                return drought;
            }
            set
            {
                drought = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Drought)));
                UpdateWettedSurfaceArea();
            }
        }
        /// <summary>
        /// 船舶平行中体长度
        /// </summary>
        public double ParallelMiddleBodyLength
        {
            get
            {
                return parallelMiddleBodyLength;
            }
            set
            {
                parallelMiddleBodyLength = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ParallelMiddleBodyLength)));
                UpdateWettedSurfaceArea();
            }
        }
        /// <summary>
        /// 船舶湿表面积，随基本尺寸的更新而异步更新
        /// </summary>
        public double WettedSurfaceArea
        {
            get
            {
                return wettedSurfaceArea;
            }
            set
            {
                wettedSurfaceArea = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WettedSurfaceArea)));
            }
        }
        /// <summary>
        /// 水密度
        /// </summary>
        public double WaterDensity
        {
            get
            {
                return waterDensity;
            }
            set
            {
                waterDensity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WaterDensity)));
            }
        }
        /// <summary>
        /// 重力加速度
        /// </summary>
        public double GravitationalAcceleration
        {
            get
            {
                return gravitationalAcceleration;
            }
            set
            {
                gravitationalAcceleration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GravitationalAcceleration)));
            }
        }
        public double IntegrationInterval
        {
            get { return integrationInterval; }
            set
            {
                integrationInterval = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(IntegrationInterval)));
                UpdateWettedSurfaceArea();
            }
        }
        /// <summary>
        /// 当前计算的傅汝德数，用于显示进度
        /// </summary>
        public string CurrentProcess
        {
            get
            {
                return currentProcess;
            }
            set
            {
                currentProcess = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentProcess)));
            }
        }
        #endregion


        #region 事件
        /// <summary>
        /// 通知控件及时更新数据
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region 构造器
        /// <summary>
        /// 初始化实例，并异步计算湿表面积
        /// </summary>
        public Configuration()
        {
            UpdateWettedSurfaceArea();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 异步计算湿表面积
        /// </summary>
        private void UpdateWettedSurfaceArea()
        {
            Thread thread = new(() =>
            {
                this.wettedSurfaceArea = ResistanceAnalysis.CalculateWettedSurfaceArea(this);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WettedSurfaceArea)));
            });
            thread.Start();
        }
        /// <summary>
        /// 验证傅汝德数数据是否符合规范
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (MaxFroudeNumber < MinFroudeNumber)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
