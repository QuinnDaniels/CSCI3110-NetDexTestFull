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




            // SocialMedia (M) - ContactInfo (1)
            modelBuilder.Entity<ContactInfo>()
                .HasMany(ci => ci.SocialMedias) // prin -> depend
                .WithOne(sm => sm.ContactInfo) // depend -> prin
                .HasForeignKey(sm => sm.ContactInfoId) // depend -> prin
                .IsRequired() // indicates that ContactInfoId in SocialMedia is req'd
                .OnDelete(DeleteBehavior.Cascade); // cascade delete when a ContactInfo is deleted



            // EntryItem (M) - RecordCollector (1)
            modelBuilder.Entity<RecordCollector>()
                .HasMany(rc => rc.EntryItems) // prin -> depend
                .WithOne(ei => ei.RecordCollector) // depend -> prin
                .HasForeignKey(ei => ei.RecordCollectorId) // depend -> prin
                .IsRequired() // indicates that RecordCollector in EntryItem is req'd
                .OnDelete(DeleteBehavior.Cascade); // cascade delete when a RecordCollector is deleted


            // Person (M) - DexHolder (1)
            modelBuilder.Entity<DexHolder>()
                .HasMany(dh => dh.People) // prin -> depend
                .WithOne(p => p.DexHolder) // depend -> prin
                .HasForeignKey(p => p.DexHolderId) // depend -> prin
                .IsRequired() // indicates that DexHolderId in Person is req'd
                .OnDelete(DeleteBehavior.Cascade); // cascade delete when a DexHolder is deleted


            // ------------------ [ [ 1-1 ] ExtensionTables -> Person ] ----------------
            modelBuilder.Entity<RecordCollector>()
                .HasOne(rc => rc.Person) // prin -> depend
                .WithOne(p => p.RecordCollector) // depend -> prin
                .HasForeignKey<RecordCollector>(rc => rc.PersonId) // depend -> prin
                .IsRequired() // indicates that PersonId in RecordCollector is req'd
                .OnDelete(DeleteBehavior.Cascade); // cascade delete when a Person is deleted
            
            modelBuilder.Entity<ContactInfo>()
                .HasOne(ci => ci.Person) // prin -> depend
                .WithOne(p => p.ContactInfo) // depend -> prin
                .HasForeignKey<ContactInfo>(ci => ci.PersonId) // depend -> prin
                .IsRequired() // indicates that PersonId in ContactInfo is req'd
                .OnDelete(DeleteBehavior.Cascade); // cascade delete when a Person is deleted

            modelBuilder.Entity<FullName>()
                .HasOne(fn => fn.Person) // prin -> depend
                .WithOne(p => p.FullName) // depend -> prin
                .HasForeignKey<FullName>(fn => fn.PersonId) // depend -> prin
                .IsRequired() // indicates that PersonId in FullName is req'd
                .OnDelete(DeleteBehavior.Cascade); // cascade delete when a Person is deleted

            // -- END ----------- [ [ 1-1 ] ExtensionTables -> Person ] ----------------




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
        public DbSet<ContactInfo> ContactInfo => Set<ContactInfo>();
        public DbSet<DexHolder> DexHolder => Set<DexHolder>();

        public DbSet<EntryItem> EntryItem => Set<EntryItem>();

        public DbSet<FullName> FullName => Set<FullName>();
        public DbSet<Person> Person => Set<Person>();
        public DbSet<RecordCollector> RecordCollector => Set<RecordCollector>();

        public DbSet<SocialMedia> SocialMedia => Set<SocialMedia>();

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
