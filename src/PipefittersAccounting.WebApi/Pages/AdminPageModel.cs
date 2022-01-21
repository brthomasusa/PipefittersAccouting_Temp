using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace PipefittersAccounting.WebApi.Pages
{
    [Authorize(Roles = "Admins")]
    public class AdminPageModel : PageModel
    {

    }
}