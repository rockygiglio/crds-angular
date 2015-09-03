﻿using System.Collections.Generic;

namespace crds_angular.Models.MP
{
    public class Household
    {
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal_Code { get; set; }
        public string Home_Phone { get; set; }
        public string Foreign_Country { get; set; }
        public string County { get; set; }
        public int? Congregation_ID { get; set; }
        public int Household_ID { get; set; }
        public List<HouseholdMember> Household_Members { get; set; }
    }
}