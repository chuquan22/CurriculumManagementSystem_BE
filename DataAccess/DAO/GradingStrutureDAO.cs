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
                .Include(x => x.GradingCLOs).ThenInclude(gc => gc.CLO)
                .Where(c => c.syllabus_id == id)
                .ToList();
            return rs;
        }

        public string DeleteGradingStrutureBySyllabusId(int syllabusId)
        {

            var oldMate = _cmsDbContext.GradingStruture.Where(a => a.syllabus_id == syllabusId).ToList();
            foreach (var item in oldMate)
            {
                var listSessionClo = _cmsDbContext.GradingCLO.Where(x => x.grading_id == item.grading_id).ToList();
                foreach (var session_clo in listSessionClo)
                {
                    _cmsDbContext.GradingCLO.Remove(session_clo);
                }
            }
            foreach (var item in oldMate)
            {
                _cmsDbContext.GradingStruture.Remove(item);
            }
            _cmsDbContext.SaveChanges();
            return Result.deleteSuccessfull.ToString();

        }

        public GradingStruture CreateGradingStruture(GradingStruture gra)
        {
            if (gra.references == null || gra.references.Equals(""))
            {
                _cmsDbContext.GradingStruture.Add(gra);
                _cmsDbContext.SaveChanges();
            }
            else if ((!gra.references.Equals("") || gra.references != null) && (gra.session_no == null || gra.session_no.Equals("")))
            {
                _cmsDbContext.GradingStruture.Add(gra);
                _cmsDbContext.SaveChanges();
            }
            else if ((gra.references.Equals("") || gra.references == null) && (gra.session_no != null || !gra.session_no.Equals("")))
            {
                _cmsDbContext.GradingStruture.Add(gra);
                _cmsDbContext.SaveChanges();
            }
            else
            {

                bool check = CheckGrading(gra);

                if (check == true)
                {
                    _cmsDbContext.GradingStruture.Add(gra);
                    _cmsDbContext.SaveChanges();
                    return gra;

                }
                else
                {
                    throw new Exception("False at creating grading struture! Wrong weight!");
                }
            }
            return gra;

        }
        public GradingStruture CreateGradingStrutureAPI(GradingStruture gra)
        {

            bool check = CheckGradingWeightCreate(gra);

            if (check == true)
            {
                var isComponentExist = _cmsDbContext.GradingStruture
                 .FirstOrDefault(x =>
                 x.syllabus_id == gra.syllabus_id &&
                    x.assessment_component.Equals(gra.assessment_component) &&
                 x.grading_id != gra.grading_id);
                if (isComponentExist != null)
                {
                    throw new Exception("Component already exists! Update failed.");
                }
                _cmsDbContext.GradingStruture.Add(gra);
                var father = _cmsDbContext.GradingStruture.Where(x => x.references == gra.references && x.session_no == null && x.syllabus_id == gra.syllabus_id).FirstOrDefault();
                father.grading_part = father.grading_part + gra.grading_part;
                father.grading_weight = father.grading_weight + gra.grading_weight;
                _cmsDbContext.GradingStruture.Update(father);
                _cmsDbContext.SaveChanges();
                return gra;

            }


            return null;

        }
      
        public bool CheckGradingWeightCreate(GradingStruture gra)
        {
            var fatherAll = _cmsDbContext.GradingStruture
                .Where(s => s.syllabus_id == gra.syllabus_id)
                .Where(x =>
        (x.references == null || x.references.Equals("") && x.session_no == null && x.syllabus_id == gra.syllabus_id) ||
        ((!x.references.Equals("") || x.references != null) && (x.session_no == null || x.session_no.Equals("")) && gra.syllabus_id == x.syllabus_id) ||
        ((x.references.Equals("") || x.references == null) && (x.session_no != null || !x.session_no.Equals("")) && gra.syllabus_id == x.syllabus_id)
    )
                .ToList();

           

            if (fatherAll == null || fatherAll.Count == 0)
            {
                throw new Exception("No Grading Structure References When Importing this References!");
            }

            decimal weightAll = fatherAll.Sum(item => item.grading_weight);
            decimal totalWeight = weightAll + gra.grading_weight;

            if (totalWeight > 100 || totalWeight < 0)
            {
                return false;
            }

            return true;
        }
        public bool CheckGradingWeight(GradingStruture gra)
        {
            var fatherAll = _cmsDbContext.GradingStruture
                .Where(s => s.syllabus_id == gra.syllabus_id)
                .Where(x =>
        (x.references == null || x.references.Equals("") && x.session_no == null && x.syllabus_id == gra.syllabus_id) ||
        ((!x.references.Equals("") || x.references != null) && (x.session_no == null || x.session_no.Equals("")) && gra.syllabus_id == x.syllabus_id) ||
        ((x.references.Equals("") || x.references == null) && (x.session_no != null || !x.session_no.Equals("")) && gra.syllabus_id == x.syllabus_id)
    )
                .ToList();

            var oldGra = _cmsDbContext.GradingStruture
                .Where(u => u.grading_id == gra.grading_id)
                .FirstOrDefault();

            if (fatherAll == null || fatherAll.Count == 0)
            {
                throw new Exception("No Grading Structure References When Importing this References!");
            }

            decimal weightAll = fatherAll.Sum(item => item.grading_weight);
            decimal totalWeight = weightAll - oldGra.grading_weight + gra.grading_weight;

            if (totalWeight > 100 || totalWeight < 0)
            {
                return false;
            }

            return true;
        }


        public bool CheckGrading(GradingStruture gra)
        {
            var father = _cmsDbContext.GradingStruture.Where(x => x.references == gra.references && x.session_no == null && x.syllabus_id == gra.syllabus_id).FirstOrDefault();
            if (father == null)
            {
                throw new Exception("No Grading Strutude References When Importing this References!.");
            }
            decimal weightAll = father.grading_weight;
            var listReferences = _cmsDbContext.GradingStruture.Where(x => x.session_no != null && x.references == gra.references && x.syllabus_id == gra.syllabus_id).ToList();
            decimal weightSon = 0;
            foreach (var reference in listReferences)
            {
                weightSon += reference.grading_weight;
            }
            if ((weightSon + gra.grading_weight) > weightAll)
            {
                return false;
            }
            else
            {
                return true;
            }
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

            var father = _cmsDbContext.GradingStruture.Where(x => x.references == oldGra.references && x.session_no == null && x.syllabus_id == oldGra.syllabus_id).FirstOrDefault();
            father.grading_part = father.grading_part - oldGra.grading_part;
            father.grading_weight = father.grading_weight - oldGra.grading_weight;
            _cmsDbContext.GradingStruture.Update(father);
            _cmsDbContext.GradingStruture.Remove(oldGra);
            _cmsDbContext.SaveChanges();
            return oldGra;
        }

        public string UpdateGradingStruture(GradingStruture gra, List<int> list)
        {
            var oldGra = _cmsDbContext.GradingStruture.Where(u => u.grading_id == gra.grading_id).FirstOrDefault();
            oldGra.syllabus_id = gra.syllabus_id;
            oldGra.minimum_value_to_meet_completion = gra.minimum_value_to_meet_completion;
            oldGra.grading_duration = gra.grading_duration;
            oldGra.type_of_questions = gra.type_of_questions;
            oldGra.session_no = gra.session_no;
            oldGra.references = gra.references;
            oldGra.number_of_questions = gra.number_of_questions;
            oldGra.scope_knowledge = oldGra.scope_knowledge;
            oldGra.how_granding_structure = gra.how_granding_structure;
            oldGra.assessment_component = gra.assessment_component;
            oldGra.assessment_type = gra.assessment_type;
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
            var isComponentExist = _cmsDbContext.GradingStruture
                  .Any(x => 
                  x.syllabus_id == oldGra.syllabus_id 
                  &&
                  x.assessment_component.Equals(gra.assessment_component) 
                  &&
                  x.grading_id != oldGra.grading_id);
            if (isComponentExist)
            {
                throw new Exception("Component already exists! Update failed.");
            }
            var father = _cmsDbContext.GradingStruture.Where(x => x.references == oldGra.references && x.session_no == null && x.syllabus_id == oldGra.syllabus_id).FirstOrDefault();
            if (father != null && gra.grading_part != oldGra.grading_part)
            {
                father.grading_part = father.grading_part - oldGra.grading_part + gra.grading_part;
            }
            bool check = CheckGradingWeight(gra);
            if (check)
            {
                if (oldGra.grading_weight > gra.grading_weight)
                {
                    father.grading_weight = father.grading_weight - oldGra.grading_weight + gra.grading_weight;
                }
                else if (oldGra.grading_weight < gra.grading_weight)

                {
                    father.grading_weight = father.grading_weight + gra.grading_weight - oldGra.grading_weight;
                }            
            }
            else
            {
                throw new Exception("Update Structure Update False! Please Check Weight <100% and > 0% !");
            }
            oldGra.grading_weight = gra.grading_weight;
            oldGra.grading_part = gra.grading_part;
            _cmsDbContext.GradingStruture.Update(oldGra);
            _cmsDbContext.GradingStruture.Update(father);
            _cmsDbContext.SaveChanges();
            return Result.updateSuccessfull.ToString();
        }
    }
}