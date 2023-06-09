using Api.Context;
using Api.Controllers;
using Api.Entities;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mock<DataBaseContext> _context = new Mock<DataBaseContext> ();
            Note note = new Note();
            _context.Setup(x => x.Add(note));
            NotesController controller = new NotesController(_context.Object);
            
            var result = controller.AddNote(note);
        }
    }
}