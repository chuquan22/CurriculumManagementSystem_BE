using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CurriculumSubjectDAO
    {
        public readonly CMSDbContext _context = new CMSDbContext();
        private SubjectDAO SubjectDAO = new SubjectDAO();
        public List<CurriculumSubject> GetCurriculumBySubject(int subjectId)
        {
            var listCurriculumSubject = _context.CurriculumSubject
                .Include(x => x.Curriculum)
                .Include(x => x.Subject)
                .Where(x => x.subject_id == subjectId)
                .ToList();
            return listCurriculumSubject;
        }

        public bool CurriculumSubjectExist(int curriId, int subId)
        {
            return (_context.CurriculumSubject?.Any(e => e.curriculum_id == curriId && e.subject_id == subId)).GetValueOrDefault();
        }

        public List<CurriculumSubject> GetListSubjectByCurriculum(int curriculumId)
        {
            var listCurriculumSubject = _context.CurriculumSubject
                .Include(x => x.Curriculum)
                .Include(x => x.Subject)
                .Where(x => x.curriculum_id == curriculumId)
                .ToList();
            return listCurriculumSubject;
        }


        public List<Subject> GetListSubject(int curriculumId)
        {
            var listSubjectIds = _context.Curriculum
                .Where(x => x.curriculum_id == curriculumId && x.is_active == true)
                .Join(_context.CurriculumSubject,
                    curriculum => curriculum.curriculum_id,
                    curriculumSubject => curriculumSubject.curriculum_id,
                     (curriculum, curriculumSubject) => curriculumSubject.subject_id)
                .ToList();

            var allSubjects = _context.Subject.ToList();

            var listSubjectsNotInCurriculum = allSubjects
                .Where(subject => !listSubjectIds.Contains(subject.subject_id))
                .ToList();

            return listSubjectsNotInCurriculum;
        }

        public List<CurriculumSubject> GetListCurriculumSubject(int curriculumId)
        {
            var listCurriculumSubject = _context.CurriculumSubject
                .Include(x => x.Subject)
                .Include(x => x.Curriculum)
                .Where(x => x.curriculum_id == curriculumId)
                .ToList();

            return listCurriculumSubject;
        }

        public bool CheckSubjectComboOrOptionMustBeEqualCreditAndToTalTime(List<CurriculumSubject> listCurriSubject)
        {
            var subjectTemp = new Subject();
            if (listCurriSubject.Count <= 2)
            {
                foreach (var curriSubject in listCurriSubject)
                {
                    var subject = _context.Subject.Where(x => x.subject_id == curriSubject.subject_id).FirstOrDefault();
                    var curriCombo = _context.CurriculumSubject.Where(x => x.curriculum_id == curriSubject.curriculum_id && x.combo_id.HasValue && x.combo_id != 0 && x.term_no == curriSubject.term_no).FirstOrDefault();
                    if (curriSubject.combo_id.HasValue && curriSubject.combo_id != 0 && curriCombo != null)
                    {
                        return CheckComboCreditOrTotalTime(curriSubject, subject);
                    }
                    if (subjectTemp.subject_id == 0)
                    {
                        subjectTemp = subject;
                    }
                    else if (subjectTemp.credit != subject.credit || subjectTemp.total_time != subject.total_time)
                    {
                        return true;
                    }
                }
            }
            else
            {
                var checkCredit = true;
                var checkTotalTime = true;
                foreach (var curriSubject in listCurriSubject)
                {
                    var subject = _context.Subject.FirstOrDefault(x => x.subject_id == curriSubject.subject_id);
                    var curriCombo = listCurriSubject.Where(x => x.curriculum_id == curriSubject.curriculum_id && x.combo_id.HasValue && x.combo_id != 0 && x.term_no == curriSubject.term_no).FirstOrDefault();
                    var curriOption = listCurriSubject.Where(x => x.curriculum_id == curriSubject.curriculum_id && curriSubject.option.HasValue && x.term_no == curriSubject.term_no).FirstOrDefault();
                    if (curriSubject.combo_id.HasValue && curriSubject.combo_id != 0 && curriCombo != null)
                    {
                        checkCredit = listCurriSubject
                          .Any(x => x.curriculum_id == curriSubject.curriculum_id && x.combo_id.HasValue && x.combo_id != 0 && x.term_no == curriSubject.term_no && SubjectDAO.GetSubjectById(x.subject_id).credit == subject.credit);

                        checkTotalTime = listCurriSubject
                            .Any(x => x.curriculum_id == curriSubject.curriculum_id && x.combo_id.HasValue && x.combo_id != 0 && x.term_no == curriSubject.term_no && SubjectDAO.GetSubjectById(x.subject_id).total_time == subject.total_time);
                    }
                    if (curriSubject.option.HasValue && curriSubject.option != 0 && curriOption != null)
                    {
                        checkCredit = listCurriSubject
                          .Any(x => x.curriculum_id == curriSubject.curriculum_id && x.option.HasValue && x.option == curriSubject.option && x.term_no == curriSubject.term_no && SubjectDAO.GetSubjectById(x.subject_id).credit == subject.credit);

                        checkTotalTime = listCurriSubject
                            .Any(x => x.curriculum_id == curriSubject.curriculum_id && x.option.HasValue && x.option == curriSubject.option && x.term_no == curriSubject.term_no && SubjectDAO.GetSubjectById(x.subject_id).total_time == subject.total_time);
                    }
                    if (checkCredit == false || checkTotalTime == false)
                    {
                        return true;
                    }

                }

            }
            return false;
        }



        private bool CheckComboCreditOrTotalTime(CurriculumSubject curriSubject, Subject subject)
        {
            var checkCredit = _context.CurriculumSubject
                       .Include(x => x.Subject)
                       .Any(x => x.curriculum_id == curriSubject.curriculum_id && x.combo_id.HasValue && x.combo_id != 0 && x.term_no == curriSubject.term_no && x.Subject.credit == subject.credit);

            var checkTotalTime = _context.CurriculumSubject
                .Include(x => x.Subject)
                .Any(x => x.curriculum_id == curriSubject.curriculum_id && x.combo_id.HasValue && x.combo_id != 0 && x.term_no == curriSubject.term_no && x.Subject.total_time == subject.total_time);

            if (checkCredit == false || checkTotalTime == false)
            {
                return true;
            }
            return false;
        }


        public CurriculumSubject GetCurriculumSubjectById(int curriculumId, int subjectId)
        {
            var curriculumSubject = _context.CurriculumSubject
                .Include(x => x.Curriculum)
                .Include(x => x.Subject)
                .FirstOrDefault(x => x.curriculum_id == curriculumId && x.subject_id == subjectId);
            return curriculumSubject;
        }

        public CurriculumSubject GetCurriculumSubjectByTermNoAndSubjectGroup(int curriId, int term_no, int subject_id, int option)
        {
            var curriculumSubject = _context.CurriculumSubject
                .Include(x => x.Curriculum)
                .Include(x => x.Subject)
                .FirstOrDefault(x => x.curriculum_id == curriId
                && x.term_no == term_no
                && x.option == option
                && x.subject_id != subject_id);
            return curriculumSubject;
        }

        public List<CurriculumSubject> GetCurriculumSubjectByTermNo(int term_no)
        {
            var curriculumSubject = _context.CurriculumSubject
                .Include(x => x.Curriculum)
                .Include(x => x.Subject)
                .Where(x => x.term_no == term_no)
                .ToList();
            return curriculumSubject;
        }

        public string CreateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            try
            {
                _context.CurriculumSubject.Add(curriculumSubject);
                int number = _context.SaveChanges();
                if (number > 0)
                {
                    return Result.createSuccessfull.ToString();
                }
                else
                {
                    return "Create Curriculum Fail";
                }

            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            try
            {
                _context.CurriculumSubject.Update(curriculumSubject);
                int number = _context.SaveChanges();
                if (number > 0)
                {
                    return Result.updateSuccessfull.ToString();
                }
                else
                {
                    return "Update Curriculum Fail";
                }
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            try
            {
                _context.CurriculumSubject.Remove(curriculumSubject);
                int number = _context.SaveChanges();
                if (number > 0)
                {
                    return Result.deleteSuccessfull.ToString();
                }
                else
                {
                    return "Delete Curriculum Fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
