using BussinessLayer.RequestModel.EcologicalCharacteristic;
using BussinessLayer.RequestModel.User;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonsaiShop.Controllers
{
    [Route("EcologicalCharacterictic/Controller/")]
    [ApiController]
    public class EcologicalCharacteristicController : ControllerBase
    {
        private readonly IEcologicalCharacteristicService _service;

        public EcologicalCharacteristicController(IEcologicalCharacteristicService services)
        {
            _service = services;
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
    }
}
