using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }



    public async void OnGet()
    {
        Func<string, Task> info = infoMessage => Task.Run(()=> Console.WriteLine("info: " + infoMessage));
        Func<string, Task> warningMessage = warningMessage => Task.Run(()=> Console.WriteLine("warning: " + warningMessage));
        var JobService = new JobServiceJSON();
        var JobService2 = new JobServiceXML();
        CancellationToken cancellationToken = new CancellationToken();

        JobArguments arguments = new JobArguments(Guid.Empty, "apikey", new Dictionary<string,string>());
        await JobService.Execute(arguments, info, warningMessage, cancellationToken);
        await JobService2.Execute(arguments, info, warningMessage, cancellationToken);
    }
}
