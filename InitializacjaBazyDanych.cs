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
            ";
            using (var command = new NpgsqlCommand(createTablesQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertDataQuery = @"

                CREATE OR REPLACE VIEW prawnicy_zarobki AS 
                SELECT p.nazwisko, SUM(w.kwota::numeric) AS zarobki
                FROM prawnik p
                JOIN przypisane pr ON p.id_prawnika = pr.id_prawnika
                JOIN sprawa s ON pr.id_sprawy = s.id_sprawy
                JOIN wynagrodzenie w ON s.stopien_wynagrodzenia = w.stopien_wynagrodzenia
                GROUP BY p.nazwisko;


            ";

            using (var command = new NpgsqlCommand(insertDataQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
