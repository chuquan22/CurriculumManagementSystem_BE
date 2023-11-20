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
                name: "DegreeLevel",
                columns: table => new
                {
                    degree_level_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    degree_level_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    degree_level_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    degree_level_english_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeLevel", x => x.degree_level_id);
                });

            migrationBuilder.CreateTable(
                name: "LearningMethod",
                columns: table => new
                {
                    learning_method_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    learning_method_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    learning_method_code = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    learning_resource_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    learning_resouce_code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningResource", x => x.learning_resource_id);
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
                name: "Batch",
                columns: table => new
                {
                    batch_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    batch_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    batch_order = table.Column<int>(type: "int", nullable: false),
                    degree_level_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.batch_id);
                    table.ForeignKey(
                        name: "FK_Batch_DegreeLevel_degree_level_id",
                        column: x => x.degree_level_id,
                        principalTable: "DegreeLevel",
                        principalColumn: "degree_level_id",
                        onDelete: ReferentialAction.Cascade);
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
                    degree_level_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.major_id);
                    table.ForeignKey(
                        name: "FK_Major_DegreeLevel_degree_level_id",
                        column: x => x.degree_level_id,
                        principalTable: "DegreeLevel",
                        principalColumn: "degree_level_id",
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
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Semester",
                columns: table => new
                {
                    semester_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    semester_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester_start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    semester_end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    school_year = table.Column<int>(type: "int", nullable: false),
                    start_batch_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semester", x => x.semester_id);
                    table.ForeignKey(
                        name: "FK_Semester_Batch_start_batch_id",
                        column: x => x.start_batch_id,
                        principalTable: "Batch",
                        principalColumn: "batch_id",
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
                name: "Syllabus",
                columns: table => new
                {
                    syllabus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    document_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    decision_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    degree_level_id = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_Syllabus_DegreeLevel_degree_level_id",
                        column: x => x.degree_level_id,
                        principalTable: "DegreeLevel",
                        principalColumn: "degree_level_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Syllabus_Subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
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
                name: "Question",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    question_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quiz_id = table.Column<int>(type: "int", nullable: false),
                    answers_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answers_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answers_C = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answers_D = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    assigment = table.Column<float>(type: "real", nullable: false),
                    CLO_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Combo",
                columns: table => new
                {
                    combo_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    combo_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    combo_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    combo_english_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    decision_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Formality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculum", x => x.curriculum_id);
                    table.ForeignKey(
                        name: "FK_Curriculum_Specialization_specialization_id",
                        column: x => x.specialization_id,
                        principalTable: "Specialization",
                        principalColumn: "specialization_id",
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
                name: "CurriculumBatch",
                columns: table => new
                {
                    curriculum_id = table.Column<int>(type: "int", nullable: false),
                    batch_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumBatch", x => new { x.curriculum_id, x.batch_id });
                    table.ForeignKey(
                        name: "FK_CurriculumBatch_Batch_batch_id",
                        column: x => x.batch_id,
                        principalTable: "Batch",
                        principalColumn: "batch_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumBatch_Curriculum_curriculum_id",
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
                    semester_id = table.Column<int>(type: "int", nullable: false),
                    term_no = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterPlan", x => new { x.semester_id, x.curriculum_id, x.term_no });
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
                    { 1, "On-going" },
                    { 2, "Final Exam" }
                });

            migrationBuilder.InsertData(
                table: "ClassSessionType",
                columns: new[] { "class_session_type_id", "class_session_type_name" },
                values: new object[,]
                {
                    { 1, "Online" },
                    { 2, "Offline" }
                });

            migrationBuilder.InsertData(
                table: "DegreeLevel",
                columns: new[] { "degree_level_id", "degree_level_code", "degree_level_english_name", "degree_level_name" },
                values: new object[,]
                {
                    { 1, "CD", "Associate Degree", "Cao Đẳng" },
                    { 2, "IC", "International Associate Degree", "Cao Đẳng Quốc Tế" },
                    { 3, "TC", "Vocational Secondary", "Phổ Thông Cao Đẳng" }
                });

            migrationBuilder.InsertData(
                table: "LearningMethod",
                columns: new[] { "learning_method_id", "learning_method_code", "learning_method_name" },
                values: new object[,]
                {
                    { 1, "T01", "Online" },
                    { 2, "T02", "Blended" },
                    { 3, "T03", "Traditional" }
                });

            migrationBuilder.InsertData(
                table: "LearningResource",
                columns: new[] { "learning_resource_id", "learning_resouce_code", "learning_resource_type" },
                values: new object[,]
                {
                    { 1, "T01", "Internet" },
                    { 2, "T02", "Purchased book" },
                    { 3, "T03", "Free e-book" },
                    { 4, "T04", "Officially published" },
                    { 5, "T05", "Self-compiled" },
                    { 6, "T06", "Udemy course" }
                });

            migrationBuilder.InsertData(
                table: "PreRequisiteType",
                columns: new[] { "pre_requisite_type_id", "pre_requisite_type_name" },
                values: new object[,]
                {
                    { 1, "Corequisite" },
                    { 2, "Prerequisite" },
                    { 3, "Recommended" },
                    { 4, "Elective" },
                    { 5, "Passed" },
                    { 6, "Participated" }
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
                    { 1, "Assignment", 2 },
                    { 2, "Bài học online", 1 },
                    { 3, "Lab", 1 },
                    { 4, "Bảo vệ assignment", 2 },
                    { 5, "Quiz", 1 },
                    { 6, "Đánh giá Assignment GĐ 1", 1 },
                    { 7, "Đánh giá Assignment GĐ 2", 1 }
                });

            migrationBuilder.InsertData(
                table: "Batch",
                columns: new[] { "batch_id", "batch_name", "batch_order", "degree_level_id" },
                values: new object[,]
                {
                    { 1, "7.1", 1, 3 },
                    { 2, "17", 1, 2 },
                    { 3, "18", 2, 2 },
                    { 4, "19.1", 1, 1 },
                    { 5, "19.2", 2, 1 },
                    { 6, "19.3", 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "Major",
                columns: new[] { "major_id", "degree_level_id", "is_active", "major_code", "major_english_name", "major_name" },
                values: new object[,]
                {
                    { 1, 1, true, "6210402", "Graphic Design", "Thiết kế đồ họa" },
                    { 2, 1, true, "6480201", "Information Technology", "Công nghệ thông tin" },
                    { 3, 1, true, "6340404", "Business Administration", "Quản trị kinh doanh" },
                    { 4, 1, true, "6510305", "Automation Engineering", "CNKTĐK & Tự động hóa" },
                    { 5, 3, true, "5340404", "Business Administration", "Quản trị kinh doanh" },
                    { 6, 2, true, "6480207", "Computing", "Lập trình máy tính" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "user_id", "full_name", "is_active", "refresh_token", "role_id", "user_email", "user_name" },
                values: new object[,]
                {
                    { 1, "Chu Quang Quan", true, null, 3, "quancqhe153661@fpt.edu.vn", "QuanCQ" },
                    { 2, "Nguyen Thi Thu", true, null, 2, "thunthe151440@fpt.edu.vn", "ThuNT" }
                });

            migrationBuilder.InsertData(
                table: "Semester",
                columns: new[] { "semester_id", "school_year", "semester_end_date", "semester_name", "semester_start_date", "start_batch_id" },
                values: new object[,]
                {
                    { 1, 2023, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fall", new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 2, 2023, new DateTime(2023, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Summer", new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 3, 2023, new DateTime(2023, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring", new DateTime(2023, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 4, 2022, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fall", new DateTime(2022, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 5, 2022, new DateTime(2022, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Summer", new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 6, 2022, new DateTime(2022, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "subject_id", "assessment_method_id", "credit", "english_subject_name", "exam_total", "is_active", "learning_method_id", "subject_code", "subject_name", "total_time", "total_time_class" },
                values: new object[,]
                {
                    { 1, 1, 3, "Communication crisis management", 56, true, 2, "PRE209", "Quản trị khủng hoảng truyền thông", 90, 34 },
                    { 2, 1, 3, "Graduation Internship (Digital Marketing)", 41, true, 3, "PRO109", "Thực tập tốt nghiệp(TMDT)", 75, 34 },
                    { 3, 1, 3, "English 2.2", 41, true, 3, "ENT2227", "Tiếng anh 2.2", 75, 34 },
                    { 4, 1, 3, "English 2.1", 41, true, 3, "ENT2127", "Tiếng anh 2.1", 75, 34 },
                    { 5, 1, 3, "English 1.2", 4, true, 3, "ENT1227", "Tiếng anh 1.2", 70, 40 },
                    { 6, 1, 3, "English 2.2", 4, true, 2, "ENT2226", "Tiếng anh 2.2", 70, 40 },
                    { 7, 1, 3, "English 2.1", 4, true, 2, "ENT2126", "Tiếng anh 2.1", 70, 40 },
                    { 8, 1, 3, "English 1.1", 4, true, 2, "ENT1127", "Tiếng anh 1.1", 70, 40 },
                    { 9, 1, 3, "English 1.2", 56, true, 2, "ENT1226", "Tiếng anh 1.2", 90, 34 },
                    { 10, 1, 3, "English for Hopitality 3", 56, true, 2, "EHO0101", "Tiếng anh chuyên ngành 3 (NHKS)", 90, 34 },
                    { 11, 1, 3, "English 1.1", 56, true, 3, "ENT1128", "Tiếng anh 1.1", 90, 31 },
                    { 12, 1, 3, "English for Hopitality 1", 56, true, 2, "EHO102", "Tiếng anh chuyên ngành 1 (NHKS)", 90, 34 },
                    { 13, 1, 3, "Informatics", 4, true, 2, "COM1071", "Tin học", 70, 40 },
                    { 14, 1, 3, "Image Design using Photoshop", 4, true, 2, "MUL1013", "Thiết kế hình ảnh với Photoshop", 70, 40 },
                    { 15, 1, 3, "Graphic Introduction", 4, true, 2, "MUL116", "Nhập môn đồ họa", 70, 40 },
                    { 16, 1, 3, "Learning Skills", 4, true, 2, "PDP102", "Kỹ năng học tập", 70, 40 },
                    { 17, 1, 3, "Physical education", 4, true, 2, "VIE103", "Gíao dục thể chất - Vovinam", 70, 40 },
                    { 18, 1, 3, "Illustration Design using Adobe Illustrator", 4, true, 2, "MUL1024", "Thiết kế minh họa với Illustrator", 70, 40 },
                    { 19, 1, 3, "Perspective and Layout in Graphic Design", 4, true, 2, "MUL1143", "Luật xa gần và bố cục trong thiết kế đồ họa", 70, 40 },
                    { 20, 1, 3, "Publication Design using InDesign", 4, true, 2, "MUL2111", "Chế bản điện tử với InDesign", 70, 40 },
                    { 21, 1, 3, "Color", 4, true, 2, "MUL2143", "Màu sắc", 70, 40 },
                    { 22, 1, 3, "Politics", 4, true, 2, "VIE1016", "Chính trị", 70, 40 },
                    { 23, 1, 3, "Packaging Design", 4, true, 2, "MUL2123", "Thiết kế bao bì", 70, 40 },
                    { 24, 1, 3, "Typography", 4, true, 2, "MUL2133", "Nghệ thuật chữ", 70, 40 },
                    { 25, 1, 3, "Digital Marketing and Media Concepts", 4, true, 2, "MUL3191", "Thiết kế thương hiệu và Maketing", 70, 40 },
                    { 26, 1, 3, "Personal Development Skills", 4, true, 2, "PDP103", "Kỹ năng phát triển bản thân", 70, 40 },
                    { 27, 1, 3, "Project 1 (Graphic Design)", 4, true, 2, "PRO1112", "Dự án 1 (TKĐH)", 70, 40 },
                    { 28, 1, 3, "Photography and Retouch", 4, true, 2, "MUL117", "Kỹ thuật nhiếp ảnh", 70, 40 },
                    { 29, 1, 3, "Printing Technical", 4, true, 2, "MUL215", "Kỹ thuật in", 70, 40 },
                    { 30, 1, 3, "Adobe Animate CC", 4, true, 2, "MUL216", "Thiết kế đa truyền thông với Animate", 70, 40 },
                    { 31, 1, 3, "Creative Idea", 4, true, 2, "MUL217", "ý tưởng sáng tạo", 70, 40 },
                    { 32, 1, 3, "3D Modeling using Maya", 4, true, 2, "MUL222", "Dựng hình với Maya 3D", 70, 40 },
                    { 33, 1, 3, "Storyboarding Advertisement", 4, true, 2, "MUL223", "Kịch bản phi quảng cáo", 70, 40 },
                    { 34, 1, 3, "Principles of Interior Design 1", 4, true, 2, "MUL225", "Nguyên lý thiết kế nội thất 1", 70, 40 },
                    { 35, 1, 3, "Autocad 2D", 4, true, 2, "MUL317", "Autocad 2D", 70, 40 },
                    { 36, 1, 3, "Interior design with 3D Max", 4, true, 2, "MUL2211", "Thiết kế nội thất với 3D Max", 70, 40 }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "subject_id", "assessment_method_id", "credit", "english_subject_name", "exam_total", "is_active", "learning_method_id", "subject_code", "subject_name", "total_time", "total_time_class" },
                values: new object[,]
                {
                    { 37, 1, 3, "Filming and editing using Adobe Premiere", 4, true, 2, "MUL224", "Quay và dựng phim với Adobe Premiere", 70, 40 },
                    { 38, 1, 3, "Principles of Interior Design 2", 4, true, 2, "MUL226", "Nguyên lý thiết kế nội thất 2", 70, 40 },
                    { 39, 1, 3, "Post Processing using Premier", 4, true, 2, "MUL3151", "Xử lý hậu kỳ với Adobe Premiere", 70, 40 },
                    { 40, 1, 3, "Adobe After Effect", 4, true, 2, "MUL3211", "Hiệu ứng kỹ xảo với Adobe After Effect", 70, 40 },
                    { 41, 1, 3, "Creating Perspectives using SketchUp", 4, true, 2, "MUL322", "Dựng phối cảnh với SketchUp", 70, 40 },
                    { 42, 1, 3, "3D Modeling using C4D", 4, true, 2, "MUL323", "Dựng phim với C4D", 70, 40 },
                    { 43, 1, 3, "Motion using C4D", 4, true, 2, "MUL324", "Chuyển động với C4D", 70, 40 },
                    { 44, 1, 3, "Professional Skills", 4, true, 2, "PDP104", "Kỹ năng làm việc", 70, 40 },
                    { 45, 1, 3, "Startup Your Business", 4, true, 2, "SYB3012", "Khởi sự doanh nghiệp", 70, 40 },
                    { 46, 1, 3, "Graduation Internship (Graphic Design)", 4, true, 2, "PRO119", "Thực tập tốt nghiệp (TKĐH)", 70, 40 },
                    { 47, 1, 3, "Graduation Project (Film and Ads)", 4, true, 2, "PRO221", "Dự án tốt nghiệp (TKĐH-Phim và Quảng cáo)", 70, 40 },
                    { 48, 1, 3, "Graduation Project (Interior and Exterior)", 4, true, 2, "PRO223", "Dự án tốt nghiệp (TKĐH Nội và Ngoại)", 70, 40 },
                    { 49, 1, 3, "Law", 4, true, 2, "VIE1026", "Pháp luật", 70, 40 },
                    { 50, 1, 4, "Defense Education", 3, true, 3, "VIE104", "giáo dục quốc phòng", 75, 71 },
                    { 51, 1, 3, "Introduction to Sofware Engineering", 4, true, 2, "SOF102", "Nhập môn kỹ thuật phần mềm", 70, 40 },
                    { 52, 1, 3, "Public Speaking Skills", 4, true, 2, "PRE1022", "Kỹ năng thuyết trình trước công chúng ", 70, 40 },
                    { 53, 1, 3, "Transportation Insurance", 4, true, 2, "LOG211", "Bảo hiểm vận tải", 70, 40 },
                    { 54, 1, 3, "Tourguide Operations 1", 4, true, 2, "TOU2033", "Nghiệp vụ hướng dẫn 1", 70, 40 },
                    { 55, 1, 3, "Tourguide Operations 1", 4, true, 2, "TOU2032", "Nghiệp vụ hướng dẫn 1", 70, 40 },
                    { 56, 1, 3, "Procurement Oprerations", 41, true, 2, "LOG105", "Nghiệp vụ mua sắm", 75, 34 },
                    { 57, 4, 3, "Electrical circuits and electrical safety", 56, true, 3, "AUT102", "Mạch điện và an toàn điện", 90, 34 },
                    { 58, 4, 3, "Basic electronics", 56, true, 3, "AUT103", "Điện tử cơ bản", 90, 34 },
                    { 59, 4, 3, "Electrical and electronic circuit design", 56, true, 2, "AUT104", "Thiết kế mạch điện - điện tử", 90, 34 },
                    { 60, 5, 3, "Pulse & Digital Engineering", 56, true, 3, "AUT105", "Kỹ thuật xung số", 90, 34 },
                    { 61, 1, 3, "Electronic circuits and applications", 56, true, 3, "AUT106", "Mạch điện tử và ứng dụng", 90, 34 },
                    { 62, 1, 3, "Installation of electrical systems", 56, true, 3, "AUT107", "Lắp đặt hệ thống điện", 90, 34 },
                    { 63, 4, 3, "Installation of industrial electrical cabinets", 56, true, 3, "AUT108", "Lắp đặt tủ điện công nghiệp", 90, 34 },
                    { 64, 5, 3, "Sensor engineering", 56, true, 3, "AUT110", "Kỹ thuật cảm biến", 90, 34 },
                    { 65, 1, 3, "Electric drive", 41, true, 3, "INE202", "Truyền động điện", 75, 34 },
                    { 66, 4, 3, "Project 1 (Automation Engineering )", 56, true, 3, "PRO125", "Dự án 1 (TĐH)", 90, 34 },
                    { 67, 5, 3, "Automatic control theory", 56, true, 3, "AUT109", "Lý thuyết điều khiển tự động", 90, 34 },
                    { 68, 3, 3, "Basic Programmable Logic Controller", 56, true, 3, "AUT206", "PLC cơ bản", 90, 34 },
                    { 69, 3, 3, "Arduino Programming", 56, true, 3, "AUT208", "Lập trình Arduino", 90, 34 },
                    { 70, 5, 3, "Industrial communication network", 56, true, 3, "AUT209", "Mạng truyền thông công nghiệp", 90, 34 },
                    { 71, 3, 3, "Software design", 56, true, 3, "AUT210", "Thiết kế bằng phần mềm", 90, 34 },
                    { 72, 5, 3, "Hydraulic & Pneumatic Control", 56, true, 3, "AUT211", "Điều khiển thủy lực và khí nén", 90, 34 },
                    { 73, 3, 3, "Microcontrollers", 56, true, 3, "INE214", "Vi điều khiển", 90, 34 },
                    { 74, 3, 3, "Advanced Programmable Logic Controller", 56, true, 3, "AUT207", "PLC nâng cao", 90, 34 },
                    { 75, 3, 3, "Embedded programming", 56, true, 3, "AUT212", "Lập trình nhúng", 90, 34 },
                    { 76, 5, 3, "Recycled energy", 6, true, 3, "AUT213", "Năng lượng tái tạo", 60, 34 },
                    { 77, 5, 3, "Process automation technology", 56, true, 3, "AUT214", "Tự động hóa quá trình công nghệ", 90, 34 },
                    { 78, 3, 3, "IoTs application development", 56, true, 3, "AUT215", "Phát triển ứng dụng IoTs", 90, 34 }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "subject_id", "assessment_method_id", "credit", "english_subject_name", "exam_total", "is_active", "learning_method_id", "subject_code", "subject_name", "total_time", "total_time_class" },
                values: new object[,]
                {
                    { 79, 4, 3, "Industrial Robots", 56, true, 3, "AUT216", "Robot công nghiệp", 90, 34 },
                    { 80, 3, 3, "Mobile robot", 56, true, 3, "AUT217", "Robot di động", 90, 34 },
                    { 81, 5, 2, "Law", 3, true, 1, "VIE102", "Pháp luật", 30, 8 },
                    { 82, 4, 5, "Capstone Project (Automation Engineering)", 6, true, 3, "PRO215", "Dự án tốt nghiệp (TĐH)", 225, 10 },
                    { 83, 1, 5, "Graduation Internship (Automation Engineering)", 0, true, 3, "PRO126", "Thực tập tốt nghiệp (TĐH)", 225, 0 },
                    { 84, 5, 3, "Personal Development Program 1", 56, true, 2, "PDP101", "Phát triển cá nhân 1", 90, 34 },
                    { 85, 3, 3, "Basic Marketing", 56, true, 2, "MAR1021", "Marketing căn bản", 90, 34 },
                    { 86, 3, 3, "Introduction to Digital Marketing", 6, true, 2, "DOM101", "Nhập môn Digital Marketing", 90, 34 },
                    { 87, 5, 3, "Email Marketing", 6, true, 1, "DOM1021", "Email Marketing", 90, 12 },
                    { 88, 3, 3, "Content Marketing", 6, true, 2, "DOM1031", "Marketing nội dung", 90, 34 },
                    { 89, 1, 3, "Web Design", 56, true, 2, "WEB107", "Thiết kế trang web", 90, 34 }
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
                table: "Quiz",
                columns: new[] { "quiz_id", "quiz_name", "subject_id" },
                values: new object[,]
                {
                    { 1, "Quiz 1", 1 },
                    { 2, "Quiz 2", 2 },
                    { 3, "Quiz 3", 1 }
                });

            migrationBuilder.InsertData(
                table: "Specialization",
                columns: new[] { "specialization_id", "is_active", "major_id", "semester_id", "specialization_code", "specialization_english_name", "specialization_name" },
                values: new object[,]
                {
                    { 1, true, 1, 1, "6216432", "Interior and Exterior Design", "Thiết kế nội và ngoại thất" },
                    { 2, true, 1, 2, "6215463", "Film Making and Advertising", "Dựng phim và quảng cáo" },
                    { 3, true, 2, 3, "6526432", "Game Development", "Lập trình Game" },
                    { 4, true, 2, 1, "6480201", "Software Application", "Ứng dụng phần mềm" },
                    { 5, true, 5, 6, "5340404", "Digital Marketing", "Digital Marketing" },
                    { 6, true, 6, 5, "6480207", "Data Analytics", "Phân tích dữ liệu" }
                });

            migrationBuilder.InsertData(
                table: "Combo",
                columns: new[] { "combo_id", "combo_code", "combo_english_name", "combo_name", "is_active", "specialization_id" },
                values: new object[,]
                {
                    { 1, ".NET", "C# Programing", "Lập trình C#", true, 4 },
                    { 2, "JS", "Japan Software", "kĩ sư Nhật Bản", true, 3 },
                    { 3, "KS", "Korea Software", "kĩ sư Hàn Quốc", false, 2 },
                    { 4, "NodeJS", "Web api using NodeJS", "Lập trình NodeJS", true, 1 }
                });

            migrationBuilder.InsertData(
                table: "Curriculum",
                columns: new[] { "curriculum_id", "Formality", "approved_date", "curriculum_code", "curriculum_description", "curriculum_name", "decision_No", "english_curriculum_name", "is_active", "specialization_id", "total_semester" },
                values: new object[,]
                {
                    { 1, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "GD-IED-CD-18.3", "", "Thiết kế đồ họa", "360/QĐ-CĐFPL", "Graphic Design", true, 1, 7 },
                    { 2, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "GD-IED-IC-19.1", "", "Thiết kế mĩ thuật số", "360/QĐ-CĐFPL", "Graphic Design", true, 1, 7 },
                    { 3, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "SE-SE-IC-18", "", "kĩ sư phần mềm", "360/QĐ-CĐFPL", "Software Engineering", true, 4, 7 },
                    { 4, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "SE-SE-CD-17", "", "kĩ thuật phần mềm", "360/QĐ-CĐFPL", "Software Engineering", true, 5, 7 },
                    { 5, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "CM-FMA-TC-19.3", "", "quản lí học liệu", "360/QĐ-CĐFPL", "Curriculum Management", true, 2, 6 },
                    { 6, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "SS-IED-TC-19", "", "kĩ năng mềm", "360/QĐ-CĐFPL", "Soft Skill", true, 4, 6 },
                    { 7, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "SWP-WP-TC-7.1", "", "kĩ năng lập trình web", "360/QĐ-CĐFPL", "Skill Web Program", true, 6, 7 },
                    { 8, "formal education", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Local), "SS-IED-TC-18.2", "", "kĩ năng mềm", "360/QĐ-CĐFPL", "Soft Skill", true, 1, 7 }
                });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "question_id", "answers_A", "answers_B", "answers_C", "answers_D", "correct_answer", "question_name", "question_type", "quiz_id" },
                values: new object[,]
                {
                    { 1, "Che dấu và bảo mật dữ liệu", "Hiển thị dữ liệu một cách tùy biến", "Thực thi nhanh hơn các câu lệnh truy vấn do đã được biên dịch sẵn", "Tất cả đáp án đều đúng", "D", "Lợi ích khi sử dụng View?", "Single Choice", 1 },
                    { 2, "Cột chứa giá trị được tính toán từ nhiều cột khác phải được đặt tên", "Cột chứa giá trị được tính toán từ nhiều cột khác không được đặt tên", "Thực thi nhanh hơn các câu lệnh truy vấn do đã được biên dịch sẵn", "Tất cả đáp án đều đúng", "D", "Qui đinh đặt tên cột trong View?", "Single Choice", 2 },
                    { 3, "Xem dữ liệu và cập nhật dữ liệu trong các bảng cơ sở qua View", "Xem dữ liệu", "cập nhật dữ liệu trong các bảng cơ sở qua View", "Tất cả đáp án đều sai", "A", "VIEW có thể cập nhật (updatable view) cho phép?", "Single Choice", 1 }
                });

            migrationBuilder.InsertData(
                table: "CurriculumBatch",
                columns: new[] { "batch_id", "curriculum_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 4, 6 },
                    { 6, 7 }
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
                name: "IX_Batch_degree_level_id",
                table: "Batch",
                column: "degree_level_id");

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
                name: "IX_Curriculum_specialization_id",
                table: "Curriculum",
                column: "specialization_id");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumBatch_batch_id",
                table: "CurriculumBatch",
                column: "batch_id");

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
                name: "IX_Major_degree_level_id",
                table: "Major",
                column: "degree_level_id");

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
                name: "IX_Semester_start_batch_id",
                table: "Semester",
                column: "start_batch_id");

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
                name: "IX_Subject_assessment_method_id",
                table: "Subject",
                column: "assessment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_learning_method_id",
                table: "Subject",
                column: "learning_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabus_degree_level_id",
                table: "Syllabus",
                column: "degree_level_id");

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
                name: "CurriculumBatch");

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

            migrationBuilder.DropTable(
                name: "DegreeLevel");
        }
    }
}
