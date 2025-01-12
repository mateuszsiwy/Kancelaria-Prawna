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

                CREATE OR REPLACE VIEW liczba_spraw_prawnika AS
                SELECT p.nazwisko, COUNT(pr.id_sprawy) AS liczba_spraw
                FROM prawnik p
                JOIN przypisane pr ON p.id_prawnika = pr.id_prawnika
                Join sprawa s ON pr.id_sprawy = s.id_sprawy
                GROUP BY p.nazwisko;

                CREATE OR REPLACE VIEW sprawy_w_toku AS
                SELECT s.id_sprawy, s.opis, p.nazwisko
                FROM sprawa s
                JOIN przypisane pr ON s.id_sprawy = pr.id_sprawy
                JOIN prawnik p ON pr.id_prawnika = p.id_prawnika
                WHERE s.data_zakonczenia IS NULL;

              ";

            using (var command = new NpgsqlCommand(insertDataQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
