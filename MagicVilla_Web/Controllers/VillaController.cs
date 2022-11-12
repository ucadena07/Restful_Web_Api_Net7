using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
	public class VillaController : Controller
	{
		private readonly IVillaService _villaService;
		private readonly IMapper _mapper;
		public VillaController(IVillaService villaService, IMapper mapper)
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

        [Authorize(Roles = "Admin,admin")]
        public  IActionResult CreateVilla()
		{
			return View();
		}

		[HttpPost]
        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> CreateVilla(VillaCreateDto model)
		{
			if (ModelState.IsValid)
			{
                List<VillaDto> list = new();
                var token = HttpContext.Session.GetString(SD.SessionToken);
                var response = await _villaService.CreateAsync<APIResponse>(model,token);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Villa created succcessfully";
					return RedirectToAction(nameof(Index));
				}
			}
			TempData["error"] = "Error encountered";
			return View(model);
		}

        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
		{

            List<VillaDto> list = new();
            var token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaService.GetAsync<APIResponse>(villaId,token);
			if (response != null && response.IsSuccess)
			{
				var model = JsonConvert.DeserializeObject<Villa>(Convert.ToString(response.Result));
				return View(_mapper.Map<VillaUpdateDto>(model));
			}

			return NotFound();
		}

		[HttpPost]
        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
		{
			if (ModelState.IsValid)
			{
                List<VillaDto> list = new();
                var token = HttpContext.Session.GetString(SD.SessionToken);
                var response = await _villaService.UpdateAsync<APIResponse>(model, token);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Villa updated succcessfully";
					return RedirectToAction(nameof(Index));
				}
			}
			TempData["error"] = "Error encountered";
			return View(model);
		}

        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
		{

            List<VillaDto> list = new();
            var token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaService.GetAsync<APIResponse>(villaId, token);
			if (response != null && response.IsSuccess)
			{
				var model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
				return View(model);
			}

			return NotFound();
		}

		[HttpPost]
        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> DeleteVilla(VillaDto model)
		{

            List<VillaDto> list = new();
            var token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id,token);
				if (response != null && response.IsSuccess)
				{
				TempData["success"] = "Villa deleted succcessfully";
				return RedirectToAction(nameof(Index));
				}

			TempData["error"] = "Error encountered";
			return View(model);
		}


	}
}
