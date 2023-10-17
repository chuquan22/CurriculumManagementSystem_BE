using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentType",
                columns: table => new
                {
                    assessment_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assessment_type_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentType", x => x.assessment_type_id);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    batch_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    batch_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.batch_id);
                });

            migrationBuilder.CreateTable(
                name: "ClassSessionType",
                columns: table => new
                {
                    class_session_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_session_type_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessionType", x => x.class_session_type_id);
                });

            migrationBuilder.CreateTable(
                name: "LearningMethod",
                columns: table => new
                {
                    learning_method_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    learning_method_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    learning_method_description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningMethod", x => x.learning_method_id);
                });

            migrationBuilder.CreateTable(
                name: "LearningResource",
                columns: table => new
                {
                    learning_resource_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    learning_resource_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningResource", x => x.learning_resource_id);
                });

            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    major_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    major_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    major_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    major_english_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.major_id);
                });

            migrationBuilder.CreateTable(
                name: "PreRequisiteType",
                columns: table => new
                {
                    pre_requisite_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pre_requisite_type_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRequisiteType", x => x.pre_requisite_type_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "Semester",
                columns: table => new
                {
                    semester_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    semester_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester_start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    semester_end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    school_year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semester", x => x.semester_id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentMethod",
                columns: table => new
                {
                    assessment_method_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assessment_method_component = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    assessment_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentMethod", x => x.assessment_method_id);
                    table.ForeignKey(
                        name: "FK_AssessmentMethod_AssessmentType_assessment_type_id",
                        column: x => x.assessment_type_id,
                        principalTable: "AssessmentType",
                        principalColumn: "assessment_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    user_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_phone = table.Column<int>(type: "int", nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_User_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialization",
                columns: table => new
                {
                    specialization_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    specialization_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    specialization_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    specialization_english_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    major_id = table.Column<int>(type: "int", nullable: false),
                    semester_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialization", x => x.specialization_id);
                    table.ForeignKey(
                        name: "FK_Specialization_Major_major_id",
                        column: x => x.major_id,
                        principalTable: "Major",
                        principalColumn: "major_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Specialization_Semester_semester_id",
                        column: x => x.semester_id,
                        principalTable: "Semester",
                        principalColumn: "semester_id");
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    english_subject_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    learning_method_id = table.Column<int>(type: "int", nullable: false),
                    assessment_method_id = table.Column<int>(type: "int", nullable: false),
                    credit = table.Column<int>(type: "int", nullable: false),
                    total_time = table.Column<int>(type: "int", nullable: false),
                    total_time_class = table.Column<int>(type: "int", nullable: false),
                    exam_total = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.subject_id);
                    table.ForeignKey(
                        name: "FK_Subject_AssessmentMethod_assessment_method_id",
                        column: x => x.assessment_method_id,
                        principalTable: "AssessmentMethod",
                        principalColumn: "assessment_method_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subject_LearningMethod_learning_method_id",
                        column: x => x.learning_method_id,
                        principalTable: "LearningMethod",
                        principalColumn: "learning_method_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Curriculum",
                columns: table => new
                {
                    curriculum_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    curriculum_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    curriculum_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    english_curriculum_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total_semester = table.Column<int>(type: "int", nullable: false),
                    curriculum_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    specialization_id = table.Column<int>(type: "int", nullable: false),
                    batch_id = table.Column<int>(type: "int", nullable: false),
                    decision_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    curriculum_status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculum", x => x.curriculum_id);
                    table.ForeignKey(
                        name: "FK_Curriculum_Batch_batch_id",
                        column: x => x.batch_id,
                        principalTable: "Batch",
                        principalColumn: "batch_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Curriculum_Specialization_specialization_id",
                        column: x => x.specialization_id,
                        principalTable: "Specialization",
                        principalColumn: "specialization_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreRequisite",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    pre_subject_id = table.Column<int>(type: "int", nullable: false),
                    pre_requisite_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRequisite", x => new { x.subject_id, x.pre_subject_id });
                    table.ForeignKey(
                        name: "FK_PreRequisite_PreRequisiteType_pre_requisite_type_id",
                        column: x => x.pre_requisite_type_id,
                        principalTable: "PreRequisiteType",
                        principalColumn: "pre_requisite_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRequisite_Subject_pre_subject_id",
                        column: x => x.pre_subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRequisite_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id");
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    quiz_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quiz_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.quiz_id);
                    table.ForeignKey(
                        name: "FK_Quiz_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationSubject",
                columns: table => new
                {
                    specialization_id = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationSubject", x => new { x.specialization_id, x.subject_id });
                    table.ForeignKey(
                        name: "FK_SpecializationSubject_Specialization_specialization_id",
                        column: x => x.specialization_id,
                        principalTable: "Specialization",
                        principalColumn: "specialization_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationSubject_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Syllabus",
                columns: table => new
                {
                    syllabus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    document_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    decision_No = table.Column<int>(type: "int", nullable: false),
                    degree_level = table.Column<int>(type: "int", nullable: false),
                    syllabus_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    student_task = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    syllabus_tool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    syllabus_note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    min_GPA_to_pass = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    scoring_scale = table.Column<int>(type: "int", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    syllabus_status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllabus", x => x.syllabus_id);
                    table.ForeignKey(
                        name: "FK_Syllabus_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Combo",
                columns: table => new
                {
                    combo_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    combo_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    combo_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    combo_english_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    combo_no = table.Column<int>(type: "int", nullable: false),
                    combo_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    specialization_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    curriculum_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combo", x => x.combo_id);
                    table.ForeignKey(
                        name: "FK_Combo_Curriculum_curriculum_id",
                        column: x => x.curriculum_id,
                        principalTable: "Curriculum",
                        principalColumn: "curriculum_id");
                    table.ForeignKey(
                        name: "FK_Combo_Specialization_specialization_id",
                        column: x => x.specialization_id,
                        principalTable: "Specialization",
                        principalColumn: "specialization_id");
                });

            migrationBuilder.CreateTable(
                name: "CurriculumSubject",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    curriculum_id = table.Column<int>(type: "int", nullable: false),
                    term_no = table.Column<int>(type: "int", nullable: false),
                    option = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumSubject", x => new { x.curriculum_id, x.subject_id });
                    table.ForeignKey(
                        name: "FK_CurriculumSubject_Curriculum_curriculum_id",
                        column: x => x.curriculum_id,
                        principalTable: "Curriculum",
                        principalColumn: "curriculum_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumSubject_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PLOs",
                columns: table => new
                {
                    PLO_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLO_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    curriculum_id = table.Column<int>(type: "int", nullable: false),
                    PLO_description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLOs", x => x.PLO_id);
                    table.ForeignKey(
                        name: "FK_PLOs_Curriculum_curriculum_id",
                        column: x => x.curriculum_id,
                        principalTable: "Curriculum",
                        principalColumn: "curriculum_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SemesterPlan",
                columns: table => new
                {
                    curriculum_id = table.Column<int>(type: "int", nullable: false),
                    semester_id = table.Column<int>(type: "int", nullable: false),
                    semester_plan_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester_plan_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    term_no = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterPlan", x => new { x.semester_id, x.curriculum_id });
                    table.ForeignKey(
                        name: "FK_SemesterPlan_Curriculum_curriculum_id",
                        column: x => x.curriculum_id,
                        principalTable: "Curriculum",
                        principalColumn: "curriculum_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterPlan_Semester_semester_id",
                        column: x => x.semester_id,
                        principalTable: "Semester",
                        principalColumn: "semester_id");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    question_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quiz_id = table.Column<int>(type: "int", nullable: false),
                    answers_1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answers_2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answers_3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answers_4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correct_answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_Question_Quiz_quiz_id",
                        column: x => x.quiz_id,
                        principalTable: "Quiz",
                        principalColumn: "quiz_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CLO",
                columns: table => new
                {
                    CLO_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLO_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    syllabus_id = table.Column<int>(type: "int", nullable: false),
                    CLO_description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLO", x => x.CLO_id);
                    table.ForeignKey(
                        name: "FK_CLO_Syllabus_syllabus_id",
                        column: x => x.syllabus_id,
                        principalTable: "Syllabus",
                        principalColumn: "syllabus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradingStruture",
                columns: table => new
                {
                    grading_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    grading_weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    grading_part = table.Column<int>(type: "int", nullable: false),
                    syllabus_id = table.Column<int>(type: "int", nullable: false),
                    minimum_value_to_meet_completion = table.Column<int>(type: "int", nullable: false),
                    grading_duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    scope_knowledge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    how_granding_structure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    assessment_method_id = table.Column<int>(type: "int", nullable: false),
                    grading_note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingStruture", x => x.grading_id);
                    table.ForeignKey(
                        name: "FK_GradingStruture_AssessmentMethod_assessment_method_id",
                        column: x => x.assessment_method_id,
                        principalTable: "AssessmentMethod",
                        principalColumn: "assessment_method_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradingStruture_Syllabus_syllabus_id",
                        column: x => x.syllabus_id,
                        principalTable: "Syllabus",
                        principalColumn: "syllabus_id");
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    material_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    material_purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    syllabus_id = table.Column<int>(type: "int", nullable: false),
                    material_note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    learning_resource_id = table.Column<int>(type: "int", nullable: false),
                    material_author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_published_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    material_edition = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.material_id);
                    table.ForeignKey(
                        name: "FK_Material_LearningResource_learning_resource_id",
                        column: x => x.learning_resource_id,
                        principalTable: "LearningResource",
                        principalColumn: "learning_resource_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Material_Syllabus_syllabus_id",
                        column: x => x.syllabus_id,
                        principalTable: "Syllabus",
                        principalColumn: "syllabus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    schedule_content = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    syllabus_id = table.Column<int>(type: "int", nullable: false),
                    ITU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    schedule_student_task = table.Column<long>(type: "bigint", nullable: false),
                    student_material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lecturer_material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    schedule_lecturer_task = table.Column<long>(type: "bigint", nullable: false),
                    student_material_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lecturer_material_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    class_session_type_id = table.Column<int>(type: "int", nullable: false),
                    remote_learning = table.Column<int>(type: "int", nullable: false),
                    ass_defense = table.Column<int>(type: "int", nullable: false),
                    eos_exam = table.Column<int>(type: "int", nullable: false),
                    video_learning = table.Column<int>(type: "int", nullable: false),
                    IVQ = table.Column<int>(type: "int", nullable: false),
                    online_lab = table.Column<int>(type: "int", nullable: false),
                    online_test = table.Column<int>(type: "int", nullable: false),
                    assigment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.schedule_id);
                    table.ForeignKey(
                        name: "FK_Session_ClassSessionType_class_session_type_id",
                        column: x => x.class_session_type_id,
                        principalTable: "ClassSessionType",
                        principalColumn: "class_session_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Session_Syllabus_syllabus_id",
                        column: x => x.syllabus_id,
                        principalTable: "Syllabus",
                        principalColumn: "syllabus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComboCurriculum",
                columns: table => new
                {
                    combo_id = table.Column<int>(type: "int", nullable: false),
                    curriculum_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboCurriculum", x => new { x.combo_id, x.curriculum_id });
                    table.ForeignKey(
                        name: "FK_ComboCurriculum_Combo_combo_id",
                        column: x => x.combo_id,
                        principalTable: "Combo",
                        principalColumn: "combo_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboCurriculum_Curriculum_curriculum_id",
                        column: x => x.curriculum_id,
                        principalTable: "Curriculum",
                        principalColumn: "curriculum_id");
                });

            migrationBuilder.CreateTable(
                name: "ComboSubject",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    combo_id = table.Column<int>(type: "int", nullable: false),
                    semester_no = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboSubject", x => new { x.combo_id, x.subject_id });
                    table.ForeignKey(
                        name: "FK_ComboSubject_Combo_combo_id",
                        column: x => x.combo_id,
                        principalTable: "Combo",
                        principalColumn: "combo_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboSubject_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PLOMapping",
                columns: table => new
                {
                    PLO_id = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLOMapping", x => new { x.PLO_id, x.subject_id });
                    table.ForeignKey(
                        name: "FK_PLOMapping_PLOs_PLO_id",
                        column: x => x.PLO_id,
                        principalTable: "PLOs",
                        principalColumn: "PLO_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PLOMapping_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradingCLO",
                columns: table => new
                {
                    grading_id = table.Column<int>(type: "int", nullable: false),
                    CLO_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingCLO", x => new { x.grading_id, x.CLO_id });
                    table.ForeignKey(
                        name: "FK_GradingCLO_CLO_CLO_id",
                        column: x => x.CLO_id,
                        principalTable: "CLO",
                        principalColumn: "CLO_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradingCLO_GradingStruture_grading_id",
                        column: x => x.grading_id,
                        principalTable: "GradingStruture",
                        principalColumn: "grading_id");
                });

            migrationBuilder.CreateTable(
                name: "SessionCLO",
                columns: table => new
                {
                    CLO_id = table.Column<int>(type: "int", nullable: false),
                    session_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionCLO", x => new { x.CLO_id, x.session_id });
                    table.ForeignKey(
                        name: "FK_SessionCLO_CLO_CLO_id",
                        column: x => x.CLO_id,
                        principalTable: "CLO",
                        principalColumn: "CLO_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionCLO_Session_session_id",
                        column: x => x.session_id,
                        principalTable: "Session",
                        principalColumn: "schedule_id");
                });

            migrationBuilder.InsertData(
                table: "AssessmentType",
                columns: new[] { "assessment_type_id", "assessment_type_name" },
                values: new object[,]
                {
                    { 1, "Online" },
                    { 2, "ORIT" }
                });

            migrationBuilder.InsertData(
                table: "Batch",
                columns: new[] { "batch_id", "batch_name" },
                values: new object[,]
                {
                    { 1, "K19.3" },
                    { 2, "K18" },
                    { 3, "K17" },
                    { 4, "K20" }
                });

            migrationBuilder.InsertData(
                table: "LearningMethod",
                columns: new[] { "learning_method_id", "learning_method_description", "learning_method_name" },
                values: new object[,]
                {
                    { 1, "", "Online Learing" },
                    { 2, "", "Balence" }
                });

            migrationBuilder.InsertData(
                table: "Major",
                columns: new[] { "major_id", "is_active", "major_code", "major_english_name", "major_name" },
                values: new object[,]
                {
                    { 1, true, "GD", "Graphic Design", "Thiết kế đồ họa" },
                    { 2, true, "IT", "Information technology", "Công nghệ thông tin" },
                    { 3, true, "BA", "Business Administration", "Quản trị kinh doanh" },
                    { 4, true, "AE", "Automation Engineering", "Kỹ thuật tự động hóa" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "role_id", "role_name" },
                values: new object[,]
                {
                    { 1, "Dispatcher" },
                    { 2, "Manager" },
                    { 3, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Semester",
                columns: new[] { "semester_id", "school_year", "semester_end_date", "semester_name", "semester_start_date" },
                values: new object[,]
                {
                    { 1, 2023, new DateTime(2023, 10, 17, 22, 48, 54, 586, DateTimeKind.Local).AddTicks(9009), "Fall", new DateTime(2023, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2023, new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 2023, new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "AssessmentMethod",
                columns: new[] { "assessment_method_id", "assessment_method_component", "assessment_type_id" },
                values: new object[,]
                {
                    { 1, "ABC", 1 },
                    { 2, "TEST", 2 }
                });

            migrationBuilder.InsertData(
                table: "Specialization",
                columns: new[] { "specialization_id", "is_active", "major_id", "semester_id", "specialization_code", "specialization_english_name", "specialization_name" },
                values: new object[,]
                {
                    { 1, true, 1, 1, "IED", "Interior and exterior design", "Thiết kế nội và ngoại thất" },
                    { 2, true, 1, 1, "FMA", "Filmmaking and advertising", "Dựng phim và quảng cáo" },
                    { 3, true, 1, 2, "IED", "Interior and exterior design", "Thiết kế nội và ngoại thất" },
                    { 4, true, 2, 2, "SE", "Software Engineering", "kĩ thuật phần mềm" },
                    { 5, true, 2, 2, "WP", "web programming", "lập trình web" },
                    { 6, true, 2, 1, "GP", "game programming", "lập trình game" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "user_id", "full_name", "is_active", "role_id", "user_email", "user_name", "user_password", "user_phone" },
                values: new object[,]
                {
                    { 1, "Chu Quang Quan", true, 1, "chuquan2k1@gmail.com", "QuanCQ", "quan123", null },
                    { 2, "Nguyen Thi Thu", true, 2, "nguyenthu120801@gmail.com", "ThuNT", "quan123", null },
                    { 3, "Nguyen Phong Hao", true, 1, "haotest@gmail.com", "admin", "quan123", null }
                });

            migrationBuilder.InsertData(
                table: "Combo",
                columns: new[] { "combo_id", "combo_code", "combo_description", "combo_english_name", "combo_name", "combo_no", "curriculum_id", "is_active", "specialization_id" },
                values: new object[,]
                {
                    { 1, ".NET", "lập trình web với ngôn ngữ C#", "C# Programing", "Lập trình C#", 1, null, true, 4 },
                    { 2, "JS", "kĩ sư lập trình với ngôn ngữ Nhật", "Japan Software", "kĩ sư Nhật Bản", 2, null, true, 3 },
                    { 3, "KS", "kĩ sư lập trình với ngôn ngữ Hàn", "Korea Software", "kĩ sư Hàn Quốc", 1, null, false, 2 },
                    { 4, "NodeJS", "lập trình web với NodeJS", "NodeJS Programing", "Lập trình NodeJS", 2, null, true, 1 }
                });

            migrationBuilder.InsertData(
                table: "Curriculum",
                columns: new[] { "curriculum_id", "approved_date", "batch_id", "curriculum_code", "curriculum_description", "curriculum_name", "curriculum_status", "decision_No", "english_curriculum_name", "specialization_id", "total_semester" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 1, "GD", "", "Thiết kế đồ họa", 1, "360/QĐ-CĐFPL", "Graphic Design", 1, 7 },
                    { 2, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 4, "GD", "", "Thiết kế mĩ thuật số", 1, "360/QĐ-CĐFPL", "Graphic Design", 1, 7 },
                    { 3, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 3, "SE", "", "kĩ sư phần mềm", 0, "360/QĐ-CĐFPL", "Software Engineering", 4, 7 },
                    { 4, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 2, "SE", "", "kĩ thuật phần mềm", 1, "360/QĐ-CĐFPL", "Software Engineering", 4, 7 },
                    { 5, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 3, "CM", "", "quản lí học liệu", 1, "360/QĐ-CĐFPL", "Curriculum Management", 2, 7 },
                    { 6, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 3, "SS", "", "kĩ năng mềm", 0, "360/QĐ-CĐFPL", "Soft Skill", 1, 7 },
                    { 7, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 3, "SWP", "", "kĩ năng lập trình web", 0, "360/QĐ-CĐFPL", "Skill Web Program", 1, 7 },
                    { 8, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), 3, "SS", "", "kĩ năng mềm", 0, "360/QĐ-CĐFPL", "Soft Skill", 1, 7 }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "subject_id", "assessment_method_id", "credit", "english_subject_name", "exam_total", "is_active", "learning_method_id", "subject_code", "subject_name", "total_time", "total_time_class" },
                values: new object[,]
                {
                    { 1, 1, 3, "Project Capstone", 3, true, 1, "SEP490", "Đồ án", 70, 40 },
                    { 2, 2, 3, "Mac-Lenin philosophy", 5, true, 1, "MLN131", "Triết học Mac-Lenin", 70, 40 },
                    { 3, 1, 3, "Soft Skill Group", 4, true, 2, "SSG104", "Kĩ năng trong làm việc nhóm", 70, 40 }
                });

            migrationBuilder.InsertData(
                table: "CurriculumSubject",
                columns: new[] { "curriculum_id", "subject_id", "option", "term_no" },
                values: new object[] { 1, 1, false, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentMethod_assessment_type_id",
                table: "AssessmentMethod",
                column: "assessment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_CLO_syllabus_id",
                table: "CLO",
                column: "syllabus_id");

            migrationBuilder.CreateIndex(
                name: "IX_Combo_curriculum_id",
                table: "Combo",
                column: "curriculum_id");

            migrationBuilder.CreateIndex(
                name: "IX_Combo_specialization_id",
                table: "Combo",
                column: "specialization_id");

            migrationBuilder.CreateIndex(
                name: "IX_ComboCurriculum_curriculum_id",
                table: "ComboCurriculum",
                column: "curriculum_id");

            migrationBuilder.CreateIndex(
                name: "IX_ComboSubject_subject_id",
                table: "ComboSubject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Curriculum_batch_id",
                table: "Curriculum",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_Curriculum_specialization_id",
                table: "Curriculum",
                column: "specialization_id");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumSubject_subject_id",
                table: "CurriculumSubject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_GradingCLO_CLO_id",
                table: "GradingCLO",
                column: "CLO_id");

            migrationBuilder.CreateIndex(
                name: "IX_GradingStruture_assessment_method_id",
                table: "GradingStruture",
                column: "assessment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_GradingStruture_syllabus_id",
                table: "GradingStruture",
                column: "syllabus_id");

            migrationBuilder.CreateIndex(
                name: "IX_Material_learning_resource_id",
                table: "Material",
                column: "learning_resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_Material_syllabus_id",
                table: "Material",
                column: "syllabus_id");

            migrationBuilder.CreateIndex(
                name: "IX_PLOMapping_subject_id",
                table: "PLOMapping",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_PLOs_curriculum_id",
                table: "PLOs",
                column: "curriculum_id");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequisite_pre_requisite_type_id",
                table: "PreRequisite",
                column: "pre_requisite_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequisite_pre_subject_id",
                table: "PreRequisite",
                column: "pre_subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Question_quiz_id",
                table: "Question",
                column: "quiz_id");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_subject_id",
                table: "Quiz",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterPlan_curriculum_id",
                table: "SemesterPlan",
                column: "curriculum_id");

            migrationBuilder.CreateIndex(
                name: "IX_Session_class_session_type_id",
                table: "Session",
                column: "class_session_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Session_syllabus_id",
                table: "Session",
                column: "syllabus_id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionCLO_session_id",
                table: "SessionCLO",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_Specialization_major_id",
                table: "Specialization",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_Specialization_semester_id",
                table: "Specialization",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationSubject_subject_id",
                table: "SpecializationSubject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_assessment_method_id",
                table: "Subject",
                column: "assessment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_learning_method_id",
                table: "Subject",
                column: "learning_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabus_subject_id",
                table: "Syllabus",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboCurriculum");

            migrationBuilder.DropTable(
                name: "ComboSubject");

            migrationBuilder.DropTable(
                name: "CurriculumSubject");

            migrationBuilder.DropTable(
                name: "GradingCLO");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "PLOMapping");

            migrationBuilder.DropTable(
                name: "PreRequisite");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "SemesterPlan");

            migrationBuilder.DropTable(
                name: "SessionCLO");

            migrationBuilder.DropTable(
                name: "SpecializationSubject");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Combo");

            migrationBuilder.DropTable(
                name: "GradingStruture");

            migrationBuilder.DropTable(
                name: "LearningResource");

            migrationBuilder.DropTable(
                name: "PLOs");

            migrationBuilder.DropTable(
                name: "PreRequisiteType");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "CLO");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Curriculum");

            migrationBuilder.DropTable(
                name: "ClassSessionType");

            migrationBuilder.DropTable(
                name: "Syllabus");

            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.DropTable(
                name: "Specialization");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.DropTable(
                name: "AssessmentMethod");

            migrationBuilder.DropTable(
                name: "LearningMethod");

            migrationBuilder.DropTable(
                name: "AssessmentType");
        }
    }
}
