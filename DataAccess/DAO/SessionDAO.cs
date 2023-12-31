﻿using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SessionDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<Session> GetSession(int id)
        {
            var rs = _cmsDbContext.Session
                .Include(x => x.ClassSessionType)
                .Include(x => x.SessionCLO).ThenInclude(g => g.CLO)
                .Where(c => c.syllabus_id == id)
                .ToList();
            return rs;
        }

        public string DeleteSessionBySyllabusId(int syllabus_id)
        {
            var oldMate = _cmsDbContext.Session.Where(a => a.syllabus_id == syllabus_id).ToList();
            foreach (var item in oldMate)
            {
                var listSessionClo = _cmsDbContext.SessionCLO.Where(x => x.session_id == item.schedule_id).ToList();
                foreach(var session_clo in listSessionClo)
                {
                    _cmsDbContext.SessionCLO.Remove(session_clo);
                }
            }
            foreach (var item in oldMate)
            {
                _cmsDbContext.Session.Remove(item);
            }
            _cmsDbContext.SaveChanges();
            return Result.deleteSuccessfull.ToString();
        }

        public Session CreateSession(Session session)
        {
            var rs = _cmsDbContext.Session.Where(s => s.session_No == session.session_No && s.syllabus_id == session.syllabus_id).FirstOrDefault();
            if(rs == null)
            {

            _cmsDbContext.Session.Add(session); 
            _cmsDbContext.SaveChanges();
            return session;
            }
            else
            {
                throw new Exception("SessionNo already exist in system.");
            }
        }

        public Session IsSessionNoExist(int sessionNo, int syllabusId)
        {
            var session = _cmsDbContext.Session.Where(x =>( x.session_No == sessionNo && x.syllabus_id == syllabusId)).FirstOrDefault();
            return session;
        }

        public string DeleteSession(int id)
        {
       
            try
            {
                var rs = _cmsDbContext.Session
             .Where(c => c.schedule_id == id)
             .FirstOrDefault();

                //Delete SessionCLO
                var listCLO = _cmsDbContext.SessionCLO.Where(c => rs.schedule_id == c.session_id).ToList();
                foreach (var c in listCLO)
                {
                    _cmsDbContext.SessionCLO.Remove(c);
                }

                _cmsDbContext.Session.Remove(rs);
                _cmsDbContext.SaveChanges();
                return Result.deleteSuccessfull.ToString();


            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public Session UpdateSessionPatchAsync(int id, JsonPatchDocument employeeDocument)
        {
            Session employeeQuery = GetSessionById(id);
            if (employeeQuery == null)
            {
                return null;
            }
            employeeDocument.ApplyTo(employeeQuery);
            _cmsDbContext.SaveChangesAsync();

            return employeeQuery;
        }

        public string UpdateSession(Session session, List<SessionCLOsRequest> listClLOs)
        {
            try
            {
                var oldRs = _cmsDbContext.Session.Where(s => s.schedule_id == session.schedule_id).FirstOrDefault();

                if (oldRs.session_No == session.session_No)
                {

                    oldRs.schedule_content = session.schedule_content;
                    oldRs.syllabus_id = session.syllabus_id;
                    oldRs.session_No = oldRs.session_No;
                    oldRs.ITU = session.ITU;
                    oldRs.schedule_student_task = session.schedule_student_task;
                    oldRs.student_material = session.student_material;
                    oldRs.lecturer_material = session.lecturer_material;
                    oldRs.schedule_lecturer_task = session.schedule_lecturer_task;
                    oldRs.student_material_link = session.student_material_link;
                    oldRs.lecturer_material_link = session.lecturer_material_link;
                    oldRs.class_session_type_id = session.class_session_type_id;
                    oldRs.remote_learning = session.remote_learning;
                    oldRs.ass_defense = session.ass_defense;
                    oldRs.eos_exam = session.eos_exam;
                    oldRs.video_learning = session.video_learning;
                    oldRs.IVQ = session.IVQ;
                    oldRs.online_lab = session.online_lab;
                    oldRs.online_test = session.online_test;
                    oldRs.assigment = session.assigment;
                    _cmsDbContext.Session.Update(oldRs);
                    var listSessionCLOsOld = _cmsDbContext.SessionCLO.Where(s => s.session_id == oldRs.schedule_id).ToList();
                    foreach (var item in listSessionCLOsOld)
                    {
                        _cmsDbContext.SessionCLO.Remove(item);
                    }
                    foreach (var item in listClLOs)
                    {
                        SessionCLO sc = new SessionCLO();
                        sc.session_id = oldRs.schedule_id;
                        sc.CLO_id = item.CLO_id;
                        _cmsDbContext.SessionCLO.Add(sc);
                    }
                    _cmsDbContext.SaveChanges();
                    return Result.updateSuccessfull.ToString();
                }
                else
                {
                    var rs = _cmsDbContext.Session.Where(s => s.session_No == session.session_No && s.syllabus_id == session.syllabus_id).FirstOrDefault();
                    if (rs == null)
                    {

                        oldRs.schedule_content = session.schedule_content;
                        oldRs.syllabus_id = session.syllabus_id;
                        oldRs.session_No = session.session_No;
                        oldRs.ITU = session.ITU;
                        oldRs.schedule_student_task = session.schedule_student_task;
                        oldRs.student_material = session.student_material;
                        oldRs.lecturer_material = session.lecturer_material;
                        oldRs.schedule_lecturer_task = session.schedule_lecturer_task;
                        oldRs.student_material_link = session.student_material_link;
                        oldRs.lecturer_material_link = session.lecturer_material_link;
                        oldRs.class_session_type_id = session.class_session_type_id;
                        oldRs.remote_learning = session.remote_learning;
                        oldRs.ass_defense = session.ass_defense;
                        oldRs.eos_exam = session.eos_exam;
                        oldRs.video_learning = session.video_learning;
                        oldRs.IVQ = session.IVQ;
                        oldRs.online_lab = session.online_lab;
                        oldRs.online_test = session.online_test;
                        oldRs.assigment = session.assigment;
                        _cmsDbContext.Session.Update(oldRs);
                        var listSessionCLOsOld = _cmsDbContext.SessionCLO.Where(s => s.session_id == oldRs.schedule_id).ToList();
                        foreach (var item in listSessionCLOsOld)
                        {
                            _cmsDbContext.SessionCLO.Remove(item);
                        }
                        foreach (var item in listClLOs)
                        {
                            SessionCLO sc = new SessionCLO();
                            sc.session_id = oldRs.schedule_id;
                            sc.CLO_id = item.CLO_id;
                            _cmsDbContext.SessionCLO.Add(sc);
                        }
                        _cmsDbContext.SaveChanges();
                        return Result.updateSuccessfull.ToString();
                    }
                    else
                    {
                        throw new Exception("SessionNo already exist in system.");
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string UpdatePatchSession(Session session)
        {
            var oldRs = _cmsDbContext.Session
                .Include(x => x.ClassSessionType)
               .Include(x => x.SessionCLO)
               .ThenInclude(g => g.CLO)
               .Where(s => s.schedule_id == session.schedule_id)
               .FirstOrDefault();

            if (oldRs != null)
            {
                if (session.remote_learning != null )
                    oldRs.remote_learning = session.remote_learning;

                if (session.ass_defense != null)
                    oldRs.ass_defense = session.ass_defense;

                if (session.video_learning != null)
                    oldRs.video_learning = session.video_learning;

                if (session.IVQ != null )
                    oldRs.IVQ = session.IVQ;

                if (session.online_lab != null)
                    oldRs.online_lab = session.online_lab;


                if (session.eos_exam != null)
                    oldRs.eos_exam = session.eos_exam;

                if (session.online_test != null )
                    oldRs.online_test = session.online_test;

                if (session.assigment != null )
                    oldRs.assigment = session.assigment;

                _cmsDbContext.Session.Update(oldRs);
                _cmsDbContext.SaveChanges();
            }

            return Result.updateSuccessfull.ToString();
        }

        public Session GetSessionById(int id)
        {
             var session = _cmsDbContext.Session
                .Include(x => x.SessionCLO)
                .ThenInclude(g => g.CLO)
                .Where(s => s.schedule_id == id).FirstOrDefault();
            return session;
        }
    }
}
