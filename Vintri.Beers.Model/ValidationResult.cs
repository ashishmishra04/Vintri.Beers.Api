using System.Collections.Generic;
using System.Linq;

namespace Vintri.Beers.Model
{
    public class ValidationResult
    {
        private readonly List<ValidationError> _errors;

        public ValidationResult() : this(new List<ValidationError>())
        {
        }

        public ValidationResult(ICollection<ValidationError> errors)
        {
            _errors = errors == null ? new List<ValidationError>() : errors.ToList();
        }


        public bool IsValid => _errors.Count == 0;

        public ICollection<ValidationError> Errors => _errors;

        public void RemoveError(ValidationError error)
        {
            if (_errors.Contains(error))
                _errors.Remove(error);
        }

        public void AddError(ValidationError error)
        {
            _errors.Add(error);
        }
    }
}
