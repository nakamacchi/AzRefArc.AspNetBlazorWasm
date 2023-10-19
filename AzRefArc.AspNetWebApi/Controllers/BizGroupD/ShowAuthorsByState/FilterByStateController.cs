using AzRefArc.AspNetWebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using AzRefArc.AspNetWebApi.Data;

namespace AzRefArc.AspNetWebApi.Controllers.BizGroupD.ShowAuthorsByState
{
    [ApiController]
    [Route("/api/BizGroupD/ShowAuthorsByState/FilterByState")]
    public class FilterByStateController : ControllerBase
    {
        private IDbContextFactory<PubsDbContext> dbFactory { get; set; }

        public FilterByStateController(IDbContextFactory<PubsDbContext> dbFactory)
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
