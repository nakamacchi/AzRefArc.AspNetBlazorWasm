using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzRefArc.AspNetBlazorWasm.Server.Data
{
    public partial class PubsEntities : DbContext
    {
        public PubsEntities(DbContextOptions<PubsEntities> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<TitleAuthor> TitleAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 複合 PK の指定 (Fluent API でしか定義できない)
            modelBuilder.Entity<Sale>().HasKey(s => new { s.StoreId, s.OrderNumber, s.TitleId });
            modelBuilder.Entity<TitleAuthor>().HasKey(ta => new { ta.AuthorId, ta.TitleId });
        }
    }
    [Table("authors")]
    public partial class Author
    {
        [Column("au_id"), Required, MaxLength(11), Key]
        public string AuthorId { get; set; }

        [Column("au_fname"), Required, MaxLength(20)]
        public string AuthorFirstName { get; set; }

        [Column("au_lname"), Required, MaxLength(40)]
        public string AuthorLastName { get; set; }

        [Column("phone"), Required, MaxLength(12)]
        public string Phone { get; set; }

        [Column("address"), MaxLength(40)]
        public string Address { get; set; }

        [Column("city"), MaxLength(20)]
        public string City { get; set; }

        [Column("state"), MaxLength(2)]
        public string State { get; set; }

        [Column("zip"), MaxLength(5)]
        public string Zip { get; set; }

        [Column("contract"), Required]
        public bool Contract { get; set; }

        [Column("rowversion"), Timestamp, ConcurrencyCheck]
        public byte[] RowVersion { get; set; }

        public ICollection<TitleAuthor> TitleAuthors { get; set; }

    }

    [Table("publishers")]
    public partial class Publisher
    {
        [Column("pub_id"), Required, MaxLength(4)]
        public string PublisherId { get; set; }

        [Column("pub_name"), MaxLength(40)]
        public string PublisherName { get; set; }

        [Column("city"), MaxLength(20)]
        public string City { get; set; }

        [Column("state"), MaxLength(2)]
        public string State { get; set; }

        [Column("country"), MaxLength(30)]
        public string Country { get; set; }

        public ICollection<Title> Titles { get; set; }
    }

    [Table("titles")]
    public partial class Title
    {
        [Column("title_id"), Required, MaxLength(6), Key]
        public string TitleId { get; set; }

        [Column("title"), Required, MaxLength(80)]
        public string TitleName { get; set; }

        [Column("type"), Required, MaxLength(12)]
        public string Type { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("advance")]
        public decimal? Advance { get; set; }
        [Column("royalty")]
        public int? Royalty { get; set; }

        [Column("ytd_sales")]
        public int? YeatToDateSales { get; set; }

        [Column("notes"), MaxLength(200)]
        public string Notes { get; set; }

        [Column("pubdate"), Required]
        public DateTime PublishedDate { get; set; }

        [Column("pub_id"), MaxLength(4)]
        public string PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public ICollection<TitleAuthor> TitleAuthors { get; set; }
    }

    [Table("sales")]
    public partial class Sale
    {
        // ※ 複合キーは Data Annotation で指定できないため、Fluent API を使う
        [Column("stor_id"), Required, MaxLength(4)]
        public string StoreId { get; set; }

        [Column("ord_num"), Required, MaxLength(20)]
        public string OrderNumber { get; set; }

        [Column("ord_date"), Required]
        public DateTime OrderDate { get; set; }

        [Column("qty"), Required]
        public int Quantity { get; set; }

        [Column("payterms"), Required, MaxLength(12)]
        public string PayTerms { get; set; }

        [Column("title_id"), Required, MaxLength(6)]
        public string TitleId { get; set; }

        public Store Store { get; set; }
        public Title Title { get; set; }
    }

    [Table("stores")]
    public partial class Store
    {
        [Column("stor_id"), Required, MaxLength(4), Key]
        public string StoreId { get; set; }

        [Column("stor_name"), Required, MaxLength(40)]
        public string StoreName { get; set; }

        [Column("stor_addr"), Required, MaxLength(40)]
        public string Address { get; set; }

        [Column("city"), Required, MaxLength(20)]
        public string City { get; set; }

        [Column("state"), Required, MaxLength(22)]
        public string State { get; set; }

        [Column("zip"), Required, MaxLength(5)]
        public string Zip { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }

    [Table("titleauthor")]
    public partial class TitleAuthor
    {
        [Column("au_id"), Required]
        public string AuthorId { get; set; }

        [Column("title_id"), Required]
        public string TitleId { get; set; }

        [Column("au_ord")]
        public byte AuthorOrder { get; set; }

        [Column("royaltyper")]
        public int RoyaltyPercentage { get; set; }

        public Author Author { get; set; }

        public Title Title { get; set; }

    }
}

