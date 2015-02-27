using Esd.FlexibleOpenGeographies.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class ReserveCode : IUnitOfWorkWithResult<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _shortCode;
        private const int MySqlDuplicateKeyErrorCode = 2300;

        public ReserveCode(IContextFactory contextFactory, string shortCode)
        {
            _contextFactory = contextFactory;
            _shortCode = shortCode;
        }

        public string ExecuteWithResult()
        {
            return ReserveAreaCode();
        }

        private string ReserveAreaCode()
        {
            var retryCount = 0;
            var existing = ExistingCodes(_shortCode);
            foreach (var suffix in GenerateAllCodes())
                try
                {
                    if (existing.Contains(suffix)) continue;
                    SaveCode(_shortCode, suffix);
                    return suffix;
                }
                //Catch exception to prevent race conditions where two clients read the same code before using it
                //Should rarely (if ever) happen so this is more efficient than using locks
                catch (MySqlException e)
                {
                    if (e.Number != MySqlDuplicateKeyErrorCode) throw;
                    if (++retryCount >= 10) throw new Exception("Error generating code suffix", e);
                }
            throw new Exception("No available code for this type");
        }

        private void SaveCode(string shortCode, string suffix)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = new ReservedCode
                {
                    TypeShortCode = shortCode,
                    AreaCodeSuffix = suffix
                };
                context.ReservedCodes.Add(entity);
                context.SaveChanges();
            }
        }

        private IList<string> ExistingCodes(string shortCode)
        {
            using (var context = _contextFactory.Create())
                return context.ReservedCodes.AsNoTracking()
                              .Where(code => code.TypeShortCode == shortCode)
                              .Select(code => code.AreaCodeSuffix)
                              .ToList();
        }

        private static IEnumerable<string> GenerateAllCodes()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz".ToArray();
            for (var k = -1; k < 25; k++)
                for (var j = -1; j < 25; j++)
                    for (var i = 0; i < 25; i++)
                        yield return new string(new[] {
                            alphabet[i],
                            j == -1 ? ' ' : alphabet[j], 
                            k == -1 ? ' ' : alphabet[k]}).Trim();
        }
    }
}
