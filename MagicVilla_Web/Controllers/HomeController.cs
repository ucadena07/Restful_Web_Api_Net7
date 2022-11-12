using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
		private readonly IVillaService _villaService;
		private readonly IMapper _mapper;
		public HomeController(IVillaService villaService, IMapper mapper)
		{
			_villaService = villaService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			List<VillaDto> list = new();
			var token = HttpContext.Session.GetString(SD.SessionToken);

            var response = await _villaService.GetAllAsync<APIResponse>(token);
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
			}

			return View(list);
		}

    }
}