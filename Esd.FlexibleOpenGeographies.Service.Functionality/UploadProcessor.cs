using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Esd.FlexibleOpenGeographies.Service.Functionality
{
    internal class UploadProcessor
    {
        internal static void Process(UploadBasic upload, IUnitOfWorkFactory workFactory, IQueryFactory queryFactory)
        {
            if (upload == null)
            {
                return;
            }

            var user = queryFactory.CreateUserByUniqueIdQuery(upload.UserId).Find();

            var areasFound = new Dictionary<string, AreaInfo>();
            var areaTypeSecurity = new Dictionary<string, AreaTypeDetails>();
            var metricTypesFound = new Dictionary<string, bool>();
            var periodsFound = new Dictionary<string, bool>();

            var errors = new List<string>();
            var row = 0;
            var totalImported = 0;
            var totalRejected = 0;
            var totalDeleted = 0;

            using (var reader = new StringReader(upload.CSV))
            {
                row++;
                string line;
                var firstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }

                    var columns = line.Split(',');

                    if (columns.Length < 3)
                    {
                        continue;
                    }

                    var metric = new MetricBasic();
                    if (columns.Length > 0)
                    {
                        metric.MetricTypeIdentifier = CleanColumn(columns[0]);
                    }

                    if (string.IsNullOrEmpty(metric.MetricTypeIdentifier))
                    {
                        totalRejected++;
                        errors.Add(string.Format("Row {0}: A metric type identifier must be specified.", row));
                        continue;
                    }

                    if (!MetricTypeExists(metric.MetricTypeIdentifier, metricTypesFound, queryFactory))
                    {
                        totalRejected++;
                        errors.Add(string.Format("Row {0}: '{1}' metric type identifier was not identified.", row, metric.MetricTypeIdentifier));
                        continue;
                    }

                    if (columns.Length > 2)
                    {
                        metric.PeriodIdentifier = CleanColumn(columns[2]);
                    }

                    if (string.IsNullOrEmpty(metric.PeriodIdentifier))
                    {
                        totalRejected++;
                        errors.Add(string.Format("Row {0}: A period identifier must be specified.", row));
                        continue;
                    }

                    if (!PeriodExists(metric.PeriodIdentifier, periodsFound, queryFactory))
                    {
                        totalRejected++;
                        errors.Add(string.Format("Row {0}: '{1}' period identifier was not identified.", row, metric.PeriodIdentifier));
                        continue;
                    }

                    if (columns.Length > 3)
                    {
                        metric.AreaIdentifier = CleanColumn(columns[4]);
                    }

                    if (columns.Length > 5)
                    {
                        metric.AreaTypeIdentifier = CleanColumn(columns[6]);
                    }

                    if (string.IsNullOrEmpty(metric.AreaTypeIdentifier))
                    {
                        totalRejected++;
                        errors.Add(string.Format("Row {0}: A area type identifier must be specified.", row));
                        continue;
                    }

                    var areaInfo = GetAreaType(metric.AreaIdentifier, metric.AreaTypeIdentifier, areasFound, queryFactory);
                    if (areaInfo == null)
                    {
                        totalRejected++;
                        errors.Add(string.Format("Row {0}: a matching record for area '{1}' and type '{2} was not found.", row, metric.AreaIdentifier, metric.AreaTypeIdentifier));
                        continue;
                    }

                    var areaTypeDetail = GetSecurityLevels(areaInfo.AreaType, areaTypeSecurity, queryFactory);
                    if (areaTypeDetail == null)
                    {
                        totalRejected++;
                        errors.Add(string.Format("Row {0}: '{1}' area type for area '{2}' was not identified.", row, areaInfo.AreaType, metric.AreaIdentifier));
                        continue;
                    }
                    
                    if (areaTypeDetail.MetricUploadPermissionLevelId == (int)SecurityLevel.Creator)
                    {
                        if (user.UserId != areaInfo.Creator)
                        {
                            totalRejected++;
                            errors.Add(string.Format("Row {0}: '{1}' area type only allows the creator to upload.", row, areaInfo.AreaType));
                            continue;
                        }
                    }

                    if (areaTypeDetail.MetricUploadPermissionLevelId == (int)SecurityLevel.CreatorOrganisation)
                    {
                        if (user.OrganisationId != areaInfo.Organisation)
                        {
                            totalRejected++;
                            errors.Add(string.Format("Row {0}: '{1}' area type only allows the creator organisation to upload.", row, areaInfo.AreaType));
                            continue;
                        }
                    }

                    if (columns.Length > 7)
                    {
                        metric.Value = CleanColumn(columns[8]);
                    }

                    if (columns.Length > 9)
                    {
                        if (Convert.ToBoolean(CleanColumn(columns[9])))
                        {
                            totalDeleted++;
                            workFactory.CreateRemoveMetricProcess(metric).Execute();
                            continue;
                        }
                    }

                    if (columns.Length > 10)
                    {
                        var val = CleanColumn(columns[10]);
                        if (!string.IsNullOrEmpty(val) && val.ToLower() != "input")
                        {
                            totalRejected++;
                            errors.Add(string.Format("Row {0}: Aggregated source data can not be uploaded.", row));
                            continue;
                        }
                    }

                    totalImported++;
                    workFactory.CreateAddMetricProcess(metric).Execute();
                }
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Your import is complete. {0} rows has been imported, {1} rows deleted and {2} have been rejected.", totalImported, totalDeleted, totalRejected);
            sb.AppendLine();
            sb.AppendLine();

            foreach(var error in errors)
            {
                sb.Append(error);   
                sb.AppendLine();
            }

            SendEmail(user.Email, "Import completed", sb.ToString());

            workFactory.CreateRemoveUploadProcess(upload).Execute();
        }

        private static AreaTypeDetails GetSecurityLevels(string identifier, Dictionary<string, AreaTypeDetails> areaTypeSecurity, IQueryFactory queryFactory)
        {
            if (!areaTypeSecurity.ContainsKey(identifier))
            {
                var areaType = queryFactory.CreateAreaTypeDetailsByCodeQuery(identifier).Find();
                areaTypeSecurity.Add(identifier, areaType);
            }
            return areaTypeSecurity[identifier];
        }

        private static bool MetricTypeExists(string identifier, Dictionary<string, bool> metricTypesFound, IQueryFactory queryFactory)
        {
            if(!metricTypesFound.ContainsKey(identifier))
            {
                metricTypesFound.Add(identifier, queryFactory.CreateMetricTypeForCodeQuery(identifier).Find() != null); 
            }

            return metricTypesFound[identifier];
        }

        private static AreaInfo GetAreaType(string identifier, string type, Dictionary<string, AreaInfo> areasFound, IQueryFactory queryFactory)
        {
            if (!areasFound.ContainsKey(identifier))
            {
                var areas = queryFactory.CreateAreaDetailsByTypeAndCodeQuery(type, identifier).Find();
                if (areas == null)
                {
                    areasFound.Add(identifier, null);
                }
                else
                {
                    areasFound.Add(identifier, new AreaInfo
                    {
                        AreaType = areas.TypeCode, Creator = areas.Creator, Organisation = areas.Organisation
                    });
                }
            }

            return areasFound[identifier];
        }

        private static bool PeriodExists(string identifier, Dictionary<string, bool> periodsFound, IQueryFactory queryFactory)
        {
            if (!periodsFound.ContainsKey(identifier))
            {
                periodsFound.Add(identifier, queryFactory.CreatePeriodByCodeQuery(identifier).Find() != null);
            }

            return periodsFound[identifier];
        }

        private static string CleanColumn(string colVal)
        {
            return string.IsNullOrEmpty(colVal) ? colVal : colVal.Trim('"');
        }

        private static void SendEmail(string email, string subject, string message)
        {
            var mail = new MailMessage {From = new MailAddress(ConfigurationManager.AppSettings["SMTP.From"])};
            mail.To.Add(email);

            var client = new SmtpClient(ConfigurationManager.AppSettings["SMTP.Host"], 
                Convert.ToInt32(ConfigurationManager.AppSettings["SMTP.Port"]))
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP.SSL"])
            };
            var creds = new NetworkCredential(ConfigurationManager.AppSettings["SMTP.Username"],
                                        ConfigurationManager.AppSettings["SMTP.Password"]);
            client.UseDefaultCredentials = false;

            client.Credentials = creds;
            client.Timeout = 150000;

            mail.Subject = subject;
            mail.Body = message;

            client.Send(mail);
        }
    }
}
