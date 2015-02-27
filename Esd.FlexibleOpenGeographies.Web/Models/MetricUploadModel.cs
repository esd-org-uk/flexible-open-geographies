namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class MetricUploadModel
    {
        public string MessageText { get; set; }

        public MetricUploadModel(){}

        public MetricUploadModel(string messageText) { MessageText = messageText; }
    }
}