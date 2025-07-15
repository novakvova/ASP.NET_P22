using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiPizushi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUpdate([FromBody] CartCreateUpdateModel model)
        {
            await cartService.CreateUpdate(model);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRange([FromBody] List<CartCreateUpdateModel> modelItems)
        {
            foreach (var item in modelItems)
            {
                await cartService.CreateUpdate(item);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var model =  await cartService.GetCartItems();
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await cartService.Delete(id);
            return Ok();
        }
    }
}
