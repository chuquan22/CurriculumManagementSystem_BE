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
                name: "Semester",
                columns: table => new
                {
                    semester_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    semester_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester_start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    semester_end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    school_year = table.Column<int>(type: "int", nullable: false),
                    batch_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semester", x => x.semester_id);
                    table.ForeignKey(
                        name: "FK_Semester_Batch_batch_id",
                        column: x => x.batch_id,
                        principalTable: "Batch",
                        principalColumn: "batch_id",
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
                name: "SemesterBatch",
                columns: table => new
                {
                    semester_batch_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    semester_id = table.Column<int>(type: "int", nullable: false),
                    batch_id = table.Column<int>(type: "int", nullable: false),
                    term_no = table.Column<int>(type: "int", nullable: true),
                    degree_level = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterBatch", x => x.semester_batch_id);
                    table.ForeignKey(
                        name: "FK_SemesterBatch_Batch_batch_id",
                        column: x => x.batch_id,
                        principalTable: "Batch",
                        principalColumn: "batch_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterBatch_Semester_semester_id",
                        column: x => x.semester_id,
                        principalTable: "Semester",
                        principalColumn: "semester_id");
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
                name: "Syllabus",
                columns: table => new
                {
                    syllabus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    document_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    decision_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    degree_level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    syllabus_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    student_task = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    syllabus_tool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    time_allocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    syllabus_note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    min_GPA_to_pass = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    scoring_scale = table.Column<int>(type: "int", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    syllabus_status = table.Column<bool>(type: "bit", nullable: false),
                    syllabus_approved = table.Column<bool>(type: "bit", nullable: false)
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
                    combo_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    specialization_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combo", x => x.combo_id);
                    table.ForeignKey(
                        name: "FK_Combo_Specialization_specialization_id",
                        column: x => x.specialization_id,
                        principalTable: "Specialization",
                        principalColumn: "specialization_id");
                });

            migrationBuilder.CreateTable(
                name: "Curriculum",
                columns: table => new
                {
                    curriculum_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    curriculum_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    curriculum_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    english_curriculum_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total_semester = table.Column<int>(type: "int", nullable: false),
                    curriculum_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    specialization_id = table.Column<int>(type: "int", nullable: false),
                    batch_id = table.Column<int>(type: "int", nullable: false),
                    decision_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    degree_level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Formality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
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
                    CLO_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    syllabus_id = table.Column<int>(type: "int", nullable: false),
                    CLO_description = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    type_of_questions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_questions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    session_no = table.Column<int>(type: "int", nullable: true),
                    references = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    grading_weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    grading_part = table.Column<int>(type: "int", nullable: false),
                    syllabus_id = table.Column<int>(type: "int", nullable: false),
                    minimum_value_to_meet_completion = table.Column<int>(type: "int", nullable: true),
                    grading_duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scope_knowledge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    how_granding_structure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assessment_method_id = table.Column<int>(type: "int", nullable: false),
                    grading_note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    clo_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    schedule_content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    syllabus_id = table.Column<int>(type: "int", nullable: false),
                    session_No = table.Column<int>(type: "int", nullable: false),
                    ITU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    schedule_student_task = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    student_material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lecturer_material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    schedule_lecturer_task = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    student_material_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lecturer_material_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    class_session_type_id = table.Column<int>(type: "int", nullable: false),
                    remote_learning = table.Column<float>(type: "real", nullable: false),
                    ass_defense = table.Column<float>(type: "real", nullable: false),
                    eos_exam = table.Column<float>(type: "real", nullable: false),
                    video_learning = table.Column<float>(type: "real", nullable: false),
                    IVQ = table.Column<float>(type: "real", nullable: false),
                    online_lab = table.Column<float>(type: "real", nullable: false),
                    online_test = table.Column<float>(type: "real", nullable: false),
                    assigment = table.Column<float>(type: "real", nullable: false)
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
                name: "CurriculumSubject",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    curriculum_id = table.Column<int>(type: "int", nullable: false),
                    term_no = table.Column<int>(type: "int", nullable: false),
                    combo_id = table.Column<int>(type: "int", nullable: true),
                    subject_group = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    semester_id = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.InsertData(
                table: "AssessmentType",
                columns: new[] { "assessment_type_id", "assessment_type_name" },
                values: new object[,]
                {
                    { 1, "Online" },
                    { 2, "ORIT" },
                    { 3, "On-going" },
                    { 4, "Final Exam" }
                });

            migrationBuilder.InsertData(
                table: "Batch",
                columns: new[] { "batch_id", "batch_name" },
                values: new object[,]
                {
                    { 1, "19.3" },
                    { 2, "19.2" },
                    { 3, "19.1" },
                    { 4, "18.3" },
                    { 5, "18.2" },
                    { 6, "18.1" }
                });

            migrationBuilder.InsertData(
                table: "ClassSessionType",
                columns: new[] { "class_session_type_id", "class_session_type_name" },
                values: new object[,]
                {
                    { 1, "Online" },
                    { 2, "Offline" },
                    { 3, "ORIT" }
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
                table: "LearningResource",
                columns: new[] { "learning_resource_id", "learning_resource_type" },
                values: new object[,]
                {
                    { 1, "Lab" },
                    { 2, "Assessment" }
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
                table: "PreRequisiteType",
                columns: new[] { "pre_requisite_type_id", "pre_requisite_type_name" },
                values: new object[,]
                {
                    { 1, "Corequisite" },
                    { 2, "Prerequisite" },
                    { 3, "Recommended" },
                    { 4, "Elective" }
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
                table: "AssessmentMethod",
                columns: new[] { "assessment_method_id", "assessment_method_component", "assessment_type_id" },
                values: new object[,]
                {
                    { 1, "Lab", 3 },
                    { 2, "Quiz", 3 },
                    { 3, "Assignment", 3 },
                    { 4, "Bài học online", 3 },
                    { 5, "Lab", 3 },
                    { 6, "Bảo vệ assignment", 4 },
                    { 7, "Quiz", 3 },
                    { 8, "Đánh giá Assignment GĐ 1", 3 }
                });

            migrationBuilder.InsertData(
                table: "Semester",
                columns: new[] { "semester_id", "batch_id", "school_year", "semester_end_date", "semester_name", "semester_start_date" },
                values: new object[,]
                {
                    { 1, 1, 2023, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fall", new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 2023, new DateTime(2023, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Summer", new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, 2023, new DateTime(2023, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring", new DateTime(2023, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, 2022, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fall", new DateTime(2022, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, 2022, new DateTime(2022, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Summer", new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 6, 2022, new DateTime(2022, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
                table: "Specialization",
                columns: new[] { "specialization_id", "is_active", "major_id", "semester_id", "specialization_code", "specialization_english_name", "specialization_name" },
                values: new object[,]
                {
                    { 1, true, 1, 1, "IED", "Interior and Exterior Design", "Thiết kế nội và ngoại thất" },
                    { 2, true, 1, 1, "FMA", "Filmmaking and Advertising", "Dựng phim và quảng cáo" },
                    { 3, true, 1, 2, "IED", "Interior and Exterior Design", "Thiết kế nội và ngoại thất" },
                    { 4, true, 2, 2, "SE", "Software Engineering", "kĩ thuật phần mềm" },
                    { 5, true, 2, 2, "WP", "Web Programming", "lập trình web" },
                    { 6, true, 2, 1, "GP", "Game Programming", "lập trình game" }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "subject_id", "assessment_method_id", "credit", "english_subject_name", "exam_total", "is_active", "learning_method_id", "subject_code", "subject_name", "total_time", "total_time_class" },
                values: new object[,]
                {
                    { 1, 1, 3, "Project Capstone", 3, true, 1, "SEP490", "Đồ án", 70, 40 },
                    { 2, 2, 3, "Mac-Lenin philosophy", 5, true, 1, "MLN131", "Triết học Mac-Lenin", 70, 40 },
                    { 3, 1, 3, "Soft Skill Group", 4, true, 2, "SSG104", "Kĩ năng trong làm việc nhóm", 70, 40 },
                    { 4, 2, 3, "Web api using asp.Net", 4, true, 2, "PRN231", "lập trình web api asp.Net", 70, 40 },
                    { 5, 1, 3, "Basic Game Programing", 4, true, 2, "PRU211", "lập trình game cơ bản", 70, 40 },
                    { 6, 1, 3, "Mathematics", 4, true, 2, "MAT101", "Toán học", 70, 40 },
                    { 7, 1, 3, "Physics", 4, true, 2, "PHY101", "Vật lý", 70, 40 },
                    { 8, 1, 3, "Chemistry", 4, true, 2, "CHE101", "Hóa học", 70, 40 },
                    { 9, 1, 3, "Biology", 4, true, 2, "BIO101", "Sinh học", 70, 40 },
                    { 10, 1, 3, "Linguistics", 4, true, 2, "LING101", "Ngôn ngữ học", 70, 40 },
                    { 11, 1, 3, "English and Literature", 4, true, 2, "ENG101", "Tiếng Anh và văn học", 70, 40 },
                    { 12, 1, 3, "History", 4, true, 2, "HIS101", "Lịch sử", 70, 40 },
                    { 13, 1, 3, "Political Science", 4, true, 2, "POL101", "Khoa học chính trị", 70, 40 },
                    { 14, 1, 3, "Social Science", 4, true, 2, "SOC101", "Khoa học xã hội", 70, 40 },
                    { 15, 1, 3, "Economics", 4, true, 2, "ECO101", "Kinh tế học", 70, 40 },
                    { 16, 1, 3, "Business Management", 4, true, 2, "BUS101", "Quản trị kinh doanh", 70, 40 },
                    { 17, 1, 3, "Finance", 4, true, 2, "FIN101", "Tài chính", 70, 40 },
                    { 18, 1, 3, "Information Systems", 4, true, 2, "IT101", "Hệ thống thông tin", 70, 40 },
                    { 19, 1, 3, "Computer Science", 4, true, 2, "CS101", "Công nghệ thông tin", 70, 40 },
                    { 20, 1, 3, "Mechanical Engineering", 4, true, 2, "MECH101", "Cơ khí học", 70, 40 },
                    { 21, 1, 3, "Electronics and Electrical Engineering", 4, true, 2, "ELEC101", "Điện tử và điện lạnh", 70, 40 },
                    { 22, 1, 3, "Architecture", 4, true, 2, "ARCH101", "Kiến trúc", 70, 40 },
                    { 23, 1, 3, "Art and Design", 4, true, 2, "ART101", "Nghệ thuật và thiết kế", 70, 40 },
                    { 24, 1, 3, "Music and Performing Arts", 4, true, 2, "MUSIC101", "Âm nhạc và nghệ thuật biểu diễn", 70, 40 },
                    { 25, 1, 3, "Foreign Language", 4, true, 2, "FOREIGN101", "Ngôn ngữ nước ngoài", 70, 40 },
                    { 26, 1, 3, "Geography", 4, true, 2, "GEO101", "Địa lý", 70, 40 },
                    { 27, 1, 3, "Environmental Science", 4, true, 2, "ENV101", "Môi trường học", 70, 40 },
                    { 28, 1, 3, "Psychology", 4, true, 2, "PSY101", "Tâm lý học", 70, 40 },
                    { 29, 1, 3, "Antropology", 4, true, 2, "ANTH101", "Antropology", 70, 40 },
                    { 30, 1, 3, "Economics 2", 4, true, 2, "ECO102", "Kinh tế học 2", 70, 40 },
                    { 31, 1, 3, "Business Management 2", 4, true, 2, "BUS102", "Quản trị kinh doanh 2", 70, 40 },
                    { 32, 1, 3, "Finance 2", 4, true, 2, "FIN102", "Tài chính 2", 70, 40 },
                    { 33, 1, 3, "Information Systems 2", 4, true, 2, "IT102", "Hệ thống thông tin 2", 70, 40 },
                    { 34, 1, 3, "Computer Science 2", 4, true, 2, "CS102", "Công nghệ thông tin 2", 70, 40 },
                    { 35, 1, 3, "Mechanical Engineering 2", 4, true, 2, "MECH102", "Cơ khí học 2", 70, 40 },
                    { 36, 1, 3, "Electronics and Electrical Engineering 2", 4, true, 2, "ELEC102", "Điện tử và điện lạnh 2", 70, 40 }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "subject_id", "assessment_method_id", "credit", "english_subject_name", "exam_total", "is_active", "learning_method_id", "subject_code", "subject_name", "total_time", "total_time_class" },
                values: new object[,]
                {
                    { 37, 1, 3, "Architecture 2", 4, true, 2, "ARCH102", "Kiến trúc 2", 70, 40 },
                    { 38, 1, 3, "Art and Design 2", 4, true, 2, "ART102", "Nghệ thuật và thiết kế 2", 70, 40 },
                    { 39, 1, 3, "Music and Performing Arts 2", 4, true, 2, "MUSIC102", "Âm nhạc và nghệ thuật biểu diễn 2", 70, 40 },
                    { 40, 1, 3, "Foreign Language 2", 4, true, 2, "FOREIGN102", "Ngôn ngữ nước ngoài 2", 70, 40 },
                    { 41, 1, 3, "Philosophy", 4, true, 2, "PHILO101", "Triết học", 70, 40 },
                    { 42, 1, 3, "Psychology 2", 4, true, 2, "PSYCH102", "Tâm lý học 2", 70, 40 },
                    { 43, 1, 3, "Linguistics 2", 4, true, 2, "LING102", "Ngôn ngữ học 2", 70, 40 },
                    { 44, 1, 3, "English and Literature 2", 4, true, 2, "ENG102", "Tiếng Anh và văn học 2", 70, 40 },
                    { 45, 1, 3, "Geography 2", 4, true, 2, "GEO102", "Địa lý 2", 70, 40 },
                    { 46, 1, 3, "Environmental Science 2", 4, true, 2, "ENV102", "Môi trường học 2", 70, 40 },
                    { 47, 1, 3, "Antropology 2", 4, true, 2, "ANTH102", "Antropology 2", 70, 40 },
                    { 48, 1, 3, "Economics 3", 4, true, 2, "ECO103", "Kinh tế học 3", 70, 40 },
                    { 49, 1, 3, "Business Management 3", 4, true, 2, "BUS103", "Quản trị kinh doanh 3", 70, 40 },
                    { 50, 1, 3, "Finance 3", 4, true, 2, "FIN103", "Tài chính 3", 70, 40 },
                    { 51, 1, 3, "Information Systems 3", 4, true, 2, "IT103", "Hệ thống thông tin 3", 70, 40 },
                    { 52, 1, 3, "Computer Science 3", 4, true, 2, "CS103", "Công nghệ thông tin 3", 70, 40 },
                    { 53, 1, 3, "Mechanical Engineering 3", 4, true, 2, "MECH103", "Cơ khí học 3", 70, 40 },
                    { 54, 1, 3, "Electronics and Electrical Engineering 3", 4, true, 2, "ELEC103", "Điện tử và điện lạnh 3", 70, 40 },
                    { 55, 1, 3, "Architecture 3", 4, true, 2, "ARCH103", "Kiến trúc 3", 70, 40 },
                    { 56, 1, 3, "Image Design using Photoshop", 4, true, 2, "MUL1013", "Thiết kế hình ảnh với Photoshop", 70, 40 }
                });

            migrationBuilder.InsertData(
                table: "Combo",
                columns: new[] { "combo_id", "combo_code", "combo_description", "combo_english_name", "combo_name", "is_active", "specialization_id" },
                values: new object[,]
                {
                    { 1, ".NET", "lập trình web với ngôn ngữ C#", "C# Programing", "Lập trình C#", true, 4 },
                    { 2, "JS", "kĩ sư lập trình với ngôn ngữ Nhật", "Japan Software", "kĩ sư Nhật Bản", true, 3 },
                    { 3, "KS", "kĩ sư lập trình với ngôn ngữ Hàn", "Korea Software", "kĩ sư Hàn Quốc", false, 2 },
                    { 4, "NodeJS", "lập trình web với NodeJS", "Web api using NodeJS", "Lập trình NodeJS", true, 1 }
                });

            migrationBuilder.InsertData(
                table: "Curriculum",
                columns: new[] { "curriculum_id", "Formality", "approved_date", "batch_id", "curriculum_code", "curriculum_description", "curriculum_name", "decision_No", "degree_level", "english_curriculum_name", "is_active", "specialization_id", "total_semester", "updated_date" },
                values: new object[,]
                {
                    { 1, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 1, "GD", "", "Thiết kế đồ họa", "360/QĐ-CĐFPL", "associate", "Graphic Design", true, 1, 7, null },
                    { 2, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 4, "GD", "", "Thiết kế mĩ thuật số", "360/QĐ-CĐFPL", "international associate ", "Graphic Design", true, 1, 7, null },
                    { 3, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, "SE", "", "kĩ sư phần mềm", "360/QĐ-CĐFPL", "associate", "Software Engineering", true, 4, 7, null },
                    { 4, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 2, "SE", "", "kĩ thuật phần mềm", "360/QĐ-CĐFPL", "international associate ", "Software Engineering", true, 4, 7, null },
                    { 5, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, "CM", "", "quản lí học liệu", "360/QĐ-CĐFPL", "vocational diploma", "Curriculum Management", true, 2, 7, null },
                    { 6, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, "SS", "", "kĩ năng mềm", "360/QĐ-CĐFPL", "vocational diploma", "Soft Skill", true, 1, 7, null },
                    { 7, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, "SWP", "", "kĩ năng lập trình web", "360/QĐ-CĐFPL", "associate", "Skill Web Program", false, 1, 7, null },
                    { 8, "formal education", new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, "SS", "", "kĩ năng mềm", "360/QĐ-CĐFPL", "vocational diploma", "Soft Skill", true, 1, 7, null }
                });

            migrationBuilder.InsertData(
                table: "PreRequisite",
                columns: new[] { "pre_subject_id", "subject_id", "pre_requisite_type_id" },
                values: new object[,]
                {
                    { 2, 1, 1 },
                    { 3, 2, 2 },
                    { 5, 3, 3 },
                    { 7, 4, 4 },
                    { 8, 5, 1 },
                    { 12, 10, 1 },
                    { 14, 11, 2 },
                    { 17, 15, 3 }
                });

            migrationBuilder.InsertData(
                table: "CurriculumSubject",
                columns: new[] { "curriculum_id", "subject_id", "combo_id", "option", "subject_group", "term_no" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "General Subject", 3 },
                    { 1, 2, 3, true, "Option Subject", 1 },
                    { 1, 3, 2, true, "Option Subject", 3 },
                    { 1, 5, 0, false, "Basic Subject", 2 },
                    { 2, 4, 0, false, "Specialization Subject", 3 }
                });

            migrationBuilder.InsertData(
                table: "PLOs",
                columns: new[] { "PLO_id", "PLO_description", "PLO_name", "curriculum_id" },
                values: new object[,]
                {
                    { 1, "Thiết kế xử lý hình ảnh, xây dựng các sản phẩm đồ họa 2D", "PLO01", 1 },
                    { 2, "Thiết kế theo các chủ đề: xây dựng thương hiệu, ấn phẩm quảng cáo, bao bì", "PLO02", 1 },
                    { 3, "Biên tập, kịch bản và xử lý kỹ xảo phim, phim quảng cáo, phim quảng cáo 3D", "PLO03", 1 },
                    { 4, "Thiết kếm xây dưng các sản phẩm đồ họa nội ngoại thất 2D&3D hoặc các sản phẩm đồ họa 3D", "PLO04", 1 },
                    { 5, "Kiến thức về đường, hình, khối và một số vấn đề mỹ thuật liên quan; kiến thức cơ bản về đồ họa; kiến thức cơ sở về mỹ thuật, thẩm mỹ; vật liệu,...", "PLO05", 1 },
                    { 6, "Giao tiếp, tìm hiểu, nắm bắt nhu cầu của khách hàng, tư vấn cho khách hàng, làm được sản phẩm theo yêu cầu của khách hàng", "PLO06", 1 },
                    { 7, "Giao tiếp, thuyết trình tự tin trước đám đông", "PLO07", 2 },
                    { 8, "Kĩ năng làm việc nhóm", "PLO08", 2 }
                });

            migrationBuilder.InsertData(
                table: "PLOMapping",
                columns: new[] { "PLO_id", "subject_id" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 3 },
                    { 2, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentMethod_assessment_type_id",
                table: "AssessmentMethod",
                column: "assessment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_CLO_syllabus_id",
                table: "CLO",
                column: "syllabus_id");

            migrationBuilder.CreateIndex(
                name: "IX_Combo_specialization_id",
                table: "Combo",
                column: "specialization_id");

            migrationBuilder.CreateIndex(
                name: "IX_ComboCurriculum_curriculum_id",
                table: "ComboCurriculum",
                column: "curriculum_id");

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
                name: "IX_Semester_batch_id",
                table: "Semester",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterBatch_batch_id",
                table: "SemesterBatch",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterBatch_semester_id",
                table: "SemesterBatch",
                column: "semester_id");

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
                name: "SemesterBatch");

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
                name: "Batch");

            migrationBuilder.DropTable(
                name: "AssessmentType");
        }
    }
}
