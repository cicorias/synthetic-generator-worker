using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntheticGenerator.Helpers
{
    internal static class Extensions
    {

        public static TimeSpan RoundedToSeconds(this TimeSpan ts)
        {
            return TimeSpan.FromSeconds((int)Math.Round(ts.TotalSeconds));
        }


        public static void ValidateObject(this IValidatableObject obj)
        {
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(obj, new ValidationContext(obj), results, validateAllProperties: true);
            var errorMessages = results.Select(x => x.ErrorMessage);

            if (!valid)
            {
                throw new ValidationException(string.Join(Environment.NewLine, errorMessages));
            }
        }
    }
}
