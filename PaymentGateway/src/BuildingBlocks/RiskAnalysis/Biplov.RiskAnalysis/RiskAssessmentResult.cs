namespace Biplov.RiskAnalysis
{
    public class RiskAssessmentResult
    {
        public bool IsRisky { get; }

        public string Reason { get; }

        public RiskAssessmentResult(bool isRisky, string reason)
        {
            IsRisky = isRisky;
            Reason = reason;
        }
    }
}
