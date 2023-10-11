using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        }

        public virtual DbSet<AssessmentMethod> AssessmentMethod { get; set; }
        public virtual DbSet<AssessmentType> AssessmentType { get; set; }
        public virtual DbSet<Batch> Batche { get; set; }
        public virtual DbSet<ClassSessionType> ClassSessionType { get; set; }
        public virtual DbSet<CLO> CLO { get; set; }
        public virtual DbSet<Combo> Combo { get; set; }
        public virtual DbSet<ComboCurriculum> ComboCurriculum { get; set; }
        public virtual DbSet<ComboSubject> ComboSubject { get; set; }
        public virtual DbSet<Curriculum> Curriculum { get; set; }
        public virtual DbSet<CurriculumSubject> CurriculumSubject { get; set; }
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
        public virtual DbSet<SpecializationSubject> SpecializationSubject { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<Syllabus> Syllabus { get; set; }
        public virtual DbSet<SessionCLO> SessionCLO { get; set; }
        public virtual DbSet<User> User { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create primary key
            modelBuilder.Entity<ComboSubject>()
               .HasKey(ba => new { ba.combo_id, ba.subject_id });

            modelBuilder.Entity<CurriculumSubject>()
               .HasKey(ba => new { ba.curriculum_id, ba.subject_id });

            modelBuilder.Entity<GradingCLO>()
               .HasKey(ba => new { ba.grading_id, ba.CLO_id });

            modelBuilder.Entity<PLOMapping>()
               .HasKey(ba => new { ba.PLO_id, ba.subject_id });

            modelBuilder.Entity<SemesterPlan>()
               .HasKey(ba => new { ba.semester_id, ba.curriculum_id });

            modelBuilder.Entity<SpecializationSubject>()
               .HasKey(ba => new { ba.specialization_id, ba.subject_id });

            modelBuilder.Entity<SessionCLO>()
               .HasKey(ba => new { ba.CLO_id, ba.session_id });

            modelBuilder.Entity<PreRequisite>()
               .HasKey(ba => new { ba.subject_id, ba.pre_subject_id });

            modelBuilder.Entity<ComboCurriculum>()
              .HasKey(ba => new { ba.combo_id, ba.curriculum_id });

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

            modelBuilder.Entity<ComboCurriculum>()
                .HasOne(x => x.Curriculum)
                .WithMany(y => y.ComboCurriculum)
                .HasForeignKey(x => x.curriculum_id)
                .OnDelete(DeleteBehavior.ClientSetNull);


            //create new Data in table
            modelBuilder.Entity<Role>().HasData(
                new Role { role_id = 1, role_name = "Dispatcher" },
                new Role { role_id = 2, role_name = "Manager" },
                new Role { role_id = 3, role_name = "Admin" }
                );

            modelBuilder.Entity<User>().HasData(

                new User { user_id = 1, full_name = "Chu Quang Quan", role_id = 1, user_email = "chuquan2k1@gmail.com",  user_name = "QuanCQ",  is_active = true },
                new User { user_id = 2, full_name = "Nguyen Thi Thu", role_id = 2, user_email = "nguyenthu120801@gmail.com", user_name = "ThuNT", is_active = true }
                , new User { user_id = 3, full_name = "Nguyen Phong Hao", role_id = 1, user_email = "haotest@gmail.com", user_name = "admin", is_active = true }

                );

            modelBuilder.Entity<Batch>().HasData(
                new Batch { batch_id = 1, batch_name = "K19.3" },
                new Batch { batch_id = 2, batch_name = "K18" },
                new Batch { batch_id = 3, batch_name = "K17" }
                );

            modelBuilder.Entity<Semester>().HasData(
                new Semester { semester_id = 1, semester_name = "Fall", semester_start_date = DateTime.Parse("05/09/2023"),semester_end_date = DateTime.Now , school_year = 2023 },
                new Semester { semester_id = 2, semester_name = "Spring", semester_start_date = DateTime.Parse("03/01/2023"),semester_end_date = DateTime.Parse("12/04/2023") , school_year = 2023 }
                );

            modelBuilder.Entity<Major>().HasData(
                new Major { major_id = 1, major_code = "GD", major_name = "Thiết kế đồ họa", major_english_name = "Graphic Design",is_active = true },
                new Major { major_id = 2, major_code = "IT", major_name = "Công nghệ thông tin", major_english_name = "Information technology", is_active = true },
                new Major { major_id = 3, major_code = "BA", major_name = "Quản trị kinh doanh", major_english_name = "Business Administration", is_active = true },
                new Major { major_id = 4, major_code = "AE", major_name = "Kỹ thuật tự động hóa", major_english_name = "Automation Engineering", is_active = true }

                );

            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { specialization_id = 1, major_id = 1, specialization_code = "IED", specialization_name = "Thiết kế nội và ngoại thất", specialization_english_name = "Interior and exterior design", semester_id = 1,is_active = true },
                new Specialization { specialization_id = 2, major_id = 1, specialization_code = "FMA", specialization_name = "Dựng phim và quảng cáo", specialization_english_name = "Filmmaking and advertising", semester_id = 1, is_active = true },
                new Specialization { specialization_id = 3, major_id = 1, specialization_code = "IED", specialization_name = "Thiết kế nội và ngoại thất", specialization_english_name = "Interior and exterior design", semester_id = 2, is_active = true      },
                new Specialization { specialization_id = 4, major_id = 2, specialization_code = "SE", specialization_name = "kĩ thuật phần mềm", specialization_english_name = "Software Engineering", semester_id = 2, is_active = true },
                new Specialization { specialization_id = 5, major_id = 2, specialization_code = "WP", specialization_name = "lập trình web", specialization_english_name = "web programming", semester_id = 2, is_active = true },
                new Specialization { specialization_id = 6, major_id = 2, specialization_code = "GP", specialization_name = "lập trình game", specialization_english_name = "game programming", semester_id = 1, is_active = true }
                );



            modelBuilder.Entity<Curriculum>().HasData(  
                new Curriculum { curriculum_id = 1, curriculum_code = "GD", curriculum_name = "Thiết kế đồ họa", english_curriculum_name = "Graphic Design", specialization_id = 1, batch_id = 1, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", curriculum_status = 1},
                new Curriculum { curriculum_id = 2, curriculum_code = "SE", curriculum_name = "kĩ sư phần mềm", english_curriculum_name = "Software Engineering", specialization_id = 4, batch_id = 2, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", curriculum_status = 0 }

                );

            modelBuilder.Entity<LearningMethod>().HasData(
               new LearningMethod { learning_method_id = 1, learning_method_name = "Online Learing", learning_method_description = "" }
                );

            modelBuilder.Entity<AssessmentType>().HasData(
                new AssessmentType { assessment_type_id = 1, assessment_type_name = "Online" }
                );

            modelBuilder.Entity<AssessmentMethod>().HasData(
                new AssessmentMethod { assessment_method_id = 1, assessment_method_component = "ABC", assessment_type_id = 1 }
                );

            modelBuilder.Entity<Subject>().HasData(
                new Subject { subject_id = 1, subject_code = "SEP490", subject_name = "Đồ án", assessment_method_id = 1, learning_method_id = 1, english_subject_name = "Project Capstone", credit = 10, total_time = 70, total_time_class = 40, exam_total = 3 ,is_active = true }
                );

            modelBuilder.Entity<CurriculumSubject>().HasData(
                new CurriculumSubject { curriculum_id = 1, subject_id = 1, term_no = 3, option = false }
                );

        }
    }
}
