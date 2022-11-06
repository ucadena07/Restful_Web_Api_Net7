﻿using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService,IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDto> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if(response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CreateVilla()
        {
            return View();  
        }

        [HttpPost]
		public async Task<IActionResult> CreateVilla(VillaCreateDto model)
		{
            if(ModelState.IsValid) 
            {
				var response = await _villaService.CreateAsync<APIResponse>(model);
				if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
				}
			}
			return View(model);
		}


	}
}
