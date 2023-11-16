using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            //optionsBuilder.UseMySql(con, ServerVersion.AutoDetect(con));
        }

        public virtual DbSet<AssessmentMethod> AssessmentMethod { get; set; }
        public virtual DbSet<AssessmentType> AssessmentType { get; set; }
        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<ClassSessionType> ClassSessionType { get; set; }
        public virtual DbSet<CLO> CLO { get; set; }
        public virtual DbSet<Combo> Combo { get; set; }
        public virtual DbSet<ComboCurriculum> ComboCurriculum { get; set; }
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

            modelBuilder.Entity<ComboCurriculum>()
              .HasKey(ba => new { ba.combo_id, ba.curriculum_id });

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

            modelBuilder.Entity<ComboCurriculum>()
                .HasOne(x => x.Curriculum)
                .WithMany(y => y.ComboCurriculum)
                .HasForeignKey(x => x.curriculum_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<CurriculumBatch>()
                .HasOne(x => x.Curriculum)
                .WithMany(y => y.CurriculumBatchs)
                .HasForeignKey(x => x.curriculum_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

           
            modelBuilder.Entity<Role>().HasData(
                new Role { role_id = 1, role_name = "Dispatcher" },
                new Role { role_id = 2, role_name = "Manager" },
                new Role { role_id = 3, role_name = "Admin" }
                );

            modelBuilder.Entity<User>().HasData(
                new User { user_id = 1, full_name = "Chu Quang Quan", role_id = 3, user_email = "quancqhe153661@fpt.edu.vn", user_name = "QuanCQ", is_active = true },
                new User { user_id = 2, full_name = "Nguyen Thi Thu", role_id = 2, user_email = "thunthe151440@fpt.edu.vn", user_name = "ThuNT", is_active = true }
                );


            modelBuilder.Entity<DegreeLevel>().HasData(
                new DegreeLevel { degree_level_id = 1, degree_level_code = "CD", degree_level_name = "Cao Đẳng", degree_level_english_name = "Associate Degree" },
                new DegreeLevel { degree_level_id = 2, degree_level_code = "IC", degree_level_name = "Cao Đẳng Quốc Tế", degree_level_english_name = "International Associate Degree" },
                new DegreeLevel { degree_level_id = 3, degree_level_code = "TC", degree_level_name = "Phổ Thông Cao Đẳng", degree_level_english_name = "Vocational Secondary" }
                );

            modelBuilder.Entity<Batch>().HasData(

              new Batch { batch_id = 1, batch_name = "7.1", batch_order = 1, degree_level_id = 3 },
              new Batch { batch_id = 2, batch_name = "17", batch_order = 1, degree_level_id = 2 },
              new Batch { batch_id = 3, batch_name = "18", batch_order = 2, degree_level_id = 2 },
              new Batch { batch_id = 4, batch_name = "19.1", batch_order = 1 , degree_level_id = 1 },
              new Batch { batch_id = 5, batch_name = "19.2", batch_order = 2 , degree_level_id = 1 },
              new Batch { batch_id = 6, batch_name = "19.3", batch_order = 3 , degree_level_id = 1 }
              );

            modelBuilder.Entity<Semester>().HasData(
                new Semester { semester_id = 1, semester_name = "Fall", semester_start_date = DateTime.Parse("09/09/2023"), semester_end_date = DateTime.Parse("12/12/2023"), school_year = 2023, start_batch_id = 6 },
                new Semester { semester_id = 2, semester_name = "Summer", semester_start_date = DateTime.Parse("05/05/2023"), semester_end_date = DateTime.Parse("08/08/2023"), school_year = 2023, start_batch_id = 5 },
                new Semester { semester_id = 3, semester_name = "Spring", semester_start_date = DateTime.Parse("01/03/2023"), semester_end_date = DateTime.Parse("04/04/2023"), school_year = 2023, start_batch_id = 4 },
                new Semester { semester_id = 4, semester_name = "Fall", semester_start_date = DateTime.Parse("09/09/2022"), semester_end_date = DateTime.Parse("12/12/2022"), school_year = 2022, start_batch_id = 3 },
                new Semester { semester_id = 5, semester_name = "Summer", semester_start_date = DateTime.Parse("05/05/2022"), semester_end_date = DateTime.Parse("08/08/2022"), school_year = 2022, start_batch_id = 2 },
                new Semester { semester_id = 6, semester_name = "Spring", semester_start_date = DateTime.Parse("01/01/2022"), semester_end_date = DateTime.Parse("04/04/2022"), school_year = 2022, start_batch_id = 1 }

                );

            modelBuilder.Entity<Major>().HasData(
                new Major { major_id = 1, major_code = "6210402", major_name = "Thiết kế đồ họa", major_english_name = "Graphic Design", degree_level_id = 1, is_active = true },
                new Major { major_id = 2, major_code = "6480201", major_name = "Công nghệ thông tin", major_english_name = "Information Technology", degree_level_id = 1, is_active = true },
                new Major { major_id = 3, major_code = "6340404", major_name = "Quản trị kinh doanh", major_english_name = "Business Administration", degree_level_id = 1, is_active = true },
                new Major { major_id = 4, major_code = "6510305", major_name = "CNKTĐK & Tự động hóa", major_english_name = "Automation Engineering", degree_level_id = 1, is_active = true },
                new Major { major_id = 5, major_code = "5340404", major_name = "Quản trị kinh doanh", major_english_name = "Business Administration", degree_level_id = 3, is_active = true },
                new Major { major_id = 6, major_code = "6480207", major_name = "Lập trình máy tính", major_english_name = "Computing", degree_level_id = 2, is_active = true }
                );

            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { specialization_id = 1, major_id = 1, specialization_code = "6216432", specialization_name = "Thiết kế nội và ngoại thất", specialization_english_name = "Interior and Exterior Design", semester_id = 1, is_active = true },
                new Specialization { specialization_id = 2, major_id = 1, specialization_code = "6215463", specialization_name = "Dựng phim và quảng cáo", specialization_english_name = "Film Making and Advertising", semester_id = 2, is_active = true },
                new Specialization { specialization_id = 3, major_id = 2, specialization_code = "6526432", specialization_name = "Lập trình Game", specialization_english_name = "Game Development", semester_id = 3, is_active = true },
                new Specialization { specialization_id = 4, major_id = 2, specialization_code = "6480201", specialization_name = "Ứng dụng phần mềm", specialization_english_name = "Software Application", semester_id = 1, is_active = true },
                new Specialization { specialization_id = 5, major_id = 5, specialization_code = "5340404", specialization_name = "Digital Marketing", specialization_english_name = "Digital Marketing", semester_id = 6, is_active = true },
                new Specialization { specialization_id = 6, major_id = 6, specialization_code = "6480207", specialization_name = "Phân tích dữ liệu", specialization_english_name = "Data Analytics", semester_id = 5, is_active = true }
                );

            modelBuilder.Entity<Curriculum>().HasData(
                new Curriculum { curriculum_id = 1, curriculum_code = "GD-IED-CD-18.3", curriculum_name = "Thiết kế đồ họa", english_curriculum_name = "Graphic Design", total_semester = 7, specialization_id = 1, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 2, curriculum_code = "GD-IED-IC-19.1", curriculum_name = "Thiết kế mĩ thuật số", english_curriculum_name = "Graphic Design", total_semester = 7, specialization_id = 1, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 3, curriculum_code = "SE-SE-IC-18", curriculum_name = "kĩ sư phần mềm", english_curriculum_name = "Software Engineering", total_semester = 7, specialization_id = 4, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 4, curriculum_code = "SE-SE-CD-17", curriculum_name = "kĩ thuật phần mềm", english_curriculum_name = "Software Engineering", total_semester = 7, specialization_id = 5, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 5, curriculum_code = "CM-FMA-TC-19.3", curriculum_name = "quản lí học liệu", english_curriculum_name = "Curriculum Management", total_semester = 6, specialization_id = 2, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 6, curriculum_code = "SS-IED-TC-19", curriculum_name = "kĩ năng mềm", english_curriculum_name = "Soft Skill", total_semester = 6, specialization_id = 4, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 7, curriculum_code = "SWP-WP-TC-7.1", curriculum_name = "kĩ năng lập trình web", english_curriculum_name = "Skill Web Program", total_semester = 7, specialization_id = 6, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true },
                new Curriculum { curriculum_id = 8, curriculum_code = "SS-IED-TC-18.2", curriculum_name = "kĩ năng mềm", english_curriculum_name = "Soft Skill", total_semester = 7, specialization_id = 1, curriculum_description = "", approved_date = DateTime.Today, decision_No = "360/QĐ-CĐFPL", Formality = "formal education", is_active = true }
                );

            modelBuilder.Entity<CurriculumBatch>().HasData(
                new CurriculumBatch { curriculum_id = 1, batch_id = 1 },
                new CurriculumBatch { curriculum_id = 2, batch_id = 1 },
                new CurriculumBatch { curriculum_id = 1, batch_id = 2 },
                new CurriculumBatch { curriculum_id = 3, batch_id = 2 },
                new CurriculumBatch { curriculum_id = 6, batch_id = 4 },
                new CurriculumBatch { curriculum_id = 7, batch_id = 6 }
                );

            modelBuilder.Entity<LearningMethod>().HasData(
               new LearningMethod { learning_method_id = 1, learning_method_name = "Online", learning_method_code = "T01" },
               new LearningMethod { learning_method_id = 2, learning_method_name = "Blended", learning_method_code = "T02" },
               new LearningMethod { learning_method_id = 3, learning_method_name = "Traditional", learning_method_code = "T03" }
                );

            modelBuilder.Entity<LearningResource>().HasData(
             new LearningResource { learning_resource_id = 1, learning_resource_type = "Internet", learning_resouce_code = "T01" },
             new LearningResource { learning_resource_id = 2, learning_resource_type = "Purchased book", learning_resouce_code = "T02" },
             new LearningResource { learning_resource_id = 3, learning_resource_type = "Free e-book", learning_resouce_code = "T03" },
             new LearningResource { learning_resource_id = 4, learning_resource_type = "Officially published", learning_resouce_code = "T04" },
             new LearningResource { learning_resource_id = 5, learning_resource_type = "Self-compiled", learning_resouce_code = "T05" },
             new LearningResource { learning_resource_id = 6, learning_resource_type = "Udemy course", learning_resouce_code = "T06" }
              );

            modelBuilder.Entity<AssessmentType>().HasData(
                new AssessmentType { assessment_type_id = 1, assessment_type_name = "On-going" },
                new AssessmentType { assessment_type_id = 2, assessment_type_name = "Final Exam" }
                );

            modelBuilder.Entity<AssessmentMethod>().HasData(
                new AssessmentMethod { assessment_method_id = 1, assessment_method_component = "Assignment", assessment_type_id = 2 },
                new AssessmentMethod { assessment_method_id = 2, assessment_method_component = "Bài học online", assessment_type_id = 1 },
                new AssessmentMethod { assessment_method_id = 3, assessment_method_component = "Lab", assessment_type_id = 1 },
                new AssessmentMethod { assessment_method_id = 4, assessment_method_component = "Bảo vệ assignment", assessment_type_id = 2 },
                new AssessmentMethod { assessment_method_id = 5, assessment_method_component = "Quiz", assessment_type_id = 1 },
                new AssessmentMethod { assessment_method_id = 6, assessment_method_component = "Đánh giá Assignment GĐ 1", assessment_type_id = 1 },
                new AssessmentMethod { assessment_method_id = 7, assessment_method_component = "Đánh giá Assignment GĐ 2", assessment_type_id = 1 }

                );



            modelBuilder.Entity<Subject>().HasData(
                new Subject { subject_id = 1, subject_code = "PRE209", subject_name = "Quản trị khủng hoảng truyền thông", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Conmunication crisis management", credit = 3, total_time = 90, total_time_class = 34, exam_total = 56, is_active = true },
                new Subject { subject_id = 2, subject_code = "PRO109", subject_name = "Thực tập tốt nghiệp(TMDT)", assessment_method_id = 1, learning_method_id = 3, english_subject_name = "Graduation Intership (Digital Marketing)", credit = 3, total_time = 75, total_time_class = 34, exam_total = 41, is_active = true },
                new Subject { subject_id = 3, subject_code = "ENT2227", subject_name = "Tiếng anh 2.2", assessment_method_id = 1, learning_method_id = 3, english_subject_name = "English 2.2", credit = 3, total_time = 75, total_time_class = 34, exam_total = 41, is_active = true },
                new Subject { subject_id = 4, subject_code = "ENT2127", subject_name = "Tiếng anh 2.1", assessment_method_id = 1, learning_method_id = 3, english_subject_name = "English 2.1", credit = 3, total_time = 75, total_time_class = 34, exam_total = 41, is_active = true },
                new Subject { subject_id = 5, subject_code = "ENT1227", subject_name = "Tiếng anh 1.2", assessment_method_id = 1, learning_method_id = 3, english_subject_name = "English 1.2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 6, subject_code = "ENT2226", subject_name = "Tiếng anh 2.2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English 2.2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 7, subject_code = "ENT2126", subject_name = "Tiếng anh 2.1", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English 2.1", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 8, subject_code = "ENT1127", subject_name = "Tiếng anh 1.1", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English 1.1", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 9, subject_code = "ENT1226", subject_name = "Tiếng anh 1.2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English 1.2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 10, subject_code = "EHO0101", subject_name = "Tiếng anh chuyên ngành 3 (NHKS)", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English for Hopitality 3", credit = 3, total_time = 90, total_time_class = 34, exam_total = 56, is_active = true },
                new Subject { subject_id = 11, subject_code = "ENT1128", subject_name = "Tiếng anh 1.1", assessment_method_id = 1, learning_method_id = 3, english_subject_name = "English 1.1", credit = 3, total_time = 90, total_time_class = 31, exam_total = 56, is_active = true },
                new Subject { subject_id = 12, subject_code = "EHO102", subject_name = "Tiếng anh chuyên ngành 1 (NHKS)", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "English for Hopitality 1", credit = 3, total_time = 90, total_time_class = 34, exam_total = 56, is_active = true },
                new Subject { subject_id = 13, subject_code = "COM1071", subject_name = "Tin học", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Informatics", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 14, subject_code = "MUL1013", subject_name = "Thiết kế hình ảnh với Photoshop", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Image Design using Photoshop", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 15, subject_code = "MUL116", subject_name = "Nhập môn đồ họa", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Graphic Introduction", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 16, subject_code = "PDP102", subject_name = "Kỹ năng học tập", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Learning Skills", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 17, subject_code = "VIE103", subject_name = "Gíao dục thể chất - Vovinam", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Physical education", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 18, subject_code = "MUL1024", subject_name = "Thiết kế minh họa với Illustrator", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Illustration Design using Adobe Illustrator", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 19, subject_code = "MUL1143", subject_name = "Luật xa gần và bố cục trong thiết kế đồ họa", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Perspective and Layout in Graphic Design", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 20, subject_code = "MUL2111", subject_name = "Chế bản điện tử với InDesign", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Publication Design using InDesign", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 21, subject_code = "MUL2143", subject_name = "Màu sắc", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Color", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 22, subject_code = "VIE1016", subject_name = "Chính trị", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Politics", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 23, subject_code = "MUL2123", subject_name = "Thiết kế bao bì", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Packaging Design", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 24, subject_code = "MUL2133", subject_name = "Nghệ thuật chữ", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Typography", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 25, subject_code = "MUL3191", subject_name = "Thiết kế thương hiệu và Maketing", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Digital Marketing and Media Concepts", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 26, subject_code = "PDP103", subject_name = "Kỹ năng phát triển bản thân", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Personal Development Skills", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 27, subject_code = "PRO1112", subject_name = "Dự án 1 (TKĐH)", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Project 1 (Graphic Design)", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 28, subject_code = "MUL117", subject_name = "Kỹ thuật nhiếp ảnh", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Photography and Retouch", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 29, subject_code = "MUL215", subject_name = "Kỹ thuật in", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Printing Technical", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 30, subject_code = "MUL216", subject_name = "Thiết kế đa truyền thông với Animate", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Adobe Animate CC", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 31, subject_code = "MUL217", subject_name = "ý tưởng sáng tạo", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Creative Idea", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 32, subject_code = "MUL222", subject_name = "Dựng hình với Maya 3D", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "3D Modeling using Maya", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 33, subject_code = "MUL223", subject_name = "Kịch bản phi quảng cáo", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Storyboarding Advertisement", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 34, subject_code = "MUL225", subject_name = "Nguyên lý thiết kế nội thất 1", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Principles of Interior Design 1", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 35, subject_code = "MUL317", subject_name = "Autocad 2D", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Autocad 2D", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 36, subject_code = "MUL2211", subject_name = "Thiết kế nội thất với 3D Max", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Interior design with 3D Max", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 37, subject_code = "MUL224", subject_name = "Quay và dựng phim với Adobe Premiere", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Filming and editing using Adobe Premiere", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 38, subject_code = "MUL226", subject_name = "Nguyên lý thiết kế nội thất 2", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Principles of Interior Design 2", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 39, subject_code = "MUL3151", subject_name = "Xử lý hậu kỳ với Adobe Premiere", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Post Processing using Premier", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 40, subject_code = "MUL3211", subject_name = "Hiệu ứng kỹ xảo với Adobe After Effect", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Adobe After Effect", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 41, subject_code = "MUL322", subject_name = "Dựng phối cảnh với SketchUp", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Creating Perspectives using SketchUp", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 42, subject_code = "MUL323", subject_name = "Dựng phim với C4D", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "3D Modeling using C4D", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 43, subject_code = "MUL324", subject_name = "Chuyển động với C4D", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Motion using C4D", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 44, subject_code = "PDP104", subject_name = "Kỹ năng làm việc", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Professional Skills", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 45, subject_code = "SYB3012", subject_name = "Khởi sự doanh nghiệp", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Startup Your Business", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 46, subject_code = "PRO119", subject_name = "Thực tập tốt nghiệp (TKĐH)", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Graduation Internship (Graphic Design)", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 47, subject_code = "PRO221", subject_name = "Dự án tốt nghiệp (TKĐH-Phim và Quảng cáo)", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Graduation Project (Film and Ads)", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 48, subject_code = "PRO223", subject_name = "Dự án tốt nghiệp (TKĐH Nội và Ngoại)", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Graduation Project (Interior and Exterior)", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 49, subject_code = "VIE1026", subject_name = "Pháp luật", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Law", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 50, subject_code = "VIE104", subject_name = "giáo dục quốc phòng", assessment_method_id = 1, learning_method_id = 3, english_subject_name = "Defense Education", credit = 4, total_time = 75, total_time_class = 71, exam_total = 3, is_active = true },
                new Subject { subject_id = 51, subject_code = "SOF102", subject_name = "Nhập môn kỹ thuật phần mềm", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Introduction to Sofware Engineering", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 52, subject_code = "PRE1022", subject_name = "Kỹ năng thuyết trình trước công chúng ", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Public Speaking Skills", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 53, subject_code = "LOG211", subject_name = "Bảo hiểm vận tải", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Transportation Insurance", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 54, subject_code = "TOU2033", subject_name = "Nghiệp vụ hướng dẫn 1", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Tourguide Operations 1", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 55, subject_code = "TOU2032", subject_name = "Nghiệp vụ hướng dẫn 1", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Tourguide Operations 1", credit = 3, total_time = 70, total_time_class = 40, exam_total = 4, is_active = true },
                new Subject { subject_id = 56, subject_code = "LOG105", subject_name = "Nghiệp vụ mua sắm", assessment_method_id = 1, learning_method_id = 2, english_subject_name = "Procurement Oprerations", credit = 3, total_time = 75, total_time_class = 34, exam_total = 41, is_active = true }

                );

            modelBuilder.Entity<CurriculumSubject>().HasData(
                new CurriculumSubject { curriculum_id = 1, subject_id = 1, term_no = 3, combo_id = 1, subject_group = "General Subject", option = false },
                new CurriculumSubject { curriculum_id = 1, subject_id = 3, term_no = 3, combo_id = 2, subject_group = "Option Subject", option = true },
                new CurriculumSubject { curriculum_id = 2, subject_id = 4, term_no = 3, combo_id = 0, subject_group = "Specialization Subject", option = false },
                new CurriculumSubject { curriculum_id = 1, subject_id = 5, term_no = 2, combo_id = 0, subject_group = "Basic Subject", option = false },
                new CurriculumSubject { curriculum_id = 1, subject_id = 2, term_no = 1, combo_id = 3, subject_group = "Option Subject", option = true }
                );

            modelBuilder.Entity<Combo>().HasData(
                new Combo { combo_id = 1, combo_code = ".NET", combo_name = "Lập trình C#", combo_english_name = "C# Programing", specialization_id = 4, is_active = true },
                new Combo { combo_id = 2, combo_code = "JS", combo_name = "kĩ sư Nhật Bản", combo_english_name = "Japan Software", specialization_id = 3, is_active = true },
                new Combo { combo_id = 3, combo_code = "KS", combo_name = "kĩ sư Hàn Quốc", combo_english_name = "Korea Software", specialization_id = 2, is_active = false },
                new Combo { combo_id = 4, combo_code = "NodeJS", combo_name = "Lập trình NodeJS", combo_english_name = "Web api using NodeJS", specialization_id = 1, is_active = true }
                );

            modelBuilder.Entity<ClassSessionType>().HasData(
                new ClassSessionType { class_session_type_id = 1, class_session_type_name = "Online" },
                 new ClassSessionType { class_session_type_id = 2, class_session_type_name = "Offline" }
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
                new PreRequisiteType { pre_requisite_type_id = 4, pre_requisite_type_name = "Elective" },
                new PreRequisiteType { pre_requisite_type_id = 5, pre_requisite_type_name = "Passed" },
                new PreRequisiteType { pre_requisite_type_id = 6, pre_requisite_type_name = "Participated" }
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

            modelBuilder.Entity<Quiz>().HasData(
                new Quiz { quiz_id = 1, quiz_name = "Quiz 1", subject_id = 1 },
                new Quiz { quiz_id = 2, quiz_name = "Quiz 2", subject_id = 2 },
                new Quiz { quiz_id = 3, quiz_name = "Quiz 3", subject_id = 1 }
                );

            modelBuilder.Entity<Question>().HasData(
                new Question { question_id = 1, question_type = "Single Choice", question_name = "Lợi ích khi sử dụng View?", answers_A = "Che dấu và bảo mật dữ liệu", answers_B = "Hiển thị dữ liệu một cách tùy biến", answers_C = "Thực thi nhanh hơn các câu lệnh truy vấn do đã được biên dịch sẵn", answers_D = "Tất cả đáp án đều đúng", correct_answer = "D", quiz_id = 1 },
                new Question { question_id = 2, question_type = "Single Choice", question_name = "Qui đinh đặt tên cột trong View?", answers_A = "Cột chứa giá trị được tính toán từ nhiều cột khác phải được đặt tên", answers_B = "Cột chứa giá trị được tính toán từ nhiều cột khác không được đặt tên", answers_C = "Thực thi nhanh hơn các câu lệnh truy vấn do đã được biên dịch sẵn", answers_D = "Tất cả đáp án đều đúng", correct_answer = "D", quiz_id = 2 },
                new Question { question_id = 3, question_type = "Single Choice", question_name = "VIEW có thể cập nhật (updatable view) cho phép?", answers_A = "Xem dữ liệu và cập nhật dữ liệu trong các bảng cơ sở qua View", answers_B = "Xem dữ liệu", answers_C = "cập nhật dữ liệu trong các bảng cơ sở qua View", answers_D = "Tất cả đáp án đều sai", correct_answer = "A", quiz_id = 1 }
                );
        }
    }
}
