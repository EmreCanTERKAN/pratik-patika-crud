using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using pratik_patika_crud.Entities;
using pratik_patika_crud.Models;

namespace pratik_patika_crud.Controllers
{
    public class TaskController : Controller
    {
        static List<TaskEntity> _tasks = new List<TaskEntity>()
        {
            new TaskEntity { Id = 1, Title = "Yazılım Çalış", Content = "Müşteri Toplantısına Katıl",OwnerId = 1 },
            new TaskEntity { Id = 2, Title = "Egzersiz Yap",Content = "Fitness Yap"},
            new TaskEntity { Id = 3, Title= "Dota Oyna", Content = "En az 3 oyun dota Oyna"}

        };

        static List<OwnerEntity> _owners = new List<OwnerEntity>()
        {
            new OwnerEntity { Id = 234532, Name="Emre Can TERKAN" },
            new OwnerEntity { Id = 2, Name="Burak Kırıcı"}
        };

        public IActionResult List()
        {
            var tasks = _tasks.Where(x => x.IsDelete == false).Select(x => new TaskEntityViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                IsDone = x.IsDone,

            }).ToList();

            return View(tasks);
        }

        public IActionResult CompleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(x => x.Id == id);
            task.IsDone = !task.IsDone;
            task.CompletedDate = DateTime.Now;
            return RedirectToAction("List");
        }

        public IActionResult Add()
        {
            ViewBag.Owners = _owners;
            return View();
        }
        [HttpPost]
        public IActionResult Add(TaskAddViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            int maxId = _tasks.Max(x => x.Id);
            var newTask = new TaskEntity()
            {
                Id = maxId + 1,
                Title = viewModel.Title,
                Content = viewModel.Content,
                OwnerId = viewModel.OwnerId
            };
            _tasks.Add(newTask);
            return RedirectToAction("List", "Task");
        }
        public IActionResult CancelTask(int id)
        {
            var task = _tasks.Find(x => x.Id == id);

            task.IsDelete = true;

            return RedirectToAction("List", "Task");
        }

        public IActionResult Edit(int id)
        {
            //bu id ile eşleşen liste elemanını yakala
            var task = _tasks.Find(x => x.Id == id);
            //gerekli özelliklerini viewe gönder
            var viewModel = new TaskEditViewModel()
            {
                Id = task.Id,
                Title = task.Title,
                Content = task.Content,
                OwnerId = task.OwnerId
            };
            ViewBag.Owners = _owners;



            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(TaskEditViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            var task = _tasks.Find(x => x.Id == formData.Id);
            task.Title = formData.Title;
            task.Content = formData.Content;
            task.OwnerId = formData.OwnerId;

            return RedirectToAction("List");

        }


    }
}
