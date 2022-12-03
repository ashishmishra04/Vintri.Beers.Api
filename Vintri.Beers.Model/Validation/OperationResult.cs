using System.Collections.Generic;
using System.Linq;

namespace Vintri.Beers.Model.Validation
{
    public class OperationResult
    {
        public ValidationResult ValidationResult { get; private set; }

        public OperationResult(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public bool IsSuccess => ValidationResult == null || ValidationResult.IsValid;

        public ICollection<string> Messages
        {
            get
            {
                return !ValidationResult.IsValid
                    ? ValidationResult.Errors.Select(x => x.Message).ToList()
                    : new List<string>();
            }
        } 
    }

    public class OperationResult<T> : OperationResult where T : class
    {
        public T Data { get; private set; }
        public OperationResult(T data, ValidationResult validationResult) : base(validationResult)
        {
            Data = data;
        }
    }
}
