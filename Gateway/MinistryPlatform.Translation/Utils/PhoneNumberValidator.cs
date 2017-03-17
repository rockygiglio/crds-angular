using System;
using System.Text.RegularExpressions;

namespace MinistryPlatform.Translation.Helpers
{
    public static class PhoneNumberValidator
    {
        private static readonly string pattern = "^\\d{3}-\\d{3}-\\d{4}$";

        public static bool Validate(params string[] phoneNumbers)
        {
            foreach (string phoneNumber in phoneNumbers)
            {
                Console.WriteLine("Phone number to be validated " + phoneNumber);

                if (phoneNumber != null && phoneNumber != "")
                {
                    if (!Regex.IsMatch(phoneNumber, pattern))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

}