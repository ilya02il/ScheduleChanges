using System.Collections.Generic;

namespace IdentityAPI.Contracts.v1.Responses
{
    public class FailResponse
    {
        public IEnumerable<string> Errors { get; init; }

        public FailResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
