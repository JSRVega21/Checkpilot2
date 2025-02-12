using Microsoft.AspNetCore.Mvc;
using System.Net;
using CheckPilot.Models;
using CheckPilot.Server.Repository;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CheckPilot.Server.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicePhotoController : ControllerBase
    {
        private readonly IPhotoRepository<InvoicePhoto, int> _photoRepository;

        public InvoicePhotoController(IPhotoRepository<InvoicePhoto, int> photoRepository)
        {
            _photoRepository = photoRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InvoicePhoto>> GetAllInvoicePhotos()
        {
            try
            {
                var photos = _photoRepository.GetList();
                return Ok(photos);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<InvoicePhoto> GetInvoicePhotoById(int id)
        {
            try
            {
                var photo = _photoRepository.GetByKey(id);
                if (photo == null)
                {
                    return NotFound();
                }
                return Ok(photo);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("PostPhotos")]
        public async Task<IActionResult> UploadPhotos([FromForm] int docEntry, [FromForm] string numAtCard, [FromForm] int docNum, 
            [FromForm] List<IFormFile> signature, [FromForm] List<IFormFile> photo, [FromForm] string location, [FromForm]string comment)
        {
            if (photo == null || photo.Count == 0 || signature == null || signature.Count == 0)
                return BadRequest("Both photo and signature must be uploaded.");

            byte[] photoBytes = null;
            byte[] signatureBytes = null;

            var photoFile = photo[0];
            if (photoFile.Length > 0)
            {
                if (!photoFile.ContentType.StartsWith("image"))
                {
                    return BadRequest("Solo imagenes puede insertar");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await photoFile.CopyToAsync(memoryStream);
                    photoBytes = memoryStream.ToArray();
                }
            }

            var signatureFile = signature[0];
            if (signatureFile.Length > 0)
            {
                if (!signatureFile.ContentType.StartsWith("image"))
                {
                    return BadRequest("Solo imagenes puede insertar.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await signatureFile.CopyToAsync(memoryStream);
                    signatureBytes = memoryStream.ToArray();
                }
            }

            var invoicePhoto = new InvoicePhoto
            {
                DocEntry = docEntry,
                NumAtCard = numAtCard,
                DocNum = docNum,
                BytePhoto = photoBytes,
                ByteSignature = signatureBytes,
                Location = location,
                Comment = comment

            };

            await _photoRepository.AddAsync(invoicePhoto);

            return Ok(new { Message = "Foto actualizada correctamente." });
        }

        [HttpGet("GetBy")]
        public async Task<ActionResult> SearchInvoicePhoto([FromQuery] int? invoicePhotoId, [FromQuery] int? docEntry, [FromQuery] string? numAtCard, [FromQuery] int? docNum)
        {
            try
            {
                var photos = await _photoRepository.FindAsync(photo =>
                    (invoicePhotoId.HasValue && photo.InvoicePhotoId == invoicePhotoId.Value) ||
                    (docEntry.HasValue && photo.DocEntry == docEntry.Value) ||
                    (!string.IsNullOrEmpty(numAtCard) && photo.NumAtCard == numAtCard) ||
                    (docNum.HasValue && photo.DocNum == docNum.Value)
                );

                if (photos == null || photos.Count == 0)
                {
                    return NotFound("No photos found with the specified criteria.");
                }

                return Ok(photos);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }
}
