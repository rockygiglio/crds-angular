﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Services.Interfaces
{
    public interface IEmailCommunication
    {
        void SendEmail(EmailCommunicationDTO email, string token = null);
        void SendEmail(CommunicationDTO emailData);
    }
}