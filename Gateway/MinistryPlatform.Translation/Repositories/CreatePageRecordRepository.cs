using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Utils;

namespace MinistryPlatform.Translation.Repositories
{

    public class CreatePageRecordRepository
    {
        public static int CreateRecord(int pageId, Dictionary<string, object> dictionary, String token, bool quickadd = true) {
            return PlatformUtils.Call<int>(token, platformClient => platformClient.CreatePageRecord(pageId, dictionary, quickadd));                  
        }

        public static int CreateSubRecord(int subPageId, int parentRecordId, Dictionary<string, object> dictionary, String token, bool quickadd = true)
        {
            return PlatformUtils.Call<int>(token, platformClient => platformClient.CreateSubpageRecord(subPageId, parentRecordId, dictionary, quickadd));
        }
    }
}
