using System.Collections.Generic;

namespace IdentityAPI.Results
{
    public class RegistrationResult
    {
        public bool IsSuccess { get; init; } = false;
        public IEnumerable<string> Errors { get; init; }

        public RegistrationResult(bool isSucess)
        {
            IsSuccess = isSucess;
        }

        public RegistrationResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
