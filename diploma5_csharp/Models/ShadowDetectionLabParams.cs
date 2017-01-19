namespace diploma5_csharp.Models
{
    public class ShadowDetectionLabParams
    {
        public const double ThresholdDefault = 256;
        private double _threshold;

        public double? Threshold
        {
            get { return this._threshold; }
            set { this._threshold = value ?? ThresholdDefault; }
        }

        public bool ShowWindows;

        //        public ShadowDetectionLabParams()
        //        {
        //            Threshold = ThresholdDefault;
        //        }
        //
        //        public bool SetThreshold(double? value)
        //        {
        //            if (value == null)
        //            {
        //                Threshold = ThresholdDefault;
        //                return false;
        //            }
        //            Threshold = (double) value;
        //            return true;
        //        }
    }
}