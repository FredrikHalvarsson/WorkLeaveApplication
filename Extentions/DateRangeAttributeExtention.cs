using LeaveApplicationApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LeaveApplicationApp.Extentions
{
    public class DateRangeAttributeExtention
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class DateCorrectRangeAttribute : ValidationAttribute
        {
            public bool ValidateStartDate { get; set; }
            public bool ValidateEndDate { get; set; }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var model = validationContext.ObjectInstance as LeaveApplication;

                if (model != null)
                {
                    if (model.StartDate > model.EndDate && ValidateEndDate
                        || model.StartDate < DateTime.Now.Date && ValidateStartDate)
                    {
                        return new ValidationResult(string.Empty);
                    }
                }

                return ValidationResult.Success;
            }
        }
    }
}
