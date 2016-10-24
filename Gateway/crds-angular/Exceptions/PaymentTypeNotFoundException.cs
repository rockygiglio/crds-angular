using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Exceptions
{
    public class PaymentTypeNotFoundException : Exception
    {
        public int PaymentTypeId;

        public PaymentTypeNotFoundException(int paymentTypeId) : base($"Payment Type {paymentTypeId} not found.")
        {
            PaymentTypeId = paymentTypeId;
        }
    }
}