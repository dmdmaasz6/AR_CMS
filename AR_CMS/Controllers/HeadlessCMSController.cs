using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Piranha.AspNetCore.Services;
using Piranha;
using AR_CMS.Models;

namespace AR_CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeadlessCMSController : ControllerBase
    {
        private readonly ILogger<HeadlessCMSController> _logger;
        private readonly IApi _api;
        private readonly IModelLoader _loader;

        public HeadlessCMSController(ILogger<HeadlessCMSController> logger, IApi api, IModelLoader loader)
        {
            _logger = logger;
            _api = api;
            _loader = loader;
        }

        [HttpGet]
        [Route("GetARContent")]
        public async Task<IActionResult> GetARContent()
        {
            try
            {
                ARContentPage model = new ARContentPage();
                ARContentPageJSON result = new ARContentPageJSON();

                model = await _api.Pages.GetBySlugAsync<ARContentPage>("arcontent");
                
                if (model.AR_Artifacts != null)
                {
                    result.TrackImageItem = new ARItemJSON[model.AR_Artifacts.Count];

                    int count = 0;
                    foreach (var artifact in model.AR_Artifacts)
                    {
                        result.TrackImageItem[count] = new ARItemJSON();
                        result.TrackImageItem[count] = new ARItemJSON{
                            ContentLink = artifact.Content.Media.PublicUrl,
                            ContentType = (int)artifact.ContentType.Value,
                            Link = artifact.TrackedImage.Media.PublicUrl,
                            Name = artifact.Name,
                        };
                        count++;
                    }
                }

                return await Task.FromResult(Ok(result));
            }
            catch (Exception e)
            {
                return await Task.FromResult(BadRequest(e.Message));
            }
        }

        [HttpGet]
        [Route("GetARContentAbout")]
        public async Task<IActionResult> GetARContentAbout()
        {
            try
            {
                // Fetch the AboutPage by its slug.
                var model = await _api.Pages.GetBySlugAsync<ARContentAboutPage>("arcontentabout");

                // Create a new AboutPageJSON object using the data from the fetched AboutPage.
                var result = new ARContentAboutPageJSON
                {
                    about = new ARContentAboutPageJSON.About
                    {
                        logo = model.AboutContent.Logo.Media.PublicUrl,
                        description = model.AboutContent.Description.Value,
                        link = model.AboutContent.SiteLink.Value
                    }
                };

                // Return the result.
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
