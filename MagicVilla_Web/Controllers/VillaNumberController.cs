using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using MagicVilla_Utility;
using Microsoft.AspNetCore.Authentication;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }
        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>(token);
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }


        [Authorize(Roles = "Admin, admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM vm = new();
            List<VillaDto> list = new();
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _villaService.GetAllAsync<APIResponse>(token);
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }

            vm.VillaList = list.Select(it => new SelectListItem
            {
                Text = it.Name,
                Value = it.Id.ToString(),
            });

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (ModelState.IsValid)
            {
           
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber,token);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if(response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages",response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            List<VillaDto> list = new();
            var resp = await _villaService.GetAllAsync<APIResponse>(token);
            if (resp != null && resp.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result));
            }

            model.VillaList = list.Select(it => new SelectListItem
            {
                Text = it.Name,
                Value = it.Id.ToString(),
            });

            return View(model);
        }

        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> UpdateVillaNumber(int VillaNo)
        {

            VillaNumberUpdateVM vm = new();
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _villaNumberService.GetAsync<APIResponse>(VillaNo,token);
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaNumber>(Convert.ToString(response.Result));
                vm.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);  
               
            }
            List<VillaDto> list = new();
            var resp = await _villaService.GetAllAsync<APIResponse>(token);
            if (resp != null && resp.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result));
                vm.VillaList = list.Select(it => new SelectListItem
                {
                    Text = it.Name,
                    Value = it.Id.ToString(),
                });

                return View(vm);
            }



            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber, token);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            List<VillaDto> list = new();
            var resp = await _villaService.GetAllAsync<APIResponse>(token);
            if (resp != null && resp.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result));
            }

            model.VillaList = list.Select(it => new SelectListItem
            {
                Text = it.Name,
                Value = it.Id.ToString(),
            });

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int VillaNo)
        {

            var token = await HttpContext.GetTokenAsync("access_token");
            VillaNumberDeleteVM vm = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(VillaNo, token);
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaNumber>(Convert.ToString(response.Result));
                vm.VillaNumber = _mapper.Map<VillaNumberDTO>(model);

            }
            List<VillaDto> list = new();
            var resp = await _villaService.GetAllAsync<APIResponse>(token);
            if (resp != null && resp.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result));
                vm.VillaList = list.Select(it => new SelectListItem
                {
                    Text = it.Name,
                    Value = it.Id.ToString(),
                });

                return View(vm);
            }



            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo, token);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
