using System;
using System.Data;
using Google.Protobuf.WellKnownTypes;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MySQLConnector : MonoBehaviour
{
    public static MySqlConnection connection;
    private static string server = "192.168.0.11";
    private static string database = "VitaTableDB";
    private static string username = "plauder";
    private static string password = "plauder1";

    public static string personID;

    public static string spieleID;

    public static string connectionString;

    public static bool datenbankzugriffGewaehrt;

    public Toggle datenbankzugriffToggle;

    public static bool personExists = false;

    private void Start()
    {
        datenbankzugriffToggle.isOn = datenbankzugriffGewaehrt;
    }

    public void DatenbankZugriffaktivieren()
    {
        if (datenbankzugriffToggle.isOn == true)
        {
            datenbankzugriffGewaehrt = true;

            connectionString = $"Server={server};Database={database};User={username};Password={password};";
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open(); // Open the connection

                Debug.Log("Connected to MySQL server!");

                string countQuery = "SELECT COUNT(*) FROM person";

                using (MySqlCommand countCommand = new MySqlCommand(countQuery, connection))
                {
                    int count = Convert.ToInt32(countCommand.ExecuteScalar());

                    //if (count == 0)
                    //{
                    //    TestPersonAnlegen();
                    //}
                    //else
                    //{
                    //    Debug.Log("Person table is not empty.");
                    //}
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError("Failed to connect to MySQL server: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        else if (datenbankzugriffToggle.isOn == false)
        {
            datenbankzugriffGewaehrt = false;

            if (connection != null)
            {
                connection.Close();
            }
        }
    }

    public static void PersonAnlegen()
    {
        string personenID = Guid.NewGuid().ToString();
        string vorname = OberflaechenManagement.vorname.ToString();
        string nachname = "Nachname";
        int alter = OberflaechenManagement.alter;
        string geschlecht = OberflaechenManagement.geschlecht;
        string passwort = "testpassword" + OberflaechenManagement.vorname.ToString();
        string userbild = OberflaechenManagement.vorname.ToString() + ".jpg";

        string selectQuery = "SELECT COUNT(*) FROM person WHERE personenID = @personenID";
        string insertQuery = "INSERT INTO person (personenID, nachname, vorname, geschlecht, `alter`, passwort, userbild) VALUES (@personenID, @nachname, @vorname, @geschlecht, @alter, @passwort, @userbild)";

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
            {
                selectCommand.Parameters.AddWithValue("@personenID", personenID);

                int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn))
                {
                    insertCommand.Parameters.AddWithValue("@personenID", personenID);
                    insertCommand.Parameters.AddWithValue("@nachname", nachname);
                    insertCommand.Parameters.AddWithValue("@vorname", vorname);
                    insertCommand.Parameters.AddWithValue("@geschlecht", geschlecht);
                    insertCommand.Parameters.AddWithValue("@alter", alter);
                    insertCommand.Parameters.AddWithValue("@passwort", passwort);
                    insertCommand.Parameters.AddWithValue("@userbild", userbild);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }

    public static void SysInfEingabe()
    {
        string seriennummer = SeriennummerEingabe.seriennummer; // Assuming you have the appropriate value for seriennummer
        string versionsnummerSW = ""; // Assuming you have the appropriate value for versionsnummerSW
        string aktuellsteVersion = ""; // Assuming you have the appropriate value for aktuellsteVersion
        string versionsnummerHW = ""; // Assuming you have the appropriate value for versionsnummerHW
        int statusflag = 0; // Assuming you have the appropriate value for statusflag

        string updateQuery = "UPDATE systeminformationen SET seriennummer = @seriennummer WHERE id = (SELECT id FROM systeminformationen ORDER BY id ASC LIMIT 1)";

        string selectQuery = "SELECT COUNT(*) FROM systeminformationen";

        connectionString = $"Server={server};Database={database};User={username};Password={password};";
        connection = new MySqlConnection(connectionString);

        try
        {
            if (!IsConnectionOpen())
            {
                connection.Open();
            }

            // Check if the table has any entries
            using (MySqlCommand countCommand = new MySqlCommand(selectQuery, connection))
            {
                int count = Convert.ToInt32(countCommand.ExecuteScalar());

                // If the table is not empty, update the seriennummer of the first entry
                if (count > 0)
                {
                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@seriennummer", seriennummer);
                        updateCommand.ExecuteNonQuery();
                    }
                }
                // If the table is empty, insert a new entry
                else
                {
                    string insertQuery = "INSERT INTO systeminformationen (seriennummer, versionsnummerSW, aktuellsteVersion, versionsnummerHW, statusflag) VALUES (@seriennummer, @versionsnummerSW, @aktuellsteVersion, @versionsnummerHW, @statusflag)";

                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@seriennummer", seriennummer);
                        insertCommand.Parameters.AddWithValue("@versionsnummerSW", versionsnummerSW);
                        insertCommand.Parameters.AddWithValue("@aktuellsteVersion", aktuellsteVersion);
                        insertCommand.Parameters.AddWithValue("@versionsnummerHW", versionsnummerHW);
                        insertCommand.Parameters.AddWithValue("@statusflag", statusflag);

                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Error inserting/updating system information: " + ex.Message);
        }
    }


    public static string GetFirstSeriennummer()
    {
        string connectionString = $"Server={server};Database={database};User={username};Password={password};";
        string seriennummer = "";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT seriennummer FROM systeminformationen";

            using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        seriennummer = reader.GetString("seriennummer");
                    }
                    // You can handle scenarios where no rows are found if needed
                }
            }
        }

        return seriennummer;
    }

    public static string GetFirstVersionsnummerHW()
    {
        string connectionString = $"Server={server};Database={database};User={username};Password={password};";
        string seriennummer = "";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT versionsnummerHW FROM systeminformationen";

            using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        seriennummer = reader.GetString("versionsnummerHW");
                    }
                    // You can handle scenarios where no rows are found if needed
                }
            }
        }

        return seriennummer;
    }

    public static string GetFirstVersionsnummerSW()
    {
        string connectionString = $"Server={server};Database={database};User={username};Password={password};";
        string seriennummer = "";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT versionsnummerSW FROM systeminformationen";

            using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        seriennummer = reader.GetString("versionsnummerSW");
                    }
                    // You can handle scenarios where no rows are found if needed
                }
            }
        }

        return seriennummer;
    }

    public static bool IsSysteminformationenTableEmpty()
    {
        bool isFirstEntryEmpty = false;
        bool isTableEmpty = true;

        string countQuery = "SELECT COUNT(*) FROM systeminformationen";
        string selectQuery = "SELECT seriennummer FROM systeminformationen ORDER BY seriennummer ASC LIMIT 1";

        connectionString = $"Server={server};Database={database};User={username};Password={password};";
        connection = new MySqlConnection(connectionString);

        try
        {
            if (!IsConnectionOpen())
            {
                connection.Open();
            }

            // Check if the table has any entries
            using (MySqlCommand countCommand = new MySqlCommand(countQuery, connection))
            {
                int count = Convert.ToInt32(countCommand.ExecuteScalar());
                isTableEmpty = count == 0;
            }

            // Check if the seriennummer of the first entry is empty
            if (!isTableEmpty)
            {
                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                {
                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstSeriennummer = reader.GetString("seriennummer");
                            isFirstEntryEmpty = string.IsNullOrEmpty(firstSeriennummer);
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Error checking if systeminformationen table is empty: " + ex.Message);
        }

        return isTableEmpty || isFirstEntryEmpty;
    }


    public static void InsertSpieleData(int aktuelleRunde, int richtigGemacht, int falschGemacht)
    {
        if (datenbankzugriffGewaehrt == true)
        {
            try
            {
                bool istMultiplayer = false;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Retrieve the first personenID from the personen table


                    //using (MySqlCommand personenIDCmd = new MySqlCommand(personenIDQuery, conn))
                    //{
                    personID = GetPersonIDFromVorname(OberflaechenManagement.vorname.ToString());

                    spieleID = personID.ToString() + Einstellungen_Script.platz + DateTime.Now.ToString("yyyyMMddHHmmss");

                    // Use the first personenID in the INSERT statement for spiele
                    string query = "INSERT INTO spiele (personenID, stimmung, spieleID, rundenZahl, spielName, schwierigkeitsstufe, richtigAnzahl, falschAnzahl, istMultiplayer, rundenAnfangZeitstempel) " +
                                   "VALUES (@personenID, @stimmung, @spieleID, @rundenZahl, @spielName, @schwierigkeitsstufe, @richtigAnzahl, @falschAnzahl, @istMultiplayer, @rundenAnfangZeitstempel)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@personenID", personID);
                        cmd.Parameters.AddWithValue("@stimmung", OberflaechenManagement.mood);
                        cmd.Parameters.AddWithValue("@spieleID", spieleID);
                        cmd.Parameters.AddWithValue("@rundenZahl", aktuelleRunde);
                        cmd.Parameters.AddWithValue("@spielName", SceneSwitcherSpielauswahl.spielName);
                        cmd.Parameters.AddWithValue("@schwierigkeitsstufe", SceneSwitcherSpielauswahl.spielLevel);
                        cmd.Parameters.AddWithValue("@richtigAnzahl", richtigGemacht);
                        cmd.Parameters.AddWithValue("@falschAnzahl", falschGemacht);
                        cmd.Parameters.AddWithValue("@istMultiplayer", istMultiplayer);
                        cmd.Parameters.AddWithValue("@rundenAnfangZeitstempel", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError("Error inserting spiele data into MySQL database: " + ex.Message);
            }
        }
    }

    public static bool IsConnectionOpen()
    {
        return connection != null && connection.State == ConnectionState.Open;
    }

    public static void InsertSensorData(float sensorRohdaten)
    {
        if (SceneSwitcherSpielauswahl.spielName != null && MySQLConnector.spieleID != null)
        {
            try
            {
                if (!IsConnectionOpen())
                {
                    connection.Open();
                }

                string sensorEintragID = Guid.NewGuid().ToString();
                int platz = Einstellungen_Script.kurbelOben; // Use the appropriate platz value

                // Assign spieleID from SceneSwitcherSpielauswahl.spieleID

                // Insert the sensor data with the assigned spieleID
                string insertQuery = "INSERT INTO sensoren (spieleID, platz, sensorBezeichnung, sensorRohdaten, sensorEintragZeitstempel) " +
                                    "VALUES (@spieleID, @platz, @sensorBezeichnung, @sensorRohdaten, @sensorEintragZeitstempel)";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@spieleID", MySQLConnector.spieleID);
                    insertCmd.Parameters.AddWithValue("@platz", Einstellungen_Script.platz);
                    insertCmd.Parameters.AddWithValue("@sensorBezeichnung", Einstellungen_Script.kurbelOben);
                    insertCmd.Parameters.AddWithValue("@sensorRohdaten", sensorRohdaten);
                    insertCmd.Parameters.AddWithValue("@sensorEintragZeitstempel", DateTime.Now);

                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError("Error inserting sensor data into MySQL database: " + ex.Message);
            }
        }
    }

    public static string GetPersonIDFromVorname(string vorname)
    {
        string personID = null;

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT personenID FROM person WHERE vorname = @vorname";

                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                {
                    selectCommand.Parameters.AddWithValue("@vorname", vorname);

                    personID = selectCommand.ExecuteScalar() as string;
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Error getting personID from vorname: " + ex.Message);
        }

        return personID;
    }


    public static bool CheckIfPersonExists(string vorname)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT COUNT(*) FROM person WHERE vorname = @vorname";

                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                {
                    selectCommand.Parameters.AddWithValue("@vorname", vorname);

                    int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        personExists = true;
                    }
                    else
                    {
                        personExists = false;
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Error checking if person exists: " + ex.Message);
        }

        return personExists;
    }
}