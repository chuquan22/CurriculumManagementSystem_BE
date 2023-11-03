﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models.Enums;

namespace DataAccess.DAO
{
    public class GradingStrutureDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<GradingStruture> GetGradingStruture(int id)
        {
            var rs = _cmsDbContext.GradingStruture
                .Include(x => x.AssessmentMethod)
                .Include(x => x.AssessmentMethod.AssessmentType)
                .Include(x => x.Syllabus)
                .Include(x => x.GradingCLOs).ThenInclude( gc => gc.CLO)
                .Where(c => c.syllabus_id == id)
                .ToList();
            return rs;
        }

        public GradingStruture CreateGradingStruture(GradingStruture gra)
        {
            _cmsDbContext.GradingStruture.Add(gra);
            _cmsDbContext.SaveChanges();
            return gra;
        }

        public bool CheckCreate(string reference, int? sessionNo)
        {
            var fatherGrading = _cmsDbContext.GradingStruture.Where(g => (g.session_no == 0 || g.session_no == null) && g.references.Equals(reference)).FirstOrDefault();
            var weight = fatherGrading.grading_weight;
            var listGrading = _cmsDbContext.GradingStruture.Where(g => (g.session_no == 0 || g.session_no == null) && g.references.Contains(reference));
            decimal weight_son = 0;
            foreach (var g in listGrading)
            {
                weight_son += g.grading_weight;
            }
            if(weight_son > weight)
            {
                return false;
            }

            return true;
        }

        public GradingStruture GetGradingStrutureById(int id)
        {
            var oldGra = _cmsDbContext.GradingStruture
                .Include(x => x.AssessmentMethod)
                .Include(x => x.AssessmentMethod.AssessmentType)
                .Include(x => x.Syllabus)
                .Include(x => x.GradingCLOs)
                .ThenInclude(gc => gc.CLO)
                .Where(u => u.grading_id == id)
                .FirstOrDefault();
            return oldGra;

        }

        public GradingStruture DeleteGradingStruture(int id)
        {
           
            var oldGra = _cmsDbContext.GradingStruture.Where(u => u.grading_id == id).FirstOrDefault();
            var listCLo = _cmsDbContext.GradingCLO.Where(u => u.grading_id == id).ToList();
            foreach (var cLo in listCLo)
            {
                _cmsDbContext.GradingCLO.Remove(cLo);
            }
            _cmsDbContext.GradingStruture.Remove(oldGra);
            _cmsDbContext.SaveChanges();
            return oldGra;
        }

        public string UpdateGradingStruture(GradingStruture gra, List<int> list)
        {
            var oldGra = _cmsDbContext.GradingStruture.Where(u => u.grading_id == gra.grading_id).FirstOrDefault();
            oldGra.grading_weight = gra.grading_weight;
            oldGra.grading_part = gra.grading_part;
            oldGra.syllabus_id = gra.syllabus_id;
            oldGra.minimum_value_to_meet_completion = gra.minimum_value_to_meet_completion; 
            oldGra.grading_duration = gra.grading_duration;
            oldGra.type_of_questions = gra.type_of_questions;
            oldGra.session_no = gra.session_no;
            oldGra.references = gra.references;
            oldGra.number_of_questions = gra.number_of_questions;
            oldGra.scope_knowledge = oldGra.scope_knowledge;
            oldGra.how_granding_structure = gra.how_granding_structure;
            oldGra.assessment_method_id = gra.assessment_method_id;
            oldGra.grading_note = gra.grading_note;
            var listCLo = _cmsDbContext.GradingCLO.Where(u => u.grading_id == gra.grading_id).ToList();
            foreach (var cLo in listCLo)
            {
                _cmsDbContext.GradingCLO.Remove(cLo);
            }
            foreach (var cLo2 in list)
            {
                GradingCLO gr = new GradingCLO();
                gr.grading_id = oldGra.grading_id;
                gr.CLO_id = cLo2;
                _cmsDbContext.GradingCLO.Add(gr);
            }
            _cmsDbContext.GradingStruture.Update(oldGra);
            _cmsDbContext.SaveChanges();
            return Result.updateSuccessfull.ToString();
        }
    }
}
