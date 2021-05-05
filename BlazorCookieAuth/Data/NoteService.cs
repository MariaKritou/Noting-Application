using SQW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCookieAuth.Data
{
  public class NoteService
  {
    private readonly ISQWWorker worker;

    public NoteService(ISQWWorker worker)
    {
      this.worker = worker;
    }

    public void createNote(Note note, int userId)
    {
      worker.run(context =>
      {
        context
        .createCommand("INSERT INTO MARIADEMO.NOTEBOOK (TEXT, DATE_UPLOADED, SUBTEXT, CATEGORY_ENUM, USER_ID) VALUES (:TEXT, :DATE_UPLOADED, :SUBTEXT, :CATEGORY_ENUM, :USER_ID)")
        .addStringInParam("CATEGORY_ENUM", note.category)
        .addStringInParam("TEXT", note.text)
        .addStringInParam("SUBTEXT", note.subtext)
        .addDateTimeInParam("DATE_UPLOADED", note.date)
        .addNumericInParam("USER_ID", userId)
        .execute();
      });
    }

    public void editNote(Note note)
    {
      worker.run(context =>
      {
        context
        .createCommand("UPDATE MARIADEMO.NOTEBOOK SET CATEGORY_ENUM= :CATEGORY_ENUM, TEXT= :TEXT, SUBTEXT= :SUBTEXT WHERE NOTE_ID= :NOTE_ID")
        .addStringInParam("CATEGORY_ENUM", note.category)
        .addStringInParam("TEXT", note.text)
        .addStringInParam("SUBTEXT", note.subtext)
        .addNumericInParam("NOTE_ID", note.noteId)
        .execute();
      });
    }

    public void deleteNote(int noteId)
    {
      worker.run(context =>
      {
        context
        .createCommand("DELETE FROM MARIADEMO.NOTEBOOK WHERE NOTE_ID=:NOTE_ID")
        .addNumericInParam("NOTE_ID", noteId)
        .execute();
      });
    }

    public List<Note> getAllNotes(int userId)
    {
      List<Note> notes = null;

        worker.run(context =>
      {
        notes = 
        context.createCommand("SELECT * FROM MARIADEMO.NOTEBOOK WHERE USER_ID=:USER_ID")
        .addNumericInParam("USER_ID", userId)
        .select<Note>();
      });

      return notes;
    }

    public List<Note> getAllNotesByCategory(int userId, string category)
    {
      List<Note> notes = null;

      worker.run(context =>
      {
        notes =
        context.createCommand("SELECT * FROM MARIADEMO.NOTEBOOK WHERE (USER_ID=:USER_ID AND CATEGORY_ENUM= :CATEGORY_ENUM)")
        .addNumericInParam("USER_ID", userId)
        .addStringInParam("CATEGORY_ENUM", category)
        .select<Note>();
      });

      return notes;
    }
  }
}
