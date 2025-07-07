using BusinessCardApi.Helpers;
using BusinessCardApi.Models;
using BusinessCardApi.Repo;
using Microsoft.AspNetCore.Mvc;
namespace BusinessCardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController : Controller
    {
        private readonly IBusinessCardRepo _repo;
        public BusinessCardController(IBusinessCardRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var cards = await _repo.GetAll();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var card = _repo.GetById(id);
            if (card == null)
                return NotFound();
            return Ok(card);
        }

        [HttpPost("create")]
        [Consumes("application/json")]

        public async Task<IActionResult> Post([FromBody] BusinessCard model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await FileHelper.TryConvertPhotoToBase64Async(model.Photo);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            model.PhotoBase64 = result.Base64;
            
            _repo.CreateCard(model);

            return Ok(new { message = "added successfully" });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var card = _repo.GetById(id);
            if (card == null)
                return NotFound();

            _repo.Delete(id);
            return NoContent();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCards(IFormFile file, IFormFile? photo)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            List<BusinessCard> cards;

            using var stream = file.OpenReadStream();
            if (extension == ".csv")
            {
                cards = FileHelper.ParseCsv(stream);
            }
            else if (extension == ".xml")
            {
                cards = FileHelper.ParseXml(stream);
            }
            else
            {
                return BadRequest("Unsupported file type.");
            }

            string base64 = null;

            if (photo != null)
            {
                var result = await FileHelper.TryConvertPhotoToBase64Async(photo);
                if (!result.Success)
                    return BadRequest(result.ErrorMessage);

                base64 = result.Base64;
            }

            foreach (var card in cards)
            {
                if (base64 != null)
                    card.PhotoBase64 = base64;

                _repo.CreateCard(card);
            }

            return Ok("business cards imported successfully.");
        }

        [HttpGet("ExportViaXml")]
        public async Task<IActionResult> GetAllViaXmlAsync()
        {
            var cards = await _repo.GetAll();

            var exportFolder = Path.Combine("Exports");
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }

            string xmlContent = FileHelper.ExportToXml(cards);
            string xmlPath = Path.Combine(exportFolder, "BusinessCards.xml");
            System.IO.File.WriteAllText(xmlPath, xmlContent);

            return Ok(new
            {
                XmlFile = xmlPath
            });
        }
        [HttpGet("ExportViaCsv")]
        public async Task<IActionResult> GetAllViaCsvAsync()
        {
            var cards = await _repo.GetAll();

            var exportFolder = Path.Combine("Exports");
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }

            string csvContent = FileHelper.ExportToCsv(cards);
            string csvPath = Path.Combine(exportFolder, "BusinessCards.csv");
            System.IO.File.WriteAllText(csvPath, csvContent);

            return Ok(new
            {
                CsvFile = csvPath,
            });
        }

    }
}