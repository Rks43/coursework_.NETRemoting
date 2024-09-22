using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Proginterface;
using System.Data.OleDb;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace RemoteObject
{

    public class ServerObject : MarshalByRefObject, Interface
    {
        protected static string connectString = @"Data Source=WIN-TLCIS55I519\SERVAK;Initial Catalog=Kursovoy;Integrated Security=True";
        private SqlConnection sqlConnection;

        public ServerObject()
        {
            Console.WriteLine("Constructor called");
        }
        ~ServerObject()
        {
            Console.WriteLine("Destructor called");
        }

        public void Connecting()
        {
            sqlConnection = new SqlConnection(connectString);
            sqlConnection.Open();
        }


        public string Autorization(string login, string password)
        {
            string slct = $"SELECT * FROM [Users] WHERE ([Логин]='{login}' AND [Пароль]= '{password}')";
            SqlCommand command = new SqlCommand(slct, sqlConnection);
            string user;
            if (command.ExecuteScalar() == null)
                return null;
            user = Convert.ToString(command.ExecuteScalar());
            return user;
        }

        public string Registration(string login1, string password, string fio, string phone)
        {
            string registred = "Вы успешно зарегистрировались!";
            string error = "Произошла ошибка ";
            string login = login1;
            string query = $"INSERT INTO [Users] (Логин, Пароль, ФИО, Телефон) VALUES (@login, @password, @fio, @phone)";
                SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@fio", fio);
            command.Parameters.AddWithValue("@phone", phone);
            try
                {
                    command.ExecuteNonQuery();
                    return registred;
                }
                catch (Exception ex)
                {
                    error = error + ex.Message;
                    Console.WriteLine(error);
                    return error;
            }
        }


        public int GetCodeUser(string login)
        {
            int code = -1;
            string query = "SELECT Код FROM Users WHERE Логин = @login";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@login", login);
            try
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return code = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при выполнении запроса: " + ex.Message);
            }
            return code;
        }


        public string AddReview(int codeUser, string feedback)
        {
            string resultMessage = "Отзыв успешно добавлен!";
            string errorMessage = "Произошла ошибка при добавлении отзыва: ";
            string query = "INSERT INTO Feedback (Пользователь, Отзыв) VALUES (@codeUser, @feedback)";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@codeUser", codeUser);
                command.Parameters.AddWithValue("@feedback", feedback);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    errorMessage += ex.Message;
                    Console.WriteLine(errorMessage);
                    return errorMessage;
                }
            return resultMessage;
        }

        public string AddRequest(string service, string login, int cost)
        {
            string resultMessage = "";

            string query = @"
        INSERT INTO [Request] ([Сервис], [Логин], [Цена])
        VALUES (@service, @login, @cost)
    ";

                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@service", service);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@cost", cost);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        resultMessage = "Заявка успешно добавлена!";
                    }
                    else
                    {
                        resultMessage = "Не удалось добавить заявку.";
                    }
                }
                catch (Exception ex)
                {
                    resultMessage = "Произошла ошибка при добавлении заявки: " + ex.Message;
                }

            return resultMessage;
        }

        public List<string> GetRequests()
        {
            List<string> requests = new List<string>();

            string query = @"
        SELECT [Сервис], [Логин], [Цена] FROM [Request]
    ";

                SqlCommand command = new SqlCommand(query, sqlConnection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string serviceName = reader.GetString(0);
                        string userLogin = reader.GetString(1);
                        int requestCost = reader.GetInt32(2);

                        string requestInfo = $"Сервис: {serviceName}, Логин: {userLogin}, Цена: {requestCost}";
                        requests.Add(requestInfo);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка при получении списка заявок: " + ex.Message);
                }
            return requests;
        }


        public List<string> GetReviews()
        {
            List<string> reviews = new List<string>();

            string query = @"
        SELECT Users.ФИО, Feedback.Отзыв
        FROM Feedback
        JOIN Users ON Feedback.Пользователь = Users.Код
    ";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
            {
                string userName = reader.GetString(0);
                string review = reader.GetString(1);

                string formattedReview = $"{userName}: {review}";
                reviews.Add(formattedReview);
            }

            reader.Close();
        }
        catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка при получении отзывов: " + ex.Message);
                }

            return reviews;
        }

        public List<string> GetProducts()
        {
            List<string> products = new List<string>();

            string query = "SELECT [Название], [Цена] FROM [Kursovoy].[dbo].[Service]";
                SqlCommand command = new SqlCommand(query, sqlConnection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    
                    while (reader.Read())
                    {
                        string productName = reader.GetString(0);
                        int productPrice = reader.GetInt32(1);
                        string productInfo = $"{productName} - Цена: {productPrice}"; 
                        products.Add(productInfo);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка при получении списка товаров/услуг: " + ex.Message);
                }

            return products;
        }


        public ILease MyInitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(10);
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(2);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;

        }
        public string RemoteMethod()
        {
            Console.WriteLine("ClientActivatedType.RemoteMethod Вызван.");
            return "RemoteMethod вызван.на-: " + AppDomain.CurrentDomain.FriendlyName;
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(10);
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(2);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;
        }


        public string RemoveService(string name)
        {
        
            string query = "DELETE FROM [Service] WHERE [Название] = @name";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@name", name);

                try
                {
                    
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return ("Услуга успешно удалена.");
                    }
                    else
                    {
                        return ("Не удалось удалить услугу.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка при удалении услуги: " + ex.Message);
                }

                return "";
        }
        public string AddService(string serviceName, int servicePrice)
        {
            string resultMessage = "";

    
            string query = @"
        INSERT INTO [Service] ([Название], [Цена])
        VALUES (@serviceName, @servicePrice)
    ";


                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@serviceName", serviceName);
                command.Parameters.AddWithValue("@servicePrice", servicePrice);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        resultMessage = "Новая услуга успешно добавлена.";
                    }
                    else
                    {
                        resultMessage = "Не удалось добавить новую услугу.";
                    }
                }
                catch (Exception ex)
                {
                    resultMessage = "Произошла ошибка при добавлении новой услуги: " + ex.Message;
                }
            

            return resultMessage;
        }

        public string DeleteRequest(int price)
        {
                string resultMessage = "";

            
                string query = @"
        DELETE FROM [Request]
        WHERE [Цена] = @price
    ";
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.Parameters.AddWithValue("@price", price);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            resultMessage = "Заявка успешно удалена.";
                        }
                        else
                        {
                            resultMessage = "Не удалось найти указанную заявку.";
                        }
                    }
                    catch (Exception ex)
                    {
                        resultMessage = "Произошла ошибка при удалении заявки: " + ex.Message;
                    }
                return resultMessage;
        }

        public string RemoveFeedback(string nameUser, string feedback)
        {
            string resultMessage = "";

     
            string userCodeQuery = "SELECT [Код] FROM [Users] WHERE [ФИО] = @nameUser";

            int userCode = 0; 

                try
                {
                    SqlCommand userCodeCommand = new SqlCommand(userCodeQuery, sqlConnection);
                    userCodeCommand.Parameters.AddWithValue("@nameUser", nameUser);

                    object userCodeResult = userCodeCommand.ExecuteScalar();

                    if (userCodeResult != null)
                    {
                        userCode = Convert.ToInt32(userCodeResult); 
                    }
                    else
                    {
                        return "Пользователь с указанным именем не найден.";
                    }

                    string removeFeedbackQuery = @"
                DELETE FROM [Feedback]
                WHERE [Пользователь] = @userCode AND [Отзыв] = @feedback
            ";

                    SqlCommand command = new SqlCommand(removeFeedbackQuery, sqlConnection);
                    command.Parameters.AddWithValue("@userCode", userCode);
                    command.Parameters.AddWithValue("@feedback", feedback);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        resultMessage = "Отзыв успешно удален.";
                    }
                    else
                    {
                        resultMessage = "Не удалось найти указанный отзыв.";
                    }
                }
                catch (Exception ex)
                {
                    resultMessage = "Произошла ошибка при удалении отзыва: " + ex.Message;
                }
            

            return resultMessage;
        }

    }
}