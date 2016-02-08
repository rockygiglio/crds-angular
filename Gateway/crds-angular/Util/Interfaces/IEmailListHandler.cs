using crds_angular.Models.Crossroads.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crds_angular.Util.Interfaces
{
    public interface IEmailListHandler
    {
        OptInResponse AddListSubscriber(string email, string listName, string token);
    }
}
