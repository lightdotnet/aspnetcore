using Microsoft.AspNetCore.Mvc;
using WebApi.Modules;

namespace WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ModuleTestController(
    OrderModuleService orderModuleService,
    ProductModuleService productModuleService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var obj = new
        {
            productModuleService.Products,
            ProductId = productModuleService.GetProductId,
            OrderId = orderModuleService.GetOrderId
        };

        return Ok(obj);
    }

    [HttpPost]
    public IActionResult Post(int productId)
    {
        productModuleService.AddProduct(productId);
        return Ok(productModuleService.Products);
    }
}
