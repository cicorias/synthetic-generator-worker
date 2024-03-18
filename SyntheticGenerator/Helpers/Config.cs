using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntheticGenerator.Generators;

namespace SyntheticGenerator.Helpers
{
    public class ReplayOrdersOptions : IValidatableObject
    {
        public DateTime? WindowStartTimeStr { get; set; }
        public DateTime? WindowEndTimeStr { get; set; }
        public string? OutputFile { get; set; }
        public double? Lambda { get; set; }
        public int? NumberOfEvents { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (WindowStartTimeStr.HasValue && WindowEndTimeStr.HasValue && WindowEndTimeStr <= WindowStartTimeStr)
            {
                yield return new ValidationResult("EndTime must be greater than StartTime.");
            }
        }
    }
}
