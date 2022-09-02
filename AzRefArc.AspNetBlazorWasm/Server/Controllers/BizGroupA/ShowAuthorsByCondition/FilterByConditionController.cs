using Microsoft.EntityFrameworkCore;
using AzRefArc.AspNetBlazorWasm.Server.Data;
using AzRefArc.AspNetBlazorWasm.Shared.Shared;
using Microsoft.AspNetCore.Mvc;
using AzRefArc.AspNetBlazorWasm.Shared.BizGroupA.ShowAuthorsByCondition.FilterByCondition;

namespace AzRefArc.AspNetBlazorWasm.Server.Controllers.BizGroupA.ShowAllAuthors
{
    [ApiController]
    [Route("BizGroupA/ShowAuthorsByCondition/FilterByCondition")]
    public class FilterByConditionController : ControllerBase
    {
        // [Inject] 属性(プロパティインジェクション)が使えないのでコンストラクタ Injection を使う
        // https://stackoverflow.com/questions/38459625/property-injection-in-asp-net-core
        // https://www.infoworld.com/article/3603572/dependency-injection-best-practices-for-aspnet-core-mvc-5.html

        private IDbContextFactory<PubsEntities> dbFactory { get; set; }

        public FilterByConditionController(IDbContextFactory<PubsEntities> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        [HttpGet("GetAllStates")]
        public SortedDictionary<string, string> GetAllStates()
        {
            return USStatesUtil.GetAllStates();
        }

        [HttpPost("GetAuthorsByCondition")]
        public async Task<List<AuthorOverview>> GetAuthorsByCondition([FromBody] GetAuthorsByConditionRequest request)
        {
            if (ModelState.IsValid == false) throw new ArgumentException("request");

            using (var pubs = dbFactory.CreateDbContext())
            {
                var query = pubs.Authors.AsQueryable();
                if (request.IsEnabledState) query = query.Where(a => a.State == request.State);
                if (request.IsEnabledPhone) query = query.Where(a => a.Phone == request.Phone);
                if (request.IsEnabledContract) query = query.Where(a => a.Contract == request.Contract);
                if (request.IsEnabledAuFname) query = query.Where(a => a.AuthorFirstName.StartsWith(request.AuFname));

                var result = await query.Select(a => new AuthorOverview()
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                    Phone = a.Phone,
                    State = a.State,
                    Contract = a.Contract
                }).ToListAsync();

                return result;
            }
        }
    }
}
