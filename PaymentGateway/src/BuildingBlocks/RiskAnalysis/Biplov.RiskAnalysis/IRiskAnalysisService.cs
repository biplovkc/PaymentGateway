using System;
using System.Net;
using System.Threading.Tasks;
using Biplov.Common.Core;

namespace Biplov.RiskAnalysis
{
    /// <summary>
    /// Risk analysis service
    /// </summary>
    public interface IRiskAnalysisService
    {
        /// <summary>
        /// Basic risk analysis, can be modified or extended as per requirement
        /// </summary>
        /// <param name="ipAddress">Request origin ip</param>
        /// <returns>Risk assessment result</returns>
        Task<Result<RiskAssessmentResult>> GetRiskAnalysis(IPAddress ipAddress);
    }
}
