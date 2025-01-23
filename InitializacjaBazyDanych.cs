using Npgsql;

public class InitializacjaBazyDanych
{
    private readonly string _connectionString;
    public InitializacjaBazyDanych(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void ZainicjalizujBazeDanych()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            string createTablesQuery = @"
                -- Tabela Prawnicy
                CREATE TABLE IF NOT EXISTS Prawnik (
                    id_prawnika SERIAL PRIMARY KEY,
                    imie VARCHAR(50) NOT NULL,
                    nazwisko VARCHAR(50) NOT NULL
                );

                -- Tabela Sprawy
                CREATE TABLE IF NOT EXISTS Sprawa (
                    id_sprawy SERIAL PRIMARY KEY,
                    opis VARCHAR(100) NOT NULL,
                    data_zakonczenia DATE,
                    stopien_wynagrodzenia INT NOT NULL
                );

                -- Tabela Przypisane
                CREATE TABLE IF NOT EXISTS Przypisane (
                    id_przypisane SERIAL PRIMARY KEY,
                    id_sprawy INT NOT NULL REFERENCES Sprawa(id_sprawy),
                    id_prawnika INT NOT NULL REFERENCES Prawnik(id_prawnika)
                );

                -- Tabela Wynagrodzenie
                CREATE TABLE IF NOT EXISTS Wynagrodzenie (
                    id_wynagrodzenia SERIAL PRIMARY KEY,
                    stopien_wynagrodzenia INT NOT NULL,
                    kwota VARCHAR(50) NOT NULL
                );

                -- Tabela Klient
                CREATE TABLE IF NOT EXISTS Klient (
                    id_klienta SERIAL PRIMARY KEY,
                    imie VARCHAR(50) NOT NULL,
                    nazwisko VARCHAR(50) NOT NULL,
                    email VARCHAR(100),
                    telefon VARCHAR(20)
                );

                -- Tabela Faktura
                CREATE TABLE IF NOT EXISTS Faktura (
                    id_faktury SERIAL PRIMARY KEY,
                    id_klienta INT NOT NULL REFERENCES Klient(id_klienta),
                    data_wystawienia DATE NOT NULL,
                    kwota DECIMAL(10, 2) NOT NULL,
                    status VARCHAR(50) NOT NULL
                );

                -- Tabela Platnosc
                CREATE TABLE IF NOT EXISTS Platnosc (
                    id_platnosci SERIAL PRIMARY KEY,
                    id_faktury INT NOT NULL REFERENCES Faktura(id_faktury),
                    data_platnosci DATE NOT NULL,
                    kwota DECIMAL(10, 2) NOT NULL
                );
            ";
            using (var command = new NpgsqlCommand(createTablesQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertDataQuery = @"
                
            ";

            using (var command = new NpgsqlCommand(insertDataQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}