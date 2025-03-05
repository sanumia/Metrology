using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

string scalaCode = @"
        case class Book(title: String, author: String, var isAvailable: Boolean = true)

class Library {
  private var books: List[Book] = List()

  def addBook(title: String, author: String): Unit = {
    books = Book(title, author) :: books
  }

  def findBook(title: String): Option[Book] = {
    books.find(_.title == title)
  }

  def removeBook(title: String): Boolean = {
    val initialSize = books.size
    books = books.filterNot(_.title == title)
    books.size != initialSize
  }

  def checkoutBook(title: String): Boolean = {
    findBook(title) match {
      case Some(book) =>
        if (book.isAvailable) {
          book.isAvailable = false
          true
        } else false
      case None => false
    }
  }

  def returnBook(title: String): Boolean = {
    findBook(title) match {
      case Some(book) =>
        if (!book.isAvailable) {
          book.isAvailable = true
          true
        } else false
      case None => false
    }
  }

  def listBooks(): List[String] = {
    books.map { book =>
      s""${book.title} by ${book.author} (${if (book.isAvailable) ""Available"" else ""Checked Out""})""
    }
  }
}

object LibraryApp extends App {
  val library = new Library

  library.addBook(""1984"", ""George Orwell"")
  library.addBook(""To Kill a Mockingbird"", ""Harper Lee"")
  library.addBook(""The Great Gatsby"", ""F. Scott Fitzgerald"")

  val bookTitle = ""1984""
  library.findBook(bookTitle) match {
    case Some(book) => println(s""Found: ${book.title} by ${book.author}"")
    case None => println(s""Book '$bookTitle' not found"")
  }

  if (library.checkoutBook(bookTitle)) {
    println(s""Book '$bookTitle' checked out successfully"")
  } else {
    println(s""Book '$bookTitle' could not be checked out"")
  }

  if (library.returnBook(bookTitle)) {
    println(s""Book '$bookTitle' returned successfully"")
  } else {
    println(s""Book '$bookTitle' could not be returned"")
  }

  if (library.removeBook(bookTitle)) {
    println(s""Book '$bookTitle' removed successfully"")
  } else {
    println(s""Book '$bookTitle' could not be removed"")
  }

  println(""All books in the library:"")
  library.listBooks().foreach(println)
}
";

MyScalaAnalyzer.AnalyzeScalaCode(scalaCode);
