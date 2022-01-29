using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace erc721_pic_api.Controllers
{
    [ApiController]
    public class PictureController : ControllerBase
    {
        [Route("api/v1/version")]
        public IActionResult version()
        {
            return Ok("v1");
        }

        [Route("api/v1/image/{Genes}")]
        public IActionResult image(string Genes)
        {
            try
            {
                return base.File(ImageHelper.pictureFromGenes(Genes, 1024), "image/jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        [Route("api/v1/metadata/{Genes}")]
        public IActionResult metadata(string Genes)
        {
            string metadata = "{ \"name\": \"ANML\", \"description\":\"\", \"image\":\"http://anmls-test.technology/api/v1/image/" + Genes + "\" }";
            FileContentResult result = new FileContentResult(System.Text.Encoding.UTF8.GetBytes(metadata), "application/json");
            return result;
        }


    }
}