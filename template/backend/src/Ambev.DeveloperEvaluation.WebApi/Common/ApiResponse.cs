using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = new List<ValidationErrorDetail>();
    public static ApiResponse BuildErrorResponse(string message) =>
        new ApiResponse
        {
            Success = false,
            Message = "Invalid operation",
            Errors = new List<ValidationErrorDetail>
            {
                new() { Error = message }
            }
        };
}
