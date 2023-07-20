using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkWave.DBModels;

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
            .HasForeignKey<JobDetails>(jd => jd.JobDetailsId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(jo => jo.OpeningCategories)
            .WithOne(oc => oc.JobOpening)
            .HasForeignKey(oc => oc.JobOpeningId).IsRequired(false);

        builder.HasMany(jo => jo.JobApplications)
            .WithOne(ja => ja.JobOpening)
            .HasForeignKey(ja => ja.JobOpeningId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
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
            .HasForeignKey<JobDetails>(jd => jd.JobOpeningId).OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureJobCategory(EntityTypeBuilder<JobCategory> builder)
    {
        builder.HasKey(jc => jc.JobCategoryId);

        builder.Property(jc => jc.Name).IsRequired();

        builder.HasMany(jc => jc.OpeningCategories)
            .WithOne(oc => oc.JobCategory)
            .HasForeignKey(oc => oc.JobCategoryId);
    }

    private void ConfigureJobType(EntityTypeBuilder<JobType> builder)
    {
        builder.HasKey(jt => jt.JobTypeId);

        builder.Property(jt => jt.Name).IsRequired();

        //TODO is correct?
        builder.HasMany(jt => jt.JobOpenings)
            .WithOne(jo => jo.JobType)
            .HasForeignKey(jo => jo.JobTypeId);
    }

    private void ConfigureJobApplication(EntityTypeBuilder<JobApplication> builder)
    {
        builder.HasKey(ja => ja.ApplicationId);

        builder.Property(ja => ja.ApplicationDate).IsRequired();

        builder.Property(ja => ja.CoverLetter).HasMaxLength(1000);

        builder.HasOne(ja => ja.JobSeeker)
            .WithMany(js => js.JobApplications)
            .HasForeignKey(ja => ja.JobSeekerId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ja => ja.JobOpening)
            .WithMany(jo => jo.JobApplications)
            .HasForeignKey(ja => ja.JobOpeningId).OnDelete(DeleteBehavior.Restrict);
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
            .HasForeignKey(ja => ja.JobSeekerId).OnDelete(DeleteBehavior.Restrict);
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
    }

    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
