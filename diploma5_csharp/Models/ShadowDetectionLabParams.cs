namespace diploma5_csharp.Models
{
    public class ShadowDetectionLabParams : ShadowDetectionParams
    {
        public const double ThresholdDefault = 256;
        private double _threshold;

        public double? Threshold
        {
            get { return this._threshold; }
            set { this._threshold = value ?? ThresholdDefault; }
        }
    }
}