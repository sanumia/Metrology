import scala.io.StdIn.readLine
import scala.util.{Try, Success, Failure}

object TextAdventure {
  def main(args: Array[String]): Unit = {
    val game = new AdventureGame
    game.run()
  }
}

case class Item(name: String, description: String)

class Location(
  val name: String,
  val description: String,
  var items: List[Item] = Nil,
  var exits: Map[String, Location] = Map()
) {
  def addExit(direction: String, location: Location): Unit = {
    exits += (direction -> location)
  }
  
  def removeItem(item: Item): Unit = {
    items = items.filterNot(_ == item)
  }
  
  def addItem(item: Item): Unit = {
    items = item :: items
  }
}

class Player {
  var currentLocation: Location = _
  var inventory: List[Item] = Nil
  
  def moveTo(location: Location): Unit = {
    currentLocation = location
  }
  
  def takeItem(item: Item): Unit = {
    inventory = item :: inventory
    currentLocation.removeItem(item)
  }
  
  def hasItem(itemName: String): Boolean = {
    inventory.exists(_.name.equalsIgnoreCase(itemName))
  }
}

class AdventureGame {
  private val player = new Player
  private var isRunning = true
  
  // Create locations
  private val entrance = new Location(
    "Главный вход",
    "Вы стоите перед старым особняком. Кованые ворота скрипят на ветру."
  )
  
  private val hallway = new Location(
    "Холл",
    "Просторный холл с мраморной лестницей. На стене висит потрескавшееся зеркало."
  )
  
  private val kitchen = new Location(
    "Кухня",
    "Заброшенная кухня. Стол покрыт толстым слоем пыли. На плите стоит ржавый чайник."
  )
  
  private val garden = new Location(
    "Сад",
    "Запущенный сад с высохшим фонтаном. Кусты роз разрослись до невероятных размеров."
  )
  
  private val study = new Location(
    "Кабинет",
    "Комната с дубовыми книжными шкафами. На столе лежит открытый дневник."
  )
  
  // Set up exits
  entrance.addExit("север", hallway)
  
  hallway.addExit("юг", entrance)
  hallway.addExit("запад", kitchen)
  hallway.addExit("восток", study)
  hallway.addExit("север", garden)
  
  kitchen.addExit("восток", hallway)
  garden.addExit("юг", hallway)
  study.addExit("запад", hallway)
  
  // Add items
  entrance.addItem(Item("ключ", "Ржавый ключ с гравировкой '1887'"))
  study.addItem(Item("дневник", "Старый дневник с записями на неизвестном языке"))
  kitchen.addItem(Item("чайник", "Ржавый медный чайник"))
  garden.addItem(Item("роза", "Черная роза необычайной красоты"))
  
  player.moveTo(entrance)
  
  private def printHelp(): Unit = {
    println("""Доступные команды:
              |go <направление> - перемещение
              |take <предмет> - взять предмет
              |inventory - показать инвентарь
              |look - осмотреться
              |use <предмет> - использовать предмет
              |help - помощь
              |quit - выход""".stripMargin)
  }
  
  private def processCommand(command: String): Unit = {
    val parts = command.toLowerCase.split(" ", 2)
    parts(0) match {
      case "go" if parts.length > 1 =>
        player.currentLocation.exits.get(parts(1)) match {
          case Some(location) =>
            player.moveTo(location)
            println(s"Вы перешли в ${location.name}")
          case None =>
            println("Нет такого пути!")
        }
        
      case "take" if parts.length > 1 =>
        player.currentLocation.items.find(_.name.equalsIgnoreCase(parts(1))) match {
          case Some(item) =>
            player.takeItem(item)
            println(s"Вы взяли ${item.name}")
          case None =>
            println("Такого предмета здесь нет")
        }
        
      case "inventory" =>
        if (player.inventory.isEmpty) println("Инвентарь пуст")
        else {
          println("В вашем инвентаре:")
          player.inventory.foreach(item => println(s"- ${item.name}: ${item.description}"))
        }
        
      case "look" =>
        println(player.currentLocation.description)
        println("Предметы здесь: " + 
          (if (player.currentLocation.items.isEmpty) "нет"

           else player.currentLocation.items.map(_.name).mkString(", ")))
        println("Выходы: " + player.currentLocation.exits.keys.mkString(", "))
        
      case "use" if parts.length > 1 =>
        if (player.hasItem(parts(1))) {
          parts(1) match {
            case "ключ" if player.currentLocation == entrance =>
              println("Вы открыли ворота и сбежали из особняка! ПОБЕДА!")
              isRunning = false
            case _ =>
              println("Здесь это нельзя использовать")
          }
        } else {
          println("У вас нет такого предмета")
        }
        
      case "help" =>
        printHelp()
        
      case "quit" =>
        println("До свидания!")
        isRunning = false
        
      case _ =>
        println("Неизвестная команда. Введите 'help' для помощи.")
    }
  }
  
  def run(): Unit = {
    println("Добро пожаловать в Текстовый Квест!\n")
    printHelp()
    println("\nСейчас вы находитесь:")
    println(player.currentLocation.name)
    
    while (isRunning) {
      print("\n> ")
      Try(readLine()) match {
        case Success(command) if command.nonEmpty =>
          processCommand(command.trim)
        case Success(_) => // пустой ввод
        case Failure(e) =>
          println(s"Ошибка ввода: ${e.getMessage}")
          isRunning = false
      }
    }
  }
}
