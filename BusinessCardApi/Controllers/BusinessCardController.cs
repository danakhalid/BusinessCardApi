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

        [HttpGet]
        public IActionResult GetAll()
        {
            var cards = _repo.GetAll();
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

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] BusinessCard model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await TryConvertPhotoToBase64Async(model.Photo);

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

        private async Task<(bool Success, string? Base64, string? ErrorMessage , string? FileName)> TryConvertPhotoToBase64Async(IFormFile? photo, int maxSizeInBytes = 1024 * 1024)
        {
            if (photo == null)
                return (false, null, "No photo provided.", photo.FileName);

            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            if (fileBytes.Length > maxSizeInBytes)
                return (false, null, "Photo exceeds 1MB.", photo.FileName);

            string base64 = Convert.ToBase64String(fileBytes);
            return (true, base64, null,photo.FileName);
        }

    }
}