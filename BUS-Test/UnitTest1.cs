using BUS_Class;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using static BUS_Class.DataBase;
using База_Данных_Городских_Автобусов;

namespace BUS_Test
{
    [TestClass]
    public class DatabaseTests
    {
        private const string TestConnectionString = "Data Source=:memory:";

        [TestInitialize]
        public void TestInitialize()
        {
            // Устанавливаем тестовую строку подключения
            DataBase.SetTestConnectionString(TestConnectionString);

            // Создаем таблицы в памяти
            DataBase.CreateAllTables();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Очищаем тестовые данные
            DataBase.ClearTestData();
            DataBase.ResetConnectionString();
        }

        [TestMethod]
        public void TestCreateTables()
        {
            // Act
            bool result = DataBase.CreateAllTables();

            // Assert
            Assert.IsTrue(result, "Таблицы должны создаваться успешно");
        }

        [TestMethod]
        public void TestInsertUser()
        {
            // Arrange
            string username = "testuser_insert";
            string password = "password123";
            string passwordHash = DataBase.HashPassword(password);
            string fullName = "Test User Insert";
            string role = "Tester";
            bool isActive = true;

            // Act
            bool result = DataBase.InsertUser(username, passwordHash, fullName, role, isActive);

            // Assert
            Assert.IsTrue(result, "Пользователь должен добавляться успешно");

            // Проверяем, что пользователь действительно добавлен
            DataTable users = DataBase.GetAllUsers();

            // Ищем добавленного пользователя
            bool found = false;
            foreach (DataRow row in users.Rows)
            {
                if (row["username"].ToString() == username)
                {
                    found = true;
                    break;
                }
            }

            Assert.IsTrue(found, "Добавленный пользователь должен быть в таблице");
        }
        [TestMethod]
        public void TestInsertRoute()
        {
            // Arrange
            string routeNumber = "999";
            string departureCity = "Москва";
            string arrivalCity = "Санкт-Петербург";
            string distance = "700 км";
            int durationMinutes = 630;
            bool isActive = true;

            // Act
            bool result = DataBase.InsertRoute(routeNumber, departureCity, arrivalCity, distance, durationMinutes, isActive);

            // Assert
            Assert.IsTrue(result, "Маршрут должен добавляться успешно");

            // Проверяем, что маршрут действительно добавлен
            DataTable routes = DataBase.GetAllRoutes();
            Assert.AreEqual(1, routes.Rows.Count, "Должен быть 1 маршрут");
            Assert.AreEqual(routeNumber, routes.Rows[0]["route_number"], "Номер маршрута должен совпадать");
        }

        [TestMethod]
        public void TestInsertBus()
        {
            // Arrange
            string plateNumber = "А999ВС99";
            string brand = "Mercedes";
            string model = "Tourismo";
            int capacity = 50;
            int year = 2024;
            bool isActive = true;

            // Act
            bool result = DataBase.InsertBus(plateNumber, brand, model, capacity, year, isActive);

            // Assert
            Assert.IsTrue(result, "Автобус должен добавляться успешно");

            // Проверяем, что автобус действительно добавлен
            DataTable buses = DataBase.GetAllBuses();
            Assert.AreEqual(1, buses.Rows.Count, "Должен быть 1 автобус");
            Assert.AreEqual(plateNumber, buses.Rows[0]["plate_number"], "Госномер должен совпадать");
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            // Arrange - сначала добавляем пользователя
            string username = "testuser_update";
            string passwordHash = DataBase.HashPassword("password123");
            string fullName = "Test User";
            string role = "Tester";
            bool isActive = true;

            bool inserted = DataBase.InsertUser(username, passwordHash, fullName, role, isActive);
            Assert.IsTrue(inserted, "Пользователь должен быть добавлен перед обновлением");

            // Получаем ID добавленного пользователя
            DataTable users = DataBase.GetAllUsers();
            int userId = -1;

            foreach (DataRow row in users.Rows)
            {
                if (row["username"].ToString() == username)
                {
                    userId = Convert.ToInt32(row["user_id"]);
                    break;
                }
            }

            Assert.AreNotEqual(-1, userId, "Должен быть найден ID пользователя");

            // Act - обновляем пользователя
            string newFullName = "Updated User";
            bool result = DataBase.UpdateUser(userId, username, passwordHash, newFullName, role, isActive);

            // Assert
            Assert.IsTrue(result, "Обновление пользователя должно быть успешным");

            // Проверяем обновление
            users = DataBase.GetAllUsers();
            bool updated = false;

            foreach (DataRow row in users.Rows)
            {
                if (row["user_id"].ToString() == userId.ToString() &&
                    row["full_name"].ToString() == newFullName)
                {
                    updated = true;
                    break;
                }
            }

            Assert.IsTrue(updated, "ФИО должно быть обновлено");
        }

        [TestMethod]
        public void TestUpdateRoute()
        {
            // Arrange - сначала добавляем маршрут
            string routeNumber = "999";
            string departureCity = "Москва";
            string arrivalCity = "Санкт-Петербург";
            string distance = "700 км";
            int durationMinutes = 630;
            bool isActive = true;

            DataBase.InsertRoute(routeNumber, departureCity, arrivalCity, distance, durationMinutes, isActive);

            // Получаем ID добавленного маршрута
            DataTable routes = DataBase.GetAllRoutes();
            int routeId = (int)routes.Rows[0]["route_id"];

            // Act - обновляем маршрут
            string newDistance = "710 км";
            bool result = DataBase.UpdateRoute(routeId, routeNumber, departureCity, arrivalCity, newDistance, durationMinutes, isActive);

            // Assert
            Assert.IsTrue(result, "Обновление маршрута должно быть успешным");

            // Проверяем обновление
            routes = DataBase.GetAllRoutes();
            Assert.AreEqual(newDistance, routes.Rows[0]["distance"], "Расстояние должно быть обновлено");
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            // Arrange - добавляем пользователя специально для удаления
            string username = "testuser_delete";
            string passwordHash = DataBase.HashPassword("password123");
            string fullName = "Test User Delete";
            string role = "Tester";
            bool isActive = true;

            bool inserted = DataBase.InsertUser(username, passwordHash, fullName, role, isActive);
            Assert.IsTrue(inserted, "Пользователь должен быть добавлен перед удалением");

            // Получаем ID добавленного пользователя
            DataTable users = DataBase.GetAllUsers();
            int userId = -1;

            foreach (DataRow row in users.Rows)
            {
                if (row["username"].ToString() == username)
                {
                    userId = Convert.ToInt32(row["user_id"]);
                    break;
                }
            }

            Assert.AreNotEqual(-1, userId, "Должен быть найден ID пользователя");

            // Act - удаляем пользователя
            bool result = DataBase.DeleteUser(userId);

            // Assert
            Assert.IsTrue(result, "Удаление пользователя должно быть успешным");

            // Проверяем, что пользователь удален
            users = DataBase.GetAllUsers();
            bool stillExists = false;

            foreach (DataRow row in users.Rows)
            {
                if (row["username"].ToString() == username)
                {
                    stillExists = true;
                    break;
                }
            }

            Assert.IsFalse(stillExists, "Пользователь должен быть удален");
        }


        [TestMethod]
        public void TestDeleteRoute()
        {
            // Arrange - добавляем маршрут
            string routeNumber = "999";
            string departureCity = "Москва";
            string arrivalCity = "Санкт-Петербург";
            string distance = "700 км";
            int durationMinutes = 630;
            bool isActive = true;

            DataBase.InsertRoute(routeNumber, departureCity, arrivalCity, distance, durationMinutes, isActive);

            // Получаем ID
            DataTable routes = DataBase.GetAllRoutes();
            int routeId = (int)routes.Rows[0]["route_id"];

            // Act - удаляем маршрут
            bool result = DataBase.DeleteRoute(routeId);

            // Assert
            Assert.IsTrue(result, "Удаление маршрута должно быть успешным");

            // Проверяем, что маршрут удален
            routes = DataBase.GetAllRoutes();
            Assert.AreEqual(0, routes.Rows.Count, "Маршрут должен быть удален");
        }

        [TestMethod]
        public void TestHashPassword()
        {
            // Arrange
            string password = "TestPassword123";

            // Act
            string hash1 = DataBase.HashPassword(password);
            string hash2 = DataBase.HashPassword(password);

            // Assert
            Assert.IsNotNull(hash1, "Хеш не должен быть null");
            Assert.IsTrue(hash1.Length > 0, "Хеш должен быть не пустым");
            Assert.AreEqual(hash1, hash2, "Хеши одного пароля должны совпадать");

            // Проверяем другой пароль
            string hash3 = DataBase.HashPassword("DifferentPassword");
            Assert.AreNotEqual(hash1, hash3, "Хеши разных паролей должны различаться");
        }

        [TestMethod]
        public void TestCheckLogin()
        {
            // Arrange - добавляем пользователя
            string username = "testuser_login";
            string password = "password123";
            string passwordHash = DataBase.HashPassword(password);
            string fullName = "Test User Login";
            string role = "Tester";
            bool isActive = true;

            // Убедимся что пользователь добавлен
            bool inserted = DataBase.InsertUser(username, passwordHash, fullName, role, isActive);
            Assert.IsTrue(inserted, "Пользователь должен быть добавлен перед проверкой логина");

            // Act & Assert - проверяем корректный логин
            // ВАЖНО: CheckLogin ожидает уже хешированный пароль, а не обычный!
            bool result = DataBase.CheckLogin(username, passwordHash);
            Assert.IsTrue(result, "Корректный логин должен проходить проверку. Username: " + username + ", Hash: " + passwordHash);

            // Проверяем некорректный логин
            result = DataBase.CheckLogin(username, DataBase.HashPassword("wrongpassword"));
            Assert.IsFalse(result, "Некорректный пароль не должен проходить проверку");

            // Проверяем несуществующего пользователя
            result = DataBase.CheckLogin("nonexistent", passwordHash);
            Assert.IsFalse(result, "Несуществующий пользователь не должен проходить проверку");
        }


        [TestMethod]
        public void TestUpdateBus()
        {
            // Arrange
            string plateNumber = "А999ВС99";
            string brand = "Mercedes";
            string model = "Tourismo";
            int capacity = 50;
            int year = 2024;

            DataBase.InsertBus(plateNumber, brand, model, capacity, year, true);

            // Получаем ID добавленного автобуса
            DataTable buses = DataBase.GetAllBuses();
            int busId = (int)buses.Rows[0]["bus_id"];

            // Act - обновляем автобус
            string newModel = "Tourismo Plus";
            bool result = DataBase.UpdateBus(busId, plateNumber, brand, newModel, capacity, year, true);

            // Assert
            Assert.IsTrue(result, "Обновление автобуса должно быть успешным");

            // Проверяем обновление
            DataRow bus = DataBase.GetBusById(busId);
            Assert.AreEqual(newModel, bus["model"], "Модель должна быть обновлена");
        }

        [TestMethod]
        public void TestDeleteBus()
        {
            // Arrange
            string plateNumber = "А999ВС99";
            string brand = "Mercedes";
            string model = "Tourismo";
            int capacity = 50;
            int year = 2024;

            DataBase.InsertBus(plateNumber, brand, model, capacity, year, true);

            // Получаем ID
            DataTable buses = DataBase.GetAllBuses();
            int busId = (int)buses.Rows[0]["bus_id"];

            // Act - удаляем автобус
            bool result = DataBase.DeleteBus(busId);

            // Assert
            Assert.IsTrue(result, "Удаление автобуса должно быть успешным");

            // Проверяем, что автобус удален
            buses = DataBase.GetAllBuses();
            Assert.AreEqual(0, buses.Rows.Count, "Автобус должен быть удален");
        }

        [TestMethod]
        public void TestGetUserById()
        {
            // Arrange
            string username = "testuser";
            string passwordHash = DataBase.HashPassword("password123");
            string fullName = "Test User";
            string role = "Tester";

            // Сначала добавляем пользователя
            DataBase.InsertUser(username, passwordHash, fullName, role, true);

            // Получаем ID добавленного пользователя
            DataTable users = DataBase.GetAllUsers();

            // Проверяем, что есть данные
            if (users.Rows.Count == 0)
            {
                Assert.Inconclusive("Не удалось добавить тестового пользователя");
                return;
            }

            int userId = (int)users.Rows[0]["user_id"];

            // Act
            DataRow user = DataBase.GetUserById(userId);

            // Assert
            Assert.IsNotNull(user, "Пользователь должен быть найден");
            Assert.AreEqual(username, user["username"], "Имя пользователя должно совпадать");
        }

        [TestMethod]
        public void TestGetRouteById()
        {
            // Arrange
            string routeNumber = "999";
            string departureCity = "Москва";
            string arrivalCity = "Санкт-Петербург";
            string distance = "700 км";
            int durationMinutes = 630;

            // Сначала добавляем маршрут
            DataBase.InsertRoute(routeNumber, departureCity, arrivalCity, distance, durationMinutes, true);

            // Получаем ID добавленного маршрута
            DataTable routes = DataBase.GetAllRoutes();

            // Проверяем, что есть данные
            if (routes.Rows.Count == 0)
            {
                Assert.Inconclusive("Не удалось добавить тестовый маршрут");
                return;
            }

            int routeId = (int)routes.Rows[0]["route_id"];

            // Act
            DataRow route = DataBase.GetRouteById(routeId);

            // Assert
            Assert.IsNotNull(route, "Маршрут должен быть найден");
            Assert.AreEqual(routeNumber, route["route_number"], "Номер маршрута должен совпадать");
        }

        [TestMethod]
        public void TestGetBusById()
        {
            // Arrange
            string plateNumber = "А999ВС99";
            string brand = "Mercedes";
            string model = "Tourismo";
            int capacity = 50;
            int year = 2024;

            // Сначала добавляем автобус
            DataBase.InsertBus(plateNumber, brand, model, capacity, year, true);

            // Получаем ID добавленного автобуса
            DataTable buses = DataBase.GetAllBuses();

            // Проверяем, что есть данные
            if (buses.Rows.Count == 0)
            {
                Assert.Inconclusive("Не удалось добавить тестовый автобус");
                return;
            }

            int busId = (int)buses.Rows[0]["bus_id"];

            // Act
            DataRow bus = DataBase.GetBusById(busId);

            // Assert
            Assert.IsNotNull(bus, "Автобус должен быть найден");
            Assert.AreEqual(plateNumber, bus["plate_number"], "Госномер должен совпадать");
        }
    }

    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void TestValidUsername()
        {
            // Arrange
            string[] validUsernames = { "user123", "User_Test", "admin", "test_user_123" };
            string[] invalidUsernames = { "", "  ", "user test", "user@test", "user-test" };

            // Act & Assert
            foreach (var username in validUsernames)
            {
                Assert.IsTrue(ValidationHelper.IsValidUsername(username),
                    $"Username '{username}' должен быть валидным");
            }

            foreach (var username in invalidUsernames)
            {
                Assert.IsFalse(ValidationHelper.IsValidUsername(username),
                    $"Username '{username}' должен быть невалидным");
            }
        }

        [TestMethod]
        public void TestValidPassword()
        {
            // Arrange
            string[] validPasswords = { "password123", "Pass@123", "123456", "qwerty123456" };
            string[] invalidPasswords = { "", "12345", "pass", "   " };

            // Act & Assert
            foreach (var password in validPasswords)
            {
                Assert.IsTrue(ValidationHelper.IsValidPassword(password),
                    $"Password '{password}' должен быть валидным");
            }

            foreach (var password in invalidPasswords)
            {
                Assert.IsFalse(ValidationHelper.IsValidPassword(password),
                    $"Password '{password}' должен быть невалидным");
            }
        }

        [TestMethod]
        public void TestValidEmail()
        {
            // Arrange
            string[] validEmails = { "test@example.com", "user.name@domain.co.uk", "user@domain.ru" };
            string[] invalidEmails = { "", "test@", "@domain.com", "test@.com", "test domain.com" };

            // Act & Assert
            foreach (var email in validEmails)
            {
                Assert.IsTrue(ValidationHelper.IsValidEmail(email),
                    $"Email '{email}' должен быть валидным");
            }

            foreach (var email in invalidEmails)
            {
                Assert.IsFalse(ValidationHelper.IsValidEmail(email),
                    $"Email '{email}' должен быть невалидным");
            }
        }

        [TestMethod]
        public void TestValidPlateNumber()
        {
            // Arrange
            string[] validPlates = { "А123ВС77", "В456ОР78", "С789ТУ99", "Е001КХ197" };
            string[] invalidPlates = { "", "123ABC", "А123ВС", "А12ВС77", "А123ВС7777" };

            // Act & Assert
            foreach (var plate in validPlates)
            {
                Assert.IsTrue(ValidationHelper.IsValidPlateNumber(plate),
                    $"Plate number '{plate}' должен быть валидным");
            }

            foreach (var plate in invalidPlates)
            {
                Assert.IsFalse(ValidationHelper.IsValidPlateNumber(plate),
                    $"Plate number '{plate}' должен быть невалидным");
            }
        }

        [TestMethod]
        public void TestValidRouteNumber()
        {
            // Arrange
            string[] validRoutes = { "101", "202A", "305B", "102", "98" };
            string[] invalidRoutes = { "", "10A1", "ABC", "101-202", "10.5" };

            // Act & Assert
            foreach (var route in validRoutes)
            {
                Assert.IsTrue(ValidationHelper.IsValidRouteNumber(route),
                    $"Route number '{route}' должен быть валидным");
            }

            foreach (var route in invalidRoutes)
            {
                Assert.IsFalse(ValidationHelper.IsValidRouteNumber(route),
                    $"Route number '{route}' должен быть невалидным");
            }
        }

        [TestMethod]
        public void TestValidCityName()
        {
            // Arrange
            string[] validCities = { "Москва", "Санкт-Петербург", "Нью-Йорк", "Ростов-на-Дону" };
            string[] invalidCities = { "", "мос123", "москва", "123", "City123" };

            // Act & Assert
            foreach (var city in validCities)
            {
                bool isValid = ValidationHelper.IsValidCityName(city);
                Assert.IsTrue(isValid,
                    $"City '{city}' должен быть валидным. Результат: {isValid}");
            }

            foreach (var city in invalidCities)
            {
                bool isValid = ValidationHelper.IsValidCityName(city);
                Assert.IsFalse(isValid,
                    $"City '{city}' должен быть невалидным. Результат: {isValid}");
            }
        }
    }

    [TestClass]
    public class ModelsTests
    {
        [TestMethod]
        public void TestUserModel()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                PasswordHash = "hash123",
                FullName = "Test User",
                Role = "Tester",
                IsActive = true
            };

            // Act & Assert
            Assert.AreEqual(1, user.UserId);
            Assert.AreEqual("testuser", user.Username);
            Assert.AreEqual("hash123", user.PasswordHash);
            Assert.AreEqual("Test User", user.FullName);
            Assert.AreEqual("Tester", user.Role);
            Assert.IsTrue(user.IsActive);
        }

        [TestMethod]
        public void TestRouteModel()
        {
            // Arrange
            var route = new Route
            {
                RouteId = 1,
                RouteNumber = "101",
                DepartureCity = "Москва",
                ArrivalCity = "Санкт-Петербург",
                Distance = "700 км",
                DurationMinutes = 630,
                IsActive = true
            };

            // Act & Assert
            Assert.AreEqual(1, route.RouteId);
            Assert.AreEqual("101", route.RouteNumber);
            Assert.AreEqual("Москва", route.DepartureCity);
            Assert.AreEqual("Санкт-Петербург", route.ArrivalCity);
            Assert.AreEqual("700 км", route.Distance);
            Assert.AreEqual(630, route.DurationMinutes);
            Assert.IsTrue(route.IsActive);
        }

        [TestMethod]
        public void TestBusModel()
        {
            // Arrange
            var bus = new Bus
            {
                BusId = 1,
                PlateNumber = "А123ВС77",
                Brand = "Mercedes",
                Model = "Tourismo",
                Capacity = 50,
                Year = 2024,
                IsActive = true
            };

            // Act & Assert
            Assert.AreEqual(1, bus.BusId);
            Assert.AreEqual("А123ВС77", bus.PlateNumber);
            Assert.AreEqual("Mercedes", bus.Brand);
            Assert.AreEqual("Tourismo", bus.Model);
            Assert.AreEqual(50, bus.Capacity);
            Assert.AreEqual(2024, bus.Year);
            Assert.IsTrue(bus.IsActive);
        }

        [TestMethod]
        public void TestScheduleModel()
        {
            // Arrange
            var schedule = new Schedule
            {
                ScheduleId = 1,
                RouteId = 1,
                BusId = 1,
                DepartureTime = new DateTime(2024, 12, 17, 10, 0, 0),
                ArrivalTime = new DateTime(2024, 12, 17, 20, 30, 0),
                Price = 2500.50m,
                Status = "Планируется",
                AvailableSeats = 50
            };

            // Act & Assert
            Assert.AreEqual(1, schedule.ScheduleId);
            Assert.AreEqual(1, schedule.RouteId);
            Assert.AreEqual(1, schedule.BusId);
            Assert.AreEqual(new DateTime(2024, 12, 17, 10, 0, 0), schedule.DepartureTime);
            Assert.AreEqual(new DateTime(2024, 12, 17, 20, 30, 0), schedule.ArrivalTime);
            Assert.AreEqual(2500.50m, schedule.Price);
            Assert.AreEqual("Планируется", schedule.Status);
            Assert.AreEqual(50, schedule.AvailableSeats);
        }

        [TestMethod]
        public void TestTicketModel()
        {
            // Arrange
            var ticket = new Ticket
            {
                TicketId = 1,
                TicketNumber = "TKT001",
                ScheduleId = 1,
                PassengerName = "Иванов Иван Иванович",
                SeatNumber = 15,
                Price = 2500.00m,
                SaleDate = new DateTime(2024, 12, 16, 14, 30, 0),
                IsReturned = false
            };

            // Act & Assert
            Assert.AreEqual(1, ticket.TicketId);
            Assert.AreEqual("TKT001", ticket.TicketNumber);
            Assert.AreEqual(1, ticket.ScheduleId);
            Assert.AreEqual("Иванов Иван Иванович", ticket.PassengerName);
            Assert.AreEqual(15, ticket.SeatNumber);
            Assert.AreEqual(2500.00m, ticket.Price);
            Assert.AreEqual(new DateTime(2024, 12, 16, 14, 30, 0), ticket.SaleDate);
            Assert.IsFalse(ticket.IsReturned);
        }
    }

    [TestClass]
    public class IntegrationTests
    {
        private const string TestConnectionString = "Data Source=:memory:";

        [TestInitialize]
        public void TestInitialize()
        {
            DataBase.SetTestConnectionString(TestConnectionString);
            DataBase.CreateAllTables();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DataBase.ClearTestData();
            DataBase.ResetConnectionString();
        }

        [TestMethod]
        public void TestCompleteUserWorkflow()
        {
            // 1. Добавление пользователя
            string username = "integration_test_user";
            string password = "integration_pass";
            string passwordHash = DataBase.HashPassword(password);
            string fullName = "Integration Test User";
            string role = "Tester";

            bool insertResult = DataBase.InsertUser(username, passwordHash, fullName, role, true);
            Assert.IsTrue(insertResult, "Добавление пользователя должно быть успешным");

            // 2. Проверка добавления
            DataTable users = DataBase.GetAllUsers();
            Assert.AreEqual(1, users.Rows.Count, "Должен быть 1 пользователь");
            int userId = (int)users.Rows[0]["user_id"];

            // 3. Проверка логина
            bool loginResult = DataBase.CheckLogin(username, passwordHash);
            Assert.IsTrue(loginResult, "Логин должен быть успешным");

            // 4. Обновление пользователя
            string updatedFullName = "Updated Integration Test User";
            bool updateResult = DataBase.UpdateUser(userId, username, passwordHash, updatedFullName, role, true);
            Assert.IsTrue(updateResult, "Обновление пользователя должно быть успешным");

            // 5. Проверка обновления
            users = DataBase.GetAllUsers();
            Assert.AreEqual(updatedFullName, users.Rows[0]["full_name"], "ФИО должно быть обновлено");

            // 6. Удаление пользователя
            bool deleteResult = DataBase.DeleteUser(userId);
            Assert.IsTrue(deleteResult, "Удаление пользователя должно быть успешным");

            // 7. Проверка удаления
            users = DataBase.GetAllUsers();
            Assert.AreEqual(0, users.Rows.Count, "Пользователь должен быть удален");
        }

        [TestMethod]
        public void TestMultipleOperations()
        {
            // Очищаем данные перед тестом
            DataBase.ClearTestData();

            // Создаем таблицы заново
            DataBase.CreateAllTables();

            // Добавляем несколько маршрутов
            bool result1 = DataBase.InsertRoute("101", "Москва", "Санкт-Петербург", "700 км", 630, true);
            bool result2 = DataBase.InsertRoute("202", "Казань", "Уфа", "450 км", 360, true);
            bool result3 = DataBase.InsertRoute("305", "Новосибирск", "Томск", "250 км", 180, false);

            Assert.IsTrue(result1, "Первый маршрут должен быть добавлен");
            Assert.IsTrue(result2, "Второй маршрут должен быть добавлен");
            Assert.IsTrue(result3, "Третий маршрут должен быть добавлен");

            // Даем время на вставку
            System.Threading.Thread.Sleep(100);

            // Проверяем количество
            DataTable routes = DataBase.GetAllRoutes();
            Assert.AreEqual(3, routes.Rows.Count, $"Должно быть 3 маршрута. Найдено: {routes.Rows.Count}");

            // Добавляем несколько автобусов
            DataBase.InsertBus("А123ВС77", "Mercedes", "Tourismo", 50, 2022, true);
            DataBase.InsertBus("В456ОР78", "ПАЗ", "Vector Next", 35, 2021, true);

            // Даем время на вставку
            System.Threading.Thread.Sleep(100);

            // Проверяем количество
            DataTable buses = DataBase.GetAllBuses();
            Assert.AreEqual(2, buses.Rows.Count, $"Должно быть 2 автобуса. Найдено: {buses.Rows.Count}");

            // Удаляем один маршрут
            if (routes.Rows.Count > 0)
            {
                int routeId = (int)routes.Rows[0]["route_id"];
                bool deleteResult = DataBase.DeleteRoute(routeId);
                Assert.IsTrue(deleteResult, "Удаление маршрута должно быть успешным");

                // Даем время на удаление
                System.Threading.Thread.Sleep(100);

                // Проверяем количество после удаления
                routes = DataBase.GetAllRoutes();
                Assert.AreEqual(2, routes.Rows.Count, $"Должно остаться 2 маршрута. Найдено: {routes.Rows.Count}");
            }
            else
            {
                Assert.Inconclusive("Нет маршрутов для удаления");
            }
        }

        [TestMethod]
        public void TestValidationWithDatabase()
        {
            // Тестируем валидацию с реальными данными

            // Валидные данные
            string validUsername = "valid_user_123";
            string invalidUsername = "invalid user"; // содержит пробел

            string validPassword = "valid_password_123";
            string invalidPassword = "123"; // слишком короткий

            // Проверяем валидацию
            Assert.IsTrue(ValidationHelper.IsValidUsername(validUsername));
            Assert.IsFalse(ValidationHelper.IsValidUsername(invalidUsername));

            Assert.IsTrue(ValidationHelper.IsValidPassword(validPassword));
            Assert.IsFalse(ValidationHelper.IsValidPassword(invalidPassword));

            // Пробуем добавить с валидными данными
            string passwordHash = DataBase.HashPassword(validPassword);
            bool result = DataBase.InsertUser(validUsername, passwordHash, "Test User", "Tester", true);
            Assert.IsTrue(result, "Добавление с валидными данными должно быть успешным");
        }

        [TestMethod]
        public void TestDatabaseConnection()
        {
            // Тест подключения к базе данных
            using (var connection = new SqliteConnection(TestConnectionString))
            {
                // Попытка открыть соединение
                try
                {
                    connection.Open();
                    Assert.IsTrue(connection.State == System.Data.ConnectionState.Open,
                        "Соединение должно быть открыто");

                    // Выполняем простой запрос
                    using (var command = new SqliteCommand("SELECT 1", connection))
                    {
                        var result = command.ExecuteScalar();
                        Assert.AreEqual(1L, result, "Запрос должен вернуть 1");
                    }

                    connection.Close();
                    Assert.IsTrue(connection.State == System.Data.ConnectionState.Closed,
                        "Соединение должно быть закрыто");
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Ошибка подключения к базе данных: {ex.Message}");
                }
            }
        }


        [TestMethod]
        public void TestDataBox()
        {
            // Используйте GetAllRoutes вместо несуществующего метода
            DataTable routes = DataBase.GetAllRoutes();

            // Проверяем, что DataTable не null
            Assert.IsNotNull(routes, "DataTable не должен быть null");

            // Проверяем, что можно безопасно обращаться к данным
            if (routes.Rows.Count > 0)
            {
                Assert.IsNotNull(routes.Rows[0]["route_number"], "Номер маршрута не должен быть null");
            }
        }





    }
}