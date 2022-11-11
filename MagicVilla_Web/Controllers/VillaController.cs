﻿using AutoMapper;
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

			var response = await _villaService.GetAllAsync<APIResponse>();
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
			}

			return View(list);
		}

        [Authorize(Roles = "Admin")]
        public  IActionResult CreateVilla()
		{
			return View();
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVilla(VillaCreateDto model)
		{
			if (ModelState.IsValid)
			{
				var response = await _villaService.CreateAsync<APIResponse>(model);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Villa created succcessfully";
					return RedirectToAction(nameof(Index));
				}
			}
			TempData["error"] = "Error encountered";
			return View(model);
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
		{


			var response = await _villaService.GetAsync<APIResponse>(villaId);
			if (response != null && response.IsSuccess)
			{
				var model = JsonConvert.DeserializeObject<Villa>(Convert.ToString(response.Result));
				return View(_mapper.Map<VillaUpdateDto>(model));
			}

			return NotFound();
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
		{
			if (ModelState.IsValid)
			{
				var response = await _villaService.UpdateAsync<APIResponse>(model);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Villa updated succcessfully";
					return RedirectToAction(nameof(Index));
				}
			}
			TempData["error"] = "Error encountered";
			return View(model);
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
		{


			var response = await _villaService.GetAsync<APIResponse>(villaId);
			if (response != null && response.IsSuccess)
			{
				var model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
				return View(model);
			}

			return NotFound();
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVilla(VillaDto model)
		{
		
				var response = await _villaService.DeleteAsync<APIResponse>(model.Id);
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
