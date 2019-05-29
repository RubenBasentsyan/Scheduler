using System.Data;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class ParticipantController : Controller
    {
        [Authorize]
        // GET: Participant
        public ActionResult Index(int page = 1)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                ViewBag.currentPage = page;
                ViewBag.maxPage = EntityFetcher.ParticipantsPageCount;
                return View(EntityFetcher.FetchParticipants(page));
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Create()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true) return View();
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ParticipantsViewModel participant)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        EntityModifier.CreateParticipant(participant);
                    }
                    catch
                    {
                        ModelState.AddModelError("Name", "Can't create a participant.");
                        return View(participant);
                    }

                    return RedirectToAction("Index");
                }

                return View(participant);
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
                try
                {
                    var participant = EntityFetcher.FetchParticipantWithId(id);
                    return View(participant);
                }
                catch (DataException)
                {
                    return RedirectToAction("Index");
                }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ParticipantsViewModel participant)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        EntityModifier.EditParticipant(participant);
                    }
                    catch
                    {
                        ModelState.AddModelError("Name", "Can't modify the participant.");
                    }

                    return RedirectToAction("Index");
                }

                return View(participant);
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
                try
                {
                    var participant = EntityFetcher.FetchParticipantWithId(id);
                    return View(participant);
                }
                catch (DataException)
                {
                    return RedirectToAction("Index");
                }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(ParticipantsViewModel participant)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                try
                {
                    EntityModifier.DeleteParticipant(participant);
                }
                catch
                {
                    ModelState.AddModelError("Name", "Can't modify the participant.");
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Home");
        }
    }
}