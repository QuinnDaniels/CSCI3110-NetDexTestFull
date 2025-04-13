using NetDexTest_01.Models.Entities;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace NetDexTest_01.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region OneToOneRequiredFromDependent
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // CHATGPT
            // Ensure DexHolder has a required one-to-one relationship with ApplicationUser
            modelBuilder.Entity<DexHolder>()
                .HasOne(dh => dh.ApplicationUser)
                .WithOne(au => au.DexHolder)
                .HasForeignKey<DexHolder>(dh => dh.ApplicationUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // CHATGPT
            // Enforce alternate key relationship with UserName
            modelBuilder.Entity<DexHolder>()
                .HasIndex(dh => dh.ApplicationUserName)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(au => au.UserName)
                .IsUnique();

            // QUINN - Establish UserName as an alternate key
            //modelBuilder.Entity<ApplicationUser>() //r //Blog
            //    .HasAlternateKey(e => e.UserName);

            //modelBuilder.Entity<ApplicationUser>() //r //Blog
            //    .HasOne(e => e.DexHolder)    //  // Header
            //    .WithOne(e => e.ApplicationUser)     //   // Blog
            //    .HasForeignKey<DexHolder>(e => e.ApplicationUserName) //  //  // BlogHeader  BlogId
            //    .IsRequired();
        }
        #endregion
        public DbSet<DexHolder> DexHolder => Set<DexHolder>();
        public DbSet<Person> Person => Set<Person>();


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ApplicationUser>()
        //        .HasOne(e => e.Dex)
        //    .WithOne(e => e.ApplicationUser)
        //        .HasForeignKey<ApplicationUser>(e => e.UserName)
        //        .IsRequired(false);
        //}





        //#region OneToOneRequired
        //// Principal (parent)
        //public class Blog
        //{
        //    public int Id { get; set; }
        //    public BlogHeader? Header { get; set; } // Reference navigation to dependent
        //}

        //// Dependent (child)
        //public class BlogHeader
        //{
        //    public int Id { get; set; }
        //    public int BlogId { get; set; } // Required foreign key property
        //    public Blog Blog { get; set; } = null!; // Required reference navigation to principal
        //}
        //#endregion

        //public class BlogContext0 : DbContext
        //{
        //    public DbSet<Blog> Blogs => Set<Blog>();
        //    public DbSet<BlogHeader> BlogHeaders => Set<BlogHeader>();

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        => optionsBuilder.UseSqlite($"DataSource = Required{GetType().Name}.db");
        //}

        //public class BlogContext1 : BlogContext0
        //{
        //    #region OneToOneRequiredFromPrincipal
        //    protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //        modelBuilder.Entity<ApplicationUser>() //r //Blog
        //            .HasOne(e => e.Dex)    //  // Header
        //            .WithOne(e => e.ApplicationUser)     //   // Blog
        //            .HasForeignKey<DexHolder>(e => e.UserName) //  //  // BlogHeader  BlogId
        //            .IsRequired();
        //    }
        //    #endregion






    }
}
