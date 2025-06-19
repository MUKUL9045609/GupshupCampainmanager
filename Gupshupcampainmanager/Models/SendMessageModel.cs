using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Gupshupcampainmanager.Service;

public class SendMessageModel : PageModel
{
 
    [BindProperty]
    public IFormFile ImageFile { get; set; }

    [BindProperty]
    public string OfferText { get; set; }

    public string ResponseMessage { get; set; }
    public string AlertClass { get; set; }

}