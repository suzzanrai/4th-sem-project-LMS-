namespace Practice_Project.Models;

public class ErrorViewModelVm
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}