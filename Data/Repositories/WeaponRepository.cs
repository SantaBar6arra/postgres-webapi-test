using Data.Context;
using Data.Models;
using Data.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Npgsql;

namespace Data.Repositories
{
    public class WeaponRepository : IRepository<Weapon>
    {
        private readonly string connectionString;
        public WeaponRepository(DataContext context)
        {
            connectionString = context.Database.GetDbConnection().ConnectionString;
        }

        public long Add(Weapon entity)
        {
            try
            {
                string query = @"insert into weapons(name, commonrange, maxrange, caliber, manufacturer, class) 
                    values (@Name, @CommonRange, @MaxRange, @Caliber, @Manufacturer, @Class) returning id";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", entity.Name);
                        command.Parameters.AddWithValue("@CommonRange", entity.CommonRange);
                        command.Parameters.AddWithValue("@MaxRange", entity.MaxRange);
                        command.Parameters.AddWithValue("@Caliber", entity.Caliber);
                        command.Parameters.AddWithValue("@Manufacturer", entity.Manufacturer);
                        command.Parameters.AddWithValue("@Class", entity.Class);
                        long id = (long)command.ExecuteScalar();

                        connection.Close();

                        return id;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Weapon> GetAll()
        {
            try
            {
                string query = @"select ""id"" as ""id"",
                    ""name"" as ""name"",
                    ""commonrange"" as ""commonrange"", 
                    ""maxrange"" as ""maxrange"", 
                    ""caliber"" as ""caliber"", 
                    ""manufacturer"" as ""manufacturer"", 
                    ""class"" as ""class""
            
                    from weapons";

                DataTable dataTable = new DataTable();

                NpgsqlDataReader reader;
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        reader = command.ExecuteReader();
                        dataTable.Load(reader);

                        reader.Close();
                        connection.Close();
                    }
                }

                var dataRows = dataTable.AsEnumerable();
                var weapons = new List<Weapon>();
                foreach (DataRow row in dataRows)
                {
                    Weapon weapon = new()
                    {
                        Id = (long)row["id"],
                        Name = (string)row["name"],
                        CommonRange = (int)row["commonrange"],
                        MaxRange = (int)row["maxrange"],
                        Caliber = (decimal)row["caliber"],
                        Class = (string)row["class"],
                        Manufacturer = (string)row["manufacturer"]
                    };
                    weapons.Add(weapon);
                }

                return weapons;
            }
            catch
            {
                throw;
            }
        }

        public Weapon GetById(long id)
        {
            try 
            { 
                string query = @"select ""id"" as ""id"",
                    ""name"" as ""name"",
                    ""commonrange"" as ""commonrange"", 
                    ""maxrange"" as ""maxrange"", 
                    ""caliber"" as ""caliber"", 
                    ""manufacturer"" as ""manufacturer"", 
                    ""class"" as ""class""
            
                    from weapons
                    where id = @Id";

                DataTable dataTable = new();
                NpgsqlDataReader reader;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        reader = command.ExecuteReader();
                        dataTable.Load(reader);

                        reader.Close();
                        connection.Close();
                    }
                }

                if (dataTable.Rows.Count != 0)
                {
                    Weapon weapon = new()
                    {
                        Id = dataTable.Rows[0].Field<long>(0),
                        Name = dataTable.Rows[0].Field<string>(1),
                        CommonRange = dataTable.Rows[0].Field<int>(2),
                        MaxRange = dataTable.Rows[0].Field<int>(3),
                        Caliber = dataTable.Rows[0].Field<decimal>(4),
                        Manufacturer = dataTable.Rows[0].Field<string>(5),
                        Class = dataTable.Rows[0].Field<string>(6)
                    };
                    return weapon;
                }
                else
                    throw new RecordNotFoundException();
            }
            catch
            {
                throw;
            }
        }

        public bool Update(Weapon entity)
        {
            try
            {
                string query = @"select id from weapons where id = @id";

                DataTable dataTable = new DataTable();
                NpgsqlDataReader reader;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", entity.Id);
                        reader = command.ExecuteReader();
                        dataTable.Load(reader);

                        reader.Close();
                        connection.Close();
                    }
                }

                if (dataTable.Rows.Count == 0)
                    throw new RecordNotFoundException();

                dataTable.Clear();

                query = @"
                    update weapons
                    set name = @Name,
                        commonrange = @CommonRange,
                        maxrange = @MaxRange,
                        caliber = @Caliber,
                        manufacturer = @Manufacturer,
                        class = @Class
                    where id = @Id;";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", entity.Id);
                        command.Parameters.AddWithValue("@Name", entity.Name);
                        command.Parameters.AddWithValue("@CommonRange", entity.CommonRange);
                        command.Parameters.AddWithValue("@MaxRange", entity.MaxRange);
                        command.Parameters.AddWithValue("@Caliber", entity.Caliber);
                        command.Parameters.AddWithValue("@Manufacturer", entity.Manufacturer);
                        command.Parameters.AddWithValue("@Class", entity.Class);
                        reader = command.ExecuteReader();
                        dataTable.Load(reader);

                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Delete(long id)
        {
            try
            {
                string query = @"select id from weapons where id = @id";

                DataTable dataTable = new DataTable();
                NpgsqlDataReader reader;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        reader = command.ExecuteReader();
                        dataTable.Load(reader);

                        reader.Close();
                        connection.Close();
                    }
                }

                if (dataTable.Rows.Count == 0)
                    throw new RecordNotFoundException();

                dataTable.Clear();

                query = @"delete from weapons where id = @id;";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        reader = command.ExecuteReader();
                        dataTable.Load(reader);

                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
