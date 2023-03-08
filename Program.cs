using MySql.Data.MySqlClient;

public class Database {
    public static string GetSessionName() {
        return Environment.UserName;
    }

    public static async Task<string> GetPublicIPAddress() {
        using (HttpClient client = new HttpClient()) {
            string url = "https://api.ipify.org";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                string ip = await response.Content.ReadAsStringAsync();
                return ip;
            }
        }

        return null;
    }


    // CREATE TABLE utilisateur (
    //     id INT NOT NULL AUTO_INCREMENT,
    //     nom_session VARCHAR(255) NOT NULL,
    //     adresse_ip VARCHAR(255) NOT NULL,
    //     PRIMARY KEY (id)
    //     );


    public static void InsertUserData(string sessionName, string ipAddress) {
        string connectionString = "server=localhost;port=3306;uid=root;database=database-test;";
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();


        string query = "INSERT INTO utilisateur (nom_session, adresse_ip) VALUES (@sessionName, @ipAddress)";
        MySqlCommand command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@sessionName", sessionName);
        command.Parameters.AddWithValue("@ipAddress", ipAddress);
        command.ExecuteNonQuery();

        connection.Close();
    }

    static void Main(string[] args) {
        string sessionName = GetSessionName();
        string ipAddress = "";
        try {
            ipAddress = GetPublicIPAddress().Result;
        }
        catch (Exception ex) {
            Console.WriteLine("Impossible d'obtenir l'adresse IP publique : " + ex.Message);
        }

        InsertUserData(sessionName, ipAddress);
    }
}