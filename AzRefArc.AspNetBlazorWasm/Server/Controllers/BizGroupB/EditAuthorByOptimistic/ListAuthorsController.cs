using AzRefArc.AspNetBlazorWasm.Server.Data;
using AzRefArc.AspNetBlazorWasm.Shared.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzRefArc.AspNetBlazorWasm.Server.Controllers.BizGroupB.EditAuthorByOptimistic
{
    [ApiController]
    [Route("BizGroupB/EditAuthorByOptimistic/ListAuthors")]
    public class ListAuthorsController : ControllerBase
    {
        // [Inject] 属性(プロパティインジェクション)が使えないのでコンストラクタ Injection を使う
        private IDbContextFactory<PubsEntities> dbFactory { get; set; }

        public ListAuthorsController(IDbContextFactory<PubsEntities> dbFactory)
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
