using BussinessLayer.RequestModel.EcologicalCharacteristic;
using BussinessLayer.RequestModel.User;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonsaiShop.Controllers
{
    [Route("api/EcologicalCharacterictic/")]
    [ApiController]
    public class EcologicalCharacteristicController : ControllerBase
    {
        private readonly IEcologicalCharacteristicService _service;

        public EcologicalCharacteristicController(IEcologicalCharacteristicService services)
        {
            _service = services;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateEco(CreateEcologicalCharacteristicRequestModel model)
        {
            try
            {
                var result = await _service.Create(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost("Search")]
        public async Task<IActionResult> GetAllListEco(GetAllEcologicalCharacteristicRequestModel model)
        {
            try
            {
                var result = await _service.GetList(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateEco(int id, UpdateEcologicalCharacteristicRequestModel model)
        {
            try
            {
                var result = await _service.Update(id ,model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost("Detele/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
