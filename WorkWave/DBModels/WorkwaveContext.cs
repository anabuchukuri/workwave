using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Crypto.Macs;
using WorkWave.DBModels;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace WorkWave.DbModels;

public partial class WorkwaveContext : IdentityDbContext<User, Role, int>
{
    public virtual DbSet<JobOpening> JobOpening { get; set; }
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<JobCategory> JobCategory { get; set; }
    public virtual DbSet<OpeningCategory> OpeningCategory { get; set; }
    public virtual DbSet<JobType> JobType { get; set; }
    public virtual DbSet<JobDetails> JobDetails { get; set; }
    public virtual DbSet<JobApplication> JobApplication { get; set; }
    public virtual DbSet<JobSeeker> JobSeeker { get; set; }
    public virtual DbSet<Employer> Employer { get; set; }

    public DbSet<Role> Roles { get; set; }
    public WorkwaveContext(DbContextOptions<WorkwaveContext> options)
        : base(options)
    {
       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<JobOpening>(ConfigureJobOpening);
        modelBuilder.Entity<JobDetails>(ConfigureJobDetails);
        modelBuilder.Entity<JobCategory>(ConfigureJobCategory);
        modelBuilder.Entity<JobType>(ConfigureJobType);
        modelBuilder.Entity<JobApplication>(ConfigureJobApplication);
        modelBuilder.Entity<OpeningCategory>(ConfigureOpeningCategory);
        modelBuilder.Entity<JobSeeker>(ConfigureJobSeeker);
        modelBuilder.Entity<Employer>(ConfigureEmployer);
        modelBuilder.Entity<User>(ConfigureUser);
        modelBuilder.Entity<Role>(ConfigureRole);

        //set user role to admin
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
        {
            RoleId = 1,
            UserId = 1
        });
    }

    private void ConfigureJobOpening(EntityTypeBuilder<JobOpening> builder)
    {
        builder.HasKey(j => j.JobOpeningId);

        builder.Property(j => j.Title).IsRequired();
        builder.Property(j => j.Description).IsRequired();
        /*builder.Property(j => j.Location).IsRequired();
        builder.Property(j => j.Salary).IsRequired();*/
        builder.Property(j => j.IsActive).IsRequired();
        builder.Property(j => j.CreationDate).IsRequired();
        builder.Property(jo => jo.Salary)
              .HasColumnType("decimal(18, 2)");

        builder.HasOne(jo => jo.Employer)
                .WithMany(e => e.JobOpenings)
                .HasForeignKey(jo => jo.EmployerId).IsRequired(false);

        builder.HasOne(jo => jo.JobType)
            .WithMany(jt => jt.JobOpenings)
            .HasForeignKey(jo => jo.JobTypeId).IsRequired(false);

        builder.HasOne(jo => jo.JobDetails)
            .WithOne(jd => jd.JobOpening)
            .HasForeignKey<JobOpening>(jd => jd.JobDetailsId).IsRequired(false);

        builder.HasMany(jo => jo.OpeningCategories)
            .WithOne(oc => oc.JobOpening)
            .HasForeignKey(oc => oc.JobOpeningId).IsRequired(false);

        /*"opeming"*/
        builder.HasMany(jo => jo.JobApplications)
            .WithOne(ja => ja.JobOpening)
            .HasForeignKey(ja => ja.JobOpeningId).IsRequired(false);
       
    }


    private void ConfigureJobDetails(EntityTypeBuilder<JobDetails> builder)
    {
        builder.HasKey(jd => jd.JobDetailsId);

        /*builder.Property(jd => jd.EmploymentType).IsRequired();
        builder.Property(jd => jd.ApplicationDeadline).IsRequired();
        builder.Property(jd => jd.RequiredExperience).IsRequired();
        builder.Property(jd => jd.Qualifications).IsRequired();
        builder.Property(jd => jd.Responsibilities).IsRequired();
        builder.Property(jd => jd.CompanyCulture).IsRequired();
        builder.Property(jd => jd.ApplicationInstructions).IsRequired();
        builder.Property(jd => jd.NumberOfOpenings).IsRequired();
        builder.Property(jd => jd.IsFullTime).IsRequired();
        builder.Property(jd => jd.IsRemote).IsRequired();*/



        builder.HasOne(jd => jd.JobOpening)
            .WithOne(jo => jo.JobDetails)
            .HasForeignKey<JobDetails>(jd => jd.JobOpeningId);
    }

    private void ConfigureJobCategory(EntityTypeBuilder<JobCategory> builder)
    {
        builder.HasKey(jc => jc.JobCategoryId);

        builder.Property(jc => jc.Name).IsRequired();

        builder.HasMany(jc => jc.OpeningCategories)
            .WithOne(oc => oc.JobCategory)
            .HasForeignKey(oc => oc.JobCategoryId);

        
        builder.HasData(new JobCategory
        {
            Name = "Sales",
            JobCategoryId = 1
        });
    }

    private void ConfigureJobType(EntityTypeBuilder<JobType> builder)
    {
        builder.HasKey(jt => jt.JobTypeId);

        builder.Property(jt => jt.Name).IsRequired();

        builder.HasMany(jt => jt.JobOpenings)
            .WithOne(jo => jo.JobType)
            .HasForeignKey(jo => jo.JobTypeId);

        builder.HasData(new JobType
        {
            Name = "Full-Time",
            JobTypeId = 1
        });
    }

    private void ConfigureJobApplication(EntityTypeBuilder<JobApplication> builder)
    {
        builder.HasKey(ja => ja.ApplicationId);

        builder.Property(ja => ja.ApplicationDate).IsRequired();

        builder.Property(ja => ja.CoverLetter).HasMaxLength(1000);

        builder.HasOne(ja => ja.JobSeeker)
            .WithMany(js => js.JobApplications)
            .HasForeignKey(ja => ja.JobSeekerId);


       /* builder.HasOne(ja => ja.JobOpening)
            .WithMany(jo => jo.JobApplications)
            .HasForeignKey(ja => ja.JobOpeningId);*/

       
    }

    private void ConfigureOpeningCategory(EntityTypeBuilder<OpeningCategory> builder)
    {
        builder.HasKey(oc => new { oc.JobOpeningId, oc.JobCategoryId });

        builder.HasOne(oc => oc.JobOpening)
            .WithMany(jo => jo.OpeningCategories)
            .HasForeignKey(oc => oc.JobOpeningId);

        builder.HasOne(oc => oc.JobCategory)
            .WithMany(jc => jc.OpeningCategories)
            .HasForeignKey(oc => oc.JobCategoryId);
    }

    private void ConfigureJobSeeker(EntityTypeBuilder<JobSeeker> builder)
    {
        builder.HasKey(js => js.JobSeekerId);
        builder.Property(js => js.ResumeUrl)
            .HasMaxLength(200); 

        builder.Property(js => js.Skills)
            .HasMaxLength(500); 

        builder.Property(js => js.Education)
            .HasMaxLength(300);

        builder.Property(u => u.FirstName)
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .HasMaxLength(50);

        builder.HasOne(js => js.User)
            .WithOne(u => u.JobSeekerProfile)
            .HasForeignKey<JobSeeker>(js => js.JobSeekerId);

        builder.HasMany(js => js.JobApplications)
            .WithOne(ja => ja.JobSeeker)
            .HasForeignKey(ja => ja.JobSeekerId);
    }

    private void ConfigureEmployer(EntityTypeBuilder<Employer> builder)
    {
        builder.HasKey(e => e.EmployerId);

        builder.Property(e => e.CompanyName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Website)
            .HasMaxLength(200); 

        builder.Property(e => e.ContactNumber)
            .HasMaxLength(20);

        builder.Property(e => e.Address)
            .HasMaxLength(200);


        builder.HasOne(e => e.User)
            .WithOne(u => u.EmployerProfile)
            .HasForeignKey<Employer>(e => e.EmployerId);

        builder.HasMany(e => e.JobOpenings)
            .WithOne(jo => jo.Employer)
            .HasForeignKey(jo => jo.EmployerId);
    }

    private void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        /*
        builder.Property(u => u.Username)
            .HasMaxLength(50)
            .IsRequired();*/

        /*builder.Property(u => u.Password)
            .HasMaxLength(100)
            .IsRequired();*/

        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(u => u.EmployerProfile)
            .WithOne(e => e.User)
            .HasForeignKey<User>(e => e.EmployerId);

        builder.HasOne(u => u.JobSeekerProfile)
            .WithOne(js => js.User)
            .HasForeignKey<User>(js => js.JobSeekerId);


        int ADMIN_ID = 1;

        //create user
        var appUser = new User
        {
            Id = ADMIN_ID,
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Role="admin",
            SecurityStamp = Guid.NewGuid().ToString()
    };

        //set user password
        PasswordHasher<User> ph = new PasswordHasher<User>();
        appUser.PasswordHash = ph.HashPassword(appUser, "adminA1.");

        //seed user
        builder.HasData(appUser);
}

    private void ConfigureRole(EntityTypeBuilder<Role> builder)
    {
       
        int ROLE_ID = 1;

        //seed admin role
        builder.HasData(new Role
        {
            Name = "admin",
            NormalizedName = "ADMIN",
            Id = ROLE_ID
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
