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
    //[Produces("application/json")]
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

        // GET: api/Books/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var items = await getJtokenListAsync();
            var volumeInfo = items.Select(i => i["volumeInfo"]).ToList();       
            var theBook = volumeInfo[id].Children().ToList();
            var title = theBook[0].First.ToString();
            var authors = theBook[1].First.ToString();
            var publisher = theBook[2].First.ToString();
            var published = theBook[3].First.ToString();
            var description = theBook[4].First.ToString();

            JArray array = new JArray();
            array.Add(theBook[0].First);
            array.Add(theBook[1].First);
            array.Add(theBook[2].First);
            array.Add(theBook[3].First);
            array.Add(theBook[4].First);

            JObject o = new JObject();
            o["MyArray"] = array;
            string json = o.ToString();
            return Ok(json);
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
