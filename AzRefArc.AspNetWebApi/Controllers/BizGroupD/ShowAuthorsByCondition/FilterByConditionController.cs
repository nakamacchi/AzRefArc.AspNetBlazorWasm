using Microsoft.EntityFrameworkCore;
using AzRefArc.AspNetWebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using AzRefArc.AspNetWebApi.Shared.BizGroupD.ShowAuthorsByCondition.FilterByCondition;
using AzRefArc.AspNetWebApi.Data;

namespace AzRefArc.AspNetWebApi.Controllers.BizGroupD.ShowAllAuthors
{
    [ApiController]
    [Route("/api/BizGroupD/ShowAuthorsByCondition/FilterByCondition")]
    public class ReportClientErrorController : ControllerBase
    {
        // [Inject] 属性(プロパティインジェクション)が使えないのでコンストラクタ Injection を使う
        // https://stackoverflow.com/questions/38459625/property-injection-in-asp-net-core
        // https://www.infoworld.com/article/3603572/dependency-injection-best-practices-for-aspnet-core-mvc-5.html

        private IDbContextFactory<PubsDbContext> dbFactory { get; set; }

        public ReportClientErrorController(IDbContextFactory<PubsDbContext> dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException("dbFactory");
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
                if (request.IsEnabledContract) query = query.Where(a => a.Contract == request.Contract!.Value);
                if (request.IsEnabledAuFname) query = query.Where(a => a.AuthorFirstName.StartsWith(request.AuFname));

                var result = await query.Select(a => new AuthorOverview()
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                    Phone = a.Phone,
                    State = a.State ?? "",
                    Contract = a.Contract
                }).ToListAsync();

                return result;
            }
        }
    }
}
