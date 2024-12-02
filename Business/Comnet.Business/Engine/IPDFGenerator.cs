using Comnet.Data.Contracts.ViewModels.Assessment;

namespace Comnet.Business.Engine
{
    public interface IPDFGenerator
    {
        byte[] GeneratePdf(AssessmentReportDetails details);
    }
}