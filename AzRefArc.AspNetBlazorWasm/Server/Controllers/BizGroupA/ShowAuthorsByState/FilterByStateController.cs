using AzRefArc.AspNetBlazorWasm.Server.Data;
using AzRefArc.AspNetBlazorWasm.Shared.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AzRefArc.AspNetBlazorWasm.Server.Controllers.BizGroupA.ShowAuthorsByState
{
    [ApiController]
    [Route("BizGroupA/ShowAuthorsByState/FilterByState")]
    public class FilterByStateController : ControllerBase
    {
        private IDbContextFactory<PubsEntities> dbFactory { get; set; }

        public FilterByStateController(IDbContextFactory<PubsEntities> dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException("dbFactory");
        }

        [HttpGet("GetAllStates")]
        public SortedDictionary<string, string> GetAllStates()
        {
            return USStatesUtil.GetAllStates();
        }

        [HttpPost("GetAuthorsByState")]
        public async Task<List<AuthorOverview>> GetAuthorsByState([FromBody] string state)
        {
            // 入力データチェック
            if (Regex.IsMatch(state, "^[A-Z]{2}$") == false) throw new ArgumentException("state");

            var result = new List<AuthorOverview>();

            using (var pubs = dbFactory.CreateDbContext())
            {
                var query = from a in pubs.Authors
                            where a.State == state
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
