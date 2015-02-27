using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    public class MetricDownload : IQueryEnumerable<MetricBasic>
    {
        private readonly IContextFactory _contextFactory;
        private string _metricTypeCode;
        private int _areaId;
        private string _areaTypeCode;
        private string _periodCode;
        private bool _includeMissingValues;

        public MetricDownload(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForMetricTypeCode(string code)
        {
            _metricTypeCode = code;
        }

        public void ForAreaId(int areaId)
        {
            _areaId = areaId;
        }

        public void ForAreaTypeCode(string code)
        {
            _areaTypeCode = code;
        }

        public void ForPeriodCode(string code)
        {
            _periodCode = code;
        }

        public void ForIncludeMissingValues(bool includeMissingValues)
        {
            _includeMissingValues = includeMissingValues;
        }

        public IEnumerable<MetricBasic> Fetch()
        {
            var areaIds = new List<int>();

            if (!string.IsNullOrEmpty(_areaTypeCode))
            {

                var query = new HierarchicalAreaByIdAndType(_contextFactory);
                query.ForAreaId(_areaId);
                query.ForTypeCode(_areaTypeCode);
                var areas = query.Fetch();

                if (areas != null)
                {
                    areaIds.AddRange(areas.Select(area => area.Id));
                }
            }
            else
            {
                areaIds.Add(_areaId);
            }

            var metricDownloadWithArea = new MetricDownloadWithArea(_contextFactory);
            metricDownloadWithArea.ForAreas(areaIds);
            metricDownloadWithArea.ForMetricTypeCode(_metricTypeCode);
            metricDownloadWithArea.ForPeriodCode(_periodCode);
            metricDownloadWithArea.ForIncludeMissingValues(_includeMissingValues);

            return metricDownloadWithArea.Fetch();
        }
    }
}
