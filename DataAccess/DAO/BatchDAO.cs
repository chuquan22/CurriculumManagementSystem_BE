﻿using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BatchDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<Batch> GetAllBatch()
        {
            var listBatch = _context.Batch
                .Include(x => x.DegreeLevel)
                .OrderByDescending(x => x.batch_name)
                .ThenByDescending(x => x.batch_order)
                .ToList();
            return listBatch;
        }

        public List<Batch> PaginationCurriculumBatch(int? degree_id, int page, int limit, string? txtSearch)
        {
            IQueryable<Batch> query = _context.Batch
                .Include(x => x.DegreeLevel)
                .Include(x => x.CurriculumBatchs);


            if (degree_id.HasValue)
            {
                query = query.Where(x => x.degree_level_id == degree_id);
            }

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.batch_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.DegreeLevel.degree_level_name.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listLearningMethod = query
                .OrderBy(x => x.degree_level_id)
                .ThenBy(x => x.batch_order)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listLearningMethod;
        }

        public int GetTotalCurriculumBatch(int? degree_id, string? txtSearch)
        {
            IQueryable<Batch> query = _context.Batch
                 .Include(x => x.CurriculumBatchs);

            if (degree_id.HasValue)
            {
                query = query.Where(x => x.degree_level_id == degree_id);
            }

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.batch_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.DegreeLevel.degree_level_name.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listLearningMethod = query
                .ToList();
            return listLearningMethod.Count;
        }

        public int GetBatchIDByName(string batchName)
        {

            var batch = _context.Batch.Where(x => x.batch_name.Equals(batchName)).FirstOrDefault();
            var batch_id = 0;
            if (batch != null)
            {
                batch_id = batch.batch_id;
            }

            return batch_id;
        }

        public Batch GetBatchById(int id)
        {
            var batch = _context.Batch
                .Include(x => x.CurriculumBatchs)
                .ThenInclude(x => x.Curriculum)
                .Include(x => x.DegreeLevel)
                .Where(x => x.batch_id == id).FirstOrDefault();
            return batch;
        }

        public List<Batch> GetBatchByDegreeLevel(int degreeId)
        {
            var listBatch = _context.Batch.Where(x => x.degree_level_id == degreeId).ToList();
            return listBatch;
        }

        public List<Batch> GetBatchNotExsitInSemester()
        {
            var listBatchInSemester = _context.Semester.Include(x => x.Batch).Select(x => x.Batch).ToList();
            var listBatchRemove = new List<Batch>();
            foreach (var item in listBatchInSemester)
            {
                var count = _context.Semester.Include(x => x.Batch).ThenInclude(x => x.DegreeLevel).Where(x => x.start_batch_id == item.batch_id && x.Batch.DegreeLevel.degree_level_code.Equals("TC")).Count();
                if(count < 3 && count > 0)
                {
                    listBatchRemove.Add(item);
                }

            }
            foreach (var item in listBatchRemove)
            {
                listBatchInSemester.Remove(item);
            }
            var listBatch = GetAllBatch();
            var listBatchNotInSemester = listBatch.Except(listBatchInSemester).ToList();
            return listBatchNotInSemester;
        }

        public bool CheckBatchDuplicate(string batch_name, int batchOrder, int degree_Id)
        {
            return (_context.Batch?.Any(x => x.batch_name.Equals(batch_name) || x.batch_order == batchOrder && x.degree_level_id == degree_Id)).GetValueOrDefault();
        }

        public bool CheckBatchUpdateDuplicate(int id, int batchOrder, int degree_Id)
        {
            return (_context.Batch?.Any(x => x.batch_order == batchOrder && x.degree_level_id == degree_Id && x.batch_id != id)).GetValueOrDefault();
        }

        public bool CheckBatchExsit(int id)
        {
            var exsitInSemester = _context.Semester.FirstOrDefault(x => x.start_batch_id == id);
            if (exsitInSemester == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckBatchNameExsit(string batchName, int degreeId)
        {
            return (_context.Batch?.Any(x => x.batch_name.Equals(batchName) && x.degree_level_id == degreeId)).GetValueOrDefault();
        }

        public List<Batch> GetBatchBySpe(int speId)
        {
            var specialization = _context.Specialization
                .Include(x => x.Major)
                .Include(x => x.Semester.Batch)
                .FirstOrDefault(x => x.specialization_id == speId);
            var batch_name = specialization.Semester.Batch.batch_name;
            var listBatch = new List<Batch>();
            foreach (var batch in GetBatchByDegreeLevel(specialization.Major.degree_level_id))
            {
                double batchValue;
                if (double.TryParse(batch.batch_name, out batchValue))
                {
                    if (batchValue >= double.Parse(batch_name))
                    {
                        listBatch.Add(batch);
                    }
                }
            }

            return listBatch;
        }

        public string CreateBatch(Batch batch)
        {
            try
            {
                _context.Batch.Add(batch);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateBatch(Batch batch)
        {
            try
            {
                _context.Batch.Update(batch);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteBatch(Batch batch)
        {
            try
            {
                _context.Batch.Remove(batch);
                _context.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

    }
}
