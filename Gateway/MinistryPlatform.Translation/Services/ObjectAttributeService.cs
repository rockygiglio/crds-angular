using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ObjectAttributeService : BaseService, IObjectAttributeService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ObjectAttributeService(IAuthenticationService authenticationService,
            IConfigurationWrapper configurationWrapper,
            IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpObjectAttribute> GetCurrentObjectAttributes(string token, int objectId, MpObjectAttributeConfiguration configuration, int? attributeTypeIdFilter = null)
        {
            var subPageViewId = configuration.SelectedSubPage;
            var searchString = attributeTypeIdFilter.HasValue ? string.Format(",,,,\"{0}\"", attributeTypeIdFilter.Value) : "";
            var records = _ministryPlatformService.GetSubpageViewRecords(subPageViewId, objectId, token, searchString);

            var keyColumn = string.Format("{0}_Attribute_ID", configuration.TableName);

            var objectAttributes = records.Select(record => new MpObjectAttribute
            {
                ObjectAttributeId = record.ToInt(keyColumn),

                AttributeId = record.ToInt("Attribute_ID"),
                StartDate = record.ToDate("Start_Date"),
                EndDate = record.ToNullableDate("End_Date"),
                Notes = record.ToString("Notes"),
                AttributeTypeId = record.ToInt("Attribute_Type_ID"),
                AttributeTypeName = record.ToString("Attribute_Type")
            }).ToList();

            return objectAttributes;
        }

        public int CreateAttribute(string token, int objectId, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration)
        {
            var attributeDictionary = TranslateAttributeToDictionary(attribute, configuration);
            var subPageId = configuration.SubPage;

            try
            {
                return _ministryPlatformService.CreateSubRecord(subPageId, objectId, attributeDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating object attribute, objectId: {0} attributeId: {1}",
                                        objectId,
                                        attribute.AttributeId);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public IObservable<int> CreateAttributeAsync(string token, int objectId, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration)
        {

            return Observable.Create<int>(o =>
            {
                var attributeDictionary = TranslateAttributeToDictionary(attribute, configuration);
                var subPageId = configuration.SubPage;
                var task = _ministryPlatformService.CreateSubRecordAsync(subPageId, objectId, attributeDictionary, token).ToObservable<int>();
                task.Subscribe();
                task.Catch<int, Exception>(tx =>
                {
                    var msg = string.Format("Error creating object attribute, objectId: {0} attributeId: {1}",
                                        objectId,
                                        attribute.AttributeId);
                    _logger.Error(msg, tx);
                    o.OnError(new ApplicationException());
                    return Observable.Return(-1);
                });
                o.OnNext(task.Wait());
                return Disposable.Empty;
            });
        }

        public void UpdateAttribute(string token, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration)
        {
            var attributeDictionary = TranslateAttributeToDictionary(attribute, configuration);
            var subPageId = configuration.SubPage;

            try
            {
                _ministryPlatformService.UpdateSubRecord(subPageId, attributeDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error updating object attribute, objectAttributeId: {0} attributeId: {1}",
                                        attribute.ObjectAttributeId, attribute.AttributeId);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public void UpdateAttributeAsync(string token, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration)
        {
            var attributeDictionary = TranslateAttributeToDictionary(attribute, configuration);
            var subPageId = configuration.SubPage;

            try
            {
                _ministryPlatformService.UpdateSubRecordAsync(subPageId, attributeDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error updating object attribute, objectAttributeId: {0} attributeId: {1}",
                                        attribute.ObjectAttributeId, attribute.AttributeId);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }

        }

        private Dictionary<string, object> TranslateAttributeToDictionary(MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration)
        {
            var keyColumn = string.Format("{0}_Attribute_ID", configuration.TableName);

            var attributeDictionary = new Dictionary<string, object>
            {
                {"Attribute_Type_ID", attribute.AttributeTypeId},
                {"Attribute_ID", attribute.AttributeId},
                {keyColumn, attribute.ObjectAttributeId},
                {"Start_Date", attribute.StartDate},
                {"End_Date", attribute.EndDate},
                {"Notes", attribute.Notes}
            };
            return attributeDictionary;
        }
    }
}