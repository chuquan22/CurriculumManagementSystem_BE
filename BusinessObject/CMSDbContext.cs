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
        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<ClassSessionType> ClassSessionType { get; set; }
        public virtual DbSet<CLO> CLO { get; set; }
        public virtual DbSet<Combo> Combo { get; set; }
        public virtual DbSet<ComboCurriculum> ComboCurriculum { get; set; }
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
        public virtual DbSet<SemesterBatch> SemesterBatch { get; set; }
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

            modelBuilder.Entity<SemesterBatch>()
                .HasOne(x => x.Semester)
                .WithMany(y => y.SemesterBatches)
                .HasForeignKey(x => x.semester_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Semester>()
        .HasMany(s => s.SemesterBatches)
        .WithOne(sb => sb.Semester)
        .HasForeignKey(sb => sb.semester_id);

            //create new Data in table
            modelBuilder.Entity<Role>().HasData(
                new Role { role_id = 1, role_name = "Dispatcher" },
                new Role { role_id = 2, role_name = "Manager" },
                new Role { role_id = 3, role_name = "Admin" }
                );

            modelBuilder.Entity<User>().HasData(

                new User { user_id = 1, full_name = "Chu Quang Quan", role_id = 1, user_email = "chuquan2k1@gmail.com", user_password = "quan123", user_name = "QuanCQ", is_active = true },
                new User { user_id = 2, full_name = "Nguyen Thi Thu", role_id = 2, user_email = "nguyenthu120801@gmail.com", user_password = "quan123", user_name = "ThuNT", is_active = true },
                new User { user_id = 3, full_name = "Nguyen Phong Hao", role_id = 1, user_email = "haotest@gmail.com", user_password = "quan123", user_name = "admin", is_active = true }

                );

            modelBuilder.Entity<Batch>().HasData(
                new Batch { batch_id = 1, batch_name = "18.1" },
                new Batch { batch_id = 2, batch_name = "18.2" },
                new Batch { batch_id = 3, batch_name = "18.3" },
                new Batch { batch_id = 4, batch_name = "19.1" },
                new Batch { batch_id = 5, batch_name = "19.2" },
                new Batch { batch_id = 6, batch_name = "19.3" }
                );

            modelBuilder.Entity<Semester>().HasData(
                new Semester { semester_id = 1, semester_name = "Fall", semester_start_date = DateTime.Parse("09/09/2023"), semester_end_date = DateTime.Parse("12/12/2023"), school_year = 2023, batch_id = 6 },
                new Semester { semester_id = 2, semester_name = "Summer", semester_start_date = DateTime.Parse("05/05/2023"), semester_end_date = DateTime.Parse("08/20/2023"), school_year = 2023, batch_id = 5 },
                new Semester { semester_id = 3, semester_name = "Spring", semester_start_date = DateTime.Parse("01/03/2023"), semester_end_date = DateTime.Parse("04/14/2023"), school_year = 2023, batch_id = 4 },
                new Semester { semester_id = 4, semester_name = "Fall", semester_start_date = DateTime.Parse("09/09/2022"), semester_end_date = DateTime.Parse("12/12/2022"), school_year = 2022, batch_id = 3 },
                new Semester { semester_id = 5, semester_name = "Summer", semester_start_date = DateTime.Parse("05/05/2022"), semester_end_date = DateTime.Parse("08/08/2022"), school_year = 2022, batch_id = 2 },
                new Semester { semester_id = 6, semester_name = "Spring", semester_start_date = DateTime.Parse("01/01/2022"), semester_end_date = DateTime.Parse("04/04/2022"), school_year = 2022, batch_id = 1 }
                );

            modelBuilder.Entity<Major>().HasData(
                new Major { major_id = 1, major_code = "6210402", major_name = "Thiết kế đồ họa", major_english_name = "Graphic Design", is_active = true },
                new Major { major_id = 2, major_code = "6215642", major_name = "Công nghệ thông tin", major_english_name = "Information technology", is_active = true },
                new Major { major_id = 3, major_code = "6516427", major_name = "Quản trị kinh doanh", major_english_name = "Business Administration", is_active = true },
                new Major { major_id = 4, major_code = "6318672", major_name = "Kỹ thuật tự động hóa", major_english_name = "Automation Engineering", is_active = true }

                );

            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { specialization_id = 1, major_id = 1, specialization_code = "6216432", specialization_name = "Thiết kế nội và ngoại thất", specialization_english_name = "Interior and Exterior Design", semester_id = 1, is_active = true },
                new Specialization { specialization_id = 2, major_id = 1, specialization_code = "6215463", specialization_name = "Dựng phim và quảng cáo", specialization_english_name = "Film Making and Advertising", semester_id = 1, is_active = true },
                new Specialization { specialization_id = 3, major_id = 1, specialization_code = "6526432", specialization_name = "Thiết kế nội và ngoại thất", specialization_english_name = "Interior and Exterior Design", semester_id = 2, is_active = true },
                new Specialization { specialization_id = 4, major_id = 2, specialization_code = "6764432", specialization_name = "kĩ thuật phần mềm", specialization_english_name = "Software Engineering", semester_id = 2, is_active = true },
                new Specialization { specialization_id = 5, major_id = 2, specialization_code = "6266132", specialization_name = "lập trình web", specialization_english_name = "Web Programming", semester_id = 2, is_active = true },
                new Specialization { specialization_id = 6, major_id = 2, specialization_code = "6276545", specialization_name = "lập trình game", specialization_english_name = "Game Programming", semester_id = 1, is_active = true }
                );

            modelBuilder.Entity<Curriculum>().HasData(
                new Curriculum { curriculum_id = 1, curriculum_code = "GD-IED-CD-18.1", curriculum_name = "Thiết kế đồ họa", english_curriculum_name = "Graphic Design", total_semester = 7, specialization_id = 1, batch_id = 1, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "Associate Degree", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 2, curriculum_code = "GD-IED-IC-19.1", curriculum_name = "Thiết kế mĩ thuật số", english_curriculum_name = "Graphic Design", total_semester = 7, specialization_id = 1, batch_id = 4, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "International associate degree", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 3, curriculum_code = "SE-SE-IC-18.3", curriculum_name = "kĩ sư phần mềm", english_curriculum_name = "Software Engineering", total_semester = 7, specialization_id = 4, batch_id = 3, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "International associate degree", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 4, curriculum_code = "SE-SE-CD-18.2", curriculum_name = "kĩ thuật phần mềm", english_curriculum_name = "Software Engineering", total_semester = 7, specialization_id = 4, batch_id = 2, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "Associate Degree ", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 5, curriculum_code = "CM-FMA-TC-18.3", curriculum_name = "quản lí học liệu", english_curriculum_name = "Curriculum Management", total_semester = 7, specialization_id = 2, batch_id = 3, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "Vocational Secondary", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 6, curriculum_code = "SS-IED-TC-18.3", curriculum_name = "kĩ năng mềm", english_curriculum_name = "Soft Skill", total_semester = 7, specialization_id = 1, batch_id = 3, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "Vocational Secondary", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 7, curriculum_code = "SWP-WP-TC-18.2", curriculum_name = "kĩ năng lập trình web", english_curriculum_name = "Skill Web Program", total_semester = 7, specialization_id = 5, batch_id = 2, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "Vocational Secondary", Formality = "formal education", is_active = false },
                new Curriculum { curriculum_id = 8, curriculum_code = "SS-IED-TC-18.3", curriculum_name = "kĩ năng mềm", english_curriculum_name = "Soft Skill", total_semester = 7, specialization_id = 1, batch_id = 3, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", degree_level = "Vocational Secondary", Formality = "formal education", is_active = true }

                );

            modelBuilder.Entity<LearningMethod>().HasData(
               new LearningMethod { learning_method_id = 1, learning_method_name = "Online Learing", learning_method_description = "" },
               new LearningMethod { learning_method_id = 2, learning_method_name = "Balence", learning_method_description = "" }
                );

            modelBuilder.Entity<LearningResource>().HasData(
             new LearningResource { learning_resource_id = 1, learning_resource_type = "Lab"},
             new LearningResource { learning_resource_id = 2, learning_resource_type = "Assessment"}
              );

            modelBuilder.Entity<AssessmentType>().HasData(

                new AssessmentType { assessment_type_id = 1, assessment_type_name = "Online" },
                new AssessmentType { assessment_type_id = 2, assessment_type_name = "ORIT" },
                 new AssessmentType { assessment_type_id = 3, assessment_type_name = "On-going" },
                  new AssessmentType { assessment_type_id = 4, assessment_type_name = "Final Exam" }
                );

            modelBuilder.Entity<AssessmentMethod>().HasData(
                new AssessmentMethod { assessment_method_id = 1, assessment_method_component = "Lab", assessment_type_id = 3 },
                new AssessmentMethod { assessment_method_id = 2, assessment_method_component = "Quiz", assessment_type_id = 3 },
                new AssessmentMethod { assessment_method_id = 3, assessment_method_component = "Assignment", assessment_type_id = 3 },
                  new AssessmentMethod { assessment_method_id = 4, assessment_method_component = "Bài học online", assessment_type_id = 3 },
                   new AssessmentMethod { assessment_method_id = 5, assessment_method_component = "Lab", assessment_type_id = 3 },
                    new AssessmentMethod { assessment_method_id = 6, assessment_method_component = "Bảo vệ assignment", assessment_type_id = 4 },
                    new AssessmentMethod { assessment_method_id = 7, assessment_method_component = "Quiz", assessment_type_id = 3 },
                    new AssessmentMethod { assessment_method_id = 8, assessment_method_component = "Đánh giá Assignment GĐ 1", assessment_type_id = 3 }

                    );



            modelBuilder.Entity<Subject>().HasData(
                new Subject { subject_id = 1, subject_code = "SEP490", subject_name = "Đồ án", assessment_method_id = 1, learning_method_id = 1, english_subject_name = "Project Capstone", credit = 3, total_time = 70, total_time_class = 40, exam_total = 3, is_active = true },
                new Subject { subject_id = 2, subject_code = "MLN131", subject_name = "Triết học Mac-Lenin", assessment_method_id = 2, learning_method_id = 1, english_subject_name = "Mac-Lenin philosophy", credit = 3, total_time = 70, total_time_class = 40, exam_total = 5, is_active = true },
                new Subject { subject_id = 3, subject_code = "SSG104", subject_name = "Kĩ năng trong làm việc nhóm", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Soft Skill Group", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 4, subject_code = "PRN231", subject_name = "lập trình web api asp.Net", assessment_method_id = 2, learning_method_id = 2, english_subject_name = "Web api using asp.Net", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 5, subject_code = "PRU211", subject_name = "lập trình game cơ bản", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Basic Game Programing", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 6, subject_code = "MAT101", subject_name = "Toán học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Mathematics", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 7, subject_code = "PHY101", subject_name = "Vật lý", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Physics", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 8, subject_code = "CHE101", subject_name = "Hóa học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Chemistry", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 9, subject_code = "BIO101", subject_name = "Sinh học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Biology", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 10, subject_code = "LING101", subject_name = "Ngôn ngữ học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Linguistics", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 11, subject_code = "ENG101", subject_name = "Tiếng Anh và văn học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English and Literature", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 12, subject_code = "HIS101", subject_name = "Lịch sử", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "History", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 13, subject_code = "POL101", subject_name = "Khoa học chính trị", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Political Science", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 14, subject_code = "SOC101", subject_name = "Khoa học xã hội", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Social Science", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 15, subject_code = "ECO101", subject_name = "Kinh tế học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Economics", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 16, subject_code = "BUS101", subject_name = "Quản trị kinh doanh", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Business Management", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 17, subject_code = "FIN101", subject_name = "Tài chính", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Finance", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 18, subject_code = "IT101", subject_name = "Hệ thống thông tin", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Information Systems", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 19, subject_code = "CS101", subject_name = "Công nghệ thông tin", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Computer Science", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 20, subject_code = "MECH101", subject_name = "Cơ khí học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Mechanical Engineering", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 21, subject_code = "ELEC101", subject_name = "Điện tử và điện lạnh", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Electronics and Electrical Engineering", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 22, subject_code = "ARCH101", subject_name = "Kiến trúc", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Architecture", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 23, subject_code = "ART101", subject_name = "Nghệ thuật và thiết kế", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Art and Design", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 24, subject_code = "MUSIC101", subject_name = "Âm nhạc và nghệ thuật biểu diễn", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Music and Performing Arts", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 25, subject_code = "FOREIGN101", subject_name = "Ngôn ngữ nước ngoài", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Foreign Language", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 26, subject_code = "GEO101", subject_name = "Địa lý", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Geography", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 27, subject_code = "ENV101", subject_name = "Môi trường học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Environmental Science", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 28, subject_code = "PSY101", subject_name = "Tâm lý học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Psychology", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 29, subject_code = "ANTH101", subject_name = "Antropology", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Antropology", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 30, subject_code = "ECO102", subject_name = "Kinh tế học 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Economics 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 31, subject_code = "BUS102", subject_name = "Quản trị kinh doanh 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Business Management 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 32, subject_code = "FIN102", subject_name = "Tài chính 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Finance 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 33, subject_code = "IT102", subject_name = "Hệ thống thông tin 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Information Systems 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 34, subject_code = "CS102", subject_name = "Công nghệ thông tin 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Computer Science 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 35, subject_code = "MECH102", subject_name = "Cơ khí học 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Mechanical Engineering 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 36, subject_code = "ELEC102", subject_name = "Điện tử và điện lạnh 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Electronics and Electrical Engineering 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 37, subject_code = "ARCH102", subject_name = "Kiến trúc 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Architecture 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 38, subject_code = "ART102", subject_name = "Nghệ thuật và thiết kế 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Art and Design 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 39, subject_code = "MUSIC102", subject_name = "Âm nhạc và nghệ thuật biểu diễn 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Music and Performing Arts 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 40, subject_code = "FOREIGN102", subject_name = "Ngôn ngữ nước ngoài 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Foreign Language 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 41, subject_code = "PHILO101", subject_name = "Triết học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Philosophy", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 42, subject_code = "PSYCH102", subject_name = "Tâm lý học 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Psychology 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 43, subject_code = "LING102", subject_name = "Ngôn ngữ học 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Linguistics 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 44, subject_code = "ENG102", subject_name = "Tiếng Anh và văn học 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English and Literature 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 45, subject_code = "GEO102", subject_name = "Địa lý 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Geography 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 46, subject_code = "ENV102", subject_name = "Môi trường học 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Environmental Science 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 47, subject_code = "ANTH102", subject_name = "Antropology 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Antropology 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 48, subject_code = "ECO103", subject_name = "Kinh tế học 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Economics 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 49, subject_code = "BUS103", subject_name = "Quản trị kinh doanh 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Business Management 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 50, subject_code = "FIN103", subject_name = "Tài chính 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Finance 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 51, subject_code = "IT103", subject_name = "Hệ thống thông tin 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Information Systems 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 52, subject_code = "CS103", subject_name = "Công nghệ thông tin 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Computer Science 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 53, subject_code = "MECH103", subject_name = "Cơ khí học 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Mechanical Engineering 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 54, subject_code = "ELEC103", subject_name = "Điện tử và điện lạnh 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Electronics and Electrical Engineering 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 55, subject_code = "ARCH103", subject_name = "Kiến trúc 3", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Architecture 3", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 56, subject_code = "MUL1013", subject_name = "Thiết kế hình ảnh với Photoshop", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Image Design using Photoshop", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true }

                );

            modelBuilder.Entity<CurriculumSubject>().HasData(
                new CurriculumSubject { curriculum_id = 1, subject_id = 1, term_no = 3, combo_id = 1, subject_group = "General Subject", option = false },
                new CurriculumSubject { curriculum_id = 1, subject_id = 3, term_no = 3, combo_id = 2, subject_group = "Option Subject", option = true },
                new CurriculumSubject { curriculum_id = 2, subject_id = 4, term_no = 3, combo_id = 0, subject_group = "Specialization Subject", option = false },
                new CurriculumSubject { curriculum_id = 1, subject_id = 5, term_no = 2, combo_id = 0, subject_group = "Basic Subject", option = false },
                new CurriculumSubject { curriculum_id = 1, subject_id = 2, term_no = 1, combo_id = 3, subject_group = "Option Subject", option = true }
                );

            modelBuilder.Entity<Combo>().HasData(
                new Combo { combo_id = 1, combo_code = ".NET", combo_name = "Lập trình C#", combo_english_name = "C# Programing",  specialization_id = 4, is_active = true },
                new Combo { combo_id = 2, combo_code = "JS", combo_name = "kĩ sư Nhật Bản", combo_english_name = "Japan Software", specialization_id = 3, is_active = true },
                new Combo { combo_id = 3, combo_code = "KS", combo_name = "kĩ sư Hàn Quốc", combo_english_name = "Korea Software", specialization_id = 2, is_active = false },
                new Combo { combo_id = 4, combo_code = "NodeJS", combo_name = "Lập trình NodeJS", combo_english_name = "Web api using NodeJS", specialization_id = 1, is_active = true }
                );

            modelBuilder.Entity<ClassSessionType>().HasData(
                new ClassSessionType { class_session_type_id = 1, class_session_type_name = "Online"},
                 new ClassSessionType { class_session_type_id = 2, class_session_type_name = "Offline" },
                  new ClassSessionType { class_session_type_id = 3, class_session_type_name = "ORIT" }
                );

            
            modelBuilder.Entity<PLOs>().HasData(
               new PLOs { PLO_id = 1, PLO_name = "PLO01", PLO_description = "Thiết kế xử lý hình ảnh, xây dựng các sản phẩm đồ họa 2D", curriculum_id = 1 },
               new PLOs { PLO_id = 2, PLO_name = "PLO02", PLO_description = "Thiết kế theo các chủ đề: xây dựng thương hiệu, ấn phẩm quảng cáo, bao bì", curriculum_id = 1 },
               new PLOs { PLO_id = 3, PLO_name = "PLO03", PLO_description = "Biên tập, kịch bản và xử lý kỹ xảo phim, phim quảng cáo, phim quảng cáo 3D", curriculum_id = 1 },
               new PLOs { PLO_id = 4, PLO_name = "PLO04", PLO_description = "Thiết kếm xây dưng các sản phẩm đồ họa nội ngoại thất 2D&3D hoặc các sản phẩm đồ họa 3D", curriculum_id = 1 },
               new PLOs { PLO_id = 5, PLO_name = "PLO05", PLO_description = "Kiến thức về đường, hình, khối và một số vấn đề mỹ thuật liên quan; kiến thức cơ bản về đồ họa; kiến thức cơ sở về mỹ thuật, thẩm mỹ; vật liệu,...", curriculum_id = 1 },
               new PLOs { PLO_id = 6, PLO_name = "PLO06", PLO_description = "Giao tiếp, tìm hiểu, nắm bắt nhu cầu của khách hàng, tư vấn cho khách hàng, làm được sản phẩm theo yêu cầu của khách hàng", curriculum_id = 1 },
               new PLOs { PLO_id = 7, PLO_name = "PLO07", PLO_description = "Giao tiếp, thuyết trình tự tin trước đám đông", curriculum_id = 2 },
               new PLOs { PLO_id = 8, PLO_name = "PLO08", PLO_description = "Kĩ năng làm việc nhóm", curriculum_id = 2 }
               );

            modelBuilder.Entity<PLOMapping>().HasData(
                new PLOMapping { PLO_id = 2, subject_id = 3 },
                new PLOMapping { PLO_id = 1, subject_id = 2 },
                new PLOMapping { PLO_id = 1, subject_id = 3 },
                new PLOMapping { PLO_id = 1, subject_id = 4 },
                new PLOMapping { PLO_id = 2, subject_id = 4 }
                );

            modelBuilder.Entity<PreRequisiteType>().HasData(
                new PreRequisiteType { pre_requisite_type_id = 1, pre_requisite_type_name = "Corequisite" },
                new PreRequisiteType { pre_requisite_type_id = 2, pre_requisite_type_name = "Prerequisite" },
                new PreRequisiteType { pre_requisite_type_id = 3, pre_requisite_type_name = "Recommended" },
                new PreRequisiteType { pre_requisite_type_id = 4, pre_requisite_type_name = "Elective" }
                );

            modelBuilder.Entity<PreRequisite>().HasData(
                new PreRequisite { subject_id = 1, pre_subject_id = 2, pre_requisite_type_id = 1 },
                new PreRequisite { subject_id = 2, pre_subject_id = 3, pre_requisite_type_id = 2 },
                new PreRequisite { subject_id = 3, pre_subject_id = 5, pre_requisite_type_id = 3 },
                new PreRequisite { subject_id = 4, pre_subject_id = 7, pre_requisite_type_id = 4 },
                new PreRequisite { subject_id = 5, pre_subject_id = 8, pre_requisite_type_id = 1 },
                new PreRequisite { subject_id = 10, pre_subject_id = 12, pre_requisite_type_id = 1 },
                new PreRequisite { subject_id = 11, pre_subject_id = 14, pre_requisite_type_id = 2 },
                new PreRequisite { subject_id = 15, pre_subject_id = 17, pre_requisite_type_id = 3 }
                );

                
        }
    }
}
