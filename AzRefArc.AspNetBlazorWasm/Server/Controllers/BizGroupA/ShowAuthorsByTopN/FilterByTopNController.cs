using AzRefArc.AspNetBlazorWasm.Server.Data;
using AzRefArc.AspNetBlazorWasm.Shared.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AzRefArc.AspNetBlazorWasm.Server.Controllers.BizGroupA.ShowAuthorsByState
{
    [ApiController]
    [Route("BizGroupA/ShowAuthorsByTopN/FilterByTopN")]
    
    public class FilterByTopNController : ControllerBase
    {
        private IDbContextFactory<PubsEntities> dbFactory { get; set; }

        public FilterByTopNController(IDbContextFactory<PubsEntities> dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException("dbFactory");
        }

        [HttpPost("GetAuthorsByTopN")]
        public async Task<List<AuthorOverview>> GetAuthorsByTopN([FromBody] int n)
        {
            // 入力データチェック
            if (n <= 0) throw new ArgumentException("n");

            var result = new List<AuthorOverview>();

            using (var pubs = dbFactory.CreateDbContext())
            {
                var query = pubs.Authors.Take(n)
                            .Select(a => new AuthorOverview()
                            {
                                AuthorId = a.AuthorId,
                                AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                                Phone = a.Phone,
                                State = a.State ?? "",
                                Contract = a.Contract
                            });
                result = await query.ToListAsync();
            }

            return result;
        }
    }
}
