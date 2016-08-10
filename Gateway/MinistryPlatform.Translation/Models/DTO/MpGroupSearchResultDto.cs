using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Models.DTO
{
    public class MpGroupSearchResultDto : MpGroup
    {
        public Dictionary<int, MpObjectAttributeType> AttributeTypes { get; set; }
        public Dictionary<int, MpObjectAttribute> SingleAttributes { get; set; }

        [JsonExtensionData]
        private IDictionary<string, JToken> _unmappedData;

        public MpGroupSearchResultDto()
        {
            AttributeTypes = new Dictionary<int, MpObjectAttributeType>();
            SingleAttributes = new Dictionary<int, MpObjectAttribute>();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (!_unmappedData.Any())
            {
                return;
            }

            if (Address == null)
            {
                MapAddressFields();
            }

            if (AttributeTypes == null || !AttributeTypes.Any())
            {
                MapMultiSelectAttributes();
            }

            if (SingleAttributes == null || !SingleAttributes.Any())
            {
                MapSingleSelectAttributes();
            }

            if (Participants == null || !Participants.Any())
            {
                MapParticipants();
            }
        }

        private void MapAddressFields()
        {
            Address = new MpAddress();
            if (_unmappedData.ContainsKey("Address_Line_1"))
            {
                Address.Address_Line_1 = _unmappedData["Address_Line_1"].Value<string>();
            }
            if (_unmappedData.ContainsKey("City"))
            {
                Address.City = _unmappedData["City"].Value<string>();
            }
            if (_unmappedData.ContainsKey("State"))
            {
                Address.State = _unmappedData["State"].Value<string>();
            }
            if (_unmappedData.ContainsKey("Postal_Code"))
            {
                Address.Postal_Code = _unmappedData["Postal_Code"].Value<string>();
            }
            if (_unmappedData.ContainsKey("Longitude"))
            {
                Address.Longitude = _unmappedData["Longitude"].Value<double?>();
            }
            if (_unmappedData.ContainsKey("Latitude"))
            {
                Address.Latitude = _unmappedData["Latitude"].Value<double?>();
            }
        }

        private void MapMultiSelectAttributes()
        {
            if (!_unmappedData.ContainsKey("MultiSelectAttributes") || string.IsNullOrWhiteSpace(_unmappedData["MultiSelectAttributes"]?.Value<string>()))
            {
                return;
            }
            var attributes = JsonConvert.DeserializeObject<List<MpObjectAttribute>>(_unmappedData["MultiSelectAttributes"].Value<string>());
            foreach (var a in attributes)
            {
                if (!AttributeTypes.ContainsKey(a.AttributeTypeId))
                {
                    AttributeTypes.Add(a.AttributeTypeId, new MpObjectAttributeType
                    {
                        AttributeTypeId = a.AttributeTypeId,
                        Name = a.AttributeTypeName
                    });
                }
                a.Selected = true;
                AttributeTypes[a.AttributeTypeId].Attributes.Add(a);
            }
        }

        private void MapSingleSelectAttributes()
        {
            if (!_unmappedData.ContainsKey("SingleSelectAttributes") || string.IsNullOrWhiteSpace(_unmappedData["SingleSelectAttributes"]?.Value<string>()))
            {
                return;
            }
            var attributes = JsonConvert.DeserializeObject<List<MpObjectAttribute>>(_unmappedData["SingleSelectAttributes"].Value<string>());
            foreach (var a in attributes.Where(a => !SingleAttributes.ContainsKey(a.AttributeTypeId)))
            {
                SingleAttributes.Add(a.AttributeTypeId, a);
            }
        }

        private void MapParticipants()
        {
            if (!_unmappedData.ContainsKey("GroupParticipants") || string.IsNullOrWhiteSpace(_unmappedData["GroupParticipants"]?.Value<string>()))
            {
                return;
            }
            Participants = JsonConvert.DeserializeObject<List<MpGroupParticipant>>(_unmappedData["GroupParticipants"].Value<string>());
        }
    }
}
