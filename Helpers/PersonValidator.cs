using IMDBSample.Exceptions;
using System;

namespace IMDBSample.Helpers
{
    public static class PersonValidator
    {
        public static void Validate(string name, string bio, DateTime dob, string gender)
        {
            // Name
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidRequestDataException("Name is required.");

            // Bio
            if (string.IsNullOrWhiteSpace(bio))
                throw new InvalidRequestDataException("Bio is required.");

            // DOB
            if (dob == default)
                throw new InvalidRequestDataException("DOB is required.");

            if (dob > DateTime.Today)
                throw new InvalidRequestDataException("DOB cannot be in the future.");

            // Gender
            if (string.IsNullOrWhiteSpace(gender))
                throw new InvalidRequestDataException("Gender is required.");

            var g = gender.Trim().ToLower();

            if (g != "m" && g != "f")
                throw new InvalidRequestDataException("Gender must be 'M' or 'F'.");
        }
    }
}
