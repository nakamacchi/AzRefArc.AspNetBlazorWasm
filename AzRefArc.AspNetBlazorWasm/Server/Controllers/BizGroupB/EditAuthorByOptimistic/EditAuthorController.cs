using AzRefArc.AspNetBlazorWasm.Server.Data;
using AzRefArc.AspNetBlazorWasm.Shared.BizGroupB.EditAuthorByOptimistic.EditAuthor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AzRefArc.AspNetBlazorWasm.Server.Controllers.BizGroupB.EditAuthorByOptimistic
{
    [ApiController]
    [Route("BizGroupB/EditAuthorByOptimistic/EditAuthor")]
    public class EditAuthorController : ControllerBase
    {
        // [Inject] 属性(プロパティインジェクション)が使えないのでコンストラクタ Injection を使う
        // https://stackoverflow.com/questions/38459625/property-injection-in-asp-net-core
        // https://www.infoworld.com/article/3603572/dependency-injection-best-practices-for-aspnet-core-mvc-5.html

        private IDbContextFactory<PubsEntities> dbFactory { get; set; }

        public EditAuthorController(IDbContextFactory<PubsEntities> dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException("dbFactory");
        }

        [HttpGet("GetAllStates")]
        public SortedDictionary<string, string> GetAllStates()
        {
            return USStatesUtil.GetAllStates();
        }

        [HttpPost("GetAuthorByAuthorId")]
        public async Task<AuthorData> GetAuthorByAuthorId([FromBody] string authorId)
        {
            if (Regex.IsMatch(authorId, "^[0-9]{3}-[0-9]{2}-[0-9]{4}$") == false) throw new ArgumentException("authorId");

            using (var pubs = dbFactory.CreateDbContext())
            {
                var query = from a in pubs.Authors where a.AuthorId == authorId select a;
                AuthorData response = await query.Select(a => new AuthorData
                {
                    AuthorId = a.AuthorId,
                    AuthorFirstName = a.AuthorFirstName,
                    AuthorLastName = a.AuthorLastName,
                    Address = a.Address,
                    City = a.City,
                    Contract = a.Contract,
                    Phone = a.Phone,
                    State = a.State ?? "",
                    Zip = a.Zip,
                    RowVersion = a.RowVersion
                }).FirstOrDefaultAsync();

                return response;
            }
        }

        [HttpPost("UpdateAuthorWithConcurrencyCheck")]
        public async Task<UpdateAuthorWithConcurrencyCheckResult> UpdateAuthorWithConcurrencyCheck([FromBody] AuthorData updatedAuthor)
        {
            if (ModelState.IsValid == false) throw new ArgumentException("request");

            // データベースに登録を試みる
            using (PubsEntities pubs = dbFactory.CreateDbContext())
            {
                Author target = new Author()
                {
                    AuthorId = updatedAuthor.AuthorId,
                    AuthorFirstName = updatedAuthor.AuthorFirstName,
                    AuthorLastName = updatedAuthor.AuthorLastName,
                    Address = updatedAuthor.Address,
                    City = updatedAuthor.City,
                    Contract = updatedAuthor.Contract,
                    Phone = updatedAuthor.Phone,
                    RowVersion = updatedAuthor.RowVersion,
                    State = updatedAuthor.State,
                    Zip = updatedAuthor.Zip
                };

                pubs.Entry<Author>(target).State = EntityState.Modified;
                try
                {
                    // 同時実行制御（Timestamp 列で制御）
                    await pubs.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return UpdateAuthorWithConcurrencyCheckResult.UpdateFailureByOptimisticConcurrencyConflict;
                }
            }

            return UpdateAuthorWithConcurrencyCheckResult.Success;
        }
    }
}