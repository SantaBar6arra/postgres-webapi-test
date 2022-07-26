using Data.Context;
using Data.Errors;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SoldierRepository : IRepository<Soldier>
    {
        private readonly string connectionString;

        public SoldierRepository(DataContext context)
        {
            connectionString = context.Database.GetDbConnection().ConnectionString;
        }

        public long Add(Soldier entity)
        {
            try
            {
                string query = @"insert into soldiers(firstname, secondname, primaryweaponid, secondaryweaponid) 
                    values (@FirstName, @SecondName, @PrimaryWeaponId, @SecondaryWeaponId) returning id";

                using (NpgsqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                        command.Parameters.AddWithValue("@SecondName", entity.SecondName);
                        command.Parameters.AddWithValue("@PrimaryWeaponId", entity.PrimaryWeaponId);
                        command.Parameters.AddWithValue("@SecondaryWeaponId", entity.SecondaryWeaponId);
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

        

        public IEnumerable<Soldier> GetAll()
        {
            try
            {
                string query = @"select ""id"" as ""id"",
                    ""firstname"" as ""firstname"",
                    ""secondname"" as ""secondname"", 
                    ""primaryweaponid"" as ""primaryweaponid"", 
                    ""secondaryweaponid"" as ""secondaryweaponid""
            
                    from soldiers";

                DataTable dataTable = new();
                NpgsqlDataReader reader;

                using (NpgsqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new(query, connection))
                    {
                        reader = command.ExecuteReader();
                        dataTable.Load(reader);

                        reader.Close();
                        connection.Close();
                    }
                }

                var dataRows = dataTable.AsEnumerable();
                var soldiers = new List<Soldier>();

                foreach (DataRow row in dataRows)
                {
                    Soldier soldier = new()
                    {
                        Id = (long)row["id"],
                        FirstName = (string)row["firstname"],
                        SecondName = (string)row["secondname"],
                        PrimaryWeaponId = (long)row["primaryweaponid"],
                        SecondaryWeaponId = (long)row["secondaryweaponid"]
                    };
                    soldiers.Add(soldier);
                }
                return soldiers;
            }
            catch
            {
                throw;
            }
        }

        public Soldier GetById(long id)
        {
            try
            {
                string query = @"select ""id"" as ""id"",
                    ""firstname"" as ""firstname"",
                    ""secondname"" as ""secondname"", 
                    ""primaryweaponid"" as ""primaryweaponid"", 
                    ""secondaryweaponid"" as ""secondaryweaponid""
                    from soldiers
                    where id = @Id";

                DataTable dataTable = new();
                NpgsqlDataReader reader;

                using (NpgsqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new(query, connection))
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
                    return new Soldier()
                    {
                        Id = dataTable.Rows[0].Field<long>(0),
                        FirstName = dataTable.Rows[0].Field<string>(1),
                        SecondName = dataTable.Rows[0].Field<string>(2),
                        PrimaryWeaponId = dataTable.Rows[0].Field<long>(3),
                        SecondaryWeaponId = dataTable.Rows[0].Field<long>(4),
                    };
                }
                else
                    throw new RecordNotFoundException();
            }
            catch
            {
                throw;
            }
        }

        public bool Update(Soldier entity)
        {
            try
            {
                string query = @"select id from soldiers where id = @id";

                DataTable dataTable = new();
                NpgsqlDataReader reader;

                using (NpgsqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new(query, connection))
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
                    update soldiers
                    set firstname = @FirstName,
                        secondname = @SecondName,
                        primaryweaponid = @PrimaryWeaponId,
                        secondaryweaponid = @SecondaryWeaponId
                    where id = @Id;";

                using (NpgsqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", entity.Id);
                        command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                        command.Parameters.AddWithValue("@SecondName", entity.SecondName);
                        command.Parameters.AddWithValue("@PrimaryWeaponId", entity.PrimaryWeaponId);
                        command.Parameters.AddWithValue("@SecondaryWeaponId", entity.SecondaryWeaponId);
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
                string query = @"select id from soldiers where id = @Id";

                DataTable dataTable = new DataTable();
                NpgsqlDataReader reader;

                using (NpgsqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new(query, connection))
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

                query = @"delete from soldiers where id = @Id;";

                using (NpgsqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new(query, connection))
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