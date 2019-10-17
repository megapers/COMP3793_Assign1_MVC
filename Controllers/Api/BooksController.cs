using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Assign1.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/Books
        [HttpGet]
        public async Task<IActionResult> GetAllTitles()
        {
            var items = await getJtokenListAsync();
            var titles = items.SelectMany(i => i["volumeInfo"].First).ToList();
            var titleStrings = titles.Select(i => i.ToString()).ToList();
            return Ok(titleStrings);
        }

        //GET: api/Books/5
        [HttpGet("{id}")]
        [Route("api/[controller]/{id:int}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var items = await getJtokenListAsync();
            var item = items[0];

            return Ok("item");
        }


        private async Task<List<JToken>> getJtokenListAsync()
        {
            string url = @"https://www.googleapis.com/books/v1/volumes?q=harry+potter";
            var req = WebRequest.Create(url);
            var r = await req.GetResponseAsync().ConfigureAwait(false);
            var responseReader = new StreamReader(r.GetResponseStream());
            var responseData = await responseReader.ReadToEndAsync();
            var jsonObj = JObject.Parse(responseData);
            var childItems = jsonObj["items"].Children().ToList();
            return childItems;
        }
    }
}
