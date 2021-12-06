using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Dapper;
using web_app_asp_net_mvc_database_query.Models;

namespace web_app_asp_net_mvc_database_query.Controllers
{
    public class LessonsController : Controller
    {
        public readonly string _connectionString = WebConfigurationManager.AppSettings["ConnectionString"];

        [HttpGet]
        public ActionResult Index()
        {
            var lessons = SelectLessons();
            return View(lessons);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var lesson = new Lesson();

            return View(lesson);
        }

        [HttpPost]
        public ActionResult Create(Lesson model)
        {
            if (!ModelState.IsValid)
                return View(model);
                       
            InsertLesson(model);

            return RedirectPermanent("/Lessons/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var lesson = SelectLessons().FirstOrDefault(x => x.Id == id);
            if (lesson == null)
                return RedirectPermanent("/Lessons/Index");

            DeleteLesson(lesson);

            return RedirectPermanent("/Lessons/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var lesson = SelectLessons().FirstOrDefault(x => x.Id == id);
            if (lesson == null)
                return RedirectPermanent("/Lessons/Index");

            return View(lesson);
        }

        [HttpPost]
        public ActionResult Edit(Lesson model)
        {
            var lesson = SelectLessons().FirstOrDefault(x => x.Id == model.Id);
            if (lesson == null)
                ModelState.AddModelError("Id", "Предмет не найден");

            if (!ModelState.IsValid)
                return View(model);

            MappingLesson(model, lesson);

            UpdateLesson(lesson);

            return RedirectPermanent("/Lessons/Index");
        }

        private void MappingLesson(Lesson sourse, Lesson destination)
        {
            destination.SequentialNumber = sourse.SequentialNumber;
            destination.LessonName = sourse.LessonName;
            destination.Group = sourse.Group;
            destination.Teacher = sourse.Teacher;
        }



        public void InsertLesson(Lesson lesson)
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $@"INSERT INTO [{connection.Database}].[dbo].[Lessons] ([SequentialNumber], [LessonName], [Group], [Teacher])  VALUES (@SequentialNumber,@LessonName, @Group, @Teacher)";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();

            cmd.Parameters.Add(new SqlParameter("@SequentialNumber", lesson.SequentialNumber));
            cmd.Parameters.Add(new SqlParameter("@LessonName", lesson.LessonName));
            cmd.Parameters.Add(new SqlParameter("@Group", lesson.Group));
            cmd.Parameters.Add(new SqlParameter("@Teacher", lesson.Teacher));
                        
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateLesson(Lesson lesson)
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $@"UPDATE [{connection.Database}].[dbo].[Lessons] SET [SequentialNumber] = @SequentialNumber, [LessonName] = @LessonName , [Group] = @Group, [Teacher] = @Teacher WHERE Id = {lesson.Id}";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();

            cmd.Parameters.Add(new SqlParameter("@SequentialNumber", lesson.SequentialNumber));
            cmd.Parameters.Add(new SqlParameter("@LessonName", lesson.LessonName));
            cmd.Parameters.Add(new SqlParameter("@Group", lesson.Group));
            cmd.Parameters.Add(new SqlParameter("@Teacher", lesson.Teacher));

            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteLesson(Lesson lesson)
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $@"DELETE [{connection.Database}].[dbo].[Lessons] WHERE Id = {lesson.Id}";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public List<Lesson> SelectLessons()
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $"SELECT * FROM [{connection.Database}].[dbo].[Lessons]";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();

            IDataReader read = cmd.ExecuteReader();
            var lessons = new List<Lesson>();
            while (read.Read())
            {
                var parser = read.GetRowParser<Lesson>(typeof(Lesson));
                var lesson = parser(read);
                lessons.Add(lesson);
            }

            connection.Close();
            return lessons;
        }

    }
}