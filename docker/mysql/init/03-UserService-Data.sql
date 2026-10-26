-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: userservice
-- ------------------------------------------------------
-- Server version	8.0.44

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `role`
--
INSERT INTO `role` (Id, RoleKey, DisplayName, Description) VALUES (1,'ADMINISTRATOR','Administrator','Administrators are responsible Team activities across a series of Warehouses.'),(2,'SUPERVISOR','Supervisor','Supervisors are responsible for team activities within a certain Warehouse.'),(3,'WAREHOUSE_WORKER','Warehouse Worker','Warehouse Workers perform various tasks for their respective teams in a specified warehouse.'),(4,'DRIVER','Driver','Drivers are responsible for transporting cargo to and from the Warehouse.');

--
-- Dumping data for table `user`
--
INSERT INTO `user` (Id, Username, PasswordHash, FirstName, LastName, Team, Email, PhoneNumber, RoleId, IsActive, LastActive) VALUES (1,'Test_1','PBKDF2-SHA256$100000$2lunOajBJgz5U8PWOvmOZg==$tPXihephrrXwQrD3fycqYrfnaAUlhdd5jn32RPt97Mw=','Tester','Tester','Test','Test_1@Test.com','(500)500-5000',3,1,NULL),(2,'Password@@Master11','PBKDF2-SHA256$100000$n/RhQPq3SMHjqyeWvpXj4Q==$xmF/jJiFlQAb0TS9nucSrIqU7Rm5vsdaWpBh0Mne2Jc=','Password','Master','Admins','Password@master.com','(555)555-5555',1,1,NULL),(3,'Admin_1','PBKDF2-SHA256$100000$xYNnHbiKA9lYWhiFFvqaxw==$rdvrY6e0shCCGMEL1j0S0rfU43qNxm9MSE8NC8Bz6To=','Admin_1','Admin_1','Admins','Admin@Admin.com','(555)555-5555',1,1,NULL),(4,'Supervisor_1','PBKDF2-SHA256$100000$cM8fUeZyHwfHlUX9lSJ3HQ==$YyQQyF9wqrwKkfQPyP5ic/sMloy5vJo6sN/j78c6t9w=','Supervisor_1','Supervisor_1','Team 1','Supervisor@Supervisor.com','(555)555-5555',2,1,NULL),(5,'Worker_1','PBKDF2-SHA256$100000$RB5AfREudfYuYJoaCh60Yw==$Tz1p/hJ+Ipn8yUKa3l/KFwn8+xlPVXXBMlS/nnAiVVk=','Worker_1','Worker_1','Team 1','Worker@Worker.com','(555)555-5555',3,1,NULL),(6,'Driver_1','PBKDF2-SHA256$100000$KQnMEFEUDe7ijhUSdpLK2Q==$ShsKUi99uyxnOQi7Rpzec9aYg5KPhLstEPonJdKPVys=','Driver_1','Driver_1','Team 1','Driver@Driver.com','(555)555-5555',4,1,NULL),(7,'Driver_2','PBKDF2-SHA256$100000$3Cej5H0daI+bc5cET/zLEg==$rjJrWBt35P+soqeSE2BUOCV5NJA7/4+jkXq0FOCIalU=','Driver_2','Driver_2','Team 2','Driver2@Driver2.com','(555)555-5555',4,1,NULL),(8,'Worker_2','PBKDF2-SHA256$100000$slNx4ZV0vwUWGhSbxrpUBA==$By+Od2Ez14gysYkfEB2t6TxDi5sxHwxGihha5gvsnfU=','Worker_2_FName','Worker_2_LName','Team 2','Worker2@Worker2.com','(555)555-5555',3,1,NULL),(9,'Supervisor_2','PBKDF2-SHA256$100000$XYd504ftXrfffDvhoflQlw==$NkO9vcULqkkUK031kVF5TA6Q1m3Dkkx10TJcsUiqtJw=','Supervisor_2','Supervisor_2','Team 2','Supervisor2@Supervisor2.com','(555)555-5555',2,1,NULL);


/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-10-22  7:46:11
