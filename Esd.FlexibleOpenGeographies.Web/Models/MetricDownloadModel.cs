using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class MetricDownloadModel
    {
        [Required, DisplayName("Metric type")]
        public string MetricType { get; set; }
        [Required]
        public string Period { get; set; }
        [Required, DisplayName("Parent area type")]
        public string ParentAreaType { get; set; }
        [Required]
        public string Area { get; set; }
        [Required, DisplayName("By")]
        public string ByArea { get; set; }
        [DisplayName("Include missing values")]
        public bool IncludeMissing { get; set; }        

        public IEnumerable<MetricTypeSelectItem> MetricTypes { get; set; }
        public IEnumerable<PeriodBasic> Periods { get; set; }
        public IEnumerable<AreaTypeBasic> ParentAreaTypes { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        public IEnumerable<AreaBasic> Areas { get; set; }

        public MetricDownloadModel()
        {
            Periods = new List<PeriodBasic>();
            MetricTypes = new List<MetricTypeSelectItem>();
            ParentAreaTypes = new List<AreaTypeBasic>();
            AreaTypes = new List<AreaTypeBasic>();
            Areas = new List<AreaBasic>();
        }
    }
}