using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CMSDbContext : DbContext
    {
        public CMSDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                               .AddJsonFile("appsettings.json", true, true)
                                               .Build();
            string? con = connectionString.GetConnectionString("CMSDb");
            optionsBuilder.UseSqlServer(con);
            //optionsBuilder.UseMySql(con, ServerVersion.AutoDetect(con));
        }

        public virtual DbSet<AssessmentMethod> AssessmentMethod { get; set; }
        public virtual DbSet<AssessmentType> AssessmentType { get; set; }
        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<ClassSessionType> ClassSessionType { get; set; }
        public virtual DbSet<CLO> CLO { get; set; }
        public virtual DbSet<Combo> Combo { get; set; }
        public virtual DbSet<Curriculum> Curriculum { get; set; }
        public virtual DbSet<CurriculumBatch> CurriculumBatch { get; set; }
        public virtual DbSet<CurriculumSubject> CurriculumSubject { get; set; }
        public virtual DbSet<DegreeLevel> DegreeLevel { get; set; }
        public virtual DbSet<GradingCLO> GradingCLO { get; set; }
        public virtual DbSet<GradingStruture> GradingStruture { get; set; }
        public virtual DbSet<LearningMethod> LearningMethod { get; set; }
        public virtual DbSet<LearningResource> LearningResource { get; set; }
        public virtual DbSet<Major> Major { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<PLOMapping> PLOMapping { get; set; }
        public virtual DbSet<PLOs> PLOs { get; set; }
        public virtual DbSet<PreRequisite> PreRequisite { get; set; }
        public virtual DbSet<PreRequisiteType> PreRequisiteType { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Quiz> Quiz { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Semester> Semester { get; set; }
        public virtual DbSet<SemesterPlan> SemesterPlan { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Specialization> Specialization { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<Syllabus> Syllabus { get; set; }
        public virtual DbSet<SessionCLO> SessionCLO { get; set; }
        public virtual DbSet<User> User { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create primary key
            modelBuilder.Entity<CurriculumSubject>()
               .HasKey(ba => new { ba.curriculum_id, ba.subject_id });

            modelBuilder.Entity<GradingCLO>()
               .HasKey(ba => new { ba.grading_id, ba.CLO_id });

            modelBuilder.Entity<PLOMapping>()
               .HasKey(ba => new { ba.PLO_id, ba.subject_id });

            modelBuilder.Entity<SemesterPlan>()
               .HasKey(ba => new { ba.semester_id, ba.curriculum_id, ba.term_no });

            modelBuilder.Entity<SessionCLO>()
               .HasKey(ba => new { ba.CLO_id, ba.session_id });

            modelBuilder.Entity<PreRequisite>()
               .HasKey(ba => new { ba.subject_id, ba.pre_subject_id });

            modelBuilder.Entity<CurriculumBatch>()
              .HasKey(ba => new { ba.curriculum_id, ba.batch_id });

            //
            modelBuilder.Entity<GradingStruture>()
                .Property(g => g.grading_weight)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Syllabus>()
                .Property(s => s.min_GPA_to_pass)
                .HasColumnType("decimal(18, 2)");

            //
            modelBuilder.Entity<Combo>()
                .HasOne(x => x.Specialization)
                .WithMany(y => y.Combos)
                .HasForeignKey(x => x.specialization_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<GradingStruture>()
                .HasOne(x => x.Syllabus)
                .WithMany(y => y.Gradings)
                .HasForeignKey(x => x.syllabus_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<GradingCLO>()
                .HasOne(x => x.GradingStruture)
                .WithMany(y => y.GradingCLOs)
                .HasForeignKey(x => x.grading_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<SessionCLO>()
                .HasOne(x => x.Session)
                .WithMany(y => y.SessionCLO)
                .HasForeignKey(x => x.session_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Subject>()
                .HasMany(x => x.PreRequisite)
                .WithOne(y => y.Subject)
                .HasForeignKey(x => x.subject_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Specialization>()
                .HasOne(x => x.Semester)
                .WithMany(y => y.Specializations)
                .HasForeignKey(x => x.semester_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<SemesterPlan>()
                .HasOne(x => x.Semester)
                .WithMany(y => y.Semesters)
                .HasForeignKey(x => x.semester_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<CurriculumBatch>()
                .HasOne(x => x.Curriculum)
                .WithMany(y => y.CurriculumBatchs)
                .HasForeignKey(x => x.curriculum_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
