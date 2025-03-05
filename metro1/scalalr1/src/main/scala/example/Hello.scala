object SimpleScalaApp {

  def factorial(n: Int): Int = {
    if (n <= 1) 1
    else n * factorial(n - 1)
  }

  def isPrime(n: Int): Boolean = {
    if (n <= 1) false
    else if (n == 2) true
    else !(2 to Math.sqrt(n).toInt).exists(x => n % x == 0)
  }

  def findPrimesInRange(start: Int, end: Int): List[Int] = {
    (start to end).filter(isPrime).toList
  }

  def sumList(numbers: List[Int]): Int = {
    numbers.foldLeft(0)(_ + _)
  }

  def findMax(numbers: List[Int]): Int = {
    numbers.foldLeft(Int.MinValue)((max, num) => if (num > max) num else max)
  }

  def findMin(numbers: List[Int]): Int = {
    numbers.foldLeft(Int.MaxValue)((min, num) => if (num < min) num else min)
  }

  def reverseList[T](list: List[T]): List[T] = {
    list.foldLeft(List.empty[T])((acc, item) => item :: acc)
  }

  def isPalindrome(str: String): Boolean = {
    val cleanedStr = str.replaceAll("\\s", "").toLowerCase
    cleanedStr == cleanedStr.reverse
  }

  def fibonacci(n: Int): Int = {
    def fibHelper(n: Int, a: Int, b: Int): Int = {
      if (n == 0) a
      else fibHelper(n - 1, b, a + b)
    }
    fibHelper(n, 0, 1)
  }

  def listToString(numbers: List[Int]): String = {
    numbers.mkString("[", ", ", "]")
  }

  def main(args: Array[String]): Unit = {
    val number = 5
    println(s"Factorial of $number: ${factorial(number)}")

    val primeCheck = 13
    println(s"Is $primeCheck a prime number? ${isPrime(primeCheck)}")

    val primesInRange = findPrimesInRange(10, 50)
    println(s"Prime numbers in the range from 10 to 50: ${listToString(primesInRange)}")

    val numbers = List(3, 1, 4, 1, 5, 9, 2, 6, 5)
    println(s"Sum of numbers in the list: ${sumList(numbers)}")
    println(s"Maximum number in the list: ${findMax(numbers)}")
    println(s"Minimum number in the list: ${findMin(numbers)}")
    println(s"Reversed list: ${listToString(reverseList(numbers))}")

    val palindromeCheck = "A man a plan a canal Panama"
    println(s"Is the string '$palindromeCheck' a palindrome? ${isPalindrome(palindromeCheck)}")

    val fibNumber = 10
    println(s"Fibonacci number at position $fibNumber: ${fibonacci(fibNumber)}")
  }
}
