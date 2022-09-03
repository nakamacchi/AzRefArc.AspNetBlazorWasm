using Microsoft.EntityFrameworkCore;
using AzRefArc.AspNetBlazorWasm.Server.Data;
using AzRefArc.AspNetBlazorWasm.Shared.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AzRefArc.AspNetBlazorWasm.Server.Controllers.BizGroupA.ShowAllAuthors
{
    [ApiController]
    [Route("BizGroupA/ShowAllAuthors/ListAuthors")]
    public class ListAuthorsController : ControllerBase
    {
        // [Inject] 属性(プロパティインジェクション)が使えないのでコンストラクタ Injection を使う
        // https://stackoverflow.com/questions/38459625/property-injection-in-asp-net-core
        // https://www.infoworld.com/article/3603572/dependency-injection-best-practices-for-aspnet-core-mvc-5.html

        private IDbContextFactory<PubsEntities> dbFactory { get; set; }

        public ListAuthorsController(IDbContextFactory<PubsEntities> dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException("dbFactory");
        }

        [HttpGet("GetAuthors")]
        public async Task<List<AuthorOverview>> GetAuthors()
        {
            var result= new List<AuthorOverview>();

            using (var pubs = dbFactory.CreateDbContext())
            {
                var query = from a in pubs.Authors
                            select new AuthorOverview()
                            {
                                AuthorId = a.AuthorId,
                                AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                                Phone = a.Phone,
                                State = a.State ?? "",
                                Contract = a.Contract
                            };
                result = await query.ToListAsync();
            }

            return result;            
        }
    }
}
