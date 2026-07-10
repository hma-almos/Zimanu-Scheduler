using System.Collections.Generic;
using System.Linq;
using WebApplication5.Data;
using WebApplication5.dto;
using WebApplication5.Models;
using WebApplication5.Repository;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lectures.Services
{
    public class RepositoryService<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public T Update(int id, T entity)
        {
            var existingEntity = _context.Set<T>().Find(id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
                return entity;
            }
            return null;
        }

        public bool Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public List<Stages> GetDistinctStages()
        {
            return _context.stages.Distinct().ToList();
        }
        public void AddAll(List<T> users)
        {
            _context.Set<T>().AddRange(users);
            _context.SaveChanges();
        }
        public void ResetPrimaryKey(string tableName)
        {
                // Delete all records
                _context.Database.ExecuteSqlRaw("DELETE FROM "+tableName);

                // Reset the primary key (set the identity to start from 1)
                _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('"+ tableName + "', RESEED, 0)");
            
        }
        void IRepository<T>.AddRange(List<T> users)
        {
            _context.Set<T>().AddRange(users);
            _context.SaveChanges();
        }
        public bool AddTeachersAndSubjects(SaveBulkDataRequest data)
        {
            try
            {
                ResetPrimaryKey("teachers");
                ResetPrimaryKey("Lectures");
                ResetPrimaryKey("rooms");
                ResetPrimaryKey("stages");
                ResetPrimaryKey("ScheduleHours");
                if (data.Teachers != null && data.Teachers.Count > 0)
                {
                    var orderedByName = data.Teachers.OrderBy(t => t.Name).ToList();
                    _context.teachers.AddRange(orderedByName);
                }

                // Save subjects to database
                if (data.Subjects != null && data.Subjects.Count > 0)
                {
                    var orderedByStage = data.Subjects.OrderBy(t => t.Stages).ToList();
                    _context.Lectures.AddRange(orderedByStage);
                }
                //List<Rooms> lst = new List<Rooms>();
                //for (int i = 1; i <= data.Rooms; i++)
                //    lst.Add(new Rooms(i + "", "ROOM"));
                //_context.rooms.AddRange(lst);

                //for (int i = 1; i <= data.Labs; i++)
                //    lst.Add(new Rooms(i + "", "LAB"));
                _context.rooms.AddRange(data.Rooms);
                //List<ScheduleHour> hours = new List<ScheduleHour>();
                //for (int i = 1; i <= data.Time; i++)
                //    hours.Add(new ScheduleHour(i));
                _context.ScheduleHours.AddRange(data.Time);
                //List<Stages> stages = new List<Stages>();
                //for (int i = 1; i <= data.Stages; i++)
                //    stages.Add(new Stages(new string("المرحلة: " + i)));
                _context.stages.AddRange(data.Stages);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (you can log it to a file or monitoring system)
                Console.Error.WriteLine(ex.Message);
                return false;
            }
        }
        public List<FinalSchedule> GetFinalSchedules()
        {
            return _context.finalSchedules
                .Include(f => f.Lec)               // Include the Teacher_Lecture bridge table
                    .ThenInclude(l => l.Lecture)   // Include Lecture details
                .Include(f => f.Lec)               // Include Teacher_Lecture again
                    .ThenInclude(l => l.Teacher)   // Include Teacher details
                .Include(f => f.Hours)             // Include Hours
                .Include(f => f.Day)               // Include Day
                .Include(f => f.Room)              // Include Room
                .Include(f => f.Stage)             // Include Stage
                .ToList();
        }
        public List<Teacher_Lecture> GetBridgeTable()
        {
          
            return _context.teacher_Lectures
               .Include(tl => tl.Lecture)  // Join with Lecture table
               .Include(tl => tl.Teacher)  // Join with Teacher table     
               .ToList();
        }
       
    }
}
