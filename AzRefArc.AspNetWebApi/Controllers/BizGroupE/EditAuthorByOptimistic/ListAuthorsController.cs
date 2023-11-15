using AzRefArc.AspNetWebApi.Shared;
using AzRefArc.AspNetWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzRefArc.AspNetWebApi.Controllers.BizGroupE.EditAuthorByOptimistic
{
    [ApiController]
    [Route("/api/BizGroupE/EditAuthorByOptimistic/ListAuthors")]
    public class ListAuthorsController : ControllerBase
    {
        // [Inject] 属性(プロパティインジェクション)が使えないのでコンストラクタ Injection を使う
        private IDbContextFactory<PubsDbContext> dbFactory { get; set; }

        public ListAuthorsController(IDbContextFactory<PubsDbContext> dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException("dbFactory");
        }

        [HttpGet("GetAuthors")]
        public async Task<List<AuthorOverview>> GetAuthors()
        {
            var result = new List<AuthorOverview>();

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
