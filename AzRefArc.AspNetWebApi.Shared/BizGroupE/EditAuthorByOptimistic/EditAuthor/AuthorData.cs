using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetWebApi.Shared.BizGroupE.EditAuthorByOptimistic.EditAuthor
{
    public class AuthorData
    {
        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{2}-[0-9]{4}$")]
        public string AuthorId { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[\u0020-\u007e]{1,20}$")]
        public string AuthorFirstName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[\u0020-\u007e]{1,40}$")]
        public string AuthorLastName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{3} \d{3}-\d{4}$")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(40)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? City { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{2}$")]
        public string State { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9]{5}$")]
        public string? Zip { get; set; }

        public bool Contract { get; set; }

        public byte[]? RowVersion { get; set; }
    }
}

