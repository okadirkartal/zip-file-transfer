namespace Domain.Entities;

public sealed class ErrorModel
{
    public int StatusCode { get; set; }

    public string ErrorMessage { get; set; }
}