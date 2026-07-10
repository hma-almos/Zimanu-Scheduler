using WebApplication5.dto;
using WebApplication5.Models;
using System.Collections.Generic;

namespace WebApplication5.Repository
{
    public interface IRepository<T>
    {
        T Create(T entity);
        T Update(int id, T entity);
        bool Delete(int id);
        List<T> GetAll();
        T GetById(int id);
        public List<Stages> GetDistinctStages();
        void AddAll(List<T> users);
        void AddRange(List<T> users);
        bool AddTeachersAndSubjects(SaveBulkDataRequest data);
        List<FinalSchedule> GetFinalSchedules();
        public void ResetPrimaryKey(string tableName);
        public List<Teacher_Lecture> GetBridgeTable();
    }
}