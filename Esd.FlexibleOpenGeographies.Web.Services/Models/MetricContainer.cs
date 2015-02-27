using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class MetricContainer
    {
        public ColumnGroup[] Columns { get; set; }
        public MetricRow[] Rows { get; set; }

        public MetricContainer(IEnumerable<MetricRow> rows, IEnumerable<ColumnGroup> columns)
        {
            if (rows != null)
            {
                Rows = rows.ToArray();
            }
            if (columns != null)
            {
                Columns = columns.ToArray();
            }
        }
    }
}