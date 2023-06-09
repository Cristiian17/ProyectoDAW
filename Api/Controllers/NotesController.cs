using Api.Context;
using Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public NotesController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<Note> GetUserNotes()
        {
            var user = HttpContext.User;

            var userId = user.FindFirst("userId")?.Value;
            var notes = _context.Notes.ToList().Where(x => x.UserId == int.Parse(userId)).OrderByDescending(x => x.CreatedAt);
            var favNotes = _context.FavNotes.ToList().Where(x => x.UserId == int.Parse(userId));

            foreach (var note in notes)
            {
                note.Favorite = favNotes.Any(x => x.NoteId == note.Id);
            }

            return notes;
        }
        
        [HttpGet]
        [Route("GetSharedNotes")]
        [Authorize]
        public IEnumerable<Note> GetSharedNotes()
        {
            var user = HttpContext.User;

            var userId = user.FindFirst("userId")?.Value;
            var notes = _context.SharedNotes.ToList().Where(x => x.UserId == int.Parse(userId));
            var sharedNotes = new List<Note>();
            

            foreach (var sharedNote in notes)
            {
                var note = _context.Notes.FirstOrDefault(x => x.Id == sharedNote.NoteId);
                if(note != null)
                {
                    sharedNotes.Add(note);
                }
            }
            return sharedNotes;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddNote([FromBody]Note note)
        {
            if (note == null)
            {
                return BadRequest("Error 404");
            }

            _context.Notes.Add(note);
            _context.SaveChanges();
            var user = HttpContext.User;

            var userId = user.FindFirst("userId").Value;
            var noteAdded = _context.Notes.ToList().FindAll(x => x.UserId == int.Parse(userId)).OrderByDescending(x => x.Id).FirstOrDefault();


            return Ok(noteAdded);

        }

        [HttpPut]
        [Authorize]
        public IActionResult EditNote([FromBody] Note note)
        {
            if (note == null)
            {
                return BadRequest("Error 404");
            }

            var editedNote = _context.Notes.FirstOrDefault(x => x.Id == note.Id);
            editedNote.Content = note.Content;
            editedNote.Title = note.Title;
            _context.SaveChanges();

            return Ok(editedNote);

        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteNote(int noteId)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("userId").Value;

            var note = _context.Notes.FirstOrDefault(x => x.Id == noteId);
            if(note.UserId != int.Parse(userId))
            {
                return BadRequest("No se puede borrar la nota");
            }
            _context.Notes.Remove(note);

            _context.SaveChanges();

            return Ok(true);

        }

        [HttpGet]
        [Route("AddFav")]
        [Authorize]
        public IActionResult AddFav(int noteId)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("userId").Value;
            FavNote favNote = new() { NoteId = noteId, UserId = int.Parse(userId) };
            _context.FavNotes.Add(favNote);

            _context.SaveChanges();

            return Ok(true);

        }

        [HttpGet]
        [Route("RemoveFav")]
        [Authorize]
        public IActionResult RemmoveFav(int noteId)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("userId").Value;
            var note = _context.FavNotes.FirstOrDefault(x => x.NoteId == noteId && x.UserId == int.Parse(userId));
            _context.FavNotes.Remove(note);

            _context.SaveChanges();

            return Ok(true);

        }

        [HttpGet]
        [Route("AddSharedNote")]
        [Authorize]
        public IActionResult AddSharedNote(string email, int noteId)
        {
            var user = HttpContext.User;
            var MyUserId = user.FindFirst("userId").Value;
            var SharedUser = _context.Users.FirstOrDefault(x => x.Email.Equals(email));
            var SharedNote = new SharedNote() { NoteId = noteId, SharedUserId = int.Parse(MyUserId), UserId = SharedUser.Id };
            _context.SharedNotes.Add(SharedNote);
            if (_context.SaveChanges() >= 1)
            {
                return Ok(true);
            }
            return BadRequest(false);

        }
    }
}
