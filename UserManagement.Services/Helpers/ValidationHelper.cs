using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UserManagement.Services.Helpers;
public class ValidationHelper
{
    /// <summary>
    /// Validates the properties of the specified object using data annotations.
    /// </summary>
    /// <param name="obj">The object to validate.</param>
    /// <exception cref="ArgumentException">Thrown if the object fails validation.</exception>
    public static void ModelValidation(object obj)
    {
        //Model validations
        var validationContext = new ValidationContext(obj);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
        if (!isValid)
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }
    }
}
