using Application.Commands.IdentityCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebUI.Pages
{
    public class ReadonlyModel : PageModel
    {
        private readonly IMediator _mediator;

        public ReadonlyModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public RegisterDto RegisterDto { get; set; }

        [BindProperty]
        public string Role { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                RegisterCommand regCommand = new RegisterCommand("admin@sbcportal.com", "08000000000", "MainSBC@2020");
                var response = await _mediator.Send(regCommand);

            }
            catch (Exception c) { }

            return Page();
        }
    }
}
