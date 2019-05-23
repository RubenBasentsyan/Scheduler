using System.Data;
using System.Web.Mvc;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class ParticipantController : Controller
    {
        // GET: Participant
        public ActionResult Index()
        {
            return View(EntityFetcher.FetchAllParticipants());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ParticipantsViewModel participant)
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

        public ActionResult Edit(int id)
        {
            try
            {
                ParticipantsViewModel participant;
                participant = EntityFetcher.FetchParticipantWithId(id);
                return View(participant);
            }
            catch(DataException)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(ParticipantsViewModel participant)
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

        public ActionResult Delete(int id)
        {
            try
            {
                ParticipantsViewModel participant;
                participant = EntityFetcher.FetchParticipantWithId(id);
                return View(participant);
            }
            catch (DataException)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(ParticipantsViewModel participant)
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
    }
}