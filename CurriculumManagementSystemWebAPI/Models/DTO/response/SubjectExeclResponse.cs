using MiniExcelLibs.Attributes;

namespace CurriculumManagementSystemWebAPI.Models.DTO.response
{
    public class SubjectExeclResponse
    {
        [ExcelColumnName("SubjectCode")]
        [ExcelColumnWidth(15)]
        public string subject_code { get; set; }
        [ExcelColumnName("SubjectName")]
        [ExcelColumnWidth(50)]
        public string subject_name { get; set; }
        [ExcelColumnName("English SubjectName")]
        [ExcelColumnWidth(50)]
        public string english_subject_name { get; set; }
        [ExcelColumnName("Learning Method")]
        [ExcelColumnWidth(20)]
        public string learning_method_name { get; set; }
        [ExcelColumnName("Assessment Method")]
        [ExcelColumnWidth(20)]
        public string assessment_method_name { get; set; }
        [ExcelColumnName("Credit")]
        [ExcelColumnWidth(10)]
        public int credit { get; set; }
        [ExcelColumnName("Option")]
        [ExcelColumnWidth(10)]
        public string option { get; set; }
        [ExcelColumnName("Is Active")]
        [ExcelColumnWidth(12)]
        public bool is_active { get; set; }
    }
}
