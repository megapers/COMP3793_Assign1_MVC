using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Assign1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace Assign1.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        public BooksController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        // GET: api/Books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  
        public async Task<IActionResult> GetAllTitles()
        {
            var items = new List<JToken>();
            var titles = new List<JToken>();
            var titleStrings = new List<string>();
            try
            {
                items = await getJtokenListAsync();
                titles = items.SelectMany(i => i["volumeInfo"].First).ToList();
                titleStrings = titles.Select(i => i.ToString()).ToList();
            }
            catch (System.NullReferenceException)
            {
                return NotFound();
            }
            return Ok(titleStrings);
        }

        // GET: api/Books/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<IActionResult> GetBookById(int id)
        {
            var items = new List<JToken>();
            JToken volumeInfo = null;
            try
            {
                items = await getJtokenListAsync();
                volumeInfo = items.Select(i => i["volumeInfo"]).ToList()[id];  
            }
            catch (Exception)
            {
                return NotFound();
            }
                 
            var title = "";
            var smallThumbnail = "";
            var authors = new List<string>();
            var publisher = "";
            var publishedDate = "";
            var description = "";
            var ISBN_10 = "";

            try
            {
                title = volumeInfo["title"].ToString();
                smallThumbnail = volumeInfo["imageLinks"].Children().Values<string>().ToList()[0];
                authors = volumeInfo["authors"].Values<string>().ToList();
                publisher = volumeInfo["publisher"].ToString();
                publishedDate = volumeInfo["publishedDate"].ToString();
                description = volumeInfo["description"].ToString();
                ISBN_10 = volumeInfo["industryIdentifiers"].Children().ToList()[1]["identifier"].ToString();
            }
            catch (System.NullReferenceException)
            {
            }

            var book = new Book()
            {
                Title = title,
                SmallThumbnail = smallThumbnail,
                Authors = authors,
                Publisher = publisher,
                PublishedDate = publishedDate,
                Description = description,
                ISBN_10_Id = ISBN_10
            };
            
            return Ok(book);
        }

        private async Task<List<JToken>> getJtokenListAsync()
        {
            if(!_cache.TryGetValue("cachedBooks", out List<JToken> tokenList))
            {
                string url = @"https://www.googleapis.com/books/v1/volumes?q=harry+potter";
                var req = WebRequest.Create(url);
                var r = await req.GetResponseAsync().ConfigureAwait(false);
                var responseReader = new StreamReader(r.GetResponseStream());
                var responseData = await responseReader.ReadToEndAsync();
                var jsonObj = JObject.Parse(responseData);
                tokenList = jsonObj["items"].Children().ToList();
                _cache.Set("cachedBooks", tokenList, TimeSpan.FromSeconds(10));
            }
            else
            {
                Console.WriteLine("Cache hit");
            }
            return tokenList;
        }

    }
}
