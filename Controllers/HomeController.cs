using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using WebApplication5.dto;
using WebApplication5.Models;
using WebApplication5.Repository;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {

        private readonly IRepository<Rooms> _rooms;
        private readonly IRepository<Teacher> _teachers;
        private readonly IRepository<Lecture> _lectures;
        private readonly IRepository<ScheduleHour> _scheduleHour;
        private readonly IRepository<Teacher_Lecture> _Bridge;
        private readonly IRepository<Stages> _stages;
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Dayes> _dayes;
        private readonly IRepository<FinalSchedule> _finalSchedule;
        public HomeController(
            ILogger<HomeController> loggerl,
            IRepository<Rooms> repository,
            IRepository<Teacher> teachers,
            IRepository<Lecture> lectures,
            IRepository<ScheduleHour> scheduleHour,
            IRepository<Teacher_Lecture> Bridge,
            IRepository<Stages> stages,
            IRepository<Dayes> dayes, IRepository<FinalSchedule> final)

        {
            _logger = loggerl;
            _rooms = repository;
            _teachers = teachers;
            _lectures = lectures;
            _scheduleHour = scheduleHour;
            _Bridge = Bridge;
            _stages = stages;
            _dayes = dayes;
            _finalSchedule = final;
        }

        public IActionResult Index()
        {
            ViewBag.Stages = _stages.GetAll();
            var bridge = _Bridge.GetBridgeTable();
            ViewBag.Bridge = _Bridge.GetBridgeTable();
            List<FinalSchedule> roo = _finalSchedule.GetFinalSchedules();
            if (roo == null || !roo.Any())
            {
                _logger.LogWarning("FinalSchedule list is empty.");
                return View(new List<FinalSchedule>());
            }
            _logger.LogWarning($"Total stages entries: {_stages.GetAll().Count}");
            foreach (var entry in _stages.GetAll())
            {
                _logger.LogInformation($"- stage name: {entry.Name}");
            }

            _logger.LogWarning($"Total FinalSchedule entries: {roo.Count}");
            
            return View(roo);
        }
        [Authorize]
        [HttpPost]
        public IActionResult SaveBulkData([FromBody] SaveBulkDataRequest data)
        {
            if (data == null)
            {
                return BadRequest(new { message = "Invalid data received. Data is null." });
            }

            if ((data.Teachers == null || !data.Teachers.Any()) &&
                (data.Subjects == null || !data.Subjects.Any()))
            {
                return BadRequest(new { message = "Both teachers and subjects are empty." });
            }

            //Console.WriteLine("Received Data: " + JsonSerializer.Serialize(data));

            // Save teachers to database
            _teachers.AddTeachersAndSubjects(data);

            return Ok(new { redirectUrl = Url.Action("Create", "Home") });
        }
        [Authorize]
        [HttpGet]
        public IActionResult SaveBulkData()
        {

            SaveBulkDataRequest dr = new SaveBulkDataRequest
            {
                Teachers = _teachers.GetAll(),
                Rooms = _rooms.GetAll(),
                Subjects = _lectures.GetAll(),
                Stages = _stages.GetAll(),
                Time = _scheduleHour.GetAll()
            };
            TempData["BulkData"] = JsonConvert.SerializeObject(dr);
            if (TempData["BulkData"] is string bulkDataJson)
            {
                var dataRequest = JsonConvert.DeserializeObject<SaveBulkDataRequest>(bulkDataJson);
                ViewBag.Data = dataRequest;
                return View();
            }

            return RedirectToAction("Error"); //
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            SaveBulkDataRequest data = new SaveBulkDataRequest()
            {
                Subjects = _lectures.GetAll(),
                Teachers = _teachers.GetAll(),
                Rooms = _rooms.GetAll(),
                Stages = _stages.GetAll(),
                Time = _scheduleHour.GetAll()
            };
            if (data == null ||
            data.Subjects == null || !data.Subjects.Any() ||
            data.Teachers == null || !data.Teachers.Any() ||
            data.Rooms == null || !data.Rooms.Any() ||
            data.Stages == null || !data.Stages.Any() ||
            data.Time == null || !data.Time.Any())
            {
                return RedirectToAction("SaveBulkData", "Home"); // Redirect to HomeController -> MissingData action
            }
            ViewBag.saveBlunk = data;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] Teacher_Lecture_id dataToSend)
        {
            List<Stages> st = _stages.GetAll();
            for (int i = 0; i < dataToSend.InputValues.Count / 2; i++)
            {
                Stages stO = st[i];
                _stages.Update(stO.Id, new Stages(stO.Id, stO.Name, dataToSend.InputValues[i + i + 1], dataToSend.InputValues[i + i]));
            }
            List<SelectedSubject> selectedData = dataToSend.SelectedData;
            if (selectedData == null || !selectedData.Any())
            {
                _logger.LogWarning("Received an empty or null selectedData list.");

                return BadRequest(new { message = "Invalid data. No selectedData provided." });
            }

            _logger.LogInformation($"Received {selectedData.Count} selectedData.");

            List<Teacher_Lecture> teacherLectures = new List<Teacher_Lecture>();

            foreach (var assignment in selectedData)
            {
                if (assignment.TeacherIds == null || !assignment.TeacherIds.Any())
                {
                    _logger.LogWarning($"Skipping SubjectId {assignment.SubjectId} due to missing teachers.");
                    continue;
                }

                foreach (var teacherId in assignment.TeacherIds)
                {
                    _logger.LogDebug($"Assigning Teacher {teacherId} to Subject {assignment.SubjectId}");

                    teacherLectures.Add(new Teacher_Lecture
                    {
                        Teacher_Id = teacherId,
                        Lecture_Id = assignment.SubjectId,
                        practical = assignment.practical,
                        theory = assignment.theory
                    });
                }
            }

            if (!teacherLectures.Any())
            {
                _logger.LogWarning("No valid teacher-lecture selectedData were processed.");
                return BadRequest(new { message = "No valid teacher-lecture selectedData found." });
            }

            try
            {
                _logger.LogInformation($"Saving {teacherLectures.Count} teacher-lecture selectedData to the database.");
                _Bridge.AddRange(teacherLectures);


                _logger.LogInformation("selectedData saved successfully.");
                return Ok(new { redirectUrl = Url.Action("GenerateSchedule", "Home") });
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving selectedData.");
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        [HttpGet]
        public IActionResult GenerateSchedule()
        {
            _finalSchedule.ResetPrimaryKey("finalSchedules");

            var scheduleHours = _scheduleHour.GetAll().OrderBy(h => h.Id).ToList();
            var rooms = _rooms.GetAll().ToList();
            var days = _dayes.GetAll().ToList();
            var stages = _stages.GetAll().ToList();
            var teacherLectures = (from tl in _Bridge.GetAll()
                                   join l in _lectures.GetAll() on tl.Lecture_Id equals l.Id
                                   join t in _teachers.GetAll() on tl.Teacher_Id equals t.Id
                                   join s in _stages.GetAll() on l.Stages equals s.Id
                                   select new
                                   {
                                       TeacherLecture = tl,
                                       Lecture = l,
                                       Teacher = t,
                                       Stage = s,
                                       Theory = tl.theory,
                                       Practice = tl.practical
                                   }).OrderBy(x => Guid.NewGuid()).ToList();

            var finalSchedules = new List<FinalSchedule>();
            var scheduledLectures = new HashSet<int>(); // Tracks scheduled lecture IDs
            List<Teacher> currentTeachers = new List<Teacher>();
           
            foreach (var stage in stages)
            {
                var currentStageLecs = teacherLectures
                    .Where(l => l.Lecture.Stages == stage.Id)
                    .ToList();

                int day = 0; // Reset day for each stage
                int hour = 0;
                int i = 0;
                bool firstTry = true;
                while (i < currentStageLecs.Count)
                {
                    if (firstTry && hour == scheduleHours.Count && day == days.Count)
                    {
                        hour = 0;
                        day = 0;
                        firstTry = false;
                    }
                    if (!firstTry)
                    {
                        throw new Exception("the schedual overflow the time");
                    }
                    var lec = currentStageLecs[i];
                    if (scheduledLectures.Contains(lec.Lecture.Id))
                    {
                        i++;
                        continue;
                    }
                    if (hour >= scheduleHours.Count)
                    {
                        hour = 0;
                        day++;
                    }
                    bool cont = false;
                    currentTeachers = teacherLectures.Where(l => l.TeacherLecture.Lecture_Id == lec.Lecture.Id)
                                                        .Select(l => l.Teacher)
                                                        .Distinct()
                                                        .ToList();
                    var roomType = lec.Theory ? "ROOM" : "LAB";
                    var availableRoom = rooms.FirstOrDefault(r =>
                        r.Type == roomType &&
                        !finalSchedules.Any(s => s.Room.Id == r.Id && s.Day.Id == days[day].Id && s.Hours.Id == scheduleHours[hour].Id)
                    );

                    if (availableRoom == null)
                    {
                        hour++;
                        continue;
                    }
                    foreach (var teach in currentTeachers)
                    {
                        if (finalSchedules.Any(s =>
                            s.Lec.Teacher_Id == teach.Id &&
                            s.Day.Id == days[day].Id &&
                            s.Hours.Id == scheduleHours[hour].Id))
                        {
                            cont = true;
                            break;
                        }
                    }
                    if (cont)
                    {
                        hour++;
                        continue;
                    }
                    for (int j = 0; j < lec.Lecture.hours; j++)
                    {
                        finalSchedules.Add(new FinalSchedule
                        {
                            Lec = lec.TeacherLecture,
                            Room = availableRoom,
                            Day = days[day],
                            Hours = scheduleHours[hour],
                            Stage = stage
                        });
                        scheduledLectures.Add(lec.Lecture.Id);
                        hour++;
                        if (hour >= scheduleHours.Count)
                        {
                            hour = 0;
                            day++;
                        }
                    }
                    i++;
                }
            }

            if (!finalSchedules.Any())
            {
                throw new Exception("No valid schedule could be generated. Please check availability.");
            }

            _finalSchedule.AddRange(finalSchedules);
            return RedirectToAction("Index", "Home");
        }
    }
}
