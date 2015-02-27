using Esd.FlexibleOpenGeographies.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class ReserveTypeCode : IUnitOfWorkWithResult<string>
    {
        private const int MySqlDuplicateKeyErrorCode = 2300;
        private readonly IContextFactory _contextFactory;

        public ReserveTypeCode(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public string ExecuteWithResult()
        {
            var value = InitialValue();
            var retryCount = 0;
            while (value <= 9999)
            {
                try
                {
                    var typeCode = string.Format("NN{0}", value);
                    SaveValue(typeCode);
                    return typeCode;
                }
                catch (MySqlException e)
                {
                    if (e.Number != MySqlDuplicateKeyErrorCode) throw;
                    if (++retryCount >= 10) throw new Exception("Error generating code", e);
                    value++;
                }
            }
            throw new Exception("No more types can be created");
        }

        private int InitialValue()
        {
            using (var context = _contextFactory.Create())
            {
                //return context.ReservedCodes.AsNoTracking()
                //              .Where(code => code.AreaCodeSuffix == string.Empty && code.TypeShortCode.StartsWith("NN"))
                //              .ToList()
                //              .Select(code => Convert.ToInt32(code.TypeShortCode.Substring(2)))
                //              .Max(code => code)
                //       + 1;                
                var codes = context.ReservedCodes.AsNoTracking()
                              .Where(code => code.AreaCodeSuffix == string.Empty && code.TypeShortCode.StartsWith("NN"))
                              .ToList();
                return !codes.Any()
                    ? 1
                    : codes.Select(code => Convert.ToInt32(code.TypeShortCode.Substring(2))).Max(code => code) + 1;
            }
        }

        private void SaveValue(string value)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = new ReservedCode {TypeShortCode = value, AreaCodeSuffix = string.Empty};
                context.ReservedCodes.Add(entity);
                context.SaveChanges();
            }
        }
    }
}
