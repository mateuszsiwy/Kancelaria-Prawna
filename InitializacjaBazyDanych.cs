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

                -- Modyfikacja tabeli Prawnik
                ALTER TABLE Prawnik 
                ADD COLUMN IF NOT EXISTS specjalizacja VARCHAR(100),
                ADD COLUMN IF NOT EXISTS stanowisko VARCHAR(50) DEFAULT 'Junior',
                ADD COLUMN IF NOT EXISTS stawka_godzinowa DECIMAL(10, 2) DEFAULT 100.00,
                ADD COLUMN IF NOT EXISTS data_zatrudnienia DATE DEFAULT CURRENT_DATE;

                -- Modyfikacja tabeli Sprawa
                ALTER TABLE Sprawa
                ADD COLUMN IF NOT EXISTS tytul VARCHAR(200),
                ADD COLUMN IF NOT EXISTS data_rozpoczecia DATE DEFAULT CURRENT_DATE,
                ADD COLUMN IF NOT EXISTS status VARCHAR(50) DEFAULT 'Nowa',
                ADD COLUMN IF NOT EXISTS priorytet INT DEFAULT 1,
                ADD COLUMN IF NOT EXISTS wynik VARCHAR(50),
                ADD COLUMN IF NOT EXISTS id_klienta INT;

                -- Dodanie powiązania z tabelą Klient
                

                -- Modyfikacja tabeli Przypisane
                ALTER TABLE Przypisane
                ADD COLUMN IF NOT EXISTS rola VARCHAR(50) DEFAULT 'Assistant',
                ADD COLUMN IF NOT EXISTS data_przypisania DATE DEFAULT CURRENT_DATE;

                -- Dodanie nowej tabeli CzasPracy
                CREATE TABLE IF NOT EXISTS CzasPracy (
                    id_czasu SERIAL PRIMARY KEY,
                    id_prawnika INT REFERENCES Prawnik(id_prawnika),
                    id_sprawy INT REFERENCES Sprawa(id_sprawy),
                    data DATE NOT NULL,
                    liczba_godzin DECIMAL(4,2) NOT NULL,
                    opis_czynnosci TEXT,
                    CONSTRAINT fk_czas_prawnik FOREIGN KEY (id_prawnika) REFERENCES Prawnik(id_prawnika),
                    CONSTRAINT fk_czas_sprawa FOREIGN KEY (id_sprawy) REFERENCES Sprawa(id_sprawy)
                );
            ";
            using (var command = new NpgsqlCommand(createTablesQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertDataQuery = @"
                -- Triggery i funkcje
                CREATE OR REPLACE FUNCTION check_lead_lawyer() RETURNS TRIGGER AS $$
                BEGIN
                    IF NEW.rola = 'Lead' AND EXISTS (
                        SELECT 1 FROM Przypisane 
                        WHERE id_sprawy = NEW.id_sprawy 
                        AND rola = 'Lead' 
                        AND id_przypisane != NEW.id_przypisane
                    ) THEN
                        RAISE EXCEPTION 'Może być tylko jeden Lead Prawnik na sprawę';
                    END IF;
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                DROP TRIGGER IF EXISTS enforce_one_lead ON Przypisane;
                CREATE TRIGGER enforce_one_lead
                BEFORE INSERT OR UPDATE ON Przypisane
                FOR EACH ROW EXECUTE FUNCTION check_lead_lawyer();

                -- Funkcja obliczająca koszt sprawy
                CREATE OR REPLACE FUNCTION oblicz_koszt_sprawy(sprawa_id INT) 
                RETURNS DECIMAL AS $$
                DECLARE
                    total_cost DECIMAL;
                BEGIN
                    SELECT SUM(cp.liczba_godzin * p.stawka_godzinowa)
                    INTO total_cost
                    FROM CzasPracy cp
                    JOIN Prawnik p ON cp.id_prawnika = p.id_prawnika
                    WHERE cp.id_sprawy = sprawa_id;
                    RETURN COALESCE(total_cost, 0);
                END;
                $$ LANGUAGE plpgsql;


                CREATE OR REPLACE VIEW PrawnicySprawyView AS
                SELECT 
                    p.id_prawnika,
                    p.imie,
                    p.nazwisko,
                    p.stanowisko,
                    s.id_sprawy,
                    s.tytul,
                    s.opis,
                    s.data_rozpoczecia,
                    s.data_zakonczenia,
                    s.status,
                    s.priorytet,
                    s.wynik,
                    pr.rola,
                    pr.data_przypisania
                FROM Prawnik p
                JOIN Przypisane pr ON p.id_prawnika = pr.id_prawnika
                JOIN Sprawa s ON pr.id_sprawy = s.id_sprawy;

                DROP Trigger if exists trigger_loguj_zmiany_sprawy on Sprawa;

                CREATE OR REPLACE FUNCTION oblicz_koszt_sprawy(sprawa_id INT) 
                RETURNS DECIMAL AS $$
                DECLARE
                    total_cost DECIMAL;
                BEGIN
                    SELECT SUM(cp.liczba_godzin * p.stawka_godzinowa)
                    INTO total_cost
                    FROM CzasPracy cp
                    JOIN Prawnik p ON cp.id_prawnika = p.id_prawnika
                    WHERE cp.id_sprawy = sprawa_id;
                    RETURN COALESCE(total_cost, 0);
                END;
                $$ LANGUAGE plpgsql;
            ";

            using (var command = new NpgsqlCommand(insertDataQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}