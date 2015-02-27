using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    public class MetricDownloadWithArea : IQueryEnumerable<MetricBasic>
    {
        private readonly IContextFactory _contextFactory;
        private string _metricTypeCode;
        private List<int> _ids;
        private string _periodCode;
        private bool _includeMissingValues;

        public MetricDownloadWithArea(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForMetricTypeCode(string code)
        {
            _metricTypeCode = code;
        }

        public void ForAreas(List<int> ids)
        {
            _ids = ids;
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
            var _areaCodes = new List<string>();
            var _areaTypes = new List<string>();

            using (var context = _contextFactory.Create())
            {
                var areas = context.AreaDetails.AsNoTracking().Where(area => _ids.Contains(area.Id)).Select(area => new AreaNoGeographyOrOwnership() { Code = area.Code, TypeCode = area.TypeCode });

                foreach (AreaNoGeographyOrOwnership area in areas)
                {
                    _areaCodes.Add(area.Code);
                    _areaTypes.Add(area.TypeCode);
                }
            }

            _areaTypes.Distinct();

            using (var context = _contextFactory.Create())
            {

                List<MetricBasic> metricBasics = new List<MetricBasic>();

                metricBasics.AddRange(context.Metrics.AsNoTracking().Where(m => m.MetricTypeIdentifier == _metricTypeCode && m.PeriodIdentifier == _periodCode && _areaCodes.Contains(m.AreaIdentifier) && _areaTypes.Contains(m.AreaTypeIdentifier)).Select(MetricMapper.Map).ToList());

                if (_includeMissingValues)
                {
                    foreach(MetricBasic basic in metricBasics)
                    {
                        _areaCodes.Remove(basic.AreaIdentifier);
                    }

                    int metricTypeId = Convert.ToInt32(_metricTypeCode);

                    metricBasics.AddRange((from m in context.MetricTypes
                                  where m.Identifier == metricTypeId
                                  from p in context.Periods
                                  where p.Identifier == _periodCode
                                  from a in context.AreaDetails
                                  where _areaCodes.Contains(a.Code) && _areaTypes.Contains(a.TypeCode)
                                  select new MetricBasic()
                                  {
                                      MetricTypeIdentifier = _metricTypeCode,
                                      PeriodIdentifier = p.Identifier,
                                      AreaIdentifier = a.Code,
                                      AreaTypeIdentifier = a.TypeCode
                                  }).ToList());
                }

                metricBasics.Sort();

                return metricBasics;
            }
        }
    }
}
