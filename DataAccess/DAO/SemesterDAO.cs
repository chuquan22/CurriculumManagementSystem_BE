using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.DAO
{
    public class SemesterDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<Semester> GetSemesters()
        {
            var listSemesters = _cmsDbContext.Semester.Include(x => x.Batch).Include(x => x.Batch.DegreeLevel).ToList();
            return listSemesters;
        }

        public List<Semester> PaginationSemester(int? degree_id, int page, int limit, string? txtSearch)
        {
            IQueryable<Semester> query = _cmsDbContext.Semester
                .Include(x => x.Batch)
                .Include(x => x.Batch.DegreeLevel);

            if (degree_id.HasValue)
            {
                query = query.Where(x => x.Batch.degree_level_id == degree_id);
            }

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.semester_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.school_year.ToString().Contains(txtSearch.Trim()));
            }

            var listSemester = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listSemester;
        }

        public List<Semester> GetAllSemestersByMajorId(int id)
        {
            var major = _cmsDbContext.Major.Where(x => x.major_id == id).FirstOrDefault();
            var listSemester = _cmsDbContext.Semester.Include(x => x.Batch).Include(x => x.Batch.DegreeLevel).Where(x => x.Batch.degree_level_id == major.degree_level_id).ToList();
            return listSemester;
        }

        public List<Semester> GetSemesterByDegreeLevel(int id)
        {
            var listSemester = _cmsDbContext.Semester.Include(x => x.Batch).ThenInclude(x => x.DegreeLevel).Where(x => x.Batch.degree_level_id == id).ToList();
            return listSemester;
        }

        public int GetTotalSemester(string? txtSearch)
        {
            IQueryable<Semester> query = _cmsDbContext.Semester
                .Include(x => x.Batch)
                .Include(x => x.Batch.DegreeLevel);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.semester_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.school_year.ToString().Contains(txtSearch.Trim()));
            }

            var listSemester = query
                .ToList();
            return listSemester.Count;
        }

        public Semester GetSemester(int id)
        {
            var semester = _cmsDbContext.Semester.Include(x => x.Batch).Include(x => x.Batch.DegreeLevel).FirstOrDefault(x => x.semester_id == id);
            return semester;
        }

        public List<Semester> GetSemesterBySpe(int speId)
        {
            var specialization = _cmsDbContext.Specialization
               .Include(x => x.Major)
               .Include(x => x.Semester.Batch)
               .FirstOrDefault(x => x.specialization_id == speId);
            var batch_name = specialization.Semester.Batch.batch_name; 
            var semester = _cmsDbContext.Semester.Include(x => x.Batch).Include(x => x.Batch.DegreeLevel).Where(x => x.Batch.degree_level_id == specialization.Major.degree_level_id).ToList();
            var listSemester = new List<Semester>();
            foreach(var s in semester)
            {
                double batchValue;
                if (double.TryParse(s.Batch.batch_name, out batchValue))
                {
                    if (batchValue >= double.Parse(batch_name))
                    {
                        listSemester.Add(s);
                    }
                }
            }
            return listSemester;
        }

        public bool CheckSemesterDuplicate(int id, string name, int schoolYear, int batchId)
        {
            var degreeLevel = _cmsDbContext.Batch.Include(x => x.DegreeLevel).FirstOrDefault(x => x.batch_id == batchId).degree_level_id;
            return (_cmsDbContext.Semester?.Include(x => x.Batch).Any(x => x.semester_name.ToLower().Equals(name.ToLower().Trim()) && x.school_year == schoolYear && x.semester_id != id && x.Batch.degree_level_id == degreeLevel)).GetValueOrDefault();
        }

        public bool CheckSemesterExsit(int id)
        {
            var exsitSemesterPlan = _cmsDbContext.SemesterPlan.FirstOrDefault(x => x.semester_id == id);
            var exsitSpecialization = _cmsDbContext.Specialization.FirstOrDefault(x => x.semester_id == id);
            if(exsitSemesterPlan == null && exsitSpecialization == null)
            {
                return false;
            }
            return true;
        }

        public string CreateSemester(Semester semester)
        {
            try
            {
                _cmsDbContext.Semester.Add(semester);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateSemester(Semester semester)
        {
            try
            {
                _cmsDbContext.Semester.Update(semester);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteSemester(Semester semester)
        {
            try
            {
                _cmsDbContext.Semester.Remove(semester);
                _cmsDbContext.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

    }
}
